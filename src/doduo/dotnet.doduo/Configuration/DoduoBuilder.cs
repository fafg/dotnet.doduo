using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.Configuration
{
    public sealed class DoduoBuilder
    {
        public DoduoBuilder(IServiceCollection services)
        {
            Services = services;
        }
        public IServiceCollection Services { get; }
        private DoduoBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }
        private DoduoBuilder AddSingleton(Type serviceType, Type concreteType)
        {
            Services.AddSingleton(serviceType, concreteType);
            return this;
        }
    }
}