import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { CommonModule } from '@angular/common';
import * as XLSX from 'xlsx-js-style';

@Component({
    selector: 'app-montar-excel-importados',
    templateUrl: './montar-excel-importados.component.html',
    styleUrls: ['./montar-excel-importados.component.css'],
    imports: [CommonModule]
})
export class MontarExcelImportadosComponent implements OnInit {

  dados: any[] = [];
  tipo: string = '';
  fileName: string = '';
  selectedTemplate: string = '';

  constructor(private router: Router) { }

  ngOnInit(): void {
    const nav = history.state;

    this.fileName = nav.fileName;
    this.tipo = nav.tipo;
    this.selectedTemplate = nav.selectedTemplate;
    this.dados = nav.dados;
  }

  voltar(): void {
    this.router.navigate(['/Lista/Planilhas/Importadas']);
  }

  exportarExcel(): void {
    const ws = XLSX.utils.json_to_sheet(this.dados);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Planilha');

    const nomeArquivo = this.fileName?.replace(/\.[^/.]+$/, '') || 'planilha';
    XLSX.writeFile(wb, `${nomeArquivo}.xlsx`);
  }

  exportarPDF(): void {
    const doc = new jsPDF();

    const colunas = this.getCabecalhos();
    const linhas = this.dados.map(row =>
      colunas.map(col => row[col] ?? '')
    );

    autoTable(doc, {
      head: [colunas],
      body: linhas
    });

    const nomeArquivo = this.fileName?.replace(/\.[^/.]+$/, '') || 'planilha';
    doc.save(`${nomeArquivo}.pdf`);
  }

  getCabecalhos(): string[] {
    if (!this.dados?.length) return [];
    return Object.keys(this.dados[0]);
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

}
