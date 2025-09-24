using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.TipoTemplate;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub;

public class PlaninhasImportadosDto
{
    public string? Id { get; set; }

    public string NomeArquivo { get; set; } = string.Empty;

    public string? Usuario { get; set; }

    public string? DataUpload { get; set; }

    public List<Dictionary<string, string>>? Dados { get; set; }

    public int Versao { get; set; }

    public TemplantesMongoDto? Template { get; set; }
}
