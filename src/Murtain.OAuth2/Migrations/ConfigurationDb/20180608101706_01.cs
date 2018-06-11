using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Murtain.OAuth2.Migrations.ConfigurationDb
{
    public partial class _01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "api_resource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_resource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>(nullable: false),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    ProtocolType = table.Column<string>(maxLength: 200, nullable: false),
                    RequireClientSecret = table.Column<bool>(nullable: false),
                    ClientName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ClientUri = table.Column<string>(maxLength: 2000, nullable: true),
                    LogoUri = table.Column<string>(maxLength: 2000, nullable: true),
                    RequireConsent = table.Column<bool>(nullable: false),
                    AllowRememberConsent = table.Column<bool>(nullable: false),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(nullable: false),
                    RequirePkce = table.Column<bool>(nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(nullable: false),
                    FrontChannelLogoutUri = table.Column<string>(maxLength: 2000, nullable: true),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(nullable: false),
                    BackChannelLogoutUri = table.Column<string>(maxLength: 2000, nullable: true),
                    BackChannelLogoutSessionRequired = table.Column<bool>(nullable: false),
                    AllowOfflineAccess = table.Column<bool>(nullable: false),
                    IdentityTokenLifetime = table.Column<int>(nullable: false),
                    AccessTokenLifetime = table.Column<int>(nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(nullable: false),
                    ConsentLifetime = table.Column<int>(nullable: true),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(nullable: false),
                    RefreshTokenUsage = table.Column<int>(nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(nullable: false),
                    RefreshTokenExpiration = table.Column<int>(nullable: false),
                    AccessTokenType = table.Column<int>(nullable: false),
                    EnableLocalLogin = table.Column<bool>(nullable: false),
                    IncludeJwtId = table.Column<bool>(nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(nullable: false),
                    ClientClaimsPrefix = table.Column<string>(maxLength: 200, nullable: true),
                    PairWiseSubjectSalt = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_resource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Required = table.Column<bool>(nullable: false),
                    Emphasize = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_resource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "api_claim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    ApiResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_api_claim_api_resource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "api_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "api_scope",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Required = table.Column<bool>(nullable: false),
                    Emphasize = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false),
                    ApiResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_scope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_api_scope_api_resource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "api_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "api_secret",
                columns: table => new
                {
                    Expiration = table.Column<DateTime>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Value = table.Column<string>(maxLength: 2000, nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: true),
                    ApiResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_secret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_api_secret_api_resource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "api_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_claim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_claim_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_cors_origin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Origin = table.Column<string>(maxLength: 150, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_cors_origin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_cors_origin_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_grant_type",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GrantType = table.Column<string>(maxLength: 250, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_grant_type", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_grant_type_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_id_prestriction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Provider = table.Column<string>(maxLength: 200, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_id_prestriction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_id_prestriction_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_post_logout_redirect_uri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PostLogoutRedirectUri = table.Column<string>(maxLength: 2000, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_post_logout_redirect_uri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_post_logout_redirect_uri_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_property",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_property_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_redirect_uri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RedirectUri = table.Column<string>(maxLength: 2000, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_redirect_uri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_redirect_uri_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_scopes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Scope = table.Column<string>(maxLength: 200, nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_scopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_scopes_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client_secret",
                columns: table => new
                {
                    Expiration = table.Column<DateTime>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    Type = table.Column<string>(maxLength: 250, nullable: true),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_secret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_secret_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_claim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    IdentityResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_identity_claim_identity_resource_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalTable: "identity_resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "api_scope_claim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    ApiScopeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_scope_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_api_scope_claim_api_scope_ApiScopeId",
                        column: x => x.ApiScopeId,
                        principalTable: "api_scope",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_api_claim_ApiResourceId",
                table: "api_claim",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_api_resource_Name",
                table: "api_resource",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_api_scope_ApiResourceId",
                table: "api_scope",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_api_scope_Name",
                table: "api_scope",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_api_scope_claim_ApiScopeId",
                table: "api_scope_claim",
                column: "ApiScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_api_secret_ApiResourceId",
                table: "api_secret",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_client_ClientId",
                table: "client",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_client_claim_ClientId",
                table: "client_claim",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_client_cors_origin_ClientId",
                table: "client_cors_origin",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_client_grant_type_ClientId",
                table: "client_grant_type",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_client_id_prestriction_ClientId",
                table: "client_id_prestriction",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_client_post_logout_redirect_uri_ClientId",
                table: "client_post_logout_redirect_uri",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_client_property_ClientId",
                table: "client_property",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_client_redirect_uri_ClientId",
                table: "client_redirect_uri",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_client_scopes_ClientId",
                table: "client_scopes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_client_secret_ClientId",
                table: "client_secret",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_identity_claim_IdentityResourceId",
                table: "identity_claim",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_identity_resource_Name",
                table: "identity_resource",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_claim");

            migrationBuilder.DropTable(
                name: "api_scope_claim");

            migrationBuilder.DropTable(
                name: "api_secret");

            migrationBuilder.DropTable(
                name: "client_claim");

            migrationBuilder.DropTable(
                name: "client_cors_origin");

            migrationBuilder.DropTable(
                name: "client_grant_type");

            migrationBuilder.DropTable(
                name: "client_id_prestriction");

            migrationBuilder.DropTable(
                name: "client_post_logout_redirect_uri");

            migrationBuilder.DropTable(
                name: "client_property");

            migrationBuilder.DropTable(
                name: "client_redirect_uri");

            migrationBuilder.DropTable(
                name: "client_scopes");

            migrationBuilder.DropTable(
                name: "client_secret");

            migrationBuilder.DropTable(
                name: "identity_claim");

            migrationBuilder.DropTable(
                name: "api_scope");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "identity_resource");

            migrationBuilder.DropTable(
                name: "api_resource");
        }
    }
}
