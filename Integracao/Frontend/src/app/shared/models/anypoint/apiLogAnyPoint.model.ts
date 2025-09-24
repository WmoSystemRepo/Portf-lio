export interface ApiLogAnyPoint {
  url: string;
  nomeApi: string;
  grupo: string;
  status: number;
  statusText: string;
  tempoMs: number;
  sucesso: boolean;
  erro?: string;
}
