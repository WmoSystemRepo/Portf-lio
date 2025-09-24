using Microsoft.AspNetCore.Identity;

namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.User
{
    public class ApplicationUser : IdentityUser
    {
        public bool FirstLogin { get; set; }
        public bool PodeLer { get; set; }
        public bool PodeEscrever { get; set; }
        public bool PodeRemover { get; set; }
        public bool PodeVerConfiguracoes { get; set; }
    }
}
