import { Component } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';

import { TemplateModel } from '../../../../../../shared/models/integracao/AdobeHub/templante.model';
import { TemplateService } from '../../../../../../core/services/intregracoes/adobe-hub/templante.service';
import { GestaoIntegracaoModel } from '../../../../../../shared/models/anypoint/gestao-integracao.model';
import { IntegrationManagementService } from '../../../../../../core/services/anypoint/integration-management/integration-management.service';
import * as XLSX from 'xlsx-js-style';

@Component({
  standalone: true,
  selector: 'app-importar-csv-excel',
  templateUrl: './planinhas-importacao.component.html',
  styleUrls: ['./planinhas-importacao.component.css'],
  imports: [CommonModule]
})
export class PlaninhasImportacaoComponent {
  selectedFile: File | null = null;
  selectedTemplateName: string = '';
  templates: TemplateModel[] = [];
  templateDetectado?: TemplateModel;

  semTipos = false;
  mensagemSemTipos = 'Nenhum template foi encontrado.';
  private integracaoIdAdobeHub?: number;

  constructor(
    private router: Router,
    private templanteService: TemplateService,
    private integracaoService: IntegrationManagementService
  ) {
    this.carregarTemplates();
  }

  private carregarTemplates(): void {
    const nomeIntegracaoReferencia = 'ADOBE - HUB';

    this.integracaoService.get().subscribe({
      next: (todasIntegracoes: GestaoIntegracaoModel[]) => {
        const integracao = todasIntegracoes.find(i =>
          i.nome?.trim().toUpperCase() === nomeIntegracaoReferencia.toUpperCase()
        );

        if (!integracao) {
          this.integracaoIdAdobeHub = undefined;
          this.templates = [];
          this.semTipos = true;
          return;
        }

        this.integracaoIdAdobeHub = Number(integracao.id);

        this.templanteService.ObterListaTemplante().subscribe({
          next: (templates: TemplateModel[]) => {
            this.templates = templates.filter(t => t.tipoTemplate?.integracaoId === this.integracaoIdAdobeHub);
            this.semTipos = this.templates.length === 0;
          },
          error: () => {
            this.templates = [];
            this.semTipos = true;
          }
        });
      },
      error: () => {
        this.integracaoIdAdobeHub = undefined;
        this.templates = [];
        this.semTipos = true;
      }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const extension = file.name.split('.').pop()?.toLowerCase();
      const allowedExtensions = ['csv', 'xls', 'xlsx'];

      if (!extension || !allowedExtensions.includes(extension)) {
        Swal.fire('Erro!', 'Apenas arquivos CSV ou Excel são permitidos.', 'error');
        return;
      }

      this.selectedFile = file;

      const nomeSemExtensao = file.name.replace(/\.[^/.]+$/, '').toLowerCase();

      // Busca pelo nome, nomeAbreviado ou sigla
      const templateEncontrado = this.templates.find(t =>
        (t.tipoTemplate?.nomeCompleto || '').toLowerCase().includes(nomeSemExtensao) ||
        (t.tipoTemplate?.nomeAbreviado || '').toLowerCase().includes(nomeSemExtensao) ||
        (t.tipoTemplate?.sigla || '').toLowerCase().includes(nomeSemExtensao)
      );

      if (!templateEncontrado) {
        Swal.fire('Erro', 'Nenhum template compatível foi encontrado.', 'error');
        return;
      }

      this.templateDetectado = templateEncontrado;
      this.selectedTemplateName = templateEncontrado.tipoTemplate?.sigla ?? '';
    }
  }

  processFile(): void {
    if (this.semTipos) {
      Swal.fire('Atenção!', 'Não há templates cadastrados para continuar.', 'warning');
      return;
    }

    if (!this.selectedFile) {
      Swal.fire('Atenção!', 'Selecione um arquivo primeiro.', 'warning');
      return;
    }

    if (!this.templateDetectado) {
      Swal.fire('Erro', 'Template detectado inválido.', 'error');
      return;
    }

    const reader = new FileReader();

    reader.onload = (e: any) => {
      let jsonData: any[][] = [];

      if (this.selectedFile!.name.endsWith('.csv')) {
        const csvData = e.target.result;
        const workbook = XLSX.read(csvData, { type: 'binary' });
        const worksheet = workbook.Sheets[workbook.SheetNames[0]];
        jsonData = XLSX.utils.sheet_to_json(worksheet, { header: 1 }) as any[][];
      } else {
        const data = new Uint8Array(e.target.result);
        const workbook = XLSX.read(data, { type: 'array' });
        const worksheet = workbook.Sheets[workbook.SheetNames[0]];
        jsonData = XLSX.utils.sheet_to_json(worksheet, { header: 1 }) as any[][];
      }

      // Navega para a próxima rota com todos os dados do template completo
      this.router.navigate(['/Adobe/Planilhas/Visualizar/Importacao'], {
        state: {
          data: jsonData,
          selectedTemplate: this.selectedTemplateName,
          templateModel: this.templateDetectado,
          fileName: this.selectedFile?.name || 'arquivo_sem_nome.csv'
        }
      });
    };

    if (this.selectedFile!.name.endsWith('.csv')) {
      reader.readAsBinaryString(this.selectedFile!);
    } else {
      reader.readAsArrayBuffer(this.selectedFile!);
    }
  }

  clearFile(): void {
    this.selectedFile = null;
    this.selectedTemplateName = '';
    this.templateDetectado = undefined;
  }

  selectTemplate(sigla: string): void {
    this.selectedTemplateName = sigla;
  }


}
