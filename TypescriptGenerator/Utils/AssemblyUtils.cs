using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace TypescriptGenerator.Utils
{
    public static class AssemblyUtils
    {
        public static bool IsWebAssembly(Assembly assembly)
        {
            return assembly.GetTypes().Any(t => typeof(ApiController).IsAssignableFrom(t));
        }

        public static IEnumerable<Type> GetKnownTypes(Assembly assembly)
        {
            if (IsWebAssembly(assembly))
                return new List<Type>();
            return assembly.GetTypes();
        }
    }
}
