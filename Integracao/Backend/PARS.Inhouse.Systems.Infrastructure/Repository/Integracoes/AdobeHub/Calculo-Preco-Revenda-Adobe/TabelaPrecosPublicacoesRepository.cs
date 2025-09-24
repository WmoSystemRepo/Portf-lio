using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE.SPP;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using System.Data;

public sealed class TabelaPrecosPublicacoesRepository : ITabelaPrecosPublicacoesRepository
{
    private readonly SPPContext _ctx;

    public TabelaPrecosPublicacoesRepository(SPPContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<TabelaPrecoDto?> ObterConfiguracaoValidaAsync(int fabricante, CancellationToken ct)
    {
        try
        {
            var hoje = DateTime.UtcNow.Date;

            // Consulta apenas pela data
            var existeVigente = await _ctx.TabelaPrecosPublicacoes.AnyAsync(t =>
                t.FabricanteId == fabricante &&
                t.DataInicioValidade <= hoje &&
                (t.DataFimValidade == null || t.DataFimValidade >= hoje),
                ct);

            if (!existeVigente)
                return null;

            const string constantesSql = @" SELECT TOP (1) MetodoMargemAdobe
                                            FROM ConstantesPedido
                                            ORDER BY ConstanteId DESC;";


            const string indicesSql = @"
                SELECT TOP (1)
                    CustoOperacional,
                    PIS,
                    COFINS,
                    ISS,
                    Marketing,
                    Outros,
                    MargemMinima,
                    ProdNivel1,
                    OutrosProd
                FROM TabelaPrecosIndicesAdobe
                ORDER BY CustoOperacional DESC;";

            var conn = _ctx.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            string metodoMargem = "N";

            // Consulta ConstantesPedido
            await using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = constantesSql;
                cmd.CommandType = CommandType.Text;

                await using var reader = await cmd.ExecuteReaderAsync(ct);
                if (await reader.ReadAsync(ct))
                {
                    metodoMargem = (reader["MetodoMargemAdobe"]?.ToString() ?? "N").Trim().ToUpperInvariant();
                }
                else
                {
                    return null;
                }
            }

            // Consulta TabelaPrecosIndicesAdobe
            await using var cmd2 = conn.CreateCommand();
            cmd2.CommandText = indicesSql;
            cmd2.CommandType = CommandType.Text;

            await using var reader2 = await cmd2.ExecuteReaderAsync(ct);
            if (await reader2.ReadAsync(ct))
            {
                decimal GetDecimal(string col) => reader2[col] is DBNull ? 0m : Convert.ToDecimal(reader2[col]);
                decimal? GetDecimalNullable(string col) => reader2[col] is DBNull ? (decimal?)null : Convert.ToDecimal(reader2[col]);

                return new TabelaPrecoDto
                {
                    MetodoMargemAdobe = metodoMargem,
                    MargemFixa = null, // não tem essa coluna na base
                    CustoOperacional = GetDecimal("CustoOperacional"),
                    PIS = GetDecimal("PIS"),
                    COFINS = GetDecimal("COFINS"),
                    ISS = GetDecimal("ISS"),
                    Marketing = GetDecimal("Marketing"),
                    Outros = GetDecimal("Outros"),
                    MargemMinima = GetDecimalNullable("MargemMinima"),
                    ProdNivel1 = GetDecimal("ProdNivel1"),
                    OutrosProd = GetDecimal("OutrosProd")
                };
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}
