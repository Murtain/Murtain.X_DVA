using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DnsClient;
using System.Net.Http;

namespace Murtain.Resource.Users.Controllers
{
    public class UserController : Controller
    {

        private readonly IDnsQuery _dns;

        public UserController(IDnsQuery dns)
        {
            _dns = dns;
        }
        [HttpGet]
        [Route("api/users")]
        public async Task<IEnumerable<string>> GetAsync()
        {
            var dnsResult = await _dns.ResolveServiceAsync("service.consul", "story");
            if (dnsResult.Length > 0)
            {
                var result = dnsResult.First();
                var address = result.AddressList.Any() ? result.AddressList.FirstOrDefault().ToString() : result.HostName.TrimEnd('.');
                var port = result.Port;

                using (var client = new HttpClient())
                {
                    var serviceResult = await client.GetStringAsync($"http://{address}:{port}/api/stories");
                    return new string[] { "users-1", "users-2", serviceResult };
                }
            }

            return new string[] { "users-1", "users-2" };
        }
    }
}
