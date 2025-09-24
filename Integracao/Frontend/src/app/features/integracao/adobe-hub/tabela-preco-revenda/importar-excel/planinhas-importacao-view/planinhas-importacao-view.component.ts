import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../../anypoint/auth';
import { PlaninhaImportadasService } from '../../../../../../core/services/intregracoes/adobe-hub/planinha-importadas.service';
import { PlanilhasImportadasModel } from '../../../../../../shared/models/integracao/AdobeHub/planilhas-importadas.model';
import { TemplateModel } from '../../../../../../shared/models/integracao/AdobeHub/templante.model';
import * as XLSX from 'xlsx-js-style';

@Component({
  standalone: true,
  selector: 'app-preview',
  templateUrl: './planinhas-importacao-view.component.html',
  styleUrls: ['./planinhas-importacao-view.component.css'],
  imports: [CommonModule]
})
export class PlaninhasImportacaoViewComponent implements OnInit {

  data: any[][] = [];
  selectedTemplateName: string = '';
  headerIndex: number | null = null;

  selectedRow: number | null = null;
  selectedCol: number | null = null;
  invalidCellMessage: string | null = null;

  private router = inject(Router);
  private location = inject(Location);
  private panilhaService = inject(PlaninhaImportadasService);
  private authService = inject(AuthService);

  templateSelecionado: string | undefined;
  nomeArquivo: string | undefined;
  versao: number | null | undefined;

  templateCompleto: TemplateModel | undefined;

  ngOnInit(): void {
    const state = history.state;

    console.log('📥 State recebido:', state);
    console.log('📄 Template completo:', state.templateModel);

    const originalData: any[][] = state.data || [];

    this.templateCompleto = state.templateModel;

    // Garantir que templateCompleto existe
    if (!this.templateCompleto) {
      Swal.fire('Erro', 'O modelo de template não foi enviado corretamente.', 'error')
        .then(() => this.router.navigate(['/templates/import-excel']));
      return;
    }

    this.templateSelecionado = this.templateCompleto?.tipoTemplate?.sigla ?? '';
    this.selectedTemplateName = this.templateSelecionado;
    this.nomeArquivo = state.fileName;
    this.versao = state.versao;

    // Validar dados do template
    const { qtdColunas, linhaCabecalho, colunaInicial } = this.templateCompleto;

    if (
      typeof qtdColunas !== 'number' || qtdColunas <= 0 ||
      typeof linhaCabecalho !== 'number' || linhaCabecalho <= 0 ||
      typeof colunaInicial !== 'string' || colunaInicial.trim() === ''
    ) {
      console.error('❌ Dados inválidos no template:', {
        qtdColunas,
        linhaCabecalho,
        colunaInicial,
        templateCompleto: this.templateCompleto
      });

      Swal.fire(
        'Erro no Template',
        'O template está com dados inválidos: verifique qtdColunas, linhaCabecalho ou colunaInicial.',
        'error'
      ).then(() => this.router.navigate(['/templates/import-excel']));
      return;
    }

    // Validar conteúdo da planilha
    if (originalData.length === 0) {
      Swal.fire('Nenhum dado encontrado!', 'Você precisa importar um arquivo primeiro.', 'warning')
        .then(() => this.router.navigate(['/templates/import-excel']));
      return;
    }

    const maxColumns = Math.max(...originalData.map(r => r.length));
    if (maxColumns !== qtdColunas) {
      Swal.fire({
        icon: 'warning',
        title: 'Inconsistência Detectada',
        html: `
        <p>O número de colunas no arquivo importado não corresponde ao template selecionado.</p>
        <p><strong>Esperado:</strong> ${qtdColunas} colunas<br>
        <strong>Encontrado:</strong> ${maxColumns} colunas</p>
      `
      }).then(() => {
        this.router.navigate(['/templates/import-excel']);
      });
      return;
    }

    const colunaInicialIndex = this.convertExcelColumnToIndex(colunaInicial);

    const slicedData = originalData.slice(linhaCabecalho - 1).map(row => {
      const filled = [...row];
      while (filled.length < qtdColunas) filled.push('');
      return filled.slice(colunaInicialIndex, colunaInicialIndex + qtdColunas);
    });

    this.data = slicedData;
    this.headerIndex = 0;
  }

  private convertExcelColumnToIndex(col: string): number {
    let index = 0;
    for (let i = 0; i < col.length; i++) {
      index *= 26;
      index += col.charCodeAt(i) - 64;
    }
    return index - 1;
  }

  markAsHeader(index: number): void {
    const cleaned = this.data.slice(index);
    const maxColumns = Math.max(...cleaned.map(r => r.length));
    this.data = cleaned.map(row => {
      const filled = [...row];
      while (filled.length < maxColumns) filled.push('');
      return filled;
    });
    this.headerIndex = 0;
  }

  salvarNoMongo(): void {
    const dadosExtraidos = this.extrairDadosDaTabela();

    const usuarioLogado =
      this.authService?.getDecodedToken?.()?.Name ??
      this.authService?.getDecodedToken?.()?.name ??
      this.authService?.getDecodedToken?.()?.email ??
      'Desconhecido';

    if (!this.templateCompleto) {
      Swal.fire('Erro', 'Template não carregado corretamente.', 'error');
      return;
    }

    const planilha: PlanilhasImportadasModel = {
      id: '',
      nomeArquivo: this.nomeArquivo!,
      dataUpload: new Date(),
      dados: dadosExtraidos,
      usuario: usuarioLogado,
      versao: this.versao ?? 1,
      template: this.templateCompleto!
    };

    if (
      !planilha.nomeArquivo ||
      !Array.isArray(planilha.dados) ||
      planilha.dados.length === 0 ||
      typeof planilha.dados[0] !== 'object' ||
      planilha.dados[0] === null
    ) {
      console.error('❌ Payload inválido:', planilha);
      this.mostrarModalErro();
      return;
    }

    this.panilhaService.SalvarNovaPlanilhaImportada(planilha).subscribe({
      next: () => this.mostrarModalSucesso(),
      error: (err) => {
        console.error('Erro ao salvar planilha no Mongo:', err);
        this.mostrarModalErro();
      }
    });
  }

  extrairDadosDaTabela(): any[] {
    if (!this.data || this.data.length < 2) return [];

    const header = this.data[0];
    const registros: any[] = [];

    for (let i = 1; i < this.data.length; i++) {
      const linha = this.data[i];
      const obj: any = {};

      header.forEach((coluna, index) => {
        const valor = linha[index];
        obj[coluna?.toString().trim()] = valor !== undefined && valor !== null ? String(valor) : '';
      });

      registros.push(obj);
    }

    return registros;
  }

  mostrarModalErro(): void {
    Swal.fire({
      icon: 'error',
      title: 'Erro',
      text: 'Erro ao salvar a planilha no Mongo.',
      confirmButtonColor: '#d33',
      confirmButtonText: 'OK'
    });
  }

  mostrarModalSucesso(): void {
    Swal.fire({
      icon: 'success',
      title: 'Sucesso',
      text: 'A planilha foi salva com sucesso no Mongo.',
      confirmButtonColor: '#3085d6',
      confirmButtonText: 'OK'
    }).then(result => {
      if (result.isConfirmed) {
        this.router.navigate(['/Lista/Planilhas/Importadas']);
      }
    });
  }

  downloadExcel(): void {
    const ws = XLSX.utils.aoa_to_sheet(this.data);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    const originalName = history.state?.fileName || 'dados_convertidos.csv';
    const baseName = originalName.replace(/\.[^/.]+$/, '');

    const today = new Date();
    const day = String(today.getDate()).padStart(2, '0');
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const year = today.getFullYear();
    const formattedDate = `${day}-${month}-${year}`;

    const finalName = `${baseName}_${formattedDate}.xlsx`;
    XLSX.writeFile(wb, finalName);
  }

  goBack(): void {
    this.location.back();
  }

  addEmptyRow(): void {
    if (this.data.length === 0) return;
    const columnCount = this.data[0].length;
    this.data.push(Array(columnCount).fill(''));
  }

  addEmptyColumn(): void {
    this.data = this.data.map(row => [...row, '']);
  }

  getColumnLetter(index: number): string {
    let result = '';
    let current = index;
    while (current >= 0) {
      result = String.fromCharCode((current % 26) + 65) + result;
      current = Math.floor(current / 26) - 1;
    }
    return result;
  }

  selectCell(row: number, col: number): void {
    this.selectedRow = row;
    this.selectedCol = col;
    this.invalidCellMessage = null;
  }

  isSelected(row: number, col: number): boolean {
    return this.selectedRow === row && this.selectedCol === col;
  }

  isInvalidCell(row: number, col: number): boolean {
    return !this.data[row][col] || this.data[row][col].toString().trim() === '';
  }

  isMergedCell(row: number, col: number): boolean {
    return false;
  }

  proceedToCreate(): void {
    if (this.selectedRow === null || this.selectedCol === null) {
      this.invalidCellMessage = 'Você precisa selecionar uma célula para continuar.';
      return;
    }

    if (this.isInvalidCell(this.selectedRow, this.selectedCol)) {
      this.invalidCellMessage = 'A célula selecionada é inválida.';
      return;
    }

    Swal.fire('👍 Cabeçalho confirmado', `Você selecionou a célula 
      ${this.getColumnLetter(this.selectedCol)}
      ${this.selectedRow + 1}`, 'success');
  }
}
