using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.Model
{
    public class DoduoResponseContent
    {
        public DoduoResponseContent(Guid responseId)
        {
            ResponseId = responseId;
        }

        public Guid ResponseId { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }
    }
}
