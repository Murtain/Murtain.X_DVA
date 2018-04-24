using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Configuration
{
    public class Clients
    {
        public static IEnumerable<Client> GetClients()
        {

            return new List<Client>()
            {
                new Client(){
                    ClientId = "client.mvc.admin",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    RequireClientSecret = true,

                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc"},

                    AllowedScopes = {
                        "resource_authrize"
                    }
                },
                 new Client(){
                    ClientId = "client.resourceowner.x-dva",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowedScopes = {
                        "resource_users",
                        "resource_message",
                        "resource_contract",
                        "resource_story",
                    }
                },
                new Client(){
                    ClientName = "配置管理",
                    ClientId = "client.resourceowner.configuration",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowedScopes = {
                        "resource_users",
                        "resource_configuration",
                    }
                }
            };
        }
    }
}
