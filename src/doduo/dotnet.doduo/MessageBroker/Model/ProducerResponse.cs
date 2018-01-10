using System;

namespace dotnet.doduo.MessageBroker.Model
{
    public class ProducerResponse
    {
        public ProducerResponseType Code { get; internal set; }
        public Guid RequestId { get; internal set; }        
        public Exception Exception { get; private set; }
        public DoduoResponseContent Body { get; internal set; }

        public static ProducerResponse Ok(Guid RequestId)
        {
            return new ProducerResponse
            {
                RequestId = RequestId,
                Code = ProducerResponseType.Ok
            };
        }

        public static ProducerResponse Ok(Guid RequestId, DoduoResponseContent body)
        {
            return new ProducerResponse
            {
                RequestId = RequestId,
                Code = ProducerResponseType.Ok,
                Body = body
            };
        }

        public static ProducerResponse Error(Guid RequestId, Exception ex)
        {
            return new ProducerResponse
            {
                RequestId = RequestId,
                Code = ProducerResponseType.Faiure,
                Exception = ex
            };
        }
    }
}
