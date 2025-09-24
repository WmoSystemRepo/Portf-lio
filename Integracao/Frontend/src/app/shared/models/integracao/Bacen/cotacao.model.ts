export interface Cotacao {
  cotacaoCompra: number;
  cotacaoVenda: number;
  dataHoraCotacao: string;
}

export interface CotacaoResponse {
  cotacaoAtual: Cotacao;
  historico: Cotacao[];
}
