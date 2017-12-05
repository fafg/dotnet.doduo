using dotnet.doduo.Configuration.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseDoduo(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            var provider = app.ApplicationServices;

            CheckRequirement(provider);

            var bootstrapper = provider.GetRequiredService<IBootstrapper>();
            bootstrapper.BootstrapAsync();

            return app;
        }

        private static void CheckRequirement(IServiceProvider services)
        {
        }
    }
}