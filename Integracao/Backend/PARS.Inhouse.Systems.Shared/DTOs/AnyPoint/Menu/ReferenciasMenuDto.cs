namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu
{
    public class ReferenciasMenuDto
    {
        public List<MenuRegraDto>? RegrasVinculadas { get; set; }
        public List<MenuUsuarioDto>? UsuariosVinculados { get; set; }
        public List<MenuIntegracaoDto>? IntegracoesVinculadas { get; set; }
        public List<MenuDto> SubMenus { get; set; }
    }
}