import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import {
  MAT_DATE_LOCALE,
  MAT_DATE_FORMATS,
  DateAdapter,
} from '@angular/material/core';
import {
  MomentDateAdapter,
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
} from '@angular/material-moment-adapter';
import { RouterModule } from '@angular/router';
import moment from 'moment';
import Shepherd from 'shepherd.js';
import Swal from 'sweetalert2';
import { MonitorSppBimerService } from '../../../../../core/services/intregracoes/spp-bimer-invoce/monitor-spp-bimer.service';
import { MonitorDeparaMensagensService } from '../../../../../core/services/intregracoes/spp-bimer-invoce/depara-mensagens-erros.service';
import { SppBimerInvoceMonitorModel } from '../../../../../shared/models/integracao/SppBimerInvoce/spp-bimerInvoce-monitor.model';
import { ReprocessarBimerRequest } from '../../../../../shared/models/integracao/SppBimerInvoce/ReprocessarBimerRequest.model';


export const DATE_FORMAT_BR = {
  parse: { dateInput: 'DD/MM/YYYY' },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MM YYYY',
    dateA11yLabel: 'DD/MM/YYYY',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  standalone: true,
  selector: 'app-monitor-spp-bimer',
  templateUrl: './monitor-spp-bimer.component.html',
  styleUrls: ['./monitor-spp-bimer.component.scss'],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },
    { provide: MAT_DATE_FORMATS, useValue: DATE_FORMAT_BR },
    DatePipe,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatIconModule,
    RouterModule,
  ],
})
export class MonitorSppBimerComponent implements OnInit {
  private service = inject(MonitorSppBimerService);
  private snackbar = inject(MatSnackBar);
  private deparaService = inject(MonitorDeparaMensagensService);
  private _statusSelecionado = signal<string | null>(null);

  statusSelecionado = computed(() => this._statusSelecionado());
  dataInicio = new FormControl<Date | null>(null);
  dataFim = new FormControl<Date | null>(null);
  filtroGeral = new FormControl<string>('', { nonNullable: true });
  loading = signal(false);

  private readonly mapeamentos: Array<{
    padrao: string;
    mensagem: string;
    ativo: boolean;
  }> = [];

  dados = signal<SppBimerInvoceMonitorModel[]>([]);

  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  private registrosInvalidos = signal<Set<string>>(new Set());
  private reprocessadosComSucesso = signal<Set<string>>(new Set());

  ngOnInit(): void {
    this.setupTour();
    this.inicializarFiltrosPadrao();
    this.buscar();
  }

  private inicializarFiltrosPadrao(): void {
    const hoje = moment().startOf('day').toDate();
    this.dataInicio.setValue(hoje);
    this.dataFim.setValue(hoje);
  }

  private normalizarData = (data: Date): moment.Moment =>
    moment(data).startOf('day');

  dadosFiltrados = computed(() => {
    const termo = this.filtroGeral.value.toLowerCase().trim();
    const inicio = this.dataInicio.value;
    const fim = this.dataFim.value;

    return this.dados()
      .filter((item) => {
        const dataItem = this.normalizarData(new Date(item.dataEmissao));
        const inicioFiltro = inicio ? this.normalizarData(inicio) : null;
        const fimFiltro = fim ? this.normalizarData(fim) : null;

        const matchTermo =
          !termo ||
          [
            item.numeroAlterData,
            item.pedidoSpp,
            item.fabricante,
            item.valorInvoice?.toString(),
            item.observacaoErro,
          ].some((val) => val?.toLowerCase?.().includes?.(termo));

        const dentroPeriodo =
          (!inicioFiltro || dataItem.isSameOrAfter(inicioFiltro)) &&
          (!fimFiltro || dataItem.isSameOrBefore(fimFiltro));

        return matchTermo && dentroPeriodo;
      })
      .sort((a, b) => {
        const fabricanteA = a.fabricante?.toLowerCase() ?? '';
        const fabricanteB = b.fabricante?.toLowerCase() ?? '';
        if (fabricanteA < fabricanteB) return -1;
        if (fabricanteA > fabricanteB) return 1;
        return (
          moment(b.dataEmissao).valueOf() - moment(a.dataEmissao).valueOf()
        );
      });
  });

  totalRegistros = computed(() => this.dados().length);
  totalSucesso = computed(
    () => this.dados().filter((d) => d.statusIntegracao === 'S').length
  );
  totalErro = computed(
    () => this.dados().filter((d) => d.statusIntegracao === 'N').length
  );

  colunas = [
    'numeroAlterData',
    'pedidoSpp',
    'fabricante',
    'dataEmissao',
    'valorInvoice',
    'statusIntegracao',
    'observacaoErro',
    'acoes',
  ];

  filtrarPorStatus(status: string | null): void {
    this._statusSelecionado.set(status);
    this.buscar();
  }

  private toIsoDate = (date: Date): string => moment(date).format('YYYY-MM-DD');

  buscar(): void {
    this.loading.set(true);

    const dataInicioFormatada = this.dataInicio.value
      ? this.toIsoDate(this.dataInicio.value)
      : undefined;

    const dataFimFormatada = this.dataFim.value
      ? this.toIsoDate(this.dataFim.value)
      : undefined;

    this.service
      .getMonitoramentos(
        this.statusSelecionado() || undefined,
        dataInicioFormatada,
        dataFimFormatada
      )
      .subscribe({
        next: (res) => {
          res.forEach((item) => {
            if (item.observacaoErro) {
              this.deparaService
                .buscarPorMensagemPadrao(item.observacaoErro)
                .subscribe((mapeada) => {
                  item.observacaoErro =
                    mapeada?.mensagemModificada ?? item.observacaoErro;
                });
            }
          });
          this.dados.set(res);
        },
        error: () =>
          this.snackbar.open('Erro ao buscar dados', 'Fechar', {
            duration: 3000,
          }),
        complete: () => this.loading.set(false),
      });
  }

  reprocessar(item: SppBimerInvoceMonitorModel): void {
    const pedido: string = item.pedidoSpp?.trim() || '';
    const fabricante: string = item.fabricanteId?.toString() || '';
    const estoque: string = item.estoque?.trim() || '';

    const camposFaltando: string[] = [];
    if (!pedido) camposFaltando.push('Invoice');
    if (!fabricante) camposFaltando.push('Fabricante');
    if (!estoque) camposFaltando.push('Estoque');

    if (camposFaltando.length > 0) {
      const mensagem = `
        <div style="text-align:left;">
          <p>❌ Não é possível reprocessar este registro.</p>
          <p><strong>Pedido:</strong> ${item.numeroAlterData}</p>
          <p><strong>Motivo:</strong> Dados ausentes para reprocessamento.</p>
          <ul>
            ${camposFaltando
          .map((c) => `<li>🔸 <strong>${c}</strong> está vazio</li>`)
          .join('')}
          </ul>
        </div>
      `;

      Swal.fire({
        icon: 'warning',
        title: 'Dados Insuficientes',
        html: mensagem,
        confirmButtonText: 'Entendi',
        customClass: {
          popup: 'swal2-rounded',
        },
      });

      const setAtual = new Set(this.registrosInvalidos());
      setAtual.add(item.pedidoSpp || item.numeroAlterData);
      this.registrosInvalidos.set(setAtual);

      return;
    }

    this.showLoadingAlert();

    const request: ReprocessarBimerRequest = { pedido, fabricante, estoque };
    const urlReprocessamento = this.service.getReprocessamentoUrl(request);

    this.loading.set(true);

    this.service.reprocessar(request).subscribe({
      next: (msg: string) => {
        Swal.fire({
          icon: 'success',
          title: '<h2 style="color:#2e7d32;">✔ Reprocessado com sucesso!</h2>',
          html: `
            <div style="text-align:left; border-radius:12px; padding:1rem; background:#f9f9f9;">
              <p><strong>🟢 Mensagem:</strong> ${msg}</p>
              <hr />
              <p><strong>🔗 URL Reprocessada:</strong></p>
              <pre style="background:#e8f5e9; padding:10px; border-radius:10px; font-size:13px;">${urlReprocessamento}</pre>
              <p><strong>📦 Dados Enviados:</strong></p>
              <pre style="background:#e3f2fd; padding:10px; border-radius:10px; font-size:13px;">${JSON.stringify(
            request,
            null,
            2
          )}</pre>
            </div>
          `,
          width: 600,
          customClass: {
            popup: 'swal2-rounded',
          },
          confirmButtonText: 'OK',
          buttonsStyling: true,
        });

        const chave = item.pedidoSpp || item.numeroAlterData;
        const setAtual = new Set(this.reprocessadosComSucesso());
        setAtual.add(chave);
        this.reprocessadosComSucesso.set(setAtual);

        this.buscar();
      },
      error: (err: any) => {
        const erroDetalhado =
          typeof err?.error === 'object'
            ? JSON.stringify(err.error, null, 2)
            : err?.error ?? 'Erro desconhecido ao reprocessar.';

        Swal.fire({
          icon: 'error',
          title: '<h2 style="color:#c62828;">❌ Falha no Reprocessamento</h2>',
          html: `
            <div style="text-align:left; border-radius:12px; padding:1rem; background:#fff3f3;">
              <p><strong>❗ Erro retornado:</strong></p>
              <pre style="background:#fce4ec; padding:10px; border-radius:10px; font-size:13px;">${erroDetalhado}</pre>
              <hr />
              <p><strong>🔗 URL Utilizada:</strong></p>
              <pre style="background:#f8bbd0; padding:10px; border-radius:10px; font-size:13px;">${urlReprocessamento}</pre>
              <p><strong>📦 Dados Enviados:</strong></p>
              <pre style="background:#fff9c4; padding:10px; border-radius:10px; font-size:13px;">${JSON.stringify(
            request,
            null,
            2
          )}</pre>
            </div>
          `,
          width: 600,
          customClass: {
            popup: 'swal2-rounded',
          },
          confirmButtonText: 'Fechar',
          buttonsStyling: true,
        });
      },
      complete: () => this.loading.set(false),
    });
  }

  isRegistroInvalido(item: SppBimerInvoceMonitorModel): boolean {
    const chave = item.pedidoSpp || item.numeroAlterData;
    return this.registrosInvalidos().has(chave);
  }

  isReprocessadoComSucesso(item: SppBimerInvoceMonitorModel): boolean {
    const chave = item.pedidoSpp || item.numeroAlterData;
    return this.reprocessadosComSucesso().has(chave);
  }

  mostrarInvoice(item: SppBimerInvoceMonitorModel): string {
    return (
      item.pedidoSpp?.trim() ||
      (item.fabricante.toLowerCase().includes('red hot')
        ? item.numeroAlterData
        : '')
    );
  }

  limparFiltros(): void {
    this.filtroGeral.setValue('');
    this.dataInicio.setValue(null);
    this.dataFim.setValue(null);
    this.buscar();
  }

  obterMensagemMapeada(mensagemOriginal: string | null | undefined): string {
    const original = mensagemOriginal ?? '';
    const mapping = this.mapeamentos.find(
      (m) => m.padrao === original && m.ativo
    );
    return mapping ? mapping.mensagem : original;
  }

  private showLoadingAlert(): void {
    Swal.fire({
      icon: 'info',
      title: 'Aguarde...',
      text: 'Reprocessando...',
      allowOutsideClick: false,
      showConfirmButton: false,
    });
  }

  setupTour(): void {
    // seu código original da função tour aqui
  }

  startTour(): void {
    this.tour.start();
  }
}