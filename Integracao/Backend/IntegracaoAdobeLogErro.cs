public class IntegracaoAdobeLogErro
{
    public Guid Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Endpoint { get; set; }
    public string Payload { get; set; }
    public string Erro { get; set; }
    public DateTime MigradoEm { get; set; }
    public bool MigradoParaSql { get; set; }
}
