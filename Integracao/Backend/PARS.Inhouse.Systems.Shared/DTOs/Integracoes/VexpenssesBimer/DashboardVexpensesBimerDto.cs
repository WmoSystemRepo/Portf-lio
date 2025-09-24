namespace PARS.Inhouse.Systems.Domain.DTOs.Integracoes.Vexpenses_Bimmer
{
    /// <summary>
    /// Representa o conjunto de dados consolidados do dashboard de integração entre Vexpenses e Bimer.
    /// </summary>
    public class DashboardVexpensesBimerDto
    {
        /// <summary>Totais agrupados por status de relatórios.</summary>
        public TotaisDto Totais { get; set; } = new();

        /// <summary>Dados para o gráfico mensal de status (pendências, erros e sucesso).</summary>
        public GraficoMensalDto GraficoMensal { get; set; } = new();

        /// <summary>Gráfico de pizza com distribuição das pendências.</summary>
        public GraficoPizzaDto GraficoPizzaPendencias { get; set; } = new();

        /// <summary>Gráfico de pizza com distribuição dos erros.</summary>
        public GraficoPizzaDto GraficoPizzaErros { get; set; } = new();

        /// <summary>Gráfico de pizza de pendências separadas por moeda.</summary>
        public GraficoPizzaDto GraficoPizzaPendenciasMoeda { get; set; } = new();
        /// <summary>Gráfico de pizza de pendências separadas por moeda.</summary>
        public GraficoPizzaDto GraficoPizzaPendenciasExcluidasPorUsario { get; set; } = new ();

        /// <summary>Gráfico de pizza de erros separados por exceção.</summary>
        public GraficoPizzaDto GraficoPizzaErrosExcecao { get; set; } = new();
    }

    /// <summary>
    /// Totais agregados por status de relatórios.
    /// </summary>
    public class TotaisDto
    {
        public int TotalGeral { get; set; }
        public int TotalPago { get; set; }
        public int TotalAprovados { get; set; }
        public int TotalAbertos { get; set; }
        public int TotalReabertos { get; set; }
        public int TotalReprovados { get; set; }
        public int TotalEnviados { get; set; }
        public int TotalPendencias { get; set; }
        public int TotalErros { get; set; }
        public int TotalSucesso { get; set; }
    }

    /// <summary>
    /// Dados para construção de gráfico mensal de linha ou barra.
    /// </summary>
    public class GraficoMensalDto
    {
        /// <summary>Rótulos dos meses (ex: "Jan", "Fev").</summary>
        public List<string> Labels { get; set; } = new();
        /// <summary>Quantidade de pendências por mês.</summary>
        public List<int> PendenciasResolvidas { get; set; } = new();

        /// <summary>Quantidade de pendências por mês.</summary>
        public List<int> Pendencias { get; set; } = new();

        /// <summary>Quantidade de erros por mês.</summary>
        public List<int> Erros { get; set; } = new();

        /// <summary>Quantidade de sucessos por mês.</summary>
        public List<int> Sucesso { get; set; } = new();
    }

    /// <summary>
    /// Dados para gráficos de pizza (rótulo x valor).
    /// </summary>
    public class GraficoPizzaDto
    {
        /// <summary>Rótulos das fatias da pizza.</summary>
        public List<string> Labels { get; set; } = new();

        /// <summary>Valores associados a cada rótulo.</summary>
        public List<int> Valores { get; set; } = new();
    }
}
