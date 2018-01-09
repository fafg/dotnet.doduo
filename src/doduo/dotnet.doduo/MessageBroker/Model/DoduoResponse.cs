using dotnet.doduo.DoduoQueue;
using dotnet.doduo.MessageBroker.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet.doduo.MessageBroker.Model
{
    public class DoduoResponse
    {
        public ProducerResponseType Code { get; internal set; }
        public Exception Exception { get; private set; }
        public DateTime StartRequest { get; internal set; }
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
                StartRequest = DateTime.Now,
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

        public static async Task<DoduoResponse> Response(string topic, Guid requestId)
        {
            DateTime dateWaitStart = DateTime.Now;
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<DoduoResponse> taskProduceResponse = Task.Run(() =>
                GetResponse(topic, requestId)
                , cts.Token);

            return await taskProduceResponse;
        }
        public static async Task<DoduoResponse> GetResponse(string topic, Guid requestId)
        {
            try
            {
                return await DoduoQueueResponse.Get(topic, requestId);
            }
            catch (Exception)
            {
                return await GetResponse(topic, requestId);
            }
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
