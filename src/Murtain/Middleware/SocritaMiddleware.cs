using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Murtain.Session;

namespace Murtain.Middleware
{
    public class SocritaMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public SocritaMiddleware(RequestDelegate next, ILogger<SocritaMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            logger.LogTrace("Murtain working ...");
            await next(context);
        }

    }
}
