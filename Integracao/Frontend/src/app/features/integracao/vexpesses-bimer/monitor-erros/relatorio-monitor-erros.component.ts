// #region Imports
import {
  Component,
  ViewChild,
  AfterViewInit,
  OnInit
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {} from '@angular/common/http';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ClipboardModule, Clipboard } from '@angular/cdk/clipboard';
import { RelatoriosService } from '../../../../core/services/intregracoes/vexpenses-bimmer/relatorios.service';

// #endregion

@Component({
    selector: 'app-relatorio-monitor-erros',
    imports: [
        CommonModule,
        MatCardModule,
        MatTableModule,
        MatPaginatorModule,
        MatDialogModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        ClipboardModule
    ],
    templateUrl: './relatorio-monitor-erros.component.html',
    styleUrls: ['./relatorio-monitor-erros.component.css']
})
export class RelatorioMonitorErrosComponent implements OnInit, AfterViewInit {
  // #region Propriedades da Tabela
  displayedColumnsErros: string[] = ['dataHora', 'endpoint', 'payload', 'migradoParaSql', 'erro'];
  dataSourceErros = new MatTableDataSource<any>();
  errosPageNumber = 1;
  errosPageSize = 10;
  totalErrosItems = 0;
  isLoading = false;
  // #endregion

  // #region Modal Customizado
  erroSelecionado: string = '';
  modalErroAberto: boolean = false;
  // #endregion

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private relatoriosService: RelatoriosService,
    private clipboard: Clipboard
  ) { }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    this.dataSourceErros.paginator = this.paginator;
  }

  carregarErros(): void {
    this.isLoading = true;

    this.relatoriosService.getErros().subscribe({
      next: (response) => {
        this.dataSourceErros.data = response.items;
        this.totalErrosItems = response.totalCount;
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
      }
    });
  }

  onErrosPageChange(event: PageEvent): void {
    this.errosPageNumber = event.pageIndex + 1;
    this.errosPageSize = event.pageSize;
  }

  abrirErroCompleto(erro: string): void {
    this.erroSelecionado = erro;
    this.modalErroAberto = true;
  }

  fecharModalErro(): void {
    this.modalErroAberto = false;
    this.erroSelecionado = '';
  }

  copiarErro(): void {
    if (this.erroSelecionado) {
      const sucesso = this.clipboard.copy(this.erroSelecionado);
      if (sucesso) {
      } else {
      }
    }
  }

}
