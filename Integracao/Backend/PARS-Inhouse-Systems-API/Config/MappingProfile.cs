using AutoMapper;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Vexpense;
using System.Text.Json;

namespace PARS_Inhouse_Systems_API.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReportDto, IntegracaoVexpenseTitulosRelatoriosStatus>();
            CreateMap<IntegracaoVexpenseTitulosRelatoriosStatus, ReportDto>();

            //Menu
            CreateMap<MenuDto, AnyPointStoreMenu>();
            CreateMap<MenuUsuarioDto, AnyPointStoreMenuUsuario>();
            CreateMap<MenuIntegracaoDto, AnyPointStoreMenuIntegracao>();


            CreateMap<IntegracaoVexpenseTitulosRelatoriosStatus, IntegracaoVexpenseTitulosRelatoriosStatusDto>()
            .ForMember(d => d.ExpensePayingCompanyId,
                       opt => opt.MapFrom<ExpensePayingCompanyIdResolver>())
            .ForMember(d => d.ExpenseTypeId,
                       opt => opt.MapFrom<ExpenseTypeIdResolver>());
        }

        public class ExpensePayingCompanyIdResolver
    : IValueResolver<IntegracaoVexpenseTitulosRelatoriosStatus, IntegracaoVexpenseTitulosRelatoriosStatusDto, string?>
        {
            public string? Resolve(
                IntegracaoVexpenseTitulosRelatoriosStatus src,
                IntegracaoVexpenseTitulosRelatoriosStatusDto dest,
                string? destMember,
                ResolutionContext context)
            {
                try
                {
                    if (string.IsNullOrEmpty(src.ExpensesData))
                        return null;

                    using var doc = JsonDocument.Parse(src.ExpensesData);
                    var root = doc.RootElement;

                    if (root.ValueKind != JsonValueKind.Array)
                        return null;

                    var ids = new List<string>();
                    foreach (var element in root.EnumerateArray())
                    {
                        if (element.TryGetProperty("paying_company_id", out var prop) &&
                            prop.ValueKind == JsonValueKind.Number)
                        {
                            ids.Add(prop.GetInt32().ToString());
                        }
                    }

                    return ids.Count > 0 ? string.Join(",", ids) : null;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public class ExpenseTypeIdResolver
     : IValueResolver<
           IntegracaoVexpenseTitulosRelatoriosStatus,
           IntegracaoVexpenseTitulosRelatoriosStatusDto,
           string?>
        {
            public string? Resolve(
                IntegracaoVexpenseTitulosRelatoriosStatus src,
                IntegracaoVexpenseTitulosRelatoriosStatusDto dest,
                string? destMember,
                ResolutionContext context)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(src.ExpensesData))
                        return null;

                    using var doc = JsonDocument.Parse(src.ExpensesData);
                    var root = doc.RootElement;

                    if (root.ValueKind != JsonValueKind.Array)
                        return null;

                    var ids = root
                        .EnumerateArray()
                        .Where(el => el.TryGetProperty("expense_type_id", out _))
                        .Select(el => el.GetProperty("expense_type_id").GetInt32().ToString())
                        .ToList();

                    return ids.Count > 0
                        ? string.Join(",", ids)
                        : null;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
