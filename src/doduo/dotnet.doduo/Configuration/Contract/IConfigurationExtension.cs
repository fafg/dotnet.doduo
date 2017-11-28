using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.Configuration.Contract
{
    public interface IConfigurationExtension
    {
        void AddServices(IServiceCollection services);
    }
}
