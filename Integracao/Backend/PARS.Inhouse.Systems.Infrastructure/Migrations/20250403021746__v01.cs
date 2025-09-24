using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnyPointCadastroIntegracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointCadastroIntegracao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointMenu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rota = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<double>(type: "float", nullable: true),
                    ParentMenuId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointMenu_AnyPointMenu_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalTable: "AnyPointMenu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnyPointPermissao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointPermissao", x => x.Id);
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
                name: "DeParaEmpresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeParaEmpresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeParaVexpensesBimmer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoItemOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescricaoItemOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoItemDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescricaoItemDestino = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeParaVexpensesBimmer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoBimmerHistoricoErrosInsercoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    Tentativas = table.Column<long>(type: "bigint", nullable: true),
                    DataTentativa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoBimmerInsercaoPendentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplatesPlanilha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplatesPlanilha", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitulosAprovados",
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
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitulosAprovados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitulosPagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    IdentificadorBimmer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitulosPagos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointUserEndpointPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointUserEndpointPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointUserEndpointPermission_AnyPointMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "AnyPointMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointMenuAnyPointPermissao",
                columns: table => new
                {
                    MenusId = table.Column<int>(type: "int", nullable: false),
                    PermissoesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointMenuAnyPointPermissao", x => new { x.MenusId, x.PermissoesId });
                    table.ForeignKey(
                        name: "FK_AnyPointMenuAnyPointPermissao_AnyPointMenu_MenusId",
                        column: x => x.MenusId,
                        principalTable: "AnyPointMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnyPointMenuAnyPointPermissao_AnyPointPermissao_PermissoesId",
                        column: x => x.PermissoesId,
                        principalTable: "AnyPointPermissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointMenuPermissao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointMenuPermissao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointMenuPermissao_AnyPointMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "AnyPointMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnyPointMenuPermissao_AnyPointPermissao_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "AnyPointPermissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointPermissaoUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdPermissao = table.Column<int>(type: "int", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointPermissaoUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointPermissaoUsuario_AnyPointPermissao_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "AnyPointPermissao",
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
                name: "TemplatesPlanilhaCampos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplatesPlanilhaId = table.Column<int>(type: "int", nullable: false),
                    ColunaPlanilha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColunaBanco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LetraColunaPlanilha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampoFixo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreencherCampoEmBranco = table.Column<bool>(type: "bit", nullable: false),
                    PreencherCampoFixo = table.Column<bool>(type: "bit", nullable: false),
                    NomePersonalizadoColunaPlanilha = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplatesPlanilhaCampos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplatesPlanilhaCampos_TemplatesPlanilha_TemplatesPlanilhaId",
                        column: x => x.TemplatesPlanilhaId,
                        principalTable: "TemplatesPlanilha",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TituloAprovadoDespesa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdResponse = table.Column<int>(type: "int", nullable: true),
                    IdTituloAprovado = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ExpenseId = table.Column<int>(type: "int", nullable: true),
                    DeviceId = table.Column<int>(type: "int", nullable: true),
                    IntegrationId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Mileage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true),
                    PayingCompanyId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    ReiceptUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Validate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reimbursable = table.Column<bool>(type: "bit", nullable: true),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rejected = table.Column<int>(type: "int", nullable: true),
                    On = table.Column<bool>(type: "bit", nullable: true),
                    MileageValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalCurrencyIso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConvertedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConvertedCurrencyIso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TituloAprovadoDespesa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TituloAprovadoDespesa_TitulosAprovados_IdTituloAprovado",
                        column: x => x.IdTituloAprovado,
                        principalTable: "TitulosAprovados",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointMenu_ParentMenuId",
                table: "AnyPointMenu",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointMenuAnyPointPermissao_PermissoesId",
                table: "AnyPointMenuAnyPointPermissao",
                column: "PermissoesId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointMenuPermissao_MenuId",
                table: "AnyPointMenuPermissao",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointMenuPermissao_PermissaoId",
                table: "AnyPointMenuPermissao",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointPermissaoUsuario_PermissaoId",
                table: "AnyPointPermissaoUsuario",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointUserEndpointPermission_MenuId",
                table: "AnyPointUserEndpointPermission",
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

            migrationBuilder.CreateIndex(
                name: "IX_TemplatesPlanilhaCampos_TemplatesPlanilhaId",
                table: "TemplatesPlanilhaCampos",
                column: "TemplatesPlanilhaId");

            migrationBuilder.CreateIndex(
                name: "IX_TituloAprovadoDespesa_IdTituloAprovado",
                table: "TituloAprovadoDespesa",
                column: "IdTituloAprovado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyPointCadastroIntegracao");

            migrationBuilder.DropTable(
                name: "AnyPointMenuAnyPointPermissao");

            migrationBuilder.DropTable(
                name: "AnyPointMenuPermissao");

            migrationBuilder.DropTable(
                name: "AnyPointPermissaoUsuario");

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
                name: "DeParaEmpresas");

            migrationBuilder.DropTable(
                name: "DeParaVexpensesBimmer");

            migrationBuilder.DropTable(
                name: "IntegracaoBimmerHistoricoErrosInsercoes");

            migrationBuilder.DropTable(
                name: "IntegracaoBimmerInsercaoPendentes");

            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "TemplatesPlanilhaCampos");

            migrationBuilder.DropTable(
                name: "TituloAprovadoDespesa");

            migrationBuilder.DropTable(
                name: "TitulosPagos");

            migrationBuilder.DropTable(
                name: "AnyPointPermissao");

            migrationBuilder.DropTable(
                name: "AnyPointMenu");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TemplatesPlanilha");

            migrationBuilder.DropTable(
                name: "TitulosAprovados");
        }
    }
}
