using System;
using System.Collections.Generic;
using System.Text;
using Murtain.Extensions;
using Murtain.Middleware;
using Murtain.Extensions.Gemini.Builder;

namespace Microsoft.AspNetCore.Builder
{
    public static class IApplicationBuilderExtensions
    {
        public static IGeminiApplicationBuilder UseGemini(this IApplicationBuilder app)
        {
            app.UseMiddleware<GeminiMiddleware>();
            return new GeminiApplicationBuilder(app);
        }
    }
}

