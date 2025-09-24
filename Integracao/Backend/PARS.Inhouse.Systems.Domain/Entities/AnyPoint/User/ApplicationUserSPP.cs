using Microsoft.AspNetCore.Identity;

namespace PARS.Inhouse.Systems.Domain.Entities.Spp
{
    public class ApplicationUserSPP : IdentityUser
    {
        public bool PodeCadastrarPreco { get; set; }
        public bool PodeExcluirIndice { get; set; }
        public bool AcessoCompleto { get; set; }
    }
}
