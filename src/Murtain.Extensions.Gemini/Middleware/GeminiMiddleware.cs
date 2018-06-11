using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Murtain.Middleware
{
    public class GeminiMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public GeminiMiddleware(RequestDelegate next, ILogger<GeminiMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            logger.LogTrace("Gemini working ...");
            await next(context);
        }

    }
}
