using dotnet.doduo.Configuration.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo
{
   public class DoduoConfiguration
    {
        public DoduoConfiguration()
        {

        }
        public int Timeout { get; set; } = 10000;
        public int MaxSizeQueue { get; set; } = 4;
        public int FailedRetryCount { get; set; }
        public List<IConfigurationExtension> Extensions { get; set; } = new List<IConfigurationExtension>();

        public void RegisterExtension(IConfigurationExtension extension)
        {
            if (extension == null)
                throw new ArgumentNullException(nameof(extension));

            Extensions.Add(extension);
        }
    }
}
