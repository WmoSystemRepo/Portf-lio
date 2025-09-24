export interface ExclusaoPendenciasModel {
  justificativa: string;
  usuario: string;
  dataHora: string;
  registrosExcluidos: {
    idResponse: number;
    userId: number;
    descricao?: string;
    valor: number;
    dataCadastro: string;
    observacao?: string;
    response?: string;
  }[];
}
