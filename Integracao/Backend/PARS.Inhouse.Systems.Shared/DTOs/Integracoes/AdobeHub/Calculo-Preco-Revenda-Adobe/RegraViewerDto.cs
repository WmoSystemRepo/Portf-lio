namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public sealed class RegraViewerDto
    {
        public string Tipo { get; set; }
        public string ColunaTabela { get; set; }
        public string Item { get; set; }

        public RegraViewerDto(string tipo, string colunaTabela, string item)
        {
            Tipo = tipo;
            ColunaTabela = colunaTabela;
            Item = item;
        }
    }
}
