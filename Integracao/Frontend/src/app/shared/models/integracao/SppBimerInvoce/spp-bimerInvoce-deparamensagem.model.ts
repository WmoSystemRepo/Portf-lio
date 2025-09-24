export interface SppBimerInvoceDeparamensagem {
  id?: number;
  mensagemPadrao: string;
  mensagemModificada: string;
  ativo: boolean;
  usuarioCadastro: string;
  usuarioEdicao?: string;
  dataCriacao: Date;
  dataEdicao?: Date;
}
