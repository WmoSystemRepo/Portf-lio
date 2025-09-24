import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { SppBimerInvoceDeparamensagem } from '../../../../../shared/models/integracao/SppBimerInvoce/spp-bimerInvoce-deparamensagem.model';
import { MonitorDeparaMensagensService } from '../../../../../core/services/intregracoes/spp-bimer-invoce/depara-mensagens-erros.service';
import { MonitorSppBimerService } from '../../../../../core/services/intregracoes/spp-bimer-invoce/monitor-spp-bimer.service';
import { AuthService } from '../../../../../core/services/anypoint/auth/auth.service';
import { SppBimerInvoceMonitorModel } from '../../../../../shared/models/integracao/SppBimerInvoce/spp-bimerInvoce-monitor.model';

@Component({
  standalone: true,
  selector: 'app-depara-mensagens-erro',
  templateUrl: './depara-mensagens-erro.component.html',
  styleUrls: ['./depara-mensagens-erro.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
})
export class DeparaMensagensErroComponent implements OnInit {
  //#region Propriedades
  model: SppBimerInvoceDeparamensagem[] = [];

  // objeto em edição
  current: SppBimerInvoceDeparamensagem = {
    mensagemPadrao: '',
    mensagemModificada: '',
    ativo: true,
    usuarioCadastro: '',
    dataCriacao: new Date(),
  };

  colunas = ['padrao', 'mensagem', 'ativo', 'acoes'];

  mensagensUnicas: string[] = [];
  mensagensColumns = ['mensagem', 'acao'];
  loadingMensagens = false;

  isDeleteModalOpen = false;
  itemToDelete: any = null;

  //#endregion

  //#region Construtor
  constructor(
    private deparaService: MonitorDeparaMensagensService,
    private route: ActivatedRoute,
    private monitorService: MonitorSppBimerService,
    private authService: AuthService
  ) {}
  //#endregion

  //#region Ciclo de vida
  ngOnInit(): void {
    this.load();
    const msg = this.route.snapshot.queryParamMap.get('fromMessage');
    if (msg) this.current.mensagemModificada = msg;
    this.carregarMensagensDoMonitor();
  }
  //#endregion

  //#region Métodos
  load(): void {
    this.deparaService.listar().subscribe({
      next: (data) => {
        this.model = data;
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao carregar os mapeamentos.', 'error');
      },
    });
  }

  private carregarMensagensDoMonitor(): void {
    this.loadingMensagens = true;
    this.monitorService
      .getMonitoramentos(undefined, undefined, undefined)
      .subscribe({
        next: (res: SppBimerInvoceMonitorModel[]) => {
          const unicas = Array.from(
            new Set(
              (res ?? [])
                .map((r) => (r.observacaoErro ?? '').trim())
                .filter((m) => m.length > 0)
            )
          ).sort((a, b) => a.localeCompare(b, 'pt-BR'));
          this.mensagensUnicas = unicas;
        },
        error: () => (this.mensagensUnicas = []),
        complete: () => (this.loadingMensagens = false),
      });
  }

  selecionarMensagem(mensagem: string): void {
    this.current.mensagemPadrao = mensagem;
  }

  salvarNovo(): void {
    const user = this.authService.getDecodedToken();
    const id = (this.current as any).id;

    const dto: SppBimerInvoceDeparamensagem = {
      ...this.current,
      usuarioCadastro: this.current.usuarioCadastro || user.Name,
      usuarioEdicao: id ? user.Name : undefined,
      dataCriacao: this.current.dataCriacao || new Date(),
      dataEdicao: id ? new Date() : undefined,
    }; 

    if (id) {
      this.deparaService.atualizar(id, dto).subscribe({
        next: () => {
          this.load();
          this.reset();
          Swal.fire('Sucesso', 'Mapeamento de mensagem atualizado com êxito.', 'success');
        },
        error: () => {
          Swal.fire('Erro', 'Erro ao atualizar o mapeamento de mensagem.', 'error');
        },
      },
    );
    } else {
      this.deparaService.criar(dto).subscribe({
        next: () => {
          this.load();
          this.reset();
          Swal.fire('Sucesso', 'Mapeamento de mensagem criado com êxito.', 'success');
        },
        error: () => {
          Swal.fire('Erro', 'Erro ao tentar criar o mapeamento de mensagem.', 'error');
        },
      });
    }
  }

  editar(item: SppBimerInvoceDeparamensagem): void {
    this.current = { ...item };
  }

  remover(item: SppBimerInvoceDeparamensagem): void {
    this.isDeleteModalOpen = true;
    this.itemToDelete = item;
  }

  confirmDelete(): void {
    const id = this.itemToDelete.id;
    if (!id) return;

    this.deparaService.excluir(id).subscribe({
      next: () => {
        this.load();
        Swal.fire('Sucesso', 'Mapeamento de mensagem excluído com êxito.', 'success');
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao excluir o mapeamento.', 'error');
      },
    });

    this.closeDeleteModal();
  }

  closeDeleteModal(): void {
    this.isDeleteModalOpen = false;
  }

  reset(): void {
    this.current = {
      mensagemPadrao: '',
      mensagemModificada: '',
      ativo: true,
      usuarioCadastro: '',
      dataCriacao: new Date(),
    };
  }

  startTour() {}
  //#endregion

  // total de mensagens únicas
  get totalMensagensUnicas(): number {
    return this.mensagensUnicas.length;
  }

  // total de mensagens com de/para configurado
  get totalMensagensComDepara(): number {
    return this.mensagensUnicas.filter(msg =>
      this.model.some(i =>
        msg.toLowerCase().includes((i.mensagemPadrao ?? '').toLowerCase())
      )
    ).length;
  }

  // total de de/para ativos
  get totalDeparasAtivos(): number {
    return this.model.filter(i => i.ativo).length;
  }

  // total de de/para inativos
  get totalDeparasInativos(): number {
    return this.model.filter(i => !i.ativo).length;
  }
}