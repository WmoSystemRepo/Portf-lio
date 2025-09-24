using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARS.Inhouse.Systems.Application.Configurations.AnyPoint;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.VexpessesBimerInvoce;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Bimer;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Bimer;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Vexpense;
using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer;
using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;
using System.Text.Json;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.VexpenssesBimer
{
    public class IntegracaoBimerService : IIntegracaoBimerService
    {
        #region Dependências injetadas

        /// <summary>
        /// Conjunto de URLs de serviço (endpoints) obtidas via
        /// <c>IOptions&lt;OpcoesUrls&gt;</c>.
        /// <para/>
        /// Centraliza os caminhos usados pela integração para evitar
        /// “strings mágicas” espalhadas pelo código.
        /// </summary>
        private readonly OpcoesUrls _optionsUrls;

        /// <summary>
        /// Cliente que encapsula todas as chamadas à API do Bimmer,
        /// oferecendo métodos de alto nível para enviar títulos,
        /// consultar status e tratar erros de comunicação.
        /// <para/>
        /// Mantém a lógica de integração desacoplada do domínio.
        /// </summary>
        private readonly IIntegracaoBimerAPI _integracaoBimerAPI;

        /// <summary>
        /// Configurações padrão de serialização JSON usadas em todas
        /// as (de)serializações do serviço, garantindo formatação
        /// consistente (ex.: <c>PropertyNamingPolicy</c>,
        /// <c>IgnoreNullValues</c>).
        /// <para/>
        /// Padronizar a serialização evita erros de mapeamento.
        /// </summary>
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Repositório responsável por operações de persistência de
        /// <c>Titulo</c> (títulos financeiros) no banco de dados.
        /// <para/>
        /// Implementa o padrão Repository para isolar a camada de dados.
        /// </summary>
        private readonly IVexpenssesRepositorio _vexpenssesRepository;

        /// <summary>
        /// Repositório com ações específicas de contas a pagar/receber,
        /// como baixa de títulos, conciliações e busca de saldos.
        /// <para/>
        /// Facilita testes unitários ao abstrair a infraestrutura.
        /// </summary>
        private readonly IBimerRepositorio _repositorioBimer;

        /// <summary>
        /// Repositório “de-para” que mantém o mapeamento entre códigos do
        /// VExpenses e do Bimmer, evitando divergências de identificação
        /// entre sistemas.
        /// <para/>
        /// Essencial para garantir integridade nas integrações.
        /// </summary>
        private readonly IGestaoMepeamentoComposRepository _deparaRepository;

        /// <summary>
        /// Serviço dedicado a operações na API do VExpenses
        /// (consulta de relatórios, envio de tokens, etc.).
        /// <para/>
        /// Separar essa lógica facilita manutenção e testes.
        /// </summary>
        private readonly IVExpensesService _vExpensesService;

        #endregion

        public IntegracaoBimerService(IIntegracaoBimerAPI integracaoBimerAPI, IOptions<OpcoesUrls> options, HttpClient httpClient, IVexpenssesRepositorio tituloRepository, IBimerRepositorio financeiroRepository, IVExpensesService vExpensesService, IGestaoMepeamentoComposRepository deparaRepository)
        {
            _integracaoBimerAPI = integracaoBimerAPI;
            _optionsUrls = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _vexpenssesRepository = tituloRepository;
            _repositorioBimer = financeiroRepository;
            _vExpensesService = vExpensesService;
            _deparaRepository = deparaRepository;
        }

        public async Task<List<(int, string, bool)>> ProcessarEnvioTitulosParaBimmerAsync(string token, bool envioModuloManual, int[]? idsTitulos = null, CancellationToken cancellationToken = default)
        {
            var listaResultadosIntegracaoBimmer = new List<(int, string, bool)>();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var aprovadoStatusEnumValue = EnumHelper.ObterValorEnumMember(StatusRelatorioVexpensses.APROVADO);
                var listaTitulosPendentes = await ObterTodosRrgistrosTitulosPendentesAsync(cancellationToken);

                List<IntegracaoVexpenseTitulosRelatoriosStatus> listaTitulosStatusAprovadoParaProcessar;

                if (envioModuloManual)
                {
                    if (idsTitulos == null || idsTitulos.Length == 0)
                    {
                        var statusPendente = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_ID_TITULO);
                        listaResultadosIntegracaoBimmer.Add((0, await ObterMensagemAutomatizadaPendenciaAsync(statusPendente), false));
                        return listaResultadosIntegracaoBimmer;
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    await _vExpensesService.ObterRelatorioPorStatusVexpenssesAsync(aprovadoStatusEnumValue, new FiltrosDto(), listaTitulosPendentes, cancellationToken);

                    listaTitulosStatusAprovadoParaProcessar = new List<IntegracaoVexpenseTitulosRelatoriosStatus>();

                    foreach (var id in idsTitulos)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var tituloPorId = await ObterTituloAprovadoPorIdAsync(id);
                        if (tituloPorId != null)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            listaTitulosStatusAprovadoParaProcessar.Add(tituloPorId);
                        }
                        else
                        {
                            listaResultadosIntegracaoBimmer.Add((id, "Título não encontrado ou não aprovado", false));
                        }
                    }
                }
                else
                {
                    var listaTitulosAprovados = await _vexpenssesRepository.ObterTitulosAprovadosAsync();

                    listaTitulosStatusAprovadoParaProcessar = listaTitulosAprovados.Where(p => listaTitulosPendentes != null && !listaTitulosPendentes
                                                                                   .Any(x => x.IdResponse == p.IdResponse))
                                                                                   .ToList();
                }

                foreach (var tituloOriginal in listaTitulosStatusAprovadoParaProcessar)
                {
                    var titulo = tituloOriginal;

                    if (!envioModuloManual)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var tituloDb = await ObterTituloAprovadoPorIdAsync(tituloOriginal.IdResponse);

                        if (tituloDb == null)
                        {
                            listaResultadosIntegracaoBimmer.Add((tituloOriginal.IdResponse, "Título não encontrado ou não aprovado", false));
                            continue;
                        }

                        titulo = tituloDb;
                    }

                    if (envioModuloManual)
                    {
                        var tituloPendente = await ObterTituloPendentePorIdAsync(titulo.IdResponse);
                        if (tituloPendente != null)
                        {
                            listaResultadosIntegracaoBimmer.Add((titulo.IdResponse, "Título já está marcado como pendente", false));
                            continue;
                        }
                    }

                    var (pessoa, atualizados) = await ObterPessoaNoBimmerPorTituloAsync(token, listaResultadosIntegracaoBimmer, titulo.IdResponse, titulo);
                    listaResultadosIntegracaoBimmer = atualizados;

                    if (pessoa == null)
                    {
                        var statusPendente = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_USUARIO);

                        cancellationToken.ThrowIfCancellationRequested();
                        await AlterarStatusRelatoioParaPendenteVexpenssesAnyPointAsync(titulo, statusPendente);
                        listaResultadosIntegracaoBimmer.Add((titulo.IdResponse, await ObterMensagemAutomatizadaPendenciaAsync(statusPendente), false));
                        continue;
                    }

                    if (envioModuloManual && !await ValidarValorTituloOuRegistrarPendenciaAsync(titulo))
                    {
                        listaResultadosIntegracaoBimmer.Add((titulo.IdResponse, "Valor não informado", false));
                        continue;
                    }

                    var bimmerDto = ConstruirRequestIntegracaoBimmer(titulo, pessoa.Identificador);
                    var despesas = ExtrairDespesasDoJsonTitulo(titulo);

                    var sucesso = await MapearDespesasParaRequestBimmerAsync(listaResultadosIntegracaoBimmer, titulo.IdResponse, titulo, bimmerDto, despesas);
                    if (!sucesso)
                        continue;

                    cancellationToken.ThrowIfCancellationRequested();
                    var resultado = await RegistrarTituloAPagarNoBimmerAsync(bimmerDto, token);

#if !DEBUG
                        if (resultado?.Erros.Count == 0)
                        {
                            await FinalizarIntegracaoTituloPagoAsync(
                                listaResultadosIntegracaoBimmer, 
                                titulo.IdResponse, 
                                titulo, 
                                resultado, 
                                bimmerDto, 
                                cancellationToken);
                        }
                        else
                        {
                            await RegistrarHistoricoDeErroNaIntegracaoAsync(titulo, resultado);
                        }
#endif

                }

                await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();
            }
            catch (Exception ex)
            {
                if (envioModuloManual && idsTitulos != null)
                    await RegistrarFalhasGlobaisIntegracaoAsync(idsTitulos, ex);

                throw;
            }

            return listaResultadosIntegracaoBimmer;
        }

        public async Task<TitlePayResponseDto?> RegistrarTituloAPagarNoBimmerAsync(BimmerRequestRequiredFieldsDto bimerRequestDto, string token, CancellationToken cancellationToken = default)
        {
            try
            {
                var uri = _optionsUrls.Bimer;
                var content = JsonConvert.SerializeObject(bimerRequestDto, Formatting.Indented);

                cancellationToken.ThrowIfCancellationRequested();
                var reports = await _integracaoBimerAPI.CriarTituloAPagar(content, uri, token);
                return System.Text.Json.JsonSerializer.Deserialize<TitlePayResponseDto>(reports, _jsonSerializerOptions);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro durante a criação de Titulo a pagar! Detalhes do erro: {ex.Message}");
            }
        }

        public async Task<AuthResponseDto> AutenticarNoBimmerAsync(AuthRequestDto requestDto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var uri = _optionsUrls.TokenServico;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", requestDto.ClientId),
                new KeyValuePair<string, string>("client_secret", requestDto.ClientSecret),
                new KeyValuePair<string, string>("grant_type", requestDto.GrantType),
                new KeyValuePair<string, string>("username", requestDto.Username),
                new KeyValuePair<string, string>("password", requestDto.Password),
                new KeyValuePair<string, string>("nonce", requestDto.Nonce)
            });

            var response = await _integracaoBimerAPI.AuthenticateAsync(content, uri);
            var responseString = response;

            var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(responseString, _jsonSerializerOptions);
            return authResponse ?? new AuthResponseDto
            {
                access_token = string.Empty,
                token_type = string.Empty,
                expires_in = 0,
                refresh_token = string.Empty,
                username = string.Empty
            };
        }

        public async Task<AuthResponseDto> RenovarAutenticacaoNoBimmerAsync(ReauthenticateRequestDto request, CancellationToken cancellationToken = default)
        {
            var uri = _optionsUrls.TokenServico;
            var content = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string, string>("client_id", request.client_id),
                        new KeyValuePair<string, string>("grant_type", request.grant_type),
                        new KeyValuePair<string, string>("refresh_token", request.refresh_token)
                    });

            var response = await _integracaoBimerAPI.ReauthenticateAsync(content, uri);
            var responseString = response;

            var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(responseString, _jsonSerializerOptions);
            return authResponse ?? new AuthResponseDto
            {
                access_token = string.Empty,
                token_type = string.Empty,
                expires_in = 0,
                refresh_token = string.Empty,
                username = string.Empty
            };
        }

        public async Task<PessoaResponseDto> ObterBimerIdentificadorPessoaPorCpfAsync(string cpf, string token, CancellationToken cancellationToken = default)
        {
            try
            {
                var uri = string.Format("{0}{1}", _optionsUrls.Pessoa, cpf);

                var response = await _integracaoBimerAPI.ObterBimerIdentificadorPessoaPorCPF(uri, token);
                var responseString = response;

                var pessoaResponse = System.Text.Json.JsonSerializer.Deserialize<PessoaResponseDto>(responseString, _jsonSerializerOptions);

                if (pessoaResponse?.ListaObjetos != null)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var identificadorObj = pessoaResponse.ListaObjetos.FirstOrDefault();

                    pessoaResponse.Identificador = identificadorObj?.Identificador ?? string.Empty;
                }
                else
                {
                    pessoaResponse ??= new PessoaResponseDto();
                    pessoaResponse.Identificador = string.Empty;
                }

                return pessoaResponse;
            }
            catch (Exception ex)
            {
                var mensagemErro = $"❌ Erro ao obter identificador da pessoa pelo CPF '{cpf}': {ex.Message} ==> {ex.Message}";
                throw new ApplicationException(mensagemErro, ex);
            }
        }

        private async Task AlterarStatusRelatoioParaPendenteVexpenssesAnyPointAsync(IntegracaoVexpenseTitulosRelatoriosStatus titulo, string statusPendente)
        {
            try
            {
                titulo.Status = statusPendente;
                await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();

                await RegistrarTitulosPendentesAsync(titulo, statusPendente);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task RegistrarTitulosPendentesAsync(IntegracaoVexpenseTitulosRelatoriosStatus titulo, string statusPendente)
        {
            try
            {
                await _repositorioBimer.RegistrarTitulosPendentesAsync(new IntegracaoBimmerInsercaoPendentes
                {
                    IdResponse = titulo.IdResponse,
                    UserId = titulo.User_id,
                    Descricao = titulo.Description,
                    Valor = titulo.ExpensesTotalValue,
                    DataCadastro = DateTime.Now,
                    Observacao = await ObterMensagemAutomatizadaPendenciaAsync(statusPendente),
                    Status = statusPendente
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> ObterMensagemAutomatizadaPendenciaAsync(string statusPendente)
        {
            string mensagemRetornada = statusPendente switch
            {
                var s when s == EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_USUARIO) =>
                    "👤 Usuário não identificado! O título foi marcado como pendente.",

                var s when s == EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_VALOR_DESPESA) =>
                    "💰 Valor da despesa não informado! Verifique o lançamento antes de prosseguir.",

                var s when s == EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_ID_TITULO) =>
                    "🆔 ID do título ausente! Nenhum envio manual foi possível.",

                _ => string.Empty
            };

            return await Task.FromResult(mensagemRetornada);
        }


        private async Task FinalizarIntegracaoTituloPagoAsync(List<(int, string, bool)> retornosBimmer, int idTitulo, IntegracaoVexpenseTitulosRelatoriosStatus titulo, TitlePayResponseDto resultado, BimmerRequestRequiredFieldsDto bimmerDto, CancellationToken cancellationToken)
        {
            try
            {
                string? identificador = await RegistrarTituloComoPagoAsync(titulo, resultado);
                if (identificador != null)
                {
                    retornosBimmer.Add((idTitulo, identificador, true));
                }

                string paymentDateFormatted = bimmerDto.DataVencimento.ToString("yyyy-MM-dd HH:mm:ss");
                var jsonResponse = await _vExpensesService.AlterarStatusAsync(idTitulo, new AlteraStatus
                {
                    payment_date = bimmerDto.DataVencimento.ToString("yyyy-MM-dd HH:mm:ss"),
                    comment = $"Integração confirmou o pagamento deste relatório {idTitulo} - {bimmerDto.DataVencimento.ToString("dd/MM/yyyy")}"
                }, cancellationToken);

                if (!jsonResponse.Item1)
                {
                    await _repositorioBimer.RegistrarTitulosPendentesAsync(new IntegracaoBimmerInsercaoPendentes
                    {
                        IdResponse = titulo.IdResponse,
                        UserId = titulo.User_id,
                        Descricao = titulo.Description,
                        Valor = titulo.ExpensesTotalValue,
                        DataCadastro = DateTime.Now,
                        Observacao = "Inserido no Bimmer porém status não foi alterado no VExpenses",
                        Response = jsonResponse.Item2
                    });

                    titulo.Status = "PENDENTE";
                    await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task RegistrarFalhasGlobaisIntegracaoAsync(int[] idsTitulos, Exception ex)
        {
            var erro = new Error
            {
                ErrorCode = "Exception",
                ErrorMessage = ex.Message,
                PossibleCause = "Unknown",
                StackTrace = ex.StackTrace ?? string.Empty
            };

            var erroResponse = new TitlePayResponseDto
            {
                Erros = new List<Error> { erro },
                ListaObjetos = new List<string> { "Erro" }
            };

            foreach (var id in idsTitulos)
            {
                var titulo = await ObterTituloAprovadoPorIdAsync(id);
                await RegistrarHistoricoDeErroNaIntegracaoAsync(titulo, erroResponse);
            }
        }

        private async Task<bool> MapearDespesasParaRequestBimmerAsync(
            List<(int, string, bool)> retornosBimmer,
            int idTitulo,
            IntegracaoVexpenseTitulosRelatoriosStatus iserirTituloBimmer,
            BimmerRequestRequiredFieldsDto bimerRequestDto,
            List<DespesaTituloDto> listaDespesasTitulos)
        {
            try
            {
                var sucesso = true;

                foreach (var despesaTitulo in listaDespesasTitulos)
                {
                    var tipoPagamento = await ValidarOuRegistrarPendenciaTipoPagamentoAsync(iserirTituloBimmer, despesaTitulo.PaymentMethodId);
                    if (tipoPagamento == null)
                    {
                        retornosBimmer.Add((idTitulo, $"Tipo de Pagamento não encontrado para PaymentMethodId: {despesaTitulo.PaymentMethodId}", false));
                        iserirTituloBimmer.Status = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_TIPO_PAGAMENTO);
                        sucesso = false;
                        continue;
                    }

                    bimerRequestDto.IdentificadorFormaPagamento = tipoPagamento.ValorDestino;

                    var naturezaLancamentoItem = await ValidarOuRegistrarPendenciaNaturezaLancamentoAsync(iserirTituloBimmer, despesaTitulo.ExpenseTypeId);
                    if (naturezaLancamentoItem == null)
                    {
                        retornosBimmer.Add((idTitulo, $"Natureza lançamento não encontrada para ExpenseTypeId: {despesaTitulo.ExpenseTypeId}", false));
                        iserirTituloBimmer.Status = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_NATUREZA_LANCAMENTO);
                        sucesso = false;
                        continue;
                    }

                    var identificadorCentroDeCusto = await ValidarOuRegistrarPendenciaCentroDeCustoAsync(iserirTituloBimmer, despesaTitulo.PayingCompanyId);
                    if (identificadorCentroDeCusto == null)
                    {
                        retornosBimmer.Add((idTitulo, $"Identificador Centro de Custo não encontrado para PayingCompanyId: {despesaTitulo.PayingCompanyId}", false));
                        iserirTituloBimmer.Status = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_CENTRO_CUSTO);
                        iserirTituloBimmer.Status = "PENDENTE-CENTRO-CUSTO";
                        sucesso = false;
                        continue;
                    }

                    bimerRequestDto.Itens.Add(new ItemDto
                    {
                        Valor = despesaTitulo.Valor,
                        IdentificadorNaturezaLancamento = naturezaLancamentoItem.ValorDestino,
                        Observacao = despesaTitulo.Title + " - " + iserirTituloBimmer.IdResponse,
                        CentrosDeCusto = new List<CentroDeCustoDto>
                        {
                            new CentroDeCustoDto
                            {
                                IdentificadorCentroDeCusto = identificadorCentroDeCusto.ValorDestino,
                                ValorLancamento = despesaTitulo.Valor,
                                DataLancamento = DateTime.Now,
                                AliquotaPorcentagem = 100
                            }
                        }
                    });
                }

                if (!sucesso)
                {

                    await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();
                }

                return sucesso;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static List<DespesaTituloDto> ExtrairDespesasDoJsonTitulo(IntegracaoVexpenseTitulosRelatoriosStatus iserirTituloBimmer)
        {
            try
            {
                var ploudTitulo = !string.IsNullOrEmpty(iserirTituloBimmer.ExpensesData)
                ? JArray.Parse(iserirTituloBimmer.ExpensesData)
                : new JArray();

                return ploudTitulo.Select(jToken => new DespesaTituloDto
                {
                    Valor = jToken["value"]?.Value<decimal>() ?? 0m,
                    ExpenseTypeId = jToken["expense_type_id"]?.Value<string>(),
                    PayingCompanyId = jToken["paying_company_id"]?.Value<string>(),
                    PaymentMethodId = jToken["payment_method_id"]?.Value<string>(),
                    Observation = jToken["observation"]?.Value<string>(),
                    Title = jToken["title"]?.Value<string>(),
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<(PessoaResponseDto? pessoa, List<(int, string, bool)> retornos)> ObterPessoaNoBimmerPorTituloAsync(string token, List<(int, string, bool)> retornosBimmer, int idTitulo, IntegracaoVexpenseTitulosRelatoriosStatus? titulo, CancellationToken cancellationToken = default)
        {
            try
            {
                var idUser = titulo?.User_id ?? 0;
                if (idUser == 0)
                {
                    retornosBimmer.Add((idTitulo, "Id do usuário não encontrado", false));
                    return (null, retornosBimmer);
                }

                var resultado = await _vExpensesService.BuscarUsuarioPorIdVexpenssesAsync(idUser, cancellationToken);
                var (cpfEncontrado, usuarioJson) = resultado;

                if (!cpfEncontrado)
                {
                    await RegistrarPendenciaPorCpfNaoEncontradoAsync(titulo);
                    retornosBimmer.Add((idTitulo, $"Cpf do usuário não encontrado: {idUser}", false));
                    return (null, retornosBimmer);
                }

                var jsonResponseUsuario = JsonConvert.DeserializeObject<ApiResponseTeamMember?>(usuarioJson);
                if (jsonResponseUsuario?.Data?.Cpf == null)
                {
                    retornosBimmer.Add((idTitulo, "CPF inválido ou ausente no JSON de resposta", false));
                    return (null, retornosBimmer);
                }

                var pessoa = await ObterBimerIdentificadorPessoaPorCpfAsync(jsonResponseUsuario.Data.Cpf, token);
                return (pessoa, retornosBimmer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<IntegracaoBimmerInsercaoPendentes?> ObterTituloPendentePorIdAsync(int idTitulo)
        {
            try
            {
                return await _vexpenssesRepository.ObterPendentePorIdResponseAsync(idTitulo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<IntegracaoVexpenseTitulosRelatoriosStatus?> ObterTituloAprovadoPorIdAsync(int idTitulo)
        {
            try
            {
                return await _vexpenssesRepository.ObterTituloAprovadoPorIdAsync(idTitulo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<IntegracaoBimmerInsercaoPendentes>?> ObterTodosRrgistrosTitulosPendentesAsync(CancellationToken cancellationToken)
        {
            return await _vexpenssesRepository.RecuperarTodosRegistrosPendentesAsync();
        }

        private async Task RegistrarPendenciaPorCpfNaoEncontradoAsync(IntegracaoVexpenseTitulosRelatoriosStatus? tituloParaInserirNoBimmer)
        {
            try
            {
                if (tituloParaInserirNoBimmer == null)
                {
                    throw new ArgumentNullException(nameof(tituloParaInserirNoBimmer), "O título para inserir no Bimmer não pode ser nulo.");
                }

                await _repositorioBimer.RegistrarTitulosPendentesAsync(new IntegracaoBimmerInsercaoPendentes
                {
                    IdResponse = tituloParaInserirNoBimmer.IdResponse,
                    UserId = tituloParaInserirNoBimmer.User_id,
                    Descricao = tituloParaInserirNoBimmer.Description,
                    Valor = tituloParaInserirNoBimmer.ExpensesTotalValue,
                    DataCadastro = DateTime.Now,
                    Observacao = "Cpf do usuário não encontrado",
                    Status = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_USUARIO),
                });
                await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task RegistrarHistoricoDeErroNaIntegracaoAsync(IntegracaoVexpenseTitulosRelatoriosStatus? item, TitlePayResponseDto? resultado)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentNullException(nameof(item), "O idTitulo não pode ser nulo.");
                }

                await _vexpenssesRepository.RegistrarHistoricoInsercaoComErroAsync(new IntegracaoBimmerHistoricoErrosInsercoes
                {
                    IdResponse = item.IdResponse,
                    Tentativas = 1,
                    MensagemErro = string.Join(", ", resultado?.Erros.FirstOrDefault()?.ErrorMessage ?? "Erro desconhecido"),
                    DataTentativa = DateTime.Now
                });
                await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string?> RegistrarTituloComoPagoAsync(IntegracaoVexpenseTitulosRelatoriosStatus item, TitlePayResponseDto? resultado)
        {
            try
            {
                var identificadorBimmer = resultado?.ListaObjetos?.FirstOrDefault();

                if (identificadorBimmer == null)
                {
                    return null;
                }

                var responseJson = System.Text.Json.JsonSerializer.Serialize(resultado);

                item.Status = "PAGO";
                await _vexpenssesRepository.RegistrarTituloPagoAsync(new IntegracaoBimmerInsertOK
                {
                    IdResponse = item.IdResponse,
                    IdentificadorBimmer = identificadorBimmer,
                    Valor = item.ExpensesTotalValue,
                    DataCadastro = DateTime.Now,
                    Observacao = "Integrado com sucesso no Bimmer",
                    Response = responseJson
                });
                await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();
                return identificadorBimmer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> ValidarValorTituloOuRegistrarPendenciaAsync(IntegracaoVexpenseTitulosRelatoriosStatus titulo)
        {
            try
            {
                var valorInvalido = titulo.ExpensesTotalValue == 0;

                if (!valorInvalido)
                {
                    return true;
                }

                var statusPendente = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_VALOR_DESPESA);
                await RegistrarTitulosPendentesAsync(titulo, statusPendente);

                titulo.Status = statusPendente;
                await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();

                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static BimmerRequestRequiredFieldsDto ConstruirRequestIntegracaoBimmer(IntegracaoVexpenseTitulosRelatoriosStatus idTitulo, string? identificador)
        {
            try
            {
                if (idTitulo == null)
                {
                    throw new ArgumentNullException(nameof(idTitulo), "O parâmetro 'idTitulo' não pode ser nulo.");
                }

                var descricao = idTitulo.Description?.Length > 50
                    ? idTitulo.Description.Substring(0, 50)
                    : idTitulo.Description ?? string.Empty;

                var bimerRequestDto = new BimmerRequestRequiredFieldsDto
                {
                    IdentificadorNaturezaLancamento = string.Empty,
                    Valor = idTitulo.ExpensesTotalValue ?? 0,
                    Numero = idTitulo.IdResponse.ToString(),
                    Descricao = descricao,
                    CodigoEmpresa = CodigoEmpresaEnum.CodigoEmpresa01,
                    DataEmissao = DateTime.Now,
                    Previsao = true,
                    IdentificadorPessoa = identificador,
                    IdentificadorSituacaoAdministrativa = "00A0000019",
                    Observacao = (idTitulo.Description ?? string.Empty) + " - " + idTitulo.IdResponse,
                    DataVencimento = DateTime.Now.AddDays(7),
                    Itens = new List<ItemDto?>()
                };

                return bimerRequestDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<AnyPointDeparas?> ValidarOuRegistrarPendenciaNaturezaLancamentoAsync(IntegracaoVexpenseTitulosRelatoriosStatus item, string? expenseTypeId)
        {
            var result = await _deparaRepository.GetDeparaAsync(x =>
                x.Integracao == "VEXPENSE_BIMMER" &&
                x.ValorOrigem == expenseTypeId &&
                x.TipoExecucao == "API" &&
                x.CampoOrigem == "EXPENSE_TYPE_ID");

            var depara = await _vexpenssesRepository.RecuperarNaturezaLancamentoVexpenseAsync(expenseTypeId);

            if (depara == null)
            {
                await _repositorioBimer.RegistrarTitulosPendentesAsync(new IntegracaoBimmerInsercaoPendentes
                {
                    IdResponse = item.IdResponse,
                    UserId = item.User_id,
                    Descricao = item.Description,
                    Valor = item.ExpensesTotalValue,
                    DataCadastro = DateTime.Now,
                    Status = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_NATUREZA_LANCAMENTO),
                    Observacao = "⚠️ Possível causa da pendência: natureza de lançamento não encontrada ou não mapeada no de-para.",
                    Response = FormatarObservacaoDeparaLog(depara)
                });

                await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();
                return null;
            }

            return depara;
        }


        private async Task<AnyPointDeparas?> ValidarOuRegistrarPendenciaCentroDeCustoAsync(IntegracaoVexpenseTitulosRelatoriosStatus item, string? payingCompanyId)
        {
            var depara = await _vexpenssesRepository.RecuperarMapeamentoCentroDeCustoAsync(payingCompanyId);

            bool isPendencia = depara == null ||
                               !string.IsNullOrEmpty(depara.ValorDestino) &&
                                depara.ValorDestino.Contains("-", StringComparison.OrdinalIgnoreCase);

            if (isPendencia)
            {
                await _repositorioBimer.RegistrarTitulosPendentesAsync(new IntegracaoBimmerInsercaoPendentes
                {
                    IdResponse = item.IdResponse,
                    UserId = item.User_id,
                    Descricao = item.Description,
                    Valor = item.ExpensesTotalValue,
                    DataCadastro = DateTime.Now,
                    Status = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_CENTRO_CUSTO),
                    Observacao = "⚠️ Possível causa da pendência: mapeamento não encontrado ou ValorDestino contém caractere inválido '-'.",
                    Response = FormatarObservacaoDeparaLog(depara)
                });

                await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();
                return null;
            }

            return depara;
        }

        private async Task<AnyPointDeparas?> ValidarOuRegistrarPendenciaTipoPagamentoAsync(IntegracaoVexpenseTitulosRelatoriosStatus item, string? paymentMethodId)
        {
            var depara = await _deparaRepository.GetDeparaAsync(x =>
                x.Integracao == "VEXPENSE_BIMMER" &&
                x.ValorOrigem == paymentMethodId &&
                x.TipoExecucao == "API" &&
                x.CampoOrigem == "PAYMENT_METHOD_ID");

            if (depara == null)
            {
                await _repositorioBimer.RegistrarTitulosPendentesAsync(new IntegracaoBimmerInsercaoPendentes
                {
                    IdResponse = item.IdResponse,
                    UserId = item.User_id,
                    Descricao = item.Description,
                    Valor = item.ExpensesTotalValue,
                    DataCadastro = DateTime.Now,
                    Status = EnumHelper.ObterValorEnumMember(StatusPendenciasVexpenses.PENDENTE_TIPO_PAGAMENTO),
                    Observacao = "⚠️ Possível causa da pendência: mapeamento de tipo de pagamento não encontrado.",
                    Response = FormatarObservacaoDeparaLog(depara),
                });

                await _vexpenssesRepository.SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async();
                return null;
            }

            return depara;
        }

        private static string FormatarDeparaLog(AnyPointDeparas? depara)
        {
            if (depara == null)
            {
                return "Depara ==> Objeto nulo.";
            }

            return @$"Depara ==> 
                   Id: {depara.Id} 
                   IdCampos: {depara.IdCampos}
                   TipoExecucao: {depara.TipoExecucao} 
                   Integracao: {depara.Integracao} 
                   NomeMapeamentoOrigem: {depara.NomeMapeamentoOrigem}
                   CampoOrigem: {depara.CampoOrigem}
                   CodigoSistemaOrigem: {depara.CodigoSistemaOrigem}
                   ValorOrigem: {depara.ValorOrigem}
                   NomeMapeamentoDestino: {depara.NomeMapeamentoDestino} 
                   CampoDestino: {depara.CampoDestino} 
                   CodigoSistemaDestino: {depara.CodigoSistemaDestino}
                   ValorDestino: {depara.ValorDestino}";
        }

        private static string FormatarObservacaoDeparaLog(AnyPointDeparas? depara)
        {
            if (depara == null)
            {
                return "Depara ==> Objeto nulo.";
            }

            return @$"Depara ==> 
                   Id: {depara.Id} 
                   IdCampos: {depara.IdCampos}
                   TipoExecucao: {depara.TipoExecucao} 
                   Integracao: {depara.Integracao} 
                   NomeMapeamentoOrigem: {depara.NomeMapeamentoOrigem}
                   CampoOrigem: {depara.CampoOrigem}
                   CodigoSistemaOrigem: {depara.CodigoSistemaOrigem}
                   ValorOrigem: {depara.ValorOrigem}
                   NomeMapeamentoDestino: {depara.NomeMapeamentoDestino} 
                   CampoDestino: {depara.CampoDestino} 
                   CodigoSistemaDestino: {depara.CodigoSistemaDestino}
                   ValorDestino: {depara.ValorDestino}";
        }

        public async Task<IReadOnlyList<IntegracaoBimmerInsercaoPendentes>> ObterPendenciasDeIntegracaoAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repositorioBimer.GetPendenciasAsync(pageNumber, pageSize, search, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<IntegracaoBimmerHistoricoErrosInsercoes>> ObterHistoricoErrosDeIntegracaoAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _vexpenssesRepository.RecuperarHistoricoErrosIntegracaoAsync(pageNumber, pageSize, search);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<IntegracaoBimmerInsertOK>> ObterInsercoesBemSucedidasDeIntegracaoComPaginamentoAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _vexpenssesRepository.RecuperarTitulosEnviadosComSucessoAsync(pageNumber, pageSize, search);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> ExcluirPendenciasAsync(int[] ids, CancellationToken cancellationToken)
        {
            await _vexpenssesRepository.ExcluirPendenciasPorIdsAsync(ids, cancellationToken);
            return ids.Length;
        }
    }
}