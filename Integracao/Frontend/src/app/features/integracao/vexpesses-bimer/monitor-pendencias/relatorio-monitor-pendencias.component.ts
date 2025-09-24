import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild
} from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PendenciasVexpenseModel } from '../../../../shared/models/anypoint/pendencias-vexpense.model';
import { IntegracaoVexpensePendenciasService } from '../../../../core/services/intregracoes/vexpenses-bimmer/integracao-vexpense-pendencias.service';
import { AuthService } from '../../../anypoint/auth';
import { ExclusaoPendenciasModel } from '../../../../shared/models/integracao/VexpesssesBimer/ExclusaoPendencias.model';

interface GetRelatoriosParams {
  pageNumber: number;
  pageSize: number;
  status?: string;
  search?: string;
}

@Component({
    selector: 'app-relatorio-monitor-pendencias',
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatTableModule,
        MatPaginatorModule,
        MatTabsModule,
        MatDialogModule,
        MatCheckboxModule,
        MatIconModule,
        MatButtonModule,
        MatTooltipModule,
        MatProgressSpinnerModule
    ],
    templateUrl: './relatorio-monitor-pendencias.component.html',
    styleUrls: ['./relatorio-monitor-pendencias.component.css']
})
export class RelatorioMonitorPendenciasComponent implements OnInit, AfterViewInit {
  filterValues = { text: '', status: '' };
  currentTabIndex = 0;

  displayedColumnsPendenciasMoeda: string[] = [
    'select',
    'id',
    'dataCadastro',
    'status',
    'observacao',
    'acoes'
  ];

  dataSourcePendenciasMoeda = new MatTableDataSource<PendenciasVexpenseModel>();
  selection = new SelectionModel<PendenciasVexpenseModel>(true, []);
  pageSizePendenciasMoeda = 10;
  pageNumberPendenciasMoeda = 1;
  totalItensPendenciasMoeda = 0;

  @ViewChild('pendenciasMoedaPaginator') pendenciasMoedaPaginator!: MatPaginator;

  exibindoModalJustificativa = false;
  exibindoModalSucesso = false;
  mensagemSucesso = '';
  formJustificativa: FormGroup;

  modalAberto = false;
  pendenciaSelecionada: PendenciasVexpenseModel | null = null;

  constructor(
    private pendenciasService: IntegracaoVexpensePendenciasService,
    private fb: FormBuilder,
    private authService: AuthService
  ) {
    this.formJustificativa = this.fb.group({
      justificativa: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  ngOnInit(): void {
    this.loadTodasPendencias();
  }

  ngAfterViewInit(): void {
    this.dataSourcePendenciasMoeda.paginator = this.pendenciasMoedaPaginator;
  }

  onTabChange(index: number): void {
    this.currentTabIndex = index;
    if (index === 0) this.loadTodasPendencias();
  }

  applyFilterPendenciasTodas(event: Event): void {
    this.filterValues.text = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.pageNumberPendenciasMoeda = 1;
    this.loadTodasPendencias();
  }

  filterByStatus(event: Event): void {
    this.filterValues.status = (event.target as HTMLSelectElement).value;
    this.pageNumberPendenciasMoeda = 1;
    this.loadTodasPendencias();
  }

  onPagePendenciasMoedaChange(event: PageEvent): void {
    this.pageNumberPendenciasMoeda = event.pageIndex + 1;
    this.pageSizePendenciasMoeda = event.pageSize;
    this.loadTodasPendencias();
  }

  loadTodasPendencias(): void {
    const params: GetRelatoriosParams = {
      pageNumber: this.pageNumberPendenciasMoeda,
      pageSize: this.pageSizePendenciasMoeda,
      status: this.filterValues.status,
      search: this.filterValues.text
    };

    this.pendenciasService.getPendencias(params).subscribe({
      next: (res: PendenciasVexpenseModel[]) => {
        this.dataSourcePendenciasMoeda.data = res;
        this.totalItensPendenciasMoeda = res.length;
        this.selection.clear();
      },
      error: (err) => {
        console.error('Erro ao carregar pendências:', err);
      }
    });
  }

  isAllSelected(): boolean {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSourcePendenciasMoeda.data.length;
    return numSelected === numRows;
  }

  masterToggle(): void {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSourcePendenciasMoeda.data.forEach(row => this.selection.select(row));
  }

  checkboxLabel(row?: PendenciasVexpenseModel): string {
    if (!row) return `${this.isAllSelected() ? 'Desmarcar' : 'Marcar'} todos`;
    return `${this.selection.isSelected(row) ? 'Desmarcar' : 'Marcar'} item ${row.idResponse}`;
  }

  abrirModalJustificativa(): void {
    this.formJustificativa.reset();
    this.exibindoModalJustificativa = true;
  }

  fecharModalJustificativa(): void {
    this.exibindoModalJustificativa = false;
  }

  exibirModalSucesso(mensagem: string): void {
    this.mensagemSucesso = mensagem;
    this.exibindoModalSucesso = true;
  }

  fecharModalSucesso(): void {
    this.exibindoModalSucesso = false;
  }

  confirmarExclusao(): void {
    if (this.formJustificativa.valid) {
      const user = this.authService.getDecodedToken();

      const payload: ExclusaoPendenciasModel = {
        justificativa: this.formJustificativa.value.justificativa,
        usuario: user.Name,
        dataHora: new Date().toISOString(),
        registrosExcluidos: this.selection.selected.map(x => ({
          idResponse: Number(x.idResponse),
          userId: Number(x.userId),
          descricao: x.descricao,
          valor: Number(x.valor),
          dataCadastro: x.dataCadastro,
          status: x.status,
          observacao: x.observacao,
          response: x.response
        }))
      };

      this.pendenciasService.excluirPendencias(payload).subscribe({
        next: (res) => {
          this.selection.clear();
          this.carregarPendencias();
          this.fecharModalJustificativa();
          this.exibirModalSucesso(`✔️ ${res.quantidade} pendência(s) excluída(s) com sucesso.`);
        },
        error: () => {
          this.exibirModalSucesso('❌ Erro ao excluir as pendências.');
        }
      });
    }
  }

  carregarPendencias(): void {
    this.loadTodasPendencias();
  }

  iniciarTourRelatorioMoeda() {
    // Placeholder
  }

  abrirDetalhesPendencia(pendencia: PendenciasVexpenseModel): void {
    this.pendenciaSelecionada = pendencia;
    this.modalAberto = true;
  }

  fecharDetalhesPendencia(): void {
    this.modalAberto = false;
    this.pendenciaSelecionada = null;
  }
}
