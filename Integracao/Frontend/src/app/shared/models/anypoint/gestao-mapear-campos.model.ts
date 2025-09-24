export interface GestaoMapearCamposModel {
  id: number;
  idCampos?: number; 
  tipoExecucao?: string;
  integracao?: string;
  nomeMapeamentoOrigem?: string;
  campoOrigem?: string;
  codigoSistemaOrigem?: string;
  valorOrigem?: string;
  nomeMapeamentoDestino?: string;
  campoDestino?: string;
  codigoSistemaDestino?: string;
  valorDestino?: string;
}
