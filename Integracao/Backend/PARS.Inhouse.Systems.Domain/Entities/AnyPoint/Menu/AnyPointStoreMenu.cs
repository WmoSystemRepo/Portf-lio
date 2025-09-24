namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu
{
    public class AnyPointStoreMenu
    {
        public int Id { get; set; }

        public string? Nome { get; set; }
        public string? Rota { get; set; }
        public string? Icone { get; set; }
        public int? OrdenacaoMenu { get; set; }
        public bool? EhMenuPrincipal { get; set; }
        public int? SubMenuReferenciaPrincipal { get; set; }
        public string? DataCriacao { get; set; } 
        public string? DataEdicao { get; set; } 
    }
}
