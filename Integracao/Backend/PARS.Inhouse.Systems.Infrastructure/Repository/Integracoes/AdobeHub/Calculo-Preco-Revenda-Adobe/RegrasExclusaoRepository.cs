using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE.SPP;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using System.Data;

namespace PARS.Inhouse.Systems.Infrastructure.Repositories.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public sealed class RegrasExclusaoRepository : IRegrasExclusaoRepository
    {
        private readonly SPPContext _ctx;

        public RegrasExclusaoRepository(SPPContext ctx) => _ctx = ctx;

        public async Task<IReadOnlyList<RegraViewerDto>> ObterPorFabricanteSegmentoAsync(int fabricanteId, string segmento, CancellationToken ct)
        {
            try
            {
                const string sql = @"SELECT Tipo, ColunaTabela, Item
                             FROM vTPRegrasImplementacao
                             WHERE FabricanteId  = @Fabricante AND Segmento = @Segmento;";

                var conn = _ctx.Database.GetDbConnection();
                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync(ct);

                var regras = new List<RegraViewerDto>();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                var pFab = cmd.CreateParameter();
                pFab.ParameterName = "@Fabricante";
                pFab.Value = fabricanteId;
                cmd.Parameters.Add(pFab);

                var pSeg = cmd.CreateParameter();
                pSeg.ParameterName = "@Segmento";
                pSeg.Value = segmento ?? string.Empty;
                cmd.Parameters.Add(pSeg);

                await using var reader = await cmd.ExecuteReaderAsync(ct);
                while (await reader.ReadAsync(ct))
                {
                    var tipo = (reader["Tipo"]?.ToString() ?? string.Empty).Trim();
                    var coluna = (reader["ColunaTabela"]?.ToString() ?? string.Empty).Trim();
                    var item = (reader["Item"]?.ToString() ?? string.Empty).Trim();

                    regras.Add(new RegraViewerDto(tipo, coluna, item));
                }

                return regras;
            }
            catch (Exception ex)
            {

                throw; 
            }
        }
    }
}
