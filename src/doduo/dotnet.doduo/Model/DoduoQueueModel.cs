using dotnet.doduo.MessageBroker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.doduo.Model
{
    public class DoduoQueueModel
    {
        public DoduoQueueModel()
        {
            _listProducerResponse = new LinkedList<DoduoResponse>();
        }
        private LinkedList<DoduoResponse> _listProducerResponse;

        public async Task AddResponse(ProducerResponse response)
        {
            DoduoResponse producerResponse = await GetResponse(response.RequestId);
            producerResponse.Body = response.Body;
            producerResponse.Code = ProducerResponseType.Ok;
        }

        public void Add(DoduoResponse response)
        {
            _listProducerResponse.AddLast(response);
        }

        public async Task<DoduoResponse> GetResponse(Guid requestId)
        {
            if (!_listProducerResponse.Any(p => p.Request.RequestId == requestId))
                return null;

            DoduoResponse result = _listProducerResponse.Where(p => p.Request.RequestId == requestId).SingleOrDefault();
            if (result.Code != ProducerResponseType.Running)
                _listProducerResponse.Remove(result);

            return await Task.FromResult(result);
        }

        public void Clear()
        {
            _listProducerResponse.Clear();
        }
    }
}
