using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.doduo.Configuration.Contract
{
    public interface IBootstrapper
    {
        Task BootstrapAsync();
    }
}
