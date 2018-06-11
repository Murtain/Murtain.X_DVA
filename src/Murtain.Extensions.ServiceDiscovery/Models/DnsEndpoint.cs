using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Murtain.Extensions.ServiceDiscovery.Models
{
    public class DnsEndpoint
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }
}
