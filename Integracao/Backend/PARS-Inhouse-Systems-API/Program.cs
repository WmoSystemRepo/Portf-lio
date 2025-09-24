using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using PARS.Inhouse.Systems.Application.BackgroundServices;
using PARS.Inhouse.Systems.Application.Configurations.AnyPoint;
using PARS.Inhouse.Systems.Application.Configurations.Integrcoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Menu;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Importar_Excel;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Templantes_Planilhas;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.SppBimerInvoice;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.VexpessesBimerInvoce;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Application.Services.Anypoint;
using PARS.Inhouse.Systems.Application.Services.Anypoint.Menu;
using PARS.Inhouse.Systems.Application.Services.Anypoint.Permicoes;
using PARS.Inhouse.Systems.Application.Services.Anypoint.Usuario;
using PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Importar_Excel;
using PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Templantes_Planinhas;
using PARS.Inhouse.Systems.Application.Services.Integracoes.Bacen;
using PARS.Inhouse.Systems.Application.Services.Integracoes.Bacen.Jobs;
using PARS.Inhouse.Systems.Application.Services.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Application.Services.Integracoes.SppBimerInvoice;
using PARS.Inhouse.Systems.Application.Services.Integracoes.VexpenssesBimer;
using PARS.Inhouse.Systems.Infrastructure.APIs.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE.SPP;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Menu;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.TipoTemplate;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Infrastructure.Repositories.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub.LogsErros;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.Bacen;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.Bacen.ErrosIntegracoes;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ExclusaoPendentes;
using PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint;
using PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.Menu;
using PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.TipoTemplate;
using PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS_Inhouse_Systems_API.Config;
using PARS_Inhouse_Systems_API.Config.AnyPonit;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

bool isMigrationOnly = args.Contains("--migrate");
bool shouldApplyMigrations = isMigrationOnly ||
    Environment.GetEnvironmentVariable("ASPNETCORE_APPLY_MIGRATIONS") == "true";

// Configura fontes de configuração
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Configura o contexto EF Core
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ParsInhouseContext") 
        ?? throw new InvalidOperationException("Connection string 'ParsInhouseContext' not found.")));

builder.Services.AddDbContext<SPPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SPPConnection") 
        ?? throw new InvalidOperationException("Connection string 'SPPConnection' not found.")));

// Apenas se não for modo migração
if (!isMigrationOnly)
{
    builder.Services.ConfigureIdentity();
    builder.Services.ConfigureJwtAuthentication(builder.Configuration);

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
    builder.Services.AddScoped<AdobeTemplantesExcelMongoService>();
    builder.Services.AddScoped<ImportHistoryService>();
    builder.Services.AddScoped<AdobePlanilhaService>();

    builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbAdobeTemplate"));

    builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbAdobeImportarPlaninhas"));

    builder.Services.AddScoped<MongoAdobeHubLogExclusaoPlanilhaRepository>();
    builder.Services.AddScoped<MongoAdobeHubLogExclusaoTemplateRepository>();
    builder.Services.AddSingleton<MongoVexpensesBimmerExclusaoLogRepository>();
    builder.Services.AddSingleton<MongoAdobeTemplateDbContext>();
    builder.Services.AddScoped<IntegracaoAdobeHubLogErroRepository>();
    builder.Services.AddScoped<IntegracaoBacenLogErroRepository>();
    builder.Services.AddScoped<IAdobeTemplantesExcelMongoRepository, AdobeTemplantesExcelMongoDbRepository>();
    builder.Services.AddSingleton<MongoClient>(sp =>
    {
        var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        return new MongoClient(settings.ConnectionString);
    });

    builder.Services.AddScoped<IMongoDatabase>(sp =>
    {
        var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        var client = sp.GetRequiredService<MongoClient>();
        return client.GetDatabase(settings.DatabaseName);
    });

    builder.Services.AddScoped<ITemplatePlanilhaService, AdobePlanilhaService>();
    builder.Services.AddScoped<IImportacaoPlanilhaService, AdobePlanilhaService>();

    builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("ParsInhouseContext"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

    builder.Services.AddHangfireServer();
}

var configuration = builder.Configuration;

// Em modo migração, adiciona apenas serviços mínimos
if (!isMigrationOnly)
{
    ConfigureServices(builder.Services, configuration);
}
else
{
    builder.Services.AddLogging(logging =>
    {
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    });
}

var app = builder.Build();

// Executa migrações se necessário
if (shouldApplyMigrations)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Environment: {Environment}", builder.Environment.EnvironmentName);
            logger.LogInformation("Configuration sources loaded:");

            foreach (var provider in ((IConfigurationRoot)builder.Configuration).Providers)
            {
                logger.LogInformation("- {ProviderType}", provider.GetType().Name);
            }

            var connectionString = builder.Configuration.GetConnectionString("ParsInhouseContext");

            var maskedConnectionString = string.IsNullOrEmpty(connectionString)
                ? "NULL CONNECTION STRING"
                : connectionString;

            if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Senha="))
            {
                var passwordPart = connectionString.Substring(connectionString.IndexOf("Senha="));
                var endIndex = passwordPart.Contains(";") ? passwordPart.IndexOf(";") : passwordPart.Length;
                var passwordValue = passwordPart.Substring(0, endIndex);
                maskedConnectionString = connectionString.Replace(passwordValue, "Senha=*******");
            }

            logger.LogInformation("Using connection string: {ConnectionString}", maskedConnectionString);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'ParsInhouseContext' not found.");
            }

            logger.LogInformation("Testing database connection...");
            var context = services.GetRequiredService<Context>();

            if (context.Database.CanConnect())
            {
                logger.LogInformation("Successfully connected to database");
            }
            else
            {
                logger.LogWarning("Unable to connect to database");
            }

            logger.LogInformation("Applying database migrations...");
            context.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully!");

            if (isMigrationOnly)
            {
                logger.LogInformation("Migration-only mode detected. Exiting application.");
                return;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");

            var currentEx = ex;
            int exceptionDepth = 0;
            while (currentEx.InnerException != null)
            {
                exceptionDepth++;
                currentEx = currentEx.InnerException;
                logger.LogError("Inner exception {Depth}: {Type} - {Message}",
                    exceptionDepth, currentEx.GetType().Name, currentEx.Message);
            }

            if (isMigrationOnly)
            {
                logger.LogError("Migration failed, exiting with error code 1");
                Environment.Exit(1);
            }
        }
    }
}

// Configura middlewares se não estiver em modo migração
if (!isMigrationOnly)
{
    ConfigureMiddlewares(app);
    app.UseHangfireDashboard();

    RecurringJob.AddOrUpdate<CambioJobService>(
     recurringJobId: "cotacao-diaria-usd",
     methodCall: service => service.ExecutarCotacaoDiariaAsync(CancellationToken.None),
     cronExpression: "30 19 * * *", 
     options: new RecurringJobOptions
     {
         TimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time") 
     });
}

app.Run();

/// <summary>
/// Configuração de serviços gerais.
/// </summary>
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddMemoryCache();
    services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    ConfigureSwagger(services);

    services.Configure<OpcoesUrls>(configuration.GetSection("VExpense"));
    services.Configure<OpcoesUrls>(configuration.GetSection("Integracao"));
    services.Configure<VexpenseTokenApiKeyConfig>(configuration.GetSection("TokenApiKey"));
    services.Configure<VexpenseFiltroDefaultsConfig>(configuration.GetSection("FiltroDefaults"));
    services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));

    services.AddHttpClient<IVExpensesApi, VExpensesApi>();
    services.AddHttpClient<IIntegracaoBimerAPI, IntegracaoBimerAPI>();

    services.AddScoped<IVExpensesService, VExpensesService>();
    services.AddHttpClient<IIntegracaoBimerService, IntegracaoBimerService>();
    services.AddAutoMapper(typeof(Program));

    services.AddScoped<AdobeTemplantesExcelMongoService>();

    services.AddScoped<IGestaoMepeamentoComposRepository, DeParaVexpensesBimmerRepository>();
    services.AddScoped<IAnyPointCadastroIntegracaoService, AnyPointCadastroIntegracaoService>();
    services.AddScoped<IAnyPointCadastroIntegracaoRepository, AnyPointCadastroIntegracaoRepository>();
    services.AddScoped<GestaoMpeamentosService>();
    services.AddScoped<IMenuRepository, MenuRepository>();
    services.AddScoped<IMenuService, MenuService>();
    services.AddScoped<IPermissaoRepository, PermissaoRepository>();
    services.AddScoped<IPermicoesService, PermicoesService>();
    services.AddScoped<IEndpointPermissionService, EndpointPermissionService>();
    services.AddScoped<IBimerRepositorio, FinanceiroRepository>();
    services.AddScoped<IVexpenssesRepositorio, TituloRepository>();
    services.AddScoped<AutoEndpointAuthorizationFilter>();
    services.AddScoped<CotacaoService>();
    services.AddTransient<BrevoEmailSender>();
    services.AddTransient<CambioJobService>();
    services.AddTransient<CotacaoDolarRepository>();
    services.AddScoped<IImportHistoryRepository, ImportHistoryRepository>();
    services.AddScoped<ImportHistoryService>();

    services.AddScoped<IGestaoMepeamentoComposService, GestaoMpeamentosService>();

    // Integração SPP BIMER INVOCE
    services.AddScoped<IMonitorSppBimerAppService, MonitorSppBimerAppService>();
    services.AddScoped<IMonitorSppBimerInvoceRepositorio, MonitorSppBimerInvoceRepositorio>();

    // Log de erros em SQL (padrão IntegracaoSppBimerInvoceLogErros)
    services.AddScoped<IIntegracaoSppBimerInvoceLogErrosRepository, IntegracaoSppBimerInvoceLogErrosRepository>();


    // Usuario
    services.AddScoped<IUsuarioService, UsuarioService>();
    services.AddScoped<IUsuarioRepository, UsuarioRepository>();

    //Log Erros
    services.AddHostedService<LogErroMigratorHostedService>();
    services.AddScoped<IntegracaoVexpensesBimmerLogErroRepository>();
    services.AddScoped<VexpensesBimmerLogErroRepository>();
    //services.AddScoped<ILogErroMigrationService, LogErroMigrationService>();
    services.AddHostedService<LogErroMigratorHostedService>();

    // TipoTemplate
    services.AddScoped<ITipoTemplateRepository, TipoTemplateRepository>();
    services.AddScoped<ITipoTemplateService, TipoTemplateService>();

    // IntegracaoSppBimerDeParaMensagem
    services.AddScoped<IDeParaMensagemRepository, DeParaMensagemRepository>();
    services.AddScoped<IDeParaMensagemService, DeParaMensagemService>();

    // ===== Serviço de Cálculo de Preço de Revenda (SPP) =====
    services.AddScoped<ConfiguracoesService>();
    services.AddScoped<CalculoPrecoService>();

    builder.Services.AddScoped<IPrecoRevendaRepository, PrecoRevendaRepository>();
    builder.Services.AddScoped<IPrecoRevendaService, PrecoRevendaService>();

    services.AddScoped<ITabelaPrecosPublicacoesService, TabelaPrecosPublicacoesService>();
    services.AddScoped<IPrecoRevendaService, PrecoRevendaService>();

    services.AddScoped<ConfiguracoesService>();
    services.AddScoped<CalculoPrecoService>();

    builder.Services.AddScoped<IConfiguracoesService, ConfiguracoesService>();

    builder.Services.AddScoped<ITabelaPrecosPublicacoesService, TabelaPrecosPublicacoesService>();

    // Serviços de aplicação
    builder.Services.AddScoped<IConfiguracoesService, ConfiguracoesService>();
    builder.Services.AddScoped<ITabelaPrecosPublicacoesService, TabelaPrecosPublicacoesService>();
    builder.Services.AddScoped<IRegrasExclusaoService, RegrasExclusaoService>();
    builder.Services.AddScoped<IRegrasExclusaoRepository, RegrasExclusaoRepository>();
    services.AddScoped<RegrasExclusaoRepository>();

    // Repositórios da camada de infraestrutura
    builder.Services.AddScoped<IConstantesPedidoRepository, ConstantesPedidoEfRepository>();
    builder.Services.AddScoped<IPrecoRevendaRepository, PrecoRevendaRepository>();
    builder.Services.AddScoped<ITabelaPrecosPublicacoesRepository, TabelaPrecosPublicacoesRepository>();

}

/// <summary>
/// Configuração do Swagger para documentação.
/// </summary>
void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "PARS RJ",
            Version = "v1",
            Description = "Essa API é a principal para utilização de outros EndPoints."
        });

        c.DocInclusionPredicate((_, apiDesc) => true);
        c.TagActionsBy(api => new List<string> { api.GroupName ?? string.Empty });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Por favor, insira 'Bearer' [espaço] e então seu token no campo abaixo.",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });

        c.UseInlineDefinitionsForEnums();
    });
}

/// <summary>
/// Configuração dos middlewares HTTP.
/// </summary>
void ConfigureMiddlewares(WebApplication app)
{
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}