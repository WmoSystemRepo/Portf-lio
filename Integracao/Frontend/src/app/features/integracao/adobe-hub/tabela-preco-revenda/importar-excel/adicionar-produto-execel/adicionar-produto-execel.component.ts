import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { PlaninhaImportadasService } from '../../../../../../core/services/intregracoes/adobe-hub/planinha-importadas.service';
import * as XLSX from 'xlsx-js-style';

@Component({
  selector: 'app-montar-excel-importados',
  templateUrl: './adicionar-produto-execel.component.html',
  styleUrls: ['./adicionar-produto-execel.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class AdicionarProdutoExecelComponent implements OnInit {

  dados: any[] = [];
  tipo = '';
  fileName = '';
  selectedTemplate = '';
  planilhaId = '';

  headers: string[] = [];
  formValues: Record<string, string> = {};
  displayHeaders: string[] = [];

  pnQuery = '';
  partNumberKey = '';

  formEnabled = false;
  showIndex = true;
  viewingFiltered = false;

  filteredRow: any | null = null;
  draftRow: any | null = null;

  layoutMode: 'compacto' | 'medio' | 'amplo' = 'compacto';

  get canSave(): boolean {
    return this.formEnabled && !this.viewingFiltered;
  }

  constructor(
    private router: Router,
    private planilhaService: PlaninhaImportadasService
  ) { }

  get fileNameNoExt(): string {
    return (this.fileName || '').replace(/\.[^/.]+$/, '');
  }

  ngOnInit(): void {
    const nav = history.state;
    this.fileName = nav.fileName;
    this.tipo = nav.tipo;
    this.selectedTemplate = nav.selectedTemplate;
    this.planilhaId = nav.planilhaId || nav.id || '';
    this.dados = Array.isArray(nav.dados) ? nav.dados : [];

    this.headers = this.getCabecalhos();
    this.partNumberKey = this.detectPartNumberKey(this.headers);
    this.initFormValues();

    this.draftRow = null;
    this.displayHeaders = this.headers.slice();
    this.formEnabled = false;
    this.showIndex = true;
    this.layoutMode = 'compacto';
  }

  setLayoutMode(mode: 'compacto' | 'medio' | 'amplo'): void {
    this.layoutMode = mode;
  }

  voltar(): void {
    this.router.navigate(['/Lista/Planilhas/Importadas']);
  }

  exportarExcel(): void {
    const ws = XLSX.utils.json_to_sheet(this.dados);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Planilha');
    XLSX.writeFile(wb, `${this.fileNameNoExt || 'planilha'}.xlsx`);
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

  private detectPartNumberKey(keys: string[]): string {
    if (!keys?.length) return '';
    const lower = keys.map(k => k.toLowerCase());
    const idx = lower.indexOf('part number');
    return idx >= 0 ? keys[idx] : (keys[0] || '');
  }

  private initFormValues(): void {
    this.formValues = {};
    for (const h of this.headers) this.formValues[h] = '';
  }

  onFormInput(field: string, value: string): void {
    this.formValues[field] = value ?? '';
    if (!this.viewingFiltered) this.syncDraftFromForm();
  }

  onFieldBlur(field: string, value: string): void {
    this.formValues[field] = value ?? '';
    if (this.partNumberKey && field === this.partNumberKey) {
      this.pnQuery = (value ?? '').trim();
      if (this.pnQuery) this.buscarPN();
    } else if (!this.viewingFiltered) {
      this.syncDraftFromForm();
    }
  }

  buscarPN(): void {
    const key = this.partNumberKey;
    const q = (this.pnQuery || this.formValues[key] || '').toLowerCase().trim();

    if (!key || !q) {
      this.formEnabled = false;
      this.viewingFiltered = false;
      this.filteredRow = null;
      this.draftRow = null;
      this.displayHeaders = [];
      this.dados = [];
      this.showIndex = false;
      this.initFormValues();
      return;
    }

    const found = this.dados.find(r => String(r?.[key] ?? '').toLowerCase() === q);

    if (found) {
      this.viewingFiltered = true;
      this.filteredRow = found;
      this.draftRow = null;
      this.applyRowToForm(found);
      this.displayHeaders = this.headers.slice();
      this.formEnabled = false;
      this.showIndex = true;
      setTimeout(() => this.scrollTableToFirstColumn(), 0);
    } else {

      this.viewingFiltered = false;
      this.filteredRow = null;
      this.initFormValues();
      this.formValues[key] = this.pnQuery;
      this.formEnabled = true;
      this.displayHeaders = this.headers.slice();
      this.dados = [];
      this.showIndex = true;
      this.syncDraftFromForm();
      setTimeout(() => this.scrollTableToFirstColumn(), 0);
    }
  }

  limparPN(): void {
    this.pnQuery = '';
    this.viewingFiltered = false;
    this.filteredRow = null;
    this.initFormValues();
    this.draftRow = null;
    this.formEnabled = false;
    this.displayHeaders = [];
    this.dados = [];
    this.showIndex = false;
  }

  private applyRowToForm(row: any): void {
    for (const h of this.headers) {
      this.formValues[h] = row?.[h] ?? '';
    }
  }

  private syncDraftFromForm(): void {
    const d: any = {};
    for (const h of this.headers) d[h] = this.formValues[h] ?? '';
    this.draftRow = d;
  }

  adicionarALinha(): void {
    if (this.viewingFiltered && this.filteredRow) {
      for (const h of this.headers) {
        this.filteredRow[h] = this.formValues[h] ?? '';
      }
      this.viewingFiltered = false;
      this.filteredRow = null;
      this.draftRow = null;
      this.initFormValues();
      this.displayHeaders = this.headers.slice();
      this.showIndex = true;
    } else if (this.draftRow) {
      this.dados = [{ ...this.draftRow }, ...this.dados];
      this.draftRow = null;
      this.initFormValues();
      this.pnQuery = '';
      if (!this.displayHeaders.length) {
        this.displayHeaders = this.headers.slice();
      }
      this.showIndex = true;
    }
  }

  limparFormulario(): void {
    this.initFormValues();
    this.draftRow = null;
    this.viewingFiltered = false;
    this.filteredRow = null;
    this.pnQuery = '';
  }

  async salvar(): Promise<void> {
    const produto = { ...this.formValues };

    if (!this.planilhaId) {
      alert('ID da planilha é obrigatório');
      return;
    }

    try {
      const response = await firstValueFrom(
        this.planilhaService.AdicionarProdutoPlaninha(this.planilhaId, produto)
      );

      if (response.status === 'updated') {
        this.buscarPN();
        alert('✅ Produto atualizado com sucesso!');
      } else {
        this.adicionarALinha();
        alert('✅ Produto adicionado com sucesso!');
      }
    } catch (error) {
      console.error(error);
      alert('❌ Ocorreu um erro ao salvar o produto.');
    }
  }

  get rowsToDisplay(): any[] {
    if (this.viewingFiltered && this.filteredRow) return [this.filteredRow];
    if (this.draftRow) return [this.draftRow];
    return this.dados;
  }

  trackByIndex(index: number): number {
    return index;
  }

  private scrollTableToFirstColumn(): void {
    const wrapper = document.querySelector<HTMLElement>('.table-wrapper');
    if (wrapper) wrapper.scrollLeft = 0;
  }
}
