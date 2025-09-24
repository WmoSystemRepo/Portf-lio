export interface MenuIntegracaoModel {
  id?: number;
  menuId: number
  integracaoId: number;
  ativo: Boolean;
  dataCriacao: string;
  dataEdicao?: string;
}
