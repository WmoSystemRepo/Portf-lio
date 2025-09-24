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
import { FormsModule } from '@angular/forms';
import { from, isObservable } from 'rxjs';
import { ModelReference } from '../../../models/anypoint/referencias.model';

@Component({
  selector: 'app-lista-crud',
  imports: [CommonModule, FormsModule],
  templateUrl: './lista-crud.component.html',
  styleUrls: ['./lista-crud.component.scss']
})
export class ListaCrudComponent implements OnDestroy, OnChanges, OnInit {

  // #region Init
  ngOnInit(): void {
    if (!this.fields?.length && this.items?.length) {
      this.ensureFieldsFromItems();
    }

    if (!this.visibleFields.length && this.fields?.length > 0) {
      this.visibleFields = this.fields.map(f => f.name);
    }
  }
  // #endregion

  // #region Helpers
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

  // #region Inputs / Outputs
  @Input() title = 'Lista';
  @Input() fields: {
    name: string;
    label: string;
    columnClass?: string;
    align?: 'left' | 'center' | 'right';
  }[] = [];

  @Input() requireDeleteJustification = false;
  @Input() justificationMinLength = 10;
  @Input() justificationPlaceholder = 'Descreva o motivo da exclusão...';

  @Input() items: any[] = [];
  @Input() modelRefs: ModelReference[] = [];
  @Input() excludeFields: string[] = [];

  @Output() add = new EventEmitter<void>();
  @Output() help = new EventEmitter<void>();
  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();
  @Output() deleteWithJustification = new EventEmitter<{ item: any, justification: string }>();
  @Output() refresh = new EventEmitter<void>();
  @Output() reference = new EventEmitter<any>();
  // #endregion

  // #region Estados e Modais
  activeRef: ModelReference | null = null;

  refModalOpen = false;
  refModalLoading = false;
  refModalData: any[] = [];
  refModalSelected: Set<any> = new Set();
  refModalTargetItem: any = null;
  refModalFilterText: string = '';

  isDeleteModalOpen = false;
  itemToDelete: any = null;

  isConfigModalOpen = false;
  visibleFields: string[] = [];

  expandedComplexInConfig: Set<string> = new Set();
  detailsExpanded: Set<string> = new Set();

  isDetailsModalOpen = false;
  itemToView: any = null;

  columnFilters: { [key: string]: string } = {};
  searchTerm = '';

  isJustificationModalOpen = false;
  itemToDeleteWithJustification: any = null;
  justificationText = '';

  isJustificationRefModalOpen = false;
  itemToDeleteWithJustificationRef: any = null;
  justificationRefText = '';
  // #endregion

  // #region Redimensionamento de colunas
  private resizing = false;
  private startX = 0;
  private startWidth = 0;
  private currentClass = '';
  // #endregion

  // #region Lifecycle
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
  }

  ngOnDestroy(): void {
    this.removeResizeListeners();
  }
  // #endregion

  // #region Ações CRUD
  onAdd(): void {
    this.add.emit();
  }

  onHelp(): void {
    this.help.emit();
  }

  onEdit(item: any): void {
    this.edit.emit(item);
  }

  onAskDelete(item: any): void {
    if (this.requireDeleteJustification) {
      this.openJustificationModal(item);
    } else {
      this.isDeleteModalOpen = true;
      this.itemToDelete = item;
    }
  }

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

  openJustificationRefModal(item: any): void {
    this.isJustificationRefModalOpen = true;
    this.itemToDeleteWithJustificationRef = item;
    this.justificationRefText = '';
  }

  closeJustificationRefModal(): void {
    this.isJustificationRefModalOpen = false;
    this.itemToDeleteWithJustificationRef = null;
    this.justificationRefText = '';
  }

  confirmDeleteWithJustificationRef(): void {
    if (this.justificationText.length >= this.justificationMinLength) {
      this.deleteWithJustification.emit({
        item: this.itemToDeleteWithJustification,
        justification: this.justificationText
      });
      this.closeJustificationModal();
    }
  }
  // #endregion

  // #region Configuração de colunas
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

  isFieldVisible(fieldName: string): boolean {
    return this.visibleFields.includes(fieldName);
  }

  visibleSubColumnsOf(fieldName: string): string[] {
    const prefix = fieldName + '.';
    return this.visibleFields
      .filter(v => v.startsWith(prefix))
      .map(v => v.slice(prefix.length));
  }

  private allFilterFieldNames(): string[] {
    const base = this.fields.map(f => f.name);
    const nestedSelected = this.visibleFields.filter(v => v.includes('.'));
    const filtersKeys = Object.keys(this.columnFilters ?? {});
    return Array.from(new Set([...base, ...nestedSelected, ...filtersKeys]));
  }
  // #endregion

  // #region Modal de detalhes
  openDetailsModal(item: any): void {
    this.itemToView = item;
    this.isDetailsModalOpen = true;
  }

  closeDetailsModal(): void {
    this.isDetailsModalOpen = false;
    this.itemToView = null;
  }
  // #endregion

  // #region Modal de Referências
  onReference(refKey: string, item: any): void {
    const ref = this.modelRefs.find(r => r.key === refKey);
    if (!ref || !ref.fetchFn || !ref.field) return;

    const itemId = item[ref.field];
    if (!itemId) return;

    this.activeRef = ref;
    this.refModalLoading = true;
    this.refModalOpen = true;
    this.refModalSelected = new Set();
    this.refModalTargetItem = item;

    const resultado = ref.fetchFn(itemId);
    const observable$ = isObservable(resultado) ? resultado : from(Promise.resolve(resultado));

    observable$.subscribe({
      next: (data: any[]) => {
        this.refModalData = data;
        this.refModalLoading = false;
      },
      error: () => {
        this.refModalData = [];
        this.refModalLoading = false;
      }
    });
  }

  saveReference(): void {
    if (this.activeRef?.onSave && this.refModalTargetItem) {
      const selectedArray = Array.from(this.refModalSelected);
      const itemId = this.refModalTargetItem[this.activeRef.field];
      this.activeRef.onSave(selectedArray, itemId);
      this.closeReferenceModal();
    }
  }

  closeReferenceModal(): void {
    this.refModalOpen = false;
    this.refModalData = [];
    this.refModalSelected.clear();
    this.refModalTargetItem = null;
    this.activeRef = null;
  }

  get refModalTitle(): string {
    if (!this.activeRef) return 'Referência';
    if (typeof this.activeRef.modalTitle === 'function') {
      return this.activeRef.modalTitle(this.refModalTargetItem);
    }
    if (typeof this.activeRef.modalTitle === 'string') {
      return this.activeRef.modalTitle;
    }
    const fieldName = this.activeRef?.displayNameField;
    const itemName = fieldName && this.refModalTargetItem
      ? this.refModalTargetItem[fieldName]
      : '';
    return `Associar ${this.activeRef.label} - ${itemName}`;
  }

  get refModalTitleName(): string {
    const fieldName = this.activeRef?.displayNameField;
    return fieldName && this.refModalTargetItem
      ? this.refModalTargetItem[fieldName] ?? ''
      : '';
  }

  get refModalFilteredData(): any[] {
    let dados = this.refModalData;
    if (this.refModalFilterText?.trim()) {
      const filtro = this.refModalFilterText.toLowerCase();
      dados = dados.filter(item => {
        const campos = this.activeRef?.displayFields;
        if (!campos) return false;
        return campos.some(f => {
          const valor = (item[f.name] ?? '').toString().toLowerCase();
          return valor.includes(filtro);
        });
      });
    }
    return dados;
  }

  onApplyFilter(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.refModalFilterText = target.value;
  }

  hasReferenceButtons(): boolean {
    return this.modelRefs?.length > 0;
  }

  get hasReferenceButton(): boolean {
    return this.hasReferenceButtons();
  }

  onToggleReference(item: any): void {
    const itemId = item.id || item.Id;
    if (item._checked) {
      this.refModalSelected.add(item);
    } else {
      const itemParaRemover = Array.from(this.refModalSelected).find(
        (i: any) => (i.id || i.Id) === itemId);
      if (itemParaRemover) {
        this.refModalSelected.delete(itemParaRemover);
      }
      if (item.menuReferenciaId || item.id) {
        const origemId = this.refModalTargetItem?.[this.activeRef?.field ?? 'id'];
        const destinoId = item.menuReferenciaId ?? item.id;
        this.activeRef?.onRemove?.(origemId, destinoId);
      }
    }
  }

  getModalSizeClass(): string {
    const size = this.activeRef?.modalSize ?? 'md';
    const sizeClassMap: { [key: string]: string } = {
      sm: 'modal-sm',
      md: 'modal-md',
      lg: 'modal-lg',
      xl: 'modal-xl'
    };
    return sizeClassMap[size] || 'modal-md';
  }

  vincularItem(ref: any): void {
    if (ref._checked) return;
    ref._checked = true;
    this.refModalSelected.add(ref);

    const itemId = this.refModalTargetItem?.[this.activeRef?.field ?? 'id'];
    if (this.activeRef?.onSave && itemId) {
      const payload = [{
        menuId: itemId,
        id: ref.Id ?? ref.id,
        dataCriacao: new Date().toISOString(),
        dataEdicao: new Date().toISOString(),
        ativo: true
      }];
      this.activeRef.onSave(payload, itemId);
    }
  }

  desvincularItem(ref: any): void {
    if (!ref._checked) return;
    ref._checked = false;
    this.refModalSelected.delete(ref);

    const origemId = this.refModalTargetItem?.[this.activeRef?.field ?? 'id'];
    const destinoId = ref.menuReferenciaId ?? ref.id;

    if (this.activeRef?.onRemove && origemId) {
      this.activeRef.onRemove(origemId, destinoId);
    }
  }
  // #endregion

  // #region Filtros e resumo
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

  clearFilters(): void {
    this.columnFilters = {};
  }
  // #endregion

  // #region Utilitários
  private ensureFieldsFromItems(): void {
    if (this.items?.length && this.fields.length === 0) {
      const sampleItem = this.items[0];
      this.fields = Object.keys(sampleItem)
        .filter(key => !this.excludeFields.includes(key))
        .map(key => ({
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

  // #region Redimensionamento
  startResize(event: MouseEvent, className: string | undefined): void {
    if (!className) return;
    this.resizing = true;
    this.startX = event.pageX;
    const sample = document.querySelector<HTMLElement>(`.${className}`);
    if (!sample) return;
    this.startWidth = sample.offsetWidth;
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

  // #region Acessores
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
  // #endregion

  // #region Expansão/Colapso Detalhes
  toggleExpandDetailsField(field: string) {
    if (this.detailsExpanded.has(field)) {
      this.detailsExpanded.delete(field);
    } else {
      this.detailsExpanded.add(field);
    }
  }

  isDetailsExpanded(field: string) {
    return this.detailsExpanded.has(field);
  }

  toggleDetailsExpand(field: string) {
    this.toggleExpandDetailsField(field);
  }
  // #endregion
}