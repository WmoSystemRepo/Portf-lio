namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.SppBimerInvoce
{
    public class DeParaMensagemDto
    {
        public int? Id { get; set; }
        public string MensagemPadrao { get; set; } = string.Empty;
        public string MensagemModificada { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public string UsuarioCadastro { get; set; } = string.Empty;
        public string? UsuarioEdicao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataEdicao { get; set; }
    }
}