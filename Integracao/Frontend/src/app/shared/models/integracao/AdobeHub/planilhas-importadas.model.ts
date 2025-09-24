import { TemplateModel } from "./templante.model";

export interface PlanilhasImportadasModel {
  id: string;
  nomeArquivo: string;
  dataUpload: Date;
  dados: { [coluna: string]: string }[];
  versao: number | null | undefined;
  usuario?: string;
  template: TemplateModel;
}
