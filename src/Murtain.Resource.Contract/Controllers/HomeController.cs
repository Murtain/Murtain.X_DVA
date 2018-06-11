using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DnsClient;
using System.Net.Http;

namespace Murtain.Resource.Contract.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDnsQuery _dns;

        public HomeController(IDnsQuery dns)
        {
            _dns = dns ?? throw new ArgumentNullException(nameof(dns));
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel()
            {
                DnsResult = await _dns.ResolveServiceAsync("service.consul", "descovery")
            };

            if (model.DnsResult.Length > 0)
            {
                var result = model.DnsResult.First();
                var address = result.AddressList.Any() ? result.AddressList.FirstOrDefault().ToString() : result.HostName.TrimEnd('.');
                var port = result.Port;

                using (var client = new HttpClient())
                {
                    model.ServiceResult = await client.GetStringAsync($"http://{address}:{port}/api/values");
                }
            }

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }

    public class IndexViewModel
    {
        public ServiceHostEntry[] DnsResult { get; set; }

        public string ServiceResult { get; set; }
    }
}