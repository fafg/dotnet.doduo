using dotnet.doduo;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static DoduoBuilder AddDoduo(this IServiceCollection services, Action<DoduoConfiguration> setupAction)
        {
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            services.Configure(setupAction);

            return new DoduoBuilder(services);
        }

    }
}
