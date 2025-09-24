export interface SppBimerInvoceMonitorModel {
  id: number;
  numeroAlterData: string;
  pedidoSpp?: string;
  fabricante: string;
  fabricanteId: string;
  estoque: string;
  dataEmissao: string;
  valorInvoice: number;
  statusIntegracao: 'S' | 'N';
  observacaoErro: string;
}