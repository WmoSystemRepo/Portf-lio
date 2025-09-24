import { Observable } from "rxjs/internal/Observable";

export interface ModelReferenceIntefracao {
  key: string;
  field: string;
  icon: string;
  label: string;
  modalSize?: 'sm' | 'md' | 'lg' | 'xl';
  displayNameField: string;
  displayFields: {
    name: string;
    label: string;
    align?: 'left' | 'center' | 'right';
  }[];
  fetchFn: (itemId: number) => Observable<any[]>;
  onSave: (selected: any[], itemId: number) => void;
  onRemove?: (refId: string | number) => void;
}