import { TipoTemplate } from "../../anypoint/tipo-template.model";

export interface TemplateModel {
  id: string;
  nome: string;
  qtdColunas: number;
  colunas: Record<string, string>;
  linhaCabecalho?: number;
  colunaInicial?: string;
  arquivoBase?: string;
  observacaoDescricao?: string;
  colunasObrigatorias?: string[];
  dataCriacao?: string;
  dataEdicao?: string;
  tipoTemplate?: TipoTemplate;
}
