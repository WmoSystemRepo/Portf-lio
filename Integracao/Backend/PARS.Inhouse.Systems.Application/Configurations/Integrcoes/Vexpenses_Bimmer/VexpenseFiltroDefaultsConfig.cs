using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;

namespace PARS.Inhouse.Systems.Application.Configurations.Integrcoes.Vexpenses_Bimmer
{
    public class VexpenseFiltroDefaultsConfig
    {
        public string Include { get; set; } = FiltroInclude.expenses.ToString();
        public string Search { get; set; } = "";
        public string SearchField { get; set; } = FiltroSearchField.approval_date_between.ToString();
        public string SearchJoin { get; set; } = FiltroSearchJoin.and.ToString();
    }
}
