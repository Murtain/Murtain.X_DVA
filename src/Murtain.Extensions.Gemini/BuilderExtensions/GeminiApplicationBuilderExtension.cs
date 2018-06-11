using Murtain.Extensions.Gemini.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class GeminiApplicationBuilderExtension
    {
        public static IGeminiApplicationBuilder UseAutoMapper(this IGeminiApplicationBuilder app)
        {
            app.Builder.UseAutoMapper();
            return app;
        }
    }
}
