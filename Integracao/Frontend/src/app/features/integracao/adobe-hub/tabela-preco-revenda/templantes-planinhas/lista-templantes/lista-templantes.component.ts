import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { TemplateService } from '../../../../../../core/services/intregracoes/adobe-hub/templante.service';
import { TemplateModel } from '../../../../../../shared/models/integracao/AdobeHub/templante.model';
import { AuthService } from '../../../../../anypoint/auth';
import { ListaCrudIntegracaoComponent } from '../../../../../../shared/components/integracao/lista-crud-importacao/lista-crud-integracao.component';
@Component({
  selector: 'app-lista-templantes',
  imports: [CommonModule, ListaCrudIntegracaoComponent],
  templateUrl: './lista-templantes.component.html'
})
export class ListaTemplantesComponent implements OnInit {

  model: TemplateModel[] = [];
  detailsExpanded: { [key: string]: boolean } = {};
  showExportButton = false;
  showAddProducts = false;

  constructor(
    private service: TemplateService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.service.ObterListaTemplante().subscribe(
      (data: TemplateModel[]) => {
        this.model = data;
      },
      (error: any) => {
        Swal.fire('Erro', 'Erro ao carregar as regras.', 'error');
      }
    );
  }

  add(): void {
    this.router.navigate(['/Adobe/Registrar/Novo/Templante']);
  }

  edit(regra: TemplateModel): void {
    this.router.navigate(['/Planilhas/Editar', regra.id]);
  }

  onDeleteWithJustification(event: { item: TemplateModel, justification: string }): void {

    const { item, justification } = event;

    const user = this.authService.getDecodedToken();

    const payload = {
      justificativa: justification,
      usuario: user.Name,
      dataHora: new Date().toISOString(),
      registrosExcluidos: [
        {
          id: item.id,
          nome: item.nome,
          qtdColunas: item.qtdColunas,
          linhaCabecalho: item.linhaCabecalho,
          colunaInicial: item.colunaInicial,
          arquivoBase: item.arquivoBase,
          observacaoDescricao: item.observacaoDescricao,
          colunasObrigatorias: item.colunasObrigatorias,
          dataCriacao: item.dataCriacao,
          dataEdicao: item.dataEdicao,
          tipoTemplate: item.tipoTemplate
        }
      ]
    };

    this.service.ExcluirTemplate(payload).subscribe({
      next: (res: any) => {
        this.carregarDadosNovamente();
        Swal.fire('Sucesso', res.message || 'Exclusão realizada com sucesso.', 'success');
      },
      error: (err: any) => {
        Swal.fire('Erro', 'Erro ao excluir a planilha.', 'error');
      }
    });
  }

  carregarDadosNovamente(): void {
    window.location.reload();
  }

  startTour(): void {
    Swal.fire('Tour', 'Aqui você pode implementar seu passo-a-passo.', 'info');
  }

  viewDoc(item: TemplateModel): void {
    if (!item || !item.nome) {
      Swal.fire('Erro', 'Documento não encontrado.', 'error');
      return;
    }

    const nomeSanitizado = item.nome.toLowerCase().replace(/ /g, '-'); // ou outro critério
    this.router.navigate(['/visualizar-doc', nomeSanitizado]);
  }

  toggleDetailsExpand(field: string): void {
    this.detailsExpanded[field] = !this.detailsExpanded[field];
  }
  isDetailsExpanded(field: string): boolean {
    return !!this.detailsExpanded[field];
  }

}