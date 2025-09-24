export interface Solicitacao {
  id: number;
  tipo: string;
  status: string;
  email: string;
  requestDate?: Date;
  token: string;
}
