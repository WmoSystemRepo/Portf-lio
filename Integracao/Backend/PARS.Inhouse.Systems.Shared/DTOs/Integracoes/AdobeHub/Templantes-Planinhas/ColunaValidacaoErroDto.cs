public class ColunaValidacaoErroDto
{
    public List<string> ColunasFaltando { get; set; } = new();
    public List<string> ColunasExtras { get; set; } = new();
    public string Mensagem { get; set; } = "Erro de validação de colunas da planilha.";
}
