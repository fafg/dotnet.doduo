using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.Model
{
    public class ProducerResponse
    {
        public static ProducerResponse Ok()
        {
            throw new NotImplementedException();
        }

        public static ProducerResponse Error(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
