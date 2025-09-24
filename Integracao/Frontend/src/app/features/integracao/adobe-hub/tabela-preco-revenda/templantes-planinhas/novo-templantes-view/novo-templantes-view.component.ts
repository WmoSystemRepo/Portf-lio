import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { TemplateService } from '../../../../../../core/services/intregracoes/adobe-hub/templante.service';
import { TemplateModel } from '../../../../../../shared/models/integracao/AdobeHub/templante.model';
import { TipoTemplate } from '../../../../../../shared/models/anypoint/tipo-template.model';

@Component({
  selector: 'app-novo-templantes-view',
  imports: [CommonModule],
  templateUrl: './novo-templantes-view.component.html',
  styleUrls: ['./novo-templantes-view.component.css']
})

export class NovoTemplantesViewComponent implements OnInit {

  selectedFileName = '';
  invalidCellMessage = '';
  selectedRow: number | null = null;
  selectedCol: number | null = null;

  mergedCells: { start: { row: number; col: number }, end: { row: number; col: number } }[] = [];
  camposMapeados: { titulo: string; coluna: string }[] = [];
  data: any[][] = [];

  private router = inject(Router);
  private location = inject(Location);
  private templateService = inject(TemplateService);
  tipoSelecionado: TipoTemplate | undefined;

  ngOnInit(): void {

    const state = history.state;

    this.data = state.data || [];
    this.selectedFileName = state.fileName || 'template';
    this.mergedCells = state.mergedCells || [];
    this.tipoSelecionado = state.tipoTemplate;

    if (this.data.length === 0) {
      alert('Nenhum dado carregado!');
      this.router.navigate(['/templates']);
    }
  }

  getColumnLetter(index: number): string {
    let letter = '';
    while (index >= 0) {
      letter = String.fromCharCode((index % 26) + 65) + letter;
      index = Math.floor(index / 26) - 1;
    }
    return letter;
  }

  isMergedCell(row: number, col: number): boolean {
    return this.mergedCells.some(range =>
      row >= range.start.row &&
      row <= range.end.row &&
      col >= range.start.col &&
      col <= range.end.col
    );
  }

  selectCell(rowIndex: number, colIndex: number): void {
    this.selectedRow = rowIndex;
    this.selectedCol = colIndex;

    if (this.isMergedCell(rowIndex, colIndex)) {
      this.invalidCellMessage = `⚠️ A célula ${this.getColumnLetter(colIndex)}${rowIndex + 1} está mesclada e não pode ser usada como cabeçalho.`;
      return;
    }

    const cellValue = this.data[rowIndex]?.[colIndex]?.toString().trim() || '';
    this.invalidCellMessage = cellValue.length === 0
      ? `⚠️ A célula ${this.getColumnLetter(colIndex)}${rowIndex + 1} está vazia.`
      : '';
  }

  isSelected(row: number, col: number): boolean {
    return this.selectedRow === row && this.selectedCol === col && !this.invalidCellMessage;
  }

  isInvalidCell(row: number, col: number): boolean {
    return this.selectedRow === row && this.selectedCol === col && !!this.invalidCellMessage;
  }

  // Gera os dados limpos, salva no backend e navega para próxima etapa
  proceedToCreate(): void {
    if (this.selectedRow === null || this.selectedCol === null || this.invalidCellMessage) return;

    const headerRow = this.data[this.selectedRow];
    const headers = headerRow.slice(this.selectedCol);

    const cleanedData = this.data.slice(this.selectedRow + 1).map(row => {
      const rowCopy = [...row];
      while (rowCopy.length < headers.length) rowCopy.push('');
      return rowCopy.slice(this.selectedCol!);
    });

    this.camposMapeados = headers.map((titulo: string, index: number) => ({
      titulo,
      coluna: this.getColumnLetter(this.selectedCol! + index)
    }));

    const templateModel: TemplateModel = {
      id: '',
      nome: this.getTemplateNameWithoutExtension(),

      tipoTemplate: {
        id: 0,
        nomeCompleto: this.getTemplateNameWithoutExtension(),
        sigla: this.tipoSelecionado!.sigla,
        integracaoId: 0,
        nomeAbreviado: '',
        integracao: null
      },

      qtdColunas: this.camposComTitulo.length,
      colunas: this.camposComTitulo.reduce((acc, campo) => {
        acc[campo.coluna] = campo.titulo;
        return acc;
      }, {} as Record<string, string>),
      linhaCabecalho: this.selectedRow! + 1,
      colunaInicial: this.getColumnLetter(this.selectedCol!),
      arquivoBase: this.selectedFileName,
      observacaoDescricao: '',
      colunasObrigatorias: []
    };

    this.router.navigate(['/Adobe/Novo/Templantes'], {
      state: {
        templateModel,
        data: cleanedData,
        fileName: this.selectedFileName,
        tipoTemplate: this.tipoSelecionado
      }
    });

  }

  goBack(): void {
    Swal.fire({
      title: 'Deseja realmente voltar?',
      text: 'Os dados ainda não foram salvos.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim, voltar',
      cancelButtonText: 'Cancelar',
      reverseButtons: true
    }).then(result => {
      if (result.isConfirmed) {
        this.location.back();
      }
    });
  }

  get camposComTitulo(): { titulo: string; coluna: string }[] {
    return this.camposMapeados.filter(
      campo => campo.titulo && campo.titulo.trim().length > 0
    );
  }

  private getTemplateNameWithoutExtension(): string {
    return this.selectedFileName?.replace(/\.[^/.]+$/, '') || 'template';
  }
}
