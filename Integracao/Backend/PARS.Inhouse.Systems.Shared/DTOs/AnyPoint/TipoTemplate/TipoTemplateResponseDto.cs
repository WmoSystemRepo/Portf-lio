namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.TipoTemplate
{
    public class TipoTemplateRequestDto
    {
        public int? Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public int IntegracaoId { get; set; }
        public string NomeAbreviado { get; set; } = string.Empty;
    }
}