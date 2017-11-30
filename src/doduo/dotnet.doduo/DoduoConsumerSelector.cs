using dotnet.doduo.Attributes;
using dotnet.doduo.Helpers;
using dotnet.doduo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dotnet.doduo
{
    public class DoduoConsumerSelector
    {
        private static IEnumerable<DoduoConsumerExecutor> m_doduoCusumerExecutorsCache = new List<DoduoConsumerExecutor>();

        private IEnumerable<DoduoConsumerExecutor> FindConsumersFromControllerTypes()
        {
            var doduoConsumerExecutors = new List<DoduoConsumerExecutor>();
            var types = Assembly.GetEntryAssembly().ExportedTypes;
            foreach (var type in types)
            {
                var typeInfo = type.GetTypeInfo();
                if (ControllerHelper.IsController(typeInfo))
                    doduoConsumerExecutors.AddRange(GetAllDoduoConsumerExecutor(typeInfo));
            }
            return doduoConsumerExecutors;
        }

        private IEnumerable<DoduoConsumerExecutor> GetAllDoduoConsumerExecutor(TypeInfo typeInfo)
        {
            foreach (var method in typeInfo.DeclaredMethods)
            {
                var topicAttr = method.GetCustomAttributes<DoduoTopicAttribute>(true);
                var topicAttributes = topicAttr as IList<DoduoTopicAttribute> ?? topicAttr.ToList();

                if (!topicAttributes.Any()) continue;

                foreach (var attr in topicAttributes)
                {
                    yield return new DoduoConsumerExecutor
                    {
                        Attribute = attr,
                        MethodInfo = method,
                        ImplTypeInfo = typeInfo
                    };
                }
            }
        }

        public IEnumerable<DoduoConsumerExecutor> GetCandidatesMethods()
        {
            if (m_doduoCusumerExecutorsCache.Any())
                return m_doduoCusumerExecutorsCache;

            m_doduoCusumerExecutorsCache = this.FindConsumersFromControllerTypes();

            return m_doduoCusumerExecutorsCache;
        }
    }
}
