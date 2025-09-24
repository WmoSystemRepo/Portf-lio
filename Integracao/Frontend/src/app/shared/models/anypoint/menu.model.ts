export interface MenuModel {
  id?: number | string;
  nome: string;
  rota: string;
  icone: string;
  ordenacaoMenu: number;
  ehMenuPrincipal?: boolean;
  subMenuReferenciaPrincipal?: number;
  dataCriacao: string;
  dataEdicao?: string;
}