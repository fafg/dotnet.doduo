using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet.doduo.MessageBroker.Model
{
    public class DoduoMessageContentObject
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public bool IsPrimitive { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.PropertyName + this.Value + this.IsPrimitive).GetHashCode();
        }
    }
}
