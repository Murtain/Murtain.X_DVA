using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Murtain.OAuth2.Infrastructure;
using System.Reflection;
using MediatR;
using Murtain.OAuth2.Providers;
using Murtain.OAuth2.Application;
using Murtain.OAuth2.Domain.Aggregates.User;

namespace Murtain.OAuth2
{
    public class Startup
    {


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });



            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                // this adds the config data from DB (clients, resources, CORS)
                .AddConfigurationStore(options =>
                {
                    options.ApiClaim.Name = "api_claim";
                    options.ApiResource.Name = "api_resource";
                    options.ApiScope.Name = "api_scope";
                    options.ApiScopeClaim.Name = "api_scope_claim";
                    options.ApiSecret.Name = "api_secret";
                    options.Client.Name = "client";
                    options.ClientClaim.Name = "client_claim";
                    options.ClientCorsOrigin.Name = "client_cors_origin";
                    options.ClientGrantType.Name = "client_grant_type";
                    options.ClientIdPRestriction.Name = "client_id_prestriction";
                    options.ClientPostLogoutRedirectUri.Name = "client_post_logout_redirect_uri";
                    options.ClientProperty.Name = "client_property";
                    options.ClientRedirectUri.Name = "client_redirect_uri";
                    options.ClientScopes.Name = "client_scopes";
                    options.ClientSecret.Name = "client_secret";
                    options.IdentityClaim.Name = "identity_claim";
                    options.IdentityResource.Name = "identity_resource";

                    options.ResolveDbContextOptions = (provider, builder) =>
                    {
                        builder.UseMySql(
                            Configuration.GetConnectionString("DefaultConnection"),
                            sql => sql.MigrationsAssembly(Assembly.GetAssembly(typeof(Startup)).GetName().Name)
                        );
                    };
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.PersistedGrants.Name = "persisted_grants";
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseMySql(
                            Configuration.GetConnectionString("DefaultConnection"),
                            sql => sql.MigrationsAssembly(Assembly.GetAssembly(typeof(Startup)).GetName().Name)
                        );
                    };
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 10; // interval in seconds, short for testing
                })


                //.Services.AddScoped<IProfileService, ProfileService>()
                ;

            services.AddTransient<ILoginApplicationService<ApplicationUser>, LoginApplicationService>();

            services.AddGemini()
                    .AddAutoMapper(option => option.AssemblyLoaderParttern = "^Murtain.OAuth2")
                    .AddDependency(option => option.AssemblyLoaderParttern = "^Murtain.OAuth2")
                    //.AddLoggerInterception()
                    .AddSettingsManager(options => options.GlobalSettingProviders.Add<EmailSettingProvider>())
            ;

            //.AddSocrita()
            //.AddLoggerInterception()
            ////.AddUnitOfWork()
            //.AddCacheManager()
            //.AddGlobalSettingManager(options =>
            //{
            //    options.Providers.Add<EmailSettingProvider>();
            //})
            ////.AddEntityFrameWork()
            //;


            //services.AddDependency(option =>
            //{
            //    option.AssemblyLoaderParttern = "^Murtain.OAuth2";
            //});

            //services.AddAutoMapper(option =>
            //{
            //    option.AssemblyLoaderParttern = "^Murtain.OAuth2";
            //});
            services.AddMediatR(typeof(Startup));
            services.AddMvc();


            System.Diagnostics.Debug.WriteLine("   　 へ　　　　　／|");
            System.Diagnostics.Debug.WriteLine("   　/＼7　　∠＿/");
            System.Diagnostics.Debug.WriteLine("    /　│　　 ／　／");
            System.Diagnostics.Debug.WriteLine("   │　Z ＿,＜　／　　 /`ヽ");
            System.Diagnostics.Debug.WriteLine("   │　　　　　ヽ　　 /　　〉");
            System.Diagnostics.Debug.WriteLine("    Y　　　　　 `　 /　　/");
            System.Diagnostics.Debug.WriteLine("   ｲ  ●　､　● ⊂⊃〈　　/");
            System.Diagnostics.Debug.WriteLine("   ()　 へ　　　　|　＼〈");
            System.Diagnostics.Debug.WriteLine("   　>ｰ ､_　 ィ　 │ ／／");
            System.Diagnostics.Debug.WriteLine("    / へ　　 /　ﾉ＜| ＼＼");
            System.Diagnostics.Debug.WriteLine("    ヽ_ﾉ　　(_／　 │／／");
            System.Diagnostics.Debug.WriteLine("   　7　　　　　　　|／");
            System.Diagnostics.Debug.WriteLine("   　＞―r￣￣`ｰ―＿");

            foreach (var service in services.Where(x => x.ServiceType.ToString().StartsWith("Murtain")))
            {
                System.Diagnostics.Debug.WriteLine(service.ServiceType.ToString(), "【Murtain->Setup->ConfigureServices】");
            }


            //return services.BuildInterceptableServiceProvider();

            return services.BuildInterceptableServiceProvider();

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

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            app.UseGemini()
               .UseAutoMapper();


            app.UseStaticFiles();
        }
    }
}
