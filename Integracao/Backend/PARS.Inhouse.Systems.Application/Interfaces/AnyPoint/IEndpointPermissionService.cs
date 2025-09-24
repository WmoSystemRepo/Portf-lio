namespace PARS.Inhouse.Systems.Application.Interfaces.AnyPoint
{
    public interface IEndpointPermissionService
    {
        Task<bool> HasPermissionAsync(string userId, string acao);
        Task<bool> HasPermissionAsync(string userId, int endpointId, string acao);
    }
}
