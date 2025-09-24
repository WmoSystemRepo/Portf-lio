import { GestaoIntegracaoModel } from "./gestao-integracao.model";

export interface TipoTemplate {
  id?: number;
  nomeCompleto: string;
  sigla: string;
  integracaoId: number;
  nomeAbreviado?: string;
  integracao?: GestaoIntegracaoModel | null;
}
