using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Murtain.Extensions.Gemini.Builder;
using Murtain.Extensions.Dependency.Configuration;
using Murtain.Extensions.Settings.Configuration;
using Microsoft.Extensions.Configuration;
using Murtain.Extensions.AutoMapper.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GeminiApplicationServiceCollectionExtensions
    {

        public static IGeminiApplicationServiceCollection AddDependency(this IGeminiApplicationServiceCollection builder, Action<IDependencyScanConfiguration> option = null)
        {
            builder.Services.AddDependency(option);
            return builder;
        }

        public static IGeminiApplicationServiceCollection AddLoggerInterception(this IGeminiApplicationServiceCollection builder)
        {
            builder.Services.AddLoggerInterception();
            return builder;
        }

        public static IGeminiApplicationServiceCollection AddSettingsManager(this IGeminiApplicationServiceCollection builder, Action<ISettingsConfiguration> option = null)
        {
            builder.Services.AddSettingsManager(option);
            return builder;
        }

        public static IGeminiApplicationServiceCollection AddServiceDiscovery(this IGeminiApplicationServiceCollection builder, IConfiguration serviceOptionsConfiguration)
        {
            builder.Services.AddServiceDiscovery(serviceOptionsConfiguration);
            return builder;
        }

        public static IGeminiApplicationServiceCollection AddAutoMapper(this IGeminiApplicationServiceCollection builder, Action<IAutoMapperConfiguration> option = null)
        {
            builder.Services.AddAutoMapper(option);
            return builder;
        }
    }
}
