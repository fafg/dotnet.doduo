using dotnet.doduo.MessageBroker.Model;
using dotnet.doduo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet.doduo.DoduoQueue
{
    public class DoduoQueueResponse
    {
        private static Dictionary<string, DoduoQueueModel> m_listQueueResponse = new Dictionary<string, DoduoQueueModel>();

        public static void Create(string topic)
        {
            if (m_listQueueResponse.ContainsKey(topic))
                return;

            var temp = m_listQueueResponse;
            temp.Add(topic, new DoduoQueueModel());
            m_listQueueResponse = temp;
        }

        public static async Task Send(string topic, ProducerResponse response)
        {
            DoduoQueueModel doduoQueueModel = m_listQueueResponse[topic];
            await doduoQueueModel.AddResponse(response);
        }

        public static void SendRequestWait(string topic, DoduoMessageContent request)
        {
            DoduoResponse result = new DoduoResponse();
            result.Request = request;

            DoduoQueueModel doduoQueueModel = m_listQueueResponse[topic];
            doduoQueueModel.Add(result);
        }

        public static async Task<DoduoResponse> Get(string topic, Guid requestId)
        {
            topic = $"{topic}.response";
            DoduoQueueModel doduoQueueModel = m_listQueueResponse[topic];

            DoduoResponse result =await doduoQueueModel.GetResponse(requestId);
            while (result.Code == ProducerResponseType.Running)
            {
                Thread.Sleep(1);
                result = await doduoQueueModel.GetResponse(requestId);
            }
            return await Task.FromResult(result);
        }

        public static void Clear()
        {
            foreach (var queue in m_listQueueResponse.Values)
                queue.Clear();

            m_listQueueResponse.Clear();
        }
    }
}
