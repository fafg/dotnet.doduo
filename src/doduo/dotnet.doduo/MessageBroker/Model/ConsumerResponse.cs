using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.Model
{
    public class ConsumerResponse
    {
        public ConsumerResponseType Code { get; private set; }
        public Exception Exception { get; private set; }

        public static ConsumerResponse Ok()
        {
            return new ConsumerResponse
            {
                Code = ConsumerResponseType.Ok
            };
        }

        public static ConsumerResponse Error(Exception ex)
        {
            return new ConsumerResponse
            {
                Code = ConsumerResponseType.Faiure,
                Exception = ex
            };
        }
    }
}
