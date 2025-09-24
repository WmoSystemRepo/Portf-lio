import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

export interface DynamicField {
  step?: any;
  max?: string | number | null;
  min?: string | number | null;
  key: string;
  label: string;
  type: 'text' | 'number' | 'email' | 'date' | 'select' | 'checkbox' | 'checkbox-list' | 'multiselect';
  required?: boolean;
  hidden?: boolean;
  options?: { label: string; value: any }[];
  loadOptions?: () => Promise<any[]>;
  labelField?: string;
  valueField?: string;
}

@Component({
  selector: 'app-form-componete-crud',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form-crud.component.html',
  styleUrls: ['./form-crud.component.css']
})
export class FormComponeteCrudComponent implements OnInit, OnChanges {
  
  // #region Inputs / Outputs
  @Input() fields: DynamicField[] = [];
  @Input() initialData: any = null;
  @Input() isEditMode = false;
  @Output() help = new EventEmitter<void>();
  @Output() create = new EventEmitter<any>();
  @Output() update = new EventEmitter<any>();
  @Output() cancel = new EventEmitter<void>();
  // #endregion

  // #region Estado
  form!: FormGroup;
  // #endregion

  constructor(private fb: FormBuilder) { }

  // #region Lifecycle
  async ngOnInit(): Promise<void> {
    if (this.fields?.length) {
      for (const field of this.fields) {
        if (
          (
            field.type === 'select' ||
            field.type === 'checkbox' ||
            field.type === 'multiselect' ||
            field.type === 'checkbox-list'
          ) &&
          field.loadOptions
        ) {
          const rawOptions = await field.loadOptions();
          const labelKey = field.labelField || 'label';
          const valueKey = field.valueField || 'value';

          field.options = rawOptions.map(item => ({
            label: item[labelKey],
            value: item[valueKey]
          }));
        }
      }
      this.buildForm();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialData'] && this.initialData && this.fields.length) {
      this.buildForm();
      Promise.resolve().then(() => {
        if (this.form && this.initialData) {
          this.form.patchValue(this.initialData);
        }
      });
    }
  }
  // #endregion

  // #region Formulário
  buildForm(): void {
    const group: any = {};

    this.fields.forEach(field => {
      let value = this.initialData?.[field.key];

      if (field.type === 'select' && (value === undefined || value === '')) {
        value = null;
      }

      if (field.type === 'checkbox') {
        value = !!value;
      }

      if ((field.type === 'multiselect' || field.type === 'checkbox-list') && !Array.isArray(value)) {
        value = [];
      }

      group[field.key] = field.required
        ? [value, Validators.required]
        : [value];
    });

    this.form = this.fb.group(group);
  }

  onCheckboxListChange(fieldKey: string, event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const control = this.form.get(fieldKey);
    const selectedValues = control?.value || [];

    if (checkbox.checked) {
      control?.setValue([...selectedValues, checkbox.value]);
    } else {
      control?.setValue(selectedValues.filter((v: any) => v !== checkbox.value));
    }
  }
  // #endregion

  // #region Ação
  onSubmit(): void {
    if (this.isEditMode) {
      this.update.emit(this.form.value);
    } else {
      this.create.emit(this.form.value);
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }

  onHelp(): void {
    this.help.emit();
  }
  // #endregion
}
