namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub
{
    /// <summary>
    /// DTO usado para salvar ou atualizar (upsert) um produto no array "Dados" de uma planilha no MongoDB.
    /// </summary>
    public class SalvarProdutoDto
    {
        /// <summary>
        /// Dados do produto (mapa de coluna => valor).
        /// </summary>
        public Dictionary<string, object>? Produto { get; set; }
    }
}
