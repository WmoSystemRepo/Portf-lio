using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE.SPP;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using System.Data;
using System.Data.Common;

namespace PARS.Inhouse.Systems.Infrastructure.Repositories.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

public sealed class ConstantesPedidoEfRepository : IConstantesPedidoRepository
{
    private readonly SPPContext _ctx;
    public ConstantesPedidoEfRepository(SPPContext ctx) => _ctx = ctx;

    public async Task<ConstantesPedido> ObterAsync(int fabricanteId, string segmento, CancellationToken ct)
    {
        const string sql = @"SELECT TOP (1)
                               MetodoMargemAdobe,
                               MargemFixa
                             FROM ConstantesPedido
                             WHERE Fabricante = @Fabricante AND Segmento = @Segmento
                             ORDER BY Id DESC;";

        var conn = _ctx.Database.GetDbConnection();
        await EnsureOpenAsync(conn, ct);

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
            throw new InvalidOperationException("ConstantesPedido não encontrado para os filtros informados.");

        var metodoStr = (reader["MetodoMargemAdobe"]?.ToString() ?? "N").Trim().ToUpperInvariant();

        decimal? margemFixa = null;
        if (!(reader["MargemFixa"] is DBNull))
            margemFixa = Convert.ToDecimal(reader["MargemFixa"]);

        return new ConstantesPedido
        {
            MetodoMargemAdobe = metodoStr,
            MargemFixa = margemFixa
        };
    }

    private static async Task EnsureOpenAsync(DbConnection conn, CancellationToken ct)
    {
        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync(ct);
    }
}
