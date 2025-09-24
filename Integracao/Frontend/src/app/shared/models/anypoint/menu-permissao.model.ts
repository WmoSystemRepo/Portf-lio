export interface MenuPermissaoModel {
  id?: number;
  menuId: number
  permissaoId: number;
  ativo: Boolean;
  dataCriacao: string;
  dataEdicao?: string;
}
