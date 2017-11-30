using dotnet.doduo.MessageBroker.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dotnet.doduo.Helpers
{
    public class ParameterBuilderHelper
    {
        public static IEnumerable<object> BuildParameters(ParameterInfo[] parameterInfos, DoduoMessageContent content)
        {
            List<object> result = new List<object>();
            result.AddRange(HydrateParammiters(parameterInfos, content));
            return result;
        }

        private static IEnumerable<object> HydrateParammiters(ParameterInfo[] parameterInfos, DoduoMessageContent content)
        {
            IList<DoduoMessageContentObject> objects = content.Objects.ToList();

            foreach (ParameterInfo parameter in parameterInfos)
            {
                var obj = objects.Where(p => p.PropertyName == parameter.ParameterType.ToString()).FirstOrDefault();

                objects.Remove(obj);

                if (obj == null)
                    yield return null;
                else
                    yield return GetValueObject(parameter, obj);
            }
        }

        private static object GetValueObject(ParameterInfo parameter, DoduoMessageContentObject obj)
        {
            object valueDafault = parameter.HasDefaultValue ? parameter.DefaultValue : null;
            object value = null;

            if (!obj.IsPrimitive)
                value = JObject.Parse(obj.Value.ToString()).ToObject(parameter.ParameterType);
            else
                value = obj.Value;

            return value ?? valueDafault;
        }
    }
}
