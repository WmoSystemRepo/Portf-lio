import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import Swal from 'sweetalert2';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RelatoriosService } from '../../../../core/services/intregracoes/vexpenses-bimmer/relatorios.service';
import { AuthService } from '../../../anypoint/auth';
import { RelatorioStatusCardsVexpensesComponent } from '../relatorios/relatorio-status-cards-vexpenses/relatorio-status-cards-vexpenses.component';

// Adicione esta interface
interface GetRelatoriosParams {
  pageNumber: number;
  pageSize: number;
  status?: string;
  search?: string;
}
@Component({
    selector: 'app-relatorio-monitor-integracao',
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
        MatProgressSpinnerModule,
        RelatorioStatusCardsVexpensesComponent,
    ],
    templateUrl: './relatorio-monitor-integracao.component.html',
    styleUrls: ['./relatorio-monitor-integracao.component.css']
})

export class RelatorioMonitorIntegracaoComponent implements OnInit, AfterViewInit {

  iniciarTourRelatorioVexpense() {
    throw new Error('Method not implemented.');
  }

  relatorios: any[] = [];
  totalItems = 0;
  pageSize = 10;
  pageNumber = 1;
  dataSource = new MatTableDataSource<any>([]);

  currentTabIndex = 0;

  // Contagens para os cards
  totalGeral: number = 0;
  totalPago: number = 0;
  totalAprovados: number = 0;
  totalAbertos: number = 0;
  totalReaberto: number = 0;
  totalReprovado: number = 0;
  totalEnviado: number = 0;
  selectedFilter: string | null = null;

  statusList: string[] = ['Aprovado', 'Reaberto', 'Pago', 'Aberto', 'Reprovado', 'Enviado', 'Pendente', 'Erro'];
  filterValues = { text: '', status: '' };

  selectedIds: number[] = [];

  @ViewChild('sucessosPaginator') sucessosPaginator!: MatPaginator;

  constructor(private relatoriosService: RelatoriosService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.loadCounts();
    this.loadRelatorios();

  }

  ngAfterViewInit() {
    this.dataSource.filterPredicate = (data: any, filter: string) => {
      const filterObj = JSON.parse(filter);
      const matchesText = filterObj.text ?
        data.description.toLowerCase().includes(filterObj.text) : true;
      const matchesStatus = filterObj.status ?
        data.status.toLowerCase() === filterObj.status.toLowerCase() : true;
      return matchesText && matchesStatus;
    };
  }

  // Método para carregar contagens dos cards
  loadCounts() {
    this.relatoriosService.getReportCounts().subscribe({
      next: (counts) => {
        this.totalGeral = counts.totalGeral;
        this.totalPago = counts.totalPago;
        this.totalAprovados = counts.totalAprovados;
        this.totalReprovado = counts.totalReprovados;
        this.totalReaberto = counts.totalReabertos;
        this.totalEnviado = counts.totalEnviados;
        this.totalAbertos = counts.totalAbertos;
      },
      error: (err) => console.error('Erro ao carregar contagens:', err)
    });
  }

  // Filtragem por cards
  filterByCard(status: string | null) {
    this.selectedFilter = status;
    this.filterValues.status = status || '';
    this.pageNumber = 1;
    this.loadRelatorios();
  }

  // Carregar relatórios com filtros
  loadRelatorios() {
    
    const params: GetRelatoriosParams = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      status: this.filterValues.status,
      search: this.filterValues.text
    };

    this.relatoriosService.getRelatorios(params).subscribe({
      next: (data) => {
        this.relatorios = data.reports;
        this.totalItems = data.totalItems;
        this.dataSource.data = this.relatorios;

        // Forçar atualização do paginator
        if (this.dataSource.paginator) {
          this.dataSource.paginator.firstPage();
        }
      },
      error: (err) => console.error('Erro ao carregar relatórios:', err)
    });
  }

  // Filtro por texto
  applyFilter(event: Event) {
    this.filterValues.text = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.pageNumber = 1;
    this.loadRelatorios();
  }

  // Filtro por select de status
  filterByStatus(event: Event) {
    this.filterValues.status = (event.target as HTMLSelectElement).value;
    this.pageNumber = 1;
  }

  // Paginação
  onPageChange(event: PageEvent) {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadRelatorios();
  }

  onRowClick(relatorio: any): void {
  }

  // Novos métodos de seleção
  toggleSelection(id: number) {
    const index = this.selectedIds.indexOf(id);
    index === -1 ? this.selectedIds.push(id) : this.selectedIds.splice(index, 1);
  }

  isSelected(id: number): boolean {
    return this.selectedIds.includes(id);
  }

  allSelected(): boolean {
    const aprovados = this.relatorios.filter(r => r.status === 'APROVADO');
    return aprovados.length > 0 && aprovados.every(r => this.selectedIds.includes(r.idResponse));
  }

  someSelected(): boolean {
    const aprovados = this.relatorios.filter(r => r.status === 'APROVADO');
    return this.selectedIds.some(id =>
      aprovados.map(r => r.idResponse).includes(id)
    ) && !this.allSelected();
  }

  toggleAll(event: MatCheckboxChange) {
    const aprovados = this.relatorios.filter(r => r.status === 'APROVADO');

    if (event.checked) {
      this.selectedIds = aprovados.map(r => r.idResponse);
    } else {
      this.selectedIds = [];
    }
  }

  async submitSelected() {
    if (this.selectedIds.length === 0) return;

    try {

      this.showLoadingAlert();
      const result = await this.relatoriosService.enviarSelecionados(this.selectedIds).toPromise();
      const hasFailures = result.some((item: any) => !item.integrado);

      this.selectedIds = [];

      Swal.fire({
        icon: hasFailures ? 'warning' : 'success',
        title: hasFailures ? 'Sucesso parcial!' : 'Sucesso total!',
        html: `
            <h4>${this.selectedIds.length} títulos processados</h4>
            <div style="text-align: left; margin-top: 20px; max-height: 300px; overflow-y: auto;">
              ${this.createResultList(result)}
            </div>
          `,
        confirmButtonText: 'Fechar',
        width: '800px',
        scrollbarPadding: false
      }).then(() => {
        this.loadRelatorios();
        window.location.reload();
      });
    } catch (error) {
      this.showErrorAlert(error);
    }
  }

  private createResultList(result: any[]): string {
    const successes = result.filter(item => item.integrado);
    const failures = result.filter(item => !item.integrado);

    return `
      <div class="result-section">
        ${successes.length > 0 ? `
          <p class="success-title">✅ Integrados com sucesso (${successes.length}):</p>
          <ul class="success-list">
            ${successes.map(item => `
              <li>${item.id} - ${item.descricao}</li>
            `).join('')}
          </ul>
        ` : ''}

        ${failures.length > 0 ? `
          <p class="error-title">❌ Não integrados (${failures.length}):</p>
          <ul class="error-list">
            ${failures.map(item => `
              <li>${item.id} ${item.mensagem ? `<div class="error-detail">Motivo: ${item.descricao}</div>` : ''}</li>
            `).join('')}
          </ul>
        ` : ''}
      </div>
    `;
  }

  private showErrorAlert(error: any): void {
    Swal.fire({
      icon: 'error',
      title: 'Falha na operação!',
      html: `
        <div style="text-align: left;">
          <p>${error?.error || 'Erro desconhecido'}</p>
          ${error?.mensagem ? `
            <details>
              <summary>Detalhes técnicos</summary>
              <pre style="white-space: pre-wrap;">${error.mensagem}</pre>
            </details>
          ` : ''}
        </div>
      `,
      confirmButtonText: 'Entendi',
      width: '600px'
    }).then(() => {
      window.location.reload();
    });
  }

  private showLoadingAlert(): void {
    Swal.fire({
      icon: 'info',
      title: 'Aguarde...',
      text: 'Enviando títulos...',
      allowOutsideClick: false,
      showConfirmButton: false
    });
  }

  checkIfAdmin(): boolean {
    const userRole = this.auth.getUserInfo();
    const role = userRole.EhAdmin;
    if (role === 'True') return true;
    else return false;
  }

  modalAberto = false;
  relatorioSelecionado: any = null;

  openDetails(relatorio: any): void {
    this.relatorioSelecionado = relatorio;
    this.modalAberto = true;
  }

  fecharModal(): void {
    this.modalAberto = false;
    this.relatorioSelecionado = null;
  }
}
