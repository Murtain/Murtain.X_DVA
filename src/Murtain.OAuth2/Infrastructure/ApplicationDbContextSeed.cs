using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Murtain.OAuth2.Domain.Aggregates.User;
using Murtain.OAuth2.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Murtain.OAuth2.Infrastructure
{
    public class ApplicationDbContextSeed
    {

        public async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var defaultUser = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString("N"),
                    UserName = "15618275259",
                    Email = "392327013@qq.com",
                    PhoneNumber = "15618275259",

                    UserProperty = new IdentityUserProperty
                    {
                        Avatar = "http://video.jessetalk.cn/files/user/2018/04-24/205959f1439c148108.jpg",
                        NickName = "小橘子",
                        Age = 27,
                        Birthday = "1991-06-09",
                        Gender = Gender.Male,
                    }
                };

                await context.Users.AddAsync(defaultUser);

                context.SaveChanges();
            }
        }


        public async Task ConfigurationDbContextSeedAsync(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {

                List<Client> clients = new List<Client>{
                     new Client
                    {
                        ClientId = "client_mvc_implicit_admin",
                        ClientName = "MVC Implicit Admin",
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },

                        AllowedGrantTypes = IdentityServer4.Models.GrantTypes.Implicit,
                        ClientUri = "http://identityserver.io",
                        AllowAccessTokensViaBrowser = true,

                        RedirectUris = { "http://localhost:5001/signin-oidc" },
                        FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                        PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            IdentityServerConstants.StandardScopes.Phone,
                            IdentityServerConstants.StandardScopes.Address,

                        }
                    },
                     new Client
                    {
                        ClientId = "client_javascript_oidc_identity_admin",
                        ClientName = "JavaScript OIDC Client Identity Admin",
                        ClientUri = "http://identityserver.io",

                        AllowedGrantTypes = IdentityServer4.Models.GrantTypes.Implicit,
                        AllowAccessTokensViaBrowser = true,
                        RequireClientSecret = false,
                        AccessTokenType = AccessTokenType.Jwt,

                        RedirectUris =
                        {
                            "http://localhost:5001/index.html",
                            "http://localhost:5001/callback.html",
                            "http://localhost:5001/silent.html",
                            "http://localhost:5001/popup.html"
                        },

                        PostLogoutRedirectUris = { "http://localhost:5001/index.html" },
                        AllowedCorsOrigins = { "http://localhost:5001" },

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                        }
                    },
                    new Client
                    {
                        ClientId = "client_mvc_gateway",
                        ClientName = "MVC Gateway",
                        ClientUri = "http://identityserver.io",

                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        AllowOfflineAccess = true,
                        RequireClientSecret = false,
                        AllowedGrantTypes = IdentityServer4.Models.GrantTypes.ClientCredentials,
                        AlwaysIncludeUserClaimsInIdToken = true,

                        AllowedScopes =
                        {
                            "users",
                            "stories",
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                        }
                    }

                };

                foreach (var client in clients)
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {

                var resources = new List<IdentityResource>
               {
                    // some standard scopes from the OIDC spec
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                    new IdentityResources.Phone(),
                    new IdentityResources.Address(),
                };

                foreach (var resource in resources)
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!context.ApiResources.Any())
            {

                var resources = new List<ApiResource>
               {
                    // some standard scopes from the OIDC spec
                    new ApiResource{

                        Name = "gateway_api",
                        ApiSecrets =  {
                            new Secret("secret".Sha256())
                        },
                        Scopes =  {
                            new Scope("users"),
                            new Scope("stories")
                        }
                    },
                };

                foreach (var resource in resources)
                {
                    await context.ApiResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
