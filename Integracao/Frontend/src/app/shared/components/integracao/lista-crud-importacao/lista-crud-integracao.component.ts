import {
  Component,
  EventEmitter,
  Input,
  Output,
  OnDestroy,
  SimpleChanges,
  OnChanges,
  OnInit
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModelReferenceIntefracao as ModelReferenceIntegracao } from '../../../models/anypoint/referenciasIntegracao.model';
import Swal from 'sweetalert2';
import { PlanilhasImportadasModel } from '../../../models/integracao/AdobeHub/planilhas-importadas.model';
import { TemplateModel } from '../../../models/integracao/AdobeHub/templante.model';

@Component({
  selector: 'app-lista-crud-integracao',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './lista-crud-integracao.component.html',
  styleUrls: ['./lista-crud-integracao.component.css']
})
export class ListaCrudIntegracaoComponent implements OnDestroy, OnChanges, OnInit {

  // #region Inputs e Outputs
  @Output() selectedChange = new EventEmitter<any[]>();
  @Output() exportSelectedToExcel = new EventEmitter<void>();
  @Output() addProductsToSelected = new EventEmitter<any>();
  @Input() title = 'Lista';
  @Input() items: any[] = [];
  @Input() requireDeleteJustification = false;
  @Input() justificationMinLength = 10;
  @Input() justificationPlaceholder = 'Descreva o motivo da exclusão...';
  @Output() add = new EventEmitter<void>();
  @Output() help = new EventEmitter<void>();
  @Output() delete = new EventEmitter<any>();
  @Output() refresh = new EventEmitter<void>();
  @Output() reference = new EventEmitter<any>();
  @Output() deleteWithJustification = new EventEmitter<{ item: any, justification: string }>();
  @Output() viewDoc = new EventEmitter<any>();
  @Output() viewExcel = new EventEmitter<PlanilhasImportadasModel>();
  @Input() showExportSelectedButton: boolean = true;
  @Input() showAddProductsButton: boolean = true;
  @Input() fields: {
    name: string;
    label: string;
    columnClass?: string;
    align?: 'left' | 'center' | 'right';
  }[] = [];
  // #endregion

  // #region Variáveis de Estado (State)
  allSelected = false;
  router: any;

  internalSelectedItems: any[] = [];
  activeRef: ModelReferenceIntegracao | null = null;

  isDeleteModalOpen = false;
  itemToDelete: any = null;

  isConfigModalOpen = false;
  visibleFields: string[] = [];

  isDetailsModalOpen = false;
  itemToView: any = null;

  isJustificationModalOpen = false;
  itemToDeleteWithJustification: any = null;
  justificationText = '';

  columnFilters: { [key: string]: string } = {};
  searchTerm = '';
  summary: { key: string; value: any; count: number }[] = [];

  private resizing = false;
  private startX = 0;
  private startWidth = 0;
  private currentClass = '';

  expandedComplexInConfig: Set<string> = new Set();
  detailsExpanded: Set<string> = new Set();
  // #endregion

  // #region Seleção de Itens
  toggleSelection(item: any): void {
    const index = this.internalSelectedItems.findIndex(i => i === item);
    if (index > -1) {
      this.internalSelectedItems = this.internalSelectedItems.filter(i => i !== item);
    } else {
      this.internalSelectedItems = [...this.internalSelectedItems, item];
    }
    this.allSelected = this.internalSelectedItems.length === this.filteredItems.length;
    this.emitSelected();
  }

  toggleSelectAll(): void {
    this.allSelected = !this.allSelected;
    this.internalSelectedItems = this.allSelected ? [...this.filteredItems] : [];
    this.emitSelected();
  }

  isItemSelected(item: any): boolean {
    return this.internalSelectedItems.includes(item);
  }

  emitSelected(): void {
    this.selectedChange.emit(this.internalSelectedItems);
  }
  // #endregion

  // #region Ciclo de Vida (Lifecycle)
  ngOnInit(): void {
    if (!this.fields?.length && this.items?.length) {
      this.ensureFieldsFromItems();
    }
    if (!this.visibleFields.length && this.fields?.length > 0) {
      this.visibleFields = this.fields.map(f => f.name);
    }
    this.generateSummary();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.ensureFieldsFromItems();
    const saved = localStorage.getItem(`visibleFields_${this.title}`);
    if (saved) {
      try {
        const parsed = JSON.parse(saved);
        if (Array.isArray(parsed)) this.visibleFields = parsed;
      } catch (err) { }
    } else if (this.fields.length) {
      this.visibleFields = this.fields.map(f => f.name);
    }
    this.generateSummary();
  }

  ngOnDestroy(): void {
    this.removeResizeListeners();
  }
  // #endregion

  // #region Helpers de Template
  getAlignClass(align?: string): string {
    switch (align) {
      case 'center':
        return 'align-center';
      case 'right':
        return 'align-right';
      default:
        return 'align-left';
    }
  }
  // #endregion

  // #region Ações do Cabeçalho
  onAdd(): void { this.add.emit(); }
  onHelp(): void { this.help.emit(); }
  // #endregion

  // #region Exclusão com Justificativa
  onAskDelete(item: any): void {
    if (this.requireDeleteJustification) {
      this.openJustificationModal(item);
    } else {
      this.isDeleteModalOpen = true;
      this.itemToDelete = item;
    }
  }

  // --- Justificativa de exclusão ---
  openJustificationModal(item: any): void {
    this.isJustificationModalOpen = true;
    this.itemToDeleteWithJustification = item;
    this.justificationText = '';
  }
  closeJustificationModal(): void {
    this.isJustificationModalOpen = false;
    this.itemToDeleteWithJustification = null;
    this.justificationText = '';
  }
  confirmDeleteWithJustification(): void {
    if (this.justificationText.length >= this.justificationMinLength) {
      this.deleteWithJustification.emit({
        item: this.itemToDeleteWithJustification,
        justification: this.justificationText
      });
      this.closeJustificationModal();
    }
  }
  isJustificationValid(): boolean {
    return this.justificationText.trim().length >= this.justificationMinLength;
  }
  onConfirmDelete(): void {
    this.delete.emit(this.itemToDelete);
    this.closeDeleteModal();
  }
  closeDeleteModal(): void {
    this.isDeleteModalOpen = false;
    this.itemToDelete = null;
  }
  // #endregion

  // #region Configuração de colunas (Grid)
  openGridConfig(): void {
    this.ensureFieldsFromItems();
    this.isConfigModalOpen = true;
  }
  closeGridConfig(): void {
    this.isConfigModalOpen = false;
  }
  toggleFieldVisibility(fieldName: string): void {
    const index = this.visibleFields.indexOf(fieldName);
    if (index > -1) {
      this.visibleFields.splice(index, 1);
    } else {
      this.visibleFields.push(fieldName);
    }
    localStorage.setItem(`visibleFields_${this.title}`, JSON.stringify(this.visibleFields));
  }
  isFieldVisible(fieldName: string): boolean {
    return this.visibleFields.includes(fieldName);
  }

  // NOVO: Selecionar/Desmarcar TODOS campos (inclui subcolunas)
  allFieldsVisible(): boolean {
    const base = this.fields.map(f => f.name);
    const nested = this.getAllNestedPathsFromData();
    const allPossible = Array.from(new Set([...base, ...nested]));
    return this.visibleFields.length === allPossible.length;
  }
  toggleAllFields(): void {
    const base = this.fields.map(f => f.name);
    const nested = this.getAllNestedPathsFromData();
    const allPossible = Array.from(new Set([...base, ...nested]));
    if (this.visibleFields.length === allPossible.length) {
      this.visibleFields = [];
    } else {
      this.visibleFields = [...allPossible];
    }
    localStorage.setItem(`visibleFields_${this.title}`, JSON.stringify(this.visibleFields));
  }

  // NOVO: Expansão de campos complexos na modal de colunas
  toggleExpandComplexField(fieldName: string): void {
    if (this.expandedComplexInConfig.has(fieldName)) {
      this.expandedComplexInConfig.delete(fieldName);
    } else {
      this.expandedComplexInConfig.add(fieldName);
    }
  }
  isComplexExpanded(fieldName: string): boolean {
    return this.expandedComplexInConfig.has(fieldName);
  }
  // #endregion

  // #region Expansão/Colapso para MODAL DE DETALHES
  toggleExpandDetailsField(field: string) {
    if (this.detailsExpanded.has(field)) this.detailsExpanded.delete(field);
    else this.detailsExpanded.add(field);
  }
  isDetailsExpanded(field: string) {
    return this.detailsExpanded.has(field);
  }
  // #endregion

  // #region Detalhes (modais)
  openDetailsModal(item: any): void {
    this.itemToView = item;
    this.isDetailsModalOpen = true;
    this.detailsExpanded.clear(); // Inicia tudo fechado!
  }
  closeDetailsModal(): void {
    this.isDetailsModalOpen = false;
    this.itemToView = null;
  }
  // #endregion

  // #region Filtros
  get filteredItems(): any[] {
    const namesToCheck = this.allFilterFieldNames();
    return this.items.filter(item =>
      namesToCheck.every(name => {
        const filter = this.columnFilters[name]?.toLowerCase()?.trim();
        if (!filter) return true;
        const val = this.getValueByPath(item, name);
        const value = String(val ?? '').toLowerCase();
        return value.includes(filter);
      })
    );
  }
  filterBySummary(value: any, label: string): void {
    const term = String(value ?? '').toLowerCase().trim();
    this.columnFilters = {};
    const matchedField = this.fields.find(f =>
      this.formatLabel(f.label || f.name).toLowerCase() === label.toLowerCase()
    );
    if (matchedField) {
      this.columnFilters[matchedField.name] = term;
    }
  }
  clearFilters(): void { this.columnFilters = {}; }
  // #endregion

  // #region Sumário (KPI)
  generateSummary(): void {
    const summaries: { key: string; value: any; count: number }[] = [];
    const minCount = 2;
    for (const field of this.fields) {
      const fieldName = field.name;
      if (!fieldName || !this.isFieldVisible(fieldName)) continue;
      const groupMap = new Map<any, number>();
      for (const item of this.items) {
        const rawValue = item[fieldName] ?? 'Sem Valor';
        const value = this.isDate(rawValue)
          ? new Date(rawValue).toISOString().slice(0, 10)
          : String(rawValue).trim();
        groupMap.set(value, (groupMap.get(value) ?? 0) + 1);
      }
      for (const [value, count] of groupMap.entries()) {
        if (count >= minCount) {
          summaries.push({
            key: field.label || this.formatLabel(fieldName),
            value,
            count
          });
        }
      }
    }
    this.summary = summaries;
  }
  // #endregion

  // #region Helpers de Fields (Campos)
  private ensureFieldsFromItems(): void {
    if (this.items?.length && this.fields.length === 0) {
      const sampleItem = this.items[0];
      this.fields = Object.keys(sampleItem).map(key => ({
        name: key,
        label: this.formatLabel(key),
        columnClass: this.isDate(sampleItem[key]) ? 'date-col' : ''
      }));
    }
    if (this.visibleFields.length === 0 && this.fields.length > 0) {
      this.visibleFields = this.fields.map(f => f.name);
    }
  }
  formatLabel(key: string): string {
    return key
      .replace(/_/g, ' ')
      .replace(/([a-z])([A-Z])/g, '$1 $2')
      .replace(/^./, str => str.toUpperCase());
  }
  isDate(value: any): boolean {
    if (!value || typeof value !== 'string') return false;
    const looksLikeDate = /\d{4}-\d{2}-\d{2}/.test(value) || value.includes('T');
    if (!looksLikeDate) return false;
    const date = new Date(value);
    return !isNaN(date.getTime());
  }
  isDateField(fieldName: string): boolean {
    const dateFields = ['dataCriacao', 'dataEdicao'];
    return dateFields.includes(fieldName);
  }
  hasHiddenFields(): boolean {
    return this.fields.some(f => !this.isFieldVisible(f.name));
  }
  // #endregion

  // #region Redimensionamento de colunas
  startResize(event: MouseEvent, className: string | undefined): void {
    if (!className) return;
    this.resizing = true;
    this.startX = event.pageX;
    const sample = document.querySelector<HTMLElement>(`.${className}`);
    if (!sample) return;
    const rect = sample.getBoundingClientRect();
    this.startWidth = rect.width;
    this.currentClass = className;
    document.addEventListener('mousemove', this.onMouseMove);
    document.addEventListener('mouseup', this.stopResize);
  }
  private onMouseMove = (event: MouseEvent): void => {
    if (!this.resizing || !this.currentClass) return;
    const dx = event.pageX - this.startX;
    const minWidth = 80;
    const maxWidth = this.currentClass === 'actions-col' ? 300 : 1000;
    const newWidth = Math.min(maxWidth, Math.max(minWidth, this.startWidth + dx));
    const elements = document.querySelectorAll<HTMLElement>(`.${this.currentClass}`);
    elements.forEach(el => {
      el.style.flex = `0 0 ${newWidth}px`;
      el.style.minWidth = `${newWidth}px`;
      el.style.maxWidth = `${newWidth}px`;
    });
  };
  private stopResize = (): void => {
    this.resizing = false;
    this.currentClass = '';
    this.removeResizeListeners();
  };
  private removeResizeListeners(): void {
    document.removeEventListener('mousemove', this.onMouseMove);
    document.removeEventListener('mouseup', this.stopResize);
  }
  // #endregion

  // #region Ações do cabeçalho
  onExportSelectedToExcel(): void {
    this.exportSelectedToExcel.emit();
  }
  async onAddProductsToSelected(): Promise<void> {
    if (!this.internalSelectedItems || this.internalSelectedItems.length === 0) {
      await Swal.fire({
        icon: 'warning',
        title: 'Atenção',
        html: 'Nenhuma lista selecionada.<br>Selecione uma única lista para adicionar produtos.',
        confirmButtonText: 'OK',
        buttonsStyling: false,
        customClass: { popup: 'swal-rounded', confirmButton: 'btn-swal-confirm' },
        didOpen: (p) => this.styleSwalPopup(p)
      });
      return;
    }
    if (this.internalSelectedItems.length === 1) {
      this.addProductsToSelected.emit(this.internalSelectedItems[0]);
      return;
    }
    const escolhido = await this.openCardSelectionModal(this.internalSelectedItems);
    if (!escolhido) return;
    this.internalSelectedItems = [escolhido];
    this.allSelected = this.internalSelectedItems.length === this.filteredItems.length;
    this.emitSelected();
    this.addProductsToSelected.emit(escolhido);
  }
  // #endregion

  // #region Helpers Privados (Swal, Escapes, Labels)
  private async openCardSelectionModal(items: any[]): Promise<any | null> {
    const optionsHtml = items
      .map((item, idx) => `
        <div style="display: flex; align-items: center; gap: 0.8rem; margin-bottom: 8px;">
          <input type="radio" name="item" value="${idx}" id="opt${idx}" style="margin-right:8px;" />
          <label for="opt${idx}">
            ${this.escapeHtml(this.buildItemLabel(item))}
          </label>
        </div>
      `).join('');

    const { value: selectedIndex } = await Swal.fire({
      title: 'Selecione uma das listas para adicionar produtos:',
      html: `<div style="max-height: 310px; overflow-y: auto;">${optionsHtml}</div>`,
      confirmButtonText: 'Selecionar',
      showCancelButton: true,
      focusConfirm: false,
      customClass: {
        popup: 'swal-rounded',
        confirmButton: 'btn-swal-confirm',
        cancelButton: 'btn-swal-cancel'
      },
      preConfirm: () => {
        const radios = Swal.getHtmlContainer()!.querySelectorAll<HTMLInputElement>('input[type="radio"][name="item"]');
        const selected = Array.from(radios).find(r => r.checked);
        if (!selected) {
          Swal.showValidationMessage('Selecione uma opção.');
          return null;
        }
        return parseInt(selected.value, 10);
      },
      didOpen: (popup) => this.styleSwalPopup(popup as HTMLElement)
    });

    if (selectedIndex == null || isNaN(selectedIndex)) return null;
    return items[selectedIndex];
  }

  private escapeHtml(str: any): string {
    return String(str ?? '')
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;');
  }
  private buildItemLabel(item: any): string {
    const tipo = item?.tipo ?? item?.Tipo ?? '(sem tipo)';
    const nome = item?.nomeArquivo ?? item?.nome ?? '(sem nome)';
    const data = item?.dataUpload ?? item?.data ?? '';
    return `Tipo: ${tipo} | Arquivo: ${nome}${data ? ' | Upload: ' + data : ''}`;
  }
  private styleSwalPopup(popup: HTMLElement): void {
    const azul = getComputedStyle(document.documentElement)
      .getPropertyValue('--primary-color-end')?.trim() || '#0d47a1';
    popup.style.borderRadius = '28px';
    popup.style.border = `3px solid ${azul}`;
    popup.style.boxShadow = '0 12px 40px rgba(13, 71, 161, 0.25)';
    popup.style.paddingBottom = '1.2rem';
    popup.style.width = 'min(92vw, 780px)';
    popup.style.maxWidth = '780px';
    const actions = popup.querySelector('.swal2-actions') as HTMLElement | null;
    if (actions) actions.style.gap = '12px';
    const ok = popup.querySelector('.swal2-confirm') as HTMLElement | null;
    if (ok) {
      ok.style.border = '0';
      ok.style.borderRadius = '999px';
      ok.style.padding = '0.7rem 1.6rem';
      ok.style.fontWeight = '700';
      ok.style.fontSize = '0.95rem';
      ok.style.color = '#fff';
      ok.style.background = 'linear-gradient(135deg, #d32f2f 20%, #ff6b6b)';
    }
    const cancel = popup.querySelector('.swal2-cancel') as HTMLElement | null;
    if (cancel) {
      cancel.style.border = '0';
      cancel.style.borderRadius = '999px';
      cancel.style.padding = '0.7rem 1.6rem';
      cancel.style.fontWeight = '700';
      cancel.style.fontSize = '0.95rem';
      cancel.style.color = '#fff';
      cancel.style.background = 'linear-gradient(135deg, #4b4b4b 20%, #1f1f1f)';
    }
  }
  // #endregion

  // #region Documentação/Visualização de Arquivo
  onViewDoc(item: TemplateModel): void {
    const docFile = item.nome?.toLowerCase().replace(/\s+/g, '-') || 'documentacao';
    this.router.navigate(['/visualizar-doc', docFile]);
  }
  // #endregion

  // #region Helpers para campos complexos e paths
  getValueByPath(obj: any, path: string): any {
    if (!obj || !path) return undefined;
    if (!path.includes('.')) return obj[path];
    return path.split('.').reduce((acc, key) => (acc == null ? undefined : acc[key]), obj);
  }
  getSubKeysOf(fieldName: string): string[] {
    if (!this.items?.length) return [];
    const sample = this.items[0]?.[fieldName];
    if (sample && typeof sample === 'object' && !Array.isArray(sample)) {
      return Object.keys(sample);
    }
    return [];
  }
  isComplexField(fieldName: string): boolean {
    if (!this.items?.length) return false;
    const val = this.items[0]?.[fieldName];
    return val && typeof val === 'object' && !Array.isArray(val);
  }
  visibleSubColumnsOf(fieldName: string): string[] {
    const prefix = fieldName + '.';
    return this.visibleFields.filter(v => v.startsWith(prefix)).map(v => v.slice(prefix.length));
  }
  // Todos os paths possíveis (baseados no primeiro item)
  private getAllNestedPathsFromData(): string[] {
    const out: string[] = [];
    if (!this.items?.length) return out;
    const sample = this.items[0] ?? {};
    for (const key of Object.keys(sample)) {
      const val = sample[key];
      if (val && typeof val === 'object' && !Array.isArray(val)) {
        for (const sub of Object.keys(val)) {
          out.push(`${key}.${sub}`);
        }
      }
    }
    return out;
  }
  // Campos a checar em filtro (campos base + subcolunas visíveis + filtros atuais)
  private allFilterFieldNames(): string[] {
    const base = this.fields.map(f => f.name);
    const nestedSelected = this.visibleFields.filter(v => v.includes('.'));
    const filtersKeys = Object.keys(this.columnFilters ?? {});
    return Array.from(new Set([...base, ...nestedSelected, ...filtersKeys]));
  }

  // Alias para evitar erro de template
  toggleDetailsExpand(field: string) {
    this.toggleExpandDetailsField(field);
  }
  // #endregion

}
