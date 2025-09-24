export interface UsuarioPermissaoModel {
  id?: number;
  UsuarioId: number
  permissaoId: number;
  ativo: Boolean;
  dataCriacao: string;
  dataEdicao?: string;
}
