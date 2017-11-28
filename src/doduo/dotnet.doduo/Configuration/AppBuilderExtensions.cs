using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.Configuration
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseDoduo(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            var provider = app.ApplicationServices;

            CheckRequirement(provider);

            return app;
        }

        private static void CheckRequirement(IServiceProvider services)
        {
        }
    }
}