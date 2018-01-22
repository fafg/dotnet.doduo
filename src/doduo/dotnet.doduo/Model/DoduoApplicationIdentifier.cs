using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.Model
{
    public sealed class DoduoApplicationIdentifier
    {
        public DoduoApplicationIdentifier()
        {
            ApplicationId = Guid.NewGuid().ToString().Replace("-", "");
        }
        public string ApplicationId { get; internal set; }
    }
}
