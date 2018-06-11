using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Murtain.OAuth2.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;

                options.ClientId = "client_mvc_implicit_admin";
                options.ClientSecret = "secret";
                options.SaveTokens = true;
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Map("/admin", adminApp =>
            {
                //var factory = new IdentityAdminServiceFactory
                //{
                //    ClientService = new Registration<IClientService, InMemoryClientService>(),
                //    IdentityResourceService = new Registration<IIdentityResourceService, InMemoryIdentityResourceService>(),
                //    ApiResourceService = new Registration<IApiResourceService, InMemoryApiResourceService>()
                //};
                //var rand = new System.Random();
                //var clients = ClientSeeder.Get(rand.Next(1000, 3000));

                //var identityResources = IdentityResourceSeeder.Get(rand.Next(25));
                //var apiResources = ApiResourceSeeder.Get(rand.Next(54));

                //factory.Register(new Registration<ICollection<InMemoryClient>>(clients));
                //factory.Register(new Registration<ICollection<InMemoryIdentityResource>>(identityResources));
                //factory.Register(new Registration<ICollection<InMemoryApiResource>>(apiResources));

                //adminApp.UseIdentityAdmin(new IdentityAdminOptions
                //{
                //    Factory = factory
                //});
            });
        }
    }
}
