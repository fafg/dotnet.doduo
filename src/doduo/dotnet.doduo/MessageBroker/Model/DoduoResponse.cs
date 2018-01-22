using System;

namespace dotnet.doduo.MessageBroker.Model
{
    public class DoduoResponse
    {
        public ProducerResponseType Code { get; internal set; }
        public Exception Exception { get; private set; }
        public DoduoResponseContent Body { get; internal set; }
        public DoduoMessageContent Request { get; internal set; }

        public static DoduoResponse Ok()
        {
            return new DoduoResponse
            {
                Code = ProducerResponseType.Ok
            };
        }

        public static DoduoResponse Running()
        {
            return new DoduoResponse
            {
                Code = ProducerResponseType.Running
            };
        }

        public static DoduoResponse Ok(DoduoResponseContent body)
        {
            return new DoduoResponse
            {
                Code = ProducerResponseType.Ok,
                Body = body
            };
        }

        public static DoduoResponse Error(Exception ex)
        {
            return new DoduoResponse
            {
                Code = ProducerResponseType.Faiure,
                Exception = ex
            };
        }
    }
}
