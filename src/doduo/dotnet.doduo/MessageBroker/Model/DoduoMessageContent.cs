
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.Model
{
    public class DoduoMessageContent
    {
        public DoduoMessageContent()
        {
            RequestId = Guid.NewGuid();
        }
        public DoduoMessageContent(Guid requestId)
        {
            RequestId = requestId;
        }
        public Guid RequestId { get; set; }
        public DoduoMessageContentObject[] Objects { get; set; }
    }
}
