using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace dotnet.doduo.Helpers
{
    public static class ControllerHelper
    {
        public static bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
                return false;

            if (typeInfo.IsAbstract)
                return false;

            if (!typeInfo.IsPublic)
                return false;

            return !typeInfo.ContainsGenericParameters
                   && typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase);
        }
    }
}
