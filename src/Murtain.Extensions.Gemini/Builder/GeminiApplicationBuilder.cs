using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Murtain.Extensions.Gemini.Builder
{
    public class GeminiApplicationBuilder : IGeminiApplicationBuilder
    {
        public IApplicationBuilder Builder { get; set; }

        public GeminiApplicationBuilder(IApplicationBuilder app)
        {
            this.Builder = app;
        }
    }
}
