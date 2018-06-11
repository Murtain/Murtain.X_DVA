using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Murtain.Extensions.AutoMapper.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection builder, Action<IAutoMapperConfiguration> invoke = null)
        {
            builder.AddSingleton<IAutoMapperConfiguration>(provider =>
            {
                var c = new AutoMapperConfiguration();
                invoke?.Invoke(c);
                return c;
            });

            return builder;
        }

    }
}
