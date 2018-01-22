
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.Model
{
    public class DoduoMessageContent
    {
        public DoduoMessageContent()
        {
            Started = DateTime.Now;
            RequestId = Guid.NewGuid();
        }
        public DoduoMessageContent(Guid requestId)
        {
            Started = DateTime.Now;
            RequestId = requestId;
        }
        public Guid RequestId { get; set; }
        public DoduoMessageContentObject[] Objects { get; set; }
        public DateTime Started { get; internal set; }
    }
}
