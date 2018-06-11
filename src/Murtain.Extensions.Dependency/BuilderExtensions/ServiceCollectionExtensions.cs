using Murtain.Extensions.Dependency;
using Murtain.Extensions.Dependency.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private const string ASSEMBLY_SKIP_LOADER_PARTTERN = "^System|^Microsoft|^AutoMapper|^IdentityServer|^Pomelo|^Dora|^Scrutor";

        public static IServiceCollection AddDependency(this IServiceCollection builder, Action<IDependencyScanConfiguration> option = null)
        {
            DependencyScanConfiguration config = new DependencyScanConfiguration(); ;
            builder.AddSingleton<IDependencyScanConfiguration>(provider =>
            {
                option?.Invoke(config);
                return config;
            });

            builder.Scan(scan =>
                {
                    scan.FromAssemblies(GetAssemblies(config.AssemblyLoaderParttern))
                              .AddClasses(c => c.Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && t != typeof(ITransientDependency) && !t.IsAbstract))
                              .AsImplementedInterfaces()
                              .WithTransientLifetime();


                    scan.FromAssemblies(GetAssemblies(config.AssemblyLoaderParttern))
                                  .AddClasses(c => c.Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && t != typeof(ISingletonDependency) && !t.IsAbstract))
                                  .AsImplementedInterfaces()
                                  .WithSingletonLifetime();

                });

            return builder;
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
