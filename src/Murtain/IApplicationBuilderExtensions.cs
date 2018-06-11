using System;
using System.Collections.Generic;
using System.Text;
using Murtain.Builder;
using Murtain.Collections;
using Murtain.Extensions;
using Murtain.Middleware;

namespace Microsoft.AspNetCore.Builder
{
    public static class IApplicationBuilderExtensions
    {
        public static IAppBuilder UseSocrita(this IApplicationBuilder app)
        {
            app.UseMiddleware<SocritaMiddleware>();

            return new AppBuilder(app);
        }
    }
}

