using System;
using System.Collections.Generic;
using System.Text;

namespace Murtain.Extensions.ServiceDiscovery.Models
{
    public class ConsulOptions
    {
        public string HttpEndpoint { get; set; }

        public DnsEndpoint DnsEndpoint { get; set; }
    }
}
