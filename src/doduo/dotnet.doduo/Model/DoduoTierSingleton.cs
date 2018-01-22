using dotnet.doduo.MessageBroker.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet.doduo.Model
{
    public static class DoduoTierSingleton
    {
        private static readonly DoduoTier m_instance = new DoduoTier();
        public static DoduoTier Instance { get; } = m_instance;

    }
}
