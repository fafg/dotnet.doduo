using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.Model
{
    public class ProducerResponse
    {
        public ProducerResponseType Code { get; private set; }
        public Exception Exception { get; private set; }

        public static ProducerResponse Ok()
        {
            return new ProducerResponse
            {
                Code = ProducerResponseType.Ok
            };
        }

        public static ProducerResponse Error(Exception ex)
        {
            return new ProducerResponse
            {
                Code = ProducerResponseType.Faiure,
                Exception = ex
            };
        }
    }
}
