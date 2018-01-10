using dotnet.doduo.MessageBroker.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet.doduo.Model
{
    public class DoduoTier
    {
        private DoduoResponseTier[] m_items;
        private string[] m_keys;
        private int m_size;
        private const int DEFAULT_CAPACITY = 4;

        private static readonly DoduoResponseTier[] m_emptyArray = new DoduoResponseTier[0];
        private static readonly string[] m_emptyKeysArray = new string[0];

        public DoduoTier()
        {
            m_items = m_emptyArray;
            m_keys = m_emptyKeysArray;
        }

        public void SendRequestWait(string topic, DoduoMessageContent content)
        {
            var keys = m_keys;
            int index = Array.IndexOf(keys, topic);
            if (index < 0)
            {
                index = AddTopicWithNotExist(topic);
            }
            DoduoResponse result = new DoduoResponse();
            result.Request = content;

            m_items[index].Add(result);
        }

        public void Clear()
        {
            foreach (var item in m_items)
            {
                item.Clear();
            }
            m_items = m_emptyArray;
            m_keys = m_emptyKeysArray;
        }

        private int AddTopicWithNotExist(string topic)
        {
            int index = Array.IndexOf(m_keys, Guid.Empty);

            if ((uint)index >= (uint)m_keys.Length)
            {
                EnsureCapacity();
                index++;
            }
            m_items[index] = new DoduoResponseTier();
            m_keys[index] = topic;

            return index;
        }

        private void EnsureCapacity()
        {
            int newCapacity = m_items.Length == 0 ? DEFAULT_CAPACITY : m_items.Length * 2;
            DoduoResponseTier[] newItems = new DoduoResponseTier[newCapacity];
            string[] newKeys = new string[newCapacity];

            Array.Copy(m_items, 0, newItems, 0, m_size);
            Array.Copy(m_keys, 0, newKeys, 0, m_size);

            m_size = newCapacity;
            m_items = newItems;
            m_keys = newKeys;
        }

        public async Task Send(string topic, ProducerResponse response)
        {
            var arrayKeys = m_keys;
            int index = Array.IndexOf(arrayKeys, topic);

            if (index >= 0)
            {
                DoduoResponseTier responseTier = m_items[index];
                await responseTier.UpdateResponse(response);
                return;
            }

            throw new Exception("No exist this topit to listern");
        }

        public async Task<DoduoResponse> Get(string topic, Guid requestId)
        {
            int index = Array.IndexOf(m_keys, topic);

            if (index >= 0)
            {
                DoduoResponseTier tier = m_items[index];

                DoduoResponse result = await tier.GetResponse(requestId);
                while (result.Code == ProducerResponseType.Running)
                {
                    Thread.Sleep(1);
                    result = await tier.GetResponse(requestId);
                }
                return await Task.FromResult(result);
            }

            throw new Exception("Not Exist List");
        }
    }
}
