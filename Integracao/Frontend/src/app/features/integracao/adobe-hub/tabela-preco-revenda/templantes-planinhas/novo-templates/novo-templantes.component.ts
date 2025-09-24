import {
  Component,
  OnInit,
  HostListener,
  ElementRef,
  AfterViewInit
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TemplateService } from '../../../../../../core/services/intregracoes/adobe-hub/templante.service';
import { TemplateModel } from '../../../../../../shared/models/integracao/AdobeHub/templante.model';
import { TipoTemplate } from '../../../../../../shared/models/anypoint/tipo-template.model';

@Component({
  selector: 'app-novo-templantes',
  templateUrl: './novo-templantes.component.html',
  styleUrls: ['./novo-templantes.component.css'],
  imports: [CommonModule, FormsModule]
})
export class NovoTemplantesComponent implements OnInit, AfterViewInit {
  headers: string[] = [];
  templateModel!: TemplateModel;
  nomeTemplate = '';
  mappings: any[] = [];
  isSaving = false;
  templateId: string | null = null;
  originalData: any[][] = [];
  originalFileName: string = '';
  columnsPerRow = 5;

  // 🔵 Agora o tipo completo
  tipoSelecionado?: TipoTemplate;

  constructor(
    private templateService: TemplateService,
    private router: Router,
    private route: ActivatedRoute,
    private el: ElementRef
  ) { }

  ngOnInit() {
    const state = history.state as any;

    if (state.templateModel) {
      this.templateModel = state.templateModel;
      this.headers = Object.values(this.templateModel.colunas);
      this.nomeTemplate = this.templateModel.nome;
      this.originalData = state.data || [];
      this.originalFileName = this.templateModel.arquivoBase ?? '';
      this.templateId = this.templateModel.id || null;

      // 🔵 Recebe tipo de template completo
      this.tipoSelecionado = state.tipoTemplate;

      this.mappings = Object.entries(this.templateModel.colunas).map(
        ([letraColunaPlanilha, colunaPlanilha]) => ({
          letraColunaPlanilha,
          colunaPlanilha
        })
      );
      return;
    }

    this.headers = state.headers || [];
    this.nomeTemplate = state.nomeTemplate || '';
    this.originalData = state.data || [];
    this.originalFileName = state.fileName || '';
    this.templateId = this.route.snapshot.paramMap.get('templateId');

    // 🔵 Recebe tipo de template completo
    this.tipoSelecionado = state.tipoTemplate;

    if (this.templateId) {
      this.loadTemplate(this.templateId);
    } else {
      this.mappings = this.headers.map((header: string, index: number) => ({
        colunaPlanilha: header,
        letraColunaPlanilha: this.getExcelColumnName(index)
      }));
    }
  }

  ngAfterViewInit() {
    this.calculateColumnsPerRow();
  }

  @HostListener('window:resize')
  onResize() {
    this.calculateColumnsPerRow();
  }

  calculateColumnsPerRow() {
    const container = this.el.nativeElement.querySelector('.mapping-cards');
    if (container) {
      const containerWidth = container.offsetWidth;
      const cardWidth = 240;
      this.columnsPerRow = Math.floor(containerWidth / cardWidth) || 1;
    }
  }

  getExcelColumnName(index: number): string {
    let result = '';
    while (index >= 0) {
      result = String.fromCharCode((index % 26) + 65) + result;
      index = Math.floor(index / 26) - 1;
    }
    return result;
  }

  getRowIndex(index: number): number {
    return Math.floor(index / this.columnsPerRow);
  }

  loadTemplate(templateId: string) {
    this.templateService.getTemplateById(templateId).subscribe((template: any) => {
      if (template) {
        this.nomeTemplate = template.nomeTemplate;
        this.tipoSelecionado = template.tipo;
        this.mappings = template.deParaMappings;
      }
    });
  }

  async saveTemplate() {
    this.isSaving = true;

    try {
      const colunas: Record<string, string> = {};
      for (const m of this.mappings) {
        colunas[m.letraColunaPlanilha] = m.colunaPlanilha;
      }

      const payload: any = {
        ...this.templateModel,
        nome: this.nomeTemplate,
        colunas,
        qtdColunas: this.mappings.length,
        dataEdicao: new Date().toISOString(),
        dataCriacao: this.templateId ? this.templateModel.dataCriacao : new Date().toISOString(),

        // ✅ Corrigir o nome aqui
        tipoTemplate: {
          id: this.tipoSelecionado?.id,
          nomeCompleto: this.tipoSelecionado?.nomeCompleto,
          nomeAbreviado: this.tipoSelecionado?.nomeAbreviado,
          sigla: this.tipoSelecionado?.sigla,
          integracaoId: this.tipoSelecionado?.integracaoId
        }
      };

      console.log('Payload enviado ao backend:', payload);

      if (this.templateId) {
        await this.templateService.updateTemplate(this.templateId, payload).toPromise();
        const result = await Swal.fire('Sucesso!', 'Template atualizado com sucesso!', 'success');
        if (result.isConfirmed) {
          this.router.navigate(['/Adobe/Templantes/Planinhas/Lista']);
        }
      } else {
        await this.templateService.NovoTemplante(payload).toPromise();
        await Swal.fire('Sucesso!', 'Template criado com sucesso!', 'success');
        this.router.navigate(['/Adobe/Templantes/Planinhas/Lista']);
      }

    } catch (error: any) {
      console.error('Erro ao salvar template:', error);
      Swal.fire('Erro!', error?.message || 'Falha ao salvar o template.', 'error');
    } finally {
      this.isSaving = false;
    }
  }

  cancel() {
    Swal.fire({
      title: 'Cancelar alterações?',
      text: 'Tudo que não foi salvo será perdido.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim, voltar',
      cancelButtonText: 'Ficar'
    }).then(result => {
      if (result.isConfirmed) {
        this.router.navigate(['/templates/preview-template-create'], {
          state: {
            data: this.originalData,
            fileName: this.originalFileName,
            selectedTemplate: this.nomeTemplate
          }
        });
      }
    });
  }

  removeMapping(index: number) {
    Swal.fire({
      title: 'Remover cabeçalho?',
      text: 'Deseja realmente excluir este campo?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não'
    }).then(result => {
      if (result.isConfirmed) {
        this.mappings.splice(index, 1);
      }
    });
  }
}
