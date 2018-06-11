using AutoMapper;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Murtain.Extensions.AutoMapper;
using Murtain.Extensions.AutoMapper.Configuration;
using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {

        private const string ASSEMBLY_SKIP_LOADER_PARTTERN = "^System|^Microsoft|^AutoMapper|^IdentityServer|^Pomelo|^Dora";

        private static bool MappingConfigure;
        private static readonly object MappingLock = new object();

        public static IApplicationBuilder UseAutoMapper(this IApplicationBuilder app)
        {

            var config = app.ApplicationServices.GetService(typeof(IAutoMapperConfiguration)).TryAs<IAutoMapperConfiguration>();

            lock (MappingLock)
            {
                if (!MappingConfigure)
                {
                    Mapper.Initialize(configuration =>
                    {
                        MapAutoAttributes(configuration, GetFilterAssembliesDefinedTypes(config.AssemblyLoaderParttern)
                                       .Where(type => type.IsDefined(typeof(AutoMapAttribute)) || type.IsDefined(typeof(AutoMapFromAttribute)) || type.IsDefined(typeof(AutoMapToAttribute))));

                        MapOtherMappings(configuration, GetFilterAssembliesDefinedTypes(config.AssemblyLoaderParttern)
                                               .Where(type => typeof(IAutoMaping).IsAssignableFrom(type) && type != typeof(IAutoMaping) && !type.IsAbstract));

                        foreach (var configurator in config.Configurators)
                        {
                            configurator(configuration);
                        }

                    });
                }

                MappingConfigure = true;
            }
            return app;
        }

        private static void MapAutoAttributes(IMapperConfigurationExpression configuration, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                configuration.CreateAttributeMaps(type);
            }
        }

        private static void MapOtherMappings(IMapperConfigurationExpression configuration, IEnumerable<Type> types)
        {

            foreach (var t in types)
            {
                t.GetMethod(nameof(IAutoMaping.CreateMappings)).Invoke(Activator.CreateInstance(t), new object[] { configuration });
            }
        }

        private static Assembly[] GetAssemblies(string assembliesLoadParttern)
        {
            if (!string.IsNullOrEmpty(assembliesLoadParttern))
            {
                return Assembly
                         .GetEntryAssembly()
                         .GetReferencedAssemblies()
                         .Where(assembly => Regex.IsMatch(assembly.FullName, assembliesLoadParttern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
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

        private static IEnumerable<Type> GetFilterAssembliesDefinedTypes(string assembliesLoadParttern)
        {
            var types = new List<Type>();

            foreach (var assembly in GetAssemblies(assembliesLoadParttern))
            {
                try
                {
                    types.AddRange(assembly.GetTypes().Where(type => type != null));
                }
                catch (ReflectionTypeLoadException e)
                {
                    throw e;
                }
            }
            return types;
        }

        private static T TryAs<T>(this object obj)
            where T : class
        {
            return obj == null ? null : (T)obj;
        }
    }


}
