namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Relatorios
{
    /// <summary>
    /// DTO utilizado para representar os totais agrupados de registros em relatórios e dashboards.
    /// Cada propriedade indica a contagem de um determinado status ou situação do processo.
    /// </summary>
    public class ContagensRelatorios
    {
        /// <summary>
        /// Total geral de registros encontrados no relatório.
        /// </summary>
        public int TotalGeral { get; set; }

        /// <summary>
        /// Total de registros com status de pagamento confirmado.
        /// </summary>
        public int TotalPago { get; set; }

        /// <summary>
        /// Total de registros com pendências a serem resolvidas.
        /// </summary>
        public int TotalPendencias { get; set; }

        /// <summary>
        /// Total de registros que apresentaram erro no processo.
        /// </summary>
        public int TotalErros { get; set; }

        /// <summary>
        /// Total de registros processados com sucesso.
        /// </summary>
        public int TotalSucesso { get; set; }

        /// <summary>
        /// Total de registros que foram aprovados.
        /// </summary>
        public int TotalAprovados { get; set; }

        /// <summary>
        /// Total de registros que ainda estão em aberto (sem resolução).
        /// </summary>
        public int TotalAbertos { get; set; }

        /// <summary>
        /// Total de registros que foram enviados ao sistema destino ou próximo fluxo.
        /// </summary>
        public int TotalEnviados { get; set; }

        /// <summary>
        /// Total de registros reabertos após encerramento prévio.
        /// </summary>
        public int TotalReabertos { get; set; }

        /// <summary>
        /// Total de registros que foram reprovados após avaliação.
        /// </summary>
        public int TotalReprovados { get; set; }
    }
}
