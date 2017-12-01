using dotnet.doduo.MessageBroker.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                    yield return GetDefaultValue(parameter);
                else
                    yield return GetValueObject(parameter, obj);
            }
        }

        private static object GetValueObject(ParameterInfo parameter, DoduoMessageContentObject obj)
        {
            object value = null;

            if (!obj.IsPrimitive)
                value = JObject.Parse(obj.Value.ToString()).ToObject(parameter.ParameterType);
            else
                value = obj.Value;

            return value ?? GetDefaultValue(parameter);
        }

        public static object GetDefaultValue(ParameterInfo parameter)
        {
            if (parameter.HasDefaultValue)
                return parameter.DefaultValue;

           else if (!parameter.ParameterType.IsPrimitive && parameter.ParameterType != typeof(IConvertible))
                return null;

            else if (parameter.ParameterType == typeof(int)
                || parameter.ParameterType == typeof(short)
                || parameter.ParameterType == typeof(long) 
                || parameter.ParameterType == typeof(decimal)
                || parameter.ParameterType == typeof(float))
                return 0;

            else if (parameter.ParameterType == typeof(DateTime))
                return DateTime.MinValue;
            return null;
        }
    }
}
