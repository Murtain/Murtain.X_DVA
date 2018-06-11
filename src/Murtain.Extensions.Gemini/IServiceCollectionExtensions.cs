using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Murtain;
using Murtain.Extensions.Gemini.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IGeminiApplicationServiceCollection AddGemini(this IServiceCollection services)
        {
            services.AddSingleton<IGeminiApplicationServiceCollection, GeminiApplicationServiceCollection>();
            return new GeminiApplicationServiceCollection(services);
        }

    }
}
