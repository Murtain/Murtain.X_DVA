using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Murtain.Extensions.Settings.Configuration;
using Murtain.Extensions.Settings.GlobalSetting.Store;
using Murtain.Extensions.Settings.GlobalSetting;
using Murtain.Extensions.Settings.Caching;
using System.Reflection;
using System.Linq;
using Murtain.Extensions.Settings.GlobalSetting.Provider;
using Murtain.Extensions.Settings.Caching.Provider;
using System.Text.RegularExpressions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private const string ASSEMBLY_SKIP_LOADER_PARTTERN = "^System|^Microsoft|^AutoMapper|^IdentityServer|^Pomelo|^Dora";

        public static IServiceCollection AddSettingsManager(this IServiceCollection services, Action<ISettingsConfiguration> options = null)
        {

            var config = new SettingsConfiguration();
            services.AddSingleton<ISettingsConfiguration>(provider =>
            {
                options?.Invoke(config);
                return config;
            });
            services.AddTransient<IGlobalSettingStore, GlobalSettingConfigurationStore>();
            services.AddSingleton<IGlobalSettingManager, GlobalSettingManager>();
            services.AddSingleton<ICacheSettingManager, CacheSettingManager>();

            services.Scan(scan =>
            {
                scan.FromAssemblies(GetAssemblies(config.AssemblyLoaderParttern))
                          .AddClasses(c => c.AssignableTo(typeof(GlobalSettingProvider)))
                          .AddClasses(c => c.AssignableTo(typeof(CacheSettingProvider)))
                          .AsSelf()
                          .WithSingletonLifetime();
            });

            return services;
        }


        private static Assembly[] GetAssemblies(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return Assembly
                         .GetEntryAssembly()
                         .GetReferencedAssemblies()
                         .Where(assembly => Regex.IsMatch(assembly.FullName, ASSEMBLY_SKIP_LOADER_PARTTERN, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                         .Select(Assembly.Load)
                         .ToArray();
            }

            return Assembly
                     .GetEntryAssembly()
                     .GetReferencedAssemblies()
                     .Where(assembly => !Regex.IsMatch(assembly.FullName, ASSEMBLY_SKIP_LOADER_PARTTERN, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                     .Select(Assembly.Load)
                     .ToArray();

        }

    }
}
