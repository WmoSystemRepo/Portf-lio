export interface ApplicationUser {
  id: string;
  email?: string;
  userName?: string;
  phoneNumber?: string;
  senha?: string;
  podeLer?: boolean;
  podeEscrever?: boolean;
  podeRemover?: boolean;
  podeVerConfiguracoes?: boolean;
}
