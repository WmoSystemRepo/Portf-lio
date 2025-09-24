export interface RegisterUser {
  email: string;
  userName: string;
  phoneNumber?: string;
  password: string;
  podeLer: boolean;
  podeEscrever: boolean;
  podeRemover: boolean;
  podeVerConfiguracoes: boolean;
}