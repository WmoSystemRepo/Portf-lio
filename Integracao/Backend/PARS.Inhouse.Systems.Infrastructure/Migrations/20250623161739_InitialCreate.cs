using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnyPointStoreGestaoIntegracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEdicao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreGestaoIntegracao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointStoreMenu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rota = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrdenacaoMenu = table.Column<int>(type: "int", nullable: true),
                    EhMenuPrincipal = table.Column<bool>(type: "bit", nullable: true),
                    SubMenuReferencaiPrincipal = table.Column<int>(type: "int", nullable: true),
                    DataCriacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEdicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegrasIds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointStorePermicoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataEdicao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStorePermicoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointUserEndpointPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointUserEndpointPermission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstLogin = table.Column<bool>(type: "bit", nullable: false),
                    PodeLer = table.Column<bool>(type: "bit", nullable: false),
                    PodeEscrever = table.Column<bool>(type: "bit", nullable: false),
                    PodeRemover = table.Column<bool>(type: "bit", nullable: false),
                    PodeVerConfiguracoes = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoBacenCotacaoMoeda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoMoeda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataHoraCotacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CotacaoCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CotacaoVenda = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoBoletim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataHoraIntegracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoBacenCotacaoMoeda", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoBimmerHistoricoErrosInsercoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdResponse = table.Column<int>(type: "int", nullable: false),
                    Tentativas = table.Column<long>(type: "bigint", nullable: true),
                    DataTentativa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MensagemErro = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoBimmerHistoricoErrosInsercoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoBimmerInsercaoPendentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdResponse = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoBimmerInsercaoPendentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoBimmerInsertOK",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdResponse = table.Column<int>(type: "int", nullable: false),
                    IdentificadorBimmer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoBimmerInsertOK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoMapeamentoDeCampos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampoOrigemSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampoDestinoSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoIntegracao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoMapeamentoDeCampos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoSppBimerInvoce",
                columns: table => new
                {
                    CdEmpresa = table.Column<int>(type: "int", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumeroDaNFSe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CdCliente = table.Column<int>(type: "int", nullable: false),
                    NomeDoFabricante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObsNotaFiscal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CDOperacoes = table.Column<int>(type: "int", nullable: false),
                    DescricaoDaOperacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CDCentroDeCusto = table.Column<int>(type: "int", nullable: false),
                    DescricaoDoCentroDeCusto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoSppBimerInvoce", x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe });
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoVexpenseTitulosRelatoriosStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdResponse = table.Column<int>(type: "int", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: true),
                    Device_id = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approval_stage_id = table.Column<int>(type: "int", nullable: true),
                    Approval_user_id = table.Column<int>(type: "int", nullable: true),
                    Approval_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Payment_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Payment_method_id = table.Column<int>(type: "int", nullable: true),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Paying_company_id = table.Column<int>(type: "int", nullable: true),
                    On = table.Column<bool>(type: "bit", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pdf_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Excel_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpensesTotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpensesData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoVexpenseTitulosRelatoriosStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointStoreMenuIntegracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataEdicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreMenuIntegracao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointStoreMenuIntegracao_AnyPointStoreMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "AnyPointStoreMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointStoreMenuPermicoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataEdicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreMenuPermicoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointStoreMenuPermicoes_AnyPointStoreMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "AnyPointStoreMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointDeparas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCampos = table.Column<int>(type: "int", nullable: true),
                    TipoExecucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Integracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeMapeamentoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoSistemaOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeMapeamentoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoSistemaDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorDestino = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointDeparas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointDeparas_IntegracaoMapeamentoDeCampos_IdCampos",
                        column: x => x.IdCampos,
                        principalTable: "IntegracaoMapeamentoDeCampos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoSppBimerInvoceItens",
                columns: table => new
                {
                    CdEmpresa = table.Column<int>(type: "int", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumeroDaNFSe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CdProduto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NomeDoProduto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qtd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoUnitItem = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoVendaUS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoVendaReal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoCompraUS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoCompraReal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoSppBimerInvoceItens", x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe, x.CdProduto });
                    table.ForeignKey(
                        name: "FK_IntegracaoSppBimerInvoceItens_IntegracaoSppBimerInvoce_CdEmpresa_NumeroDocumento_NumeroDaNFSe",
                        columns: x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe },
                        principalTable: "IntegracaoSppBimerInvoce",
                        principalColumns: new[] { "CdEmpresa", "NumeroDocumento", "NumeroDaNFSe" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoSppBimerInvoceResumo",
                columns: table => new
                {
                    CdEmpresa = table.Column<int>(type: "int", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumeroDaNFSe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataDeVencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorPagamentoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoSppBimerInvoceResumo", x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe });
                    table.ForeignKey(
                        name: "FK_IntegracaoSppBimerInvoceResumo_IntegracaoSppBimerInvoce_CdEmpresa_NumeroDocumento_NumeroDaNFSe",
                        columns: x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe },
                        principalTable: "IntegracaoSppBimerInvoce",
                        principalColumns: new[] { "CdEmpresa", "NumeroDocumento", "NumeroDaNFSe" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointDeparas_IdCampos",
                table: "AnyPointDeparas",
                column: "IdCampos");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuIntegracao_MenuId",
                table: "AnyPointStoreMenuIntegracao",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuPermicoes_MenuId",
                table: "AnyPointStoreMenuPermicoes",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyPointDeparas");

            migrationBuilder.DropTable(
                name: "AnyPointPermissaoUsuario");

            migrationBuilder.DropTable(
                name: "AnyPointStoreGestaoIntegracao");

            migrationBuilder.DropTable(
                name: "AnyPointStoreMenuIntegracao");

            migrationBuilder.DropTable(
                name: "AnyPointStoreMenuPermicoes");

            migrationBuilder.DropTable(
                name: "AnyPointStorePermicoes");

            migrationBuilder.DropTable(
                name: "AnyPointUserEndpointPermission");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "IntegracaoBacenCotacaoMoeda");

            migrationBuilder.DropTable(
                name: "IntegracaoBimmerHistoricoErrosInsercoes");

            migrationBuilder.DropTable(
                name: "IntegracaoBimmerInsercaoPendentes");

            migrationBuilder.DropTable(
                name: "IntegracaoBimmerInsertOK");

            migrationBuilder.DropTable(
                name: "IntegracaoSppBimerInvoceItens");

            migrationBuilder.DropTable(
                name: "IntegracaoSppBimerInvoceResumo");

            migrationBuilder.DropTable(
                name: "IntegracaoVexpenseTitulosRelatoriosStatus");

            migrationBuilder.DropTable(
                name: "IntegracaoMapeamentoDeCampos");

            migrationBuilder.DropTable(
                name: "AnyPointStoreMenu");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "IntegracaoSppBimerInvoce");
        }
    }
}
