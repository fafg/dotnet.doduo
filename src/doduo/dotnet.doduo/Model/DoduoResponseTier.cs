using dotnet.doduo.MessageBroker.Model;
using System;
using System.Threading.Tasks;

namespace dotnet.doduo.Model
{
    public class DoduoResponseTier
    {
        private DoduoResponse[] m_items;
        private Guid[] m_keys;
        private int m_size;
        private const int DEFAULT_CAPACITY = 12;

        private static readonly DoduoResponse[] m_emptyArray = new DoduoResponse[0];
        private static readonly Guid[] m_emptyKeysArray = new Guid[0];

        public DoduoResponseTier()
        {
            m_items = m_emptyArray;
            m_keys = m_emptyKeysArray;
        }

        public Task UpdateResponse(ProducerResponse response)
        {
            int index = Array.IndexOf(m_keys, response.RequestId);

            DoduoResponse producerResponse = m_items[index];
            producerResponse.Body = response.Body;
            producerResponse.Code = ProducerResponseType.Ok;

            return Task.CompletedTask;
        }

        public void Add(DoduoResponse response)
        {
            int index = Array.IndexOf(m_keys, response.Request.RequestId);

            if (index < 0)
            {
                AddDoduoResponseWithNotExist(response);
            }
        }

        private void AddDoduoResponseWithNotExist(DoduoResponse response)
        {
            int index = Array.IndexOf(m_keys, Guid.Empty);

            lock (m_items)
                lock (m_keys)
                {
                    if ((uint)index >= (uint)m_keys.Length)
                    {
                        EnsureCapacity();
                        index++;
                    }

                    m_items[index] = response;
                    m_keys[index] = response.Request.RequestId;
                }
        }

        private void EnsureCapacity()
        {
            int newCapacity = m_items.Length == 0 ? DEFAULT_CAPACITY : m_items.Length * 2;
            DoduoResponse[] newItems = new DoduoResponse[newCapacity];
            Guid[] newKeys = new Guid[newCapacity];

            Array.Copy(m_items, 0, newItems, 0, m_size);
            Array.Copy(m_keys, 0, newKeys, 0, m_size);

            m_size = newCapacity;
            m_items = newItems;
            m_keys = newKeys;
        }

        public async Task<DoduoResponse> GetResponse(Guid requestId)
        {
            int index = Array.IndexOf(m_keys, requestId);

            if (index < 0)
                return DoduoResponse.Running();

            DoduoResponse result = m_items[index];
            if (result.Code != ProducerResponseType.Running)
            {
                RemoveAt(index);
                Console.Write($"Time Daley :: {(DateTime.Now - result.Request.Started).TotalMilliseconds}");
            }

            return await Task.FromResult(result);
        }

        private void RemoveAt(int index)
        {
            lock (m_items)
                lock (m_keys)
                {
                    m_items[index] = null;
                    m_keys[index] = Guid.Empty;
                }
        }

        public void Clear()
        {
            m_items = m_emptyArray;
            m_keys = m_emptyKeysArray;
        }
    }
}
