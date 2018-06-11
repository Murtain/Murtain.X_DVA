using System;
using System.Collections.Generic;
using System.Text;

namespace Murtain.Extensions.Polly.Resilience.Http
{
    public interface IResilientHttpClientFactory
    {
        ResilientHttpClient CreateResilientHttpClient();
    }
}
