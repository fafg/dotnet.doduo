using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.Model
{
    public class DoduoMessage
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
