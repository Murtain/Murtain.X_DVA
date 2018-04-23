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
                    ClientName = "授权服务器管理控台",
                    ClientId = "client.credentials.authrize.admin",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    RequireClientSecret = true,
                    AllowedScopes = {
                        "resource_authrize"
                    }
                },
                 new Client(){
                    ClientName = "X-DVA客户端",
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
