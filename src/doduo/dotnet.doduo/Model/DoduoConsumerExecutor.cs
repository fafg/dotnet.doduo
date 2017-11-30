using dotnet.doduo.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace dotnet.doduo.Model
{
    public class DoduoConsumerExecutor
    {
        public MethodInfo MethodInfo { get; set; }

        public TypeInfo ImplTypeInfo { get; set; }

        public DoduoTopicAttribute Attribute { get; set; }
    }
}
