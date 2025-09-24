namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Regras
{
    /// <summary>
    /// DTO que representa uma regra definida no sistema.
    /// Pode ser usada para agrupar permissões, controlar acessos ou definir comportamentos específicos em contextos configuráveis.
    /// </summary>
    public class RegraDto
    {
        /// <summary>
        /// Identificador único da regra.
        /// Pode ser um GUID, código ou chave do domínio de regras.
        /// </summary>
        public string Id { get; set; } = default!;

        /// <summary>
        /// Nome descritivo da regra, utilizado para exibição e identificação.
        /// Exemplo: "Visualizar Relatórios", "Aprovar Lançamentos".
        /// </summary>
        public string Nome { get; set; } = default!;
    }
}
