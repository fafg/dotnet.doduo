using Microsoft.Extensions.DependencyInjection;

namespace dotnet.doduo
{
    public static class DoduoServiceCollectionExtensions
    {
        public static DoduoBuilder AddDoduoService(this IServiceCollection services)
        {
            services.AddRouting();

            return new DoduoBuilder(services);
        }
    }
}
