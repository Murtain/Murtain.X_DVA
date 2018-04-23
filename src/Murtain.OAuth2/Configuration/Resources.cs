using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Configuration
{
    public class Resources
    {
        public static IEnumerable<ApiResource> GetResources()
        {

            return new List<ApiResource>()
            {
                new ApiResource("resource_users","用户服务"),
                new ApiResource("resource_message","消息服务"),
                new ApiResource("resource_contract","通讯录服务"),
                new ApiResource("resource_story","用户故事服务"),
                new ApiResource("resource_configuration","系统配置服务"),
                new ApiResource("resource_authrize","授权服务"),
            };
        }
    }
}
