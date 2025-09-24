using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE.SPP;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using System.Data;

namespace PARS.Inhouse.Systems.Infrastructure.Repositories.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public sealed class IndicesAdobeRepository : IIndicesAdobeRepository
    {
        private readonly SPPContext _ctx;

        public IndicesAdobeRepository(SPPContext ctx) => _ctx = ctx;

        public async Task<(decimal PIS, decimal COFINS, decimal ISS, decimal CustoOperacional, decimal ProdNivel1, decimal OutrosProd, decimal? MargemMinima)>
            ObterAsync(int fabricanteId, string segmento, CancellationToken ct)
        {
            const string sql = @"SELECT TOP (1)
                                     PIS,
                                     COFINS,
                                     ISS,
                                     CustoOperacional,
                                     ProdNivel1,
                                     OutrosProd,
                                     MargemMinima
                                 FROM TabelaPrecosIndicesAdobe
                                 WHERE Fabricante = @Fabricante AND Segmento = @Segmento
                                 ORDER BY Id DESC;";

            var conn = _ctx.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

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
            if (!await reader.ReadAsync(ct))
                throw new InvalidOperationException("Índices Adobe não encontrados para os filtros informados.");

            decimal GetDec(string col) => reader[col] is DBNull ? 0m : Convert.ToDecimal(reader[col]);
            decimal? GetDecNull(string col) => reader[col] is DBNull ? (decimal?)null : Convert.ToDecimal(reader[col]);

            return (
                GetDec("PIS"),
                GetDec("COFINS"),
                GetDec("ISS"),
                GetDec("CustoOperacional"),
                GetDec("ProdNivel1"),
                GetDec("OutrosProd"),
                GetDecNull("MargemMinima")
            );
        }
    }
}
