using System;
using System.Collections.Generic;
using System.Text;

namespace Murtain.Extensions.ServiceDiscovery.Models
{
    public class ServiceDiscoveryOptions
    {
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }

        public string HealthCheckTemplate { get; set; }

        public string[] Endpoints { get; set; }
    }
}
