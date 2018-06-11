using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Murtain.Extensions.Gemini.Builder
{
    public class GeminiApplicationServiceCollection : IGeminiApplicationServiceCollection
    {
        public IServiceCollection Services { get; }

        public GeminiApplicationServiceCollection(IServiceCollection services)
        {
            this.Services = services;
        }
    }
}
