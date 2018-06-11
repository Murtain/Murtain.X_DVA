using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Murtain.OAuth2.Infrastructure;
using IdentityServer4.EntityFramework.DbContexts;

namespace Murtain.OAuth2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                 //.MigrateDbContext<ApplicationDbContext>((context, services) =>
                 //{
                 //    new ApplicationDbContextSeed().SeedAsync(context)
                 //    .Wait();
                 //})
                 //.MigrateDbContext<ConfigurationDbContext>((context, services) =>
                 //{
                 //    new ApplicationDbContextSeed().ConfigurationDbContextSeedAsync(context)
                 //    .Wait();
                 //})
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(factory => factory.AddConsole())
                .UseStartup<Startup>()
                .Build();
    }
}
