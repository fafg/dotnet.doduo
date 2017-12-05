using System;

namespace dotnet.doduo.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class DoduoTopicAttribute : Attribute
    {
        public DoduoTopicAttribute(string topic)
        {
            Topic = topic;
        }

        public string Topic { get; set; }
    }
}
