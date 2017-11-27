using Microsoft.Extensions.DependencyInjection;

namespace dotnet.doduo
{
    public sealed class DoduoBuilder
    {
        public IServiceCollection Services { get; }

        public DoduoBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
