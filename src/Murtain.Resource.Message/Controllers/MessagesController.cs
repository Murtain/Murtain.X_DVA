using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DnsClient;
using System.Net.Http;

namespace Murtain.Resource.Message.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {

        private readonly IDnsQuery _dns;
        public MessagesController(IDnsQuery dns)
        {
            _dns = dns ?? throw new ArgumentNullException(nameof(dns));
        }
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            var dnsResult = await _dns.ResolveServiceAsync("service.consul", "descovery");
            if (dnsResult.Length > 0)
            {
                var result = dnsResult.First();
                var address = result.AddressList.Any() ? result.AddressList.FirstOrDefault().ToString() : result.HostName.TrimEnd('.');
                var port = result.Port;

                using (var client = new HttpClient())
                {
                    var serviceResult = await client.GetStringAsync($"http://{address}:{port}/api/descoveries");
                    return new string[] { "message-1", "message-2", serviceResult };
                }
            }

            return new string[] { "message-1", "message-2" };
        }

    }
}
