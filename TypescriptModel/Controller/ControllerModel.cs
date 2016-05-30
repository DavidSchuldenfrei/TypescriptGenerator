using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypescriptModel.Common;

namespace TypescriptModel.Controller
{
    public class ControllerModel
    {
        public ControllerModel(Type type, string module, Dictionary<string, string> knownTypesModule)
        {
            Name = type.Name;
            Module = module;
            var controllerName = Name;
            if (controllerName.EndsWith("Controller"))
                controllerName = controllerName.Substring(0, controllerName.Length - 10);
            var controllerRoutePrefix = $"api/{controllerName}";

            Methods = type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(methodInfo => new MethodModel(methodInfo, controllerRoutePrefix, knownTypesModule))
                .ToList();
            var usedTypes = Methods.SelectMany(GetMethodTypes).Distinct();
            UsedClasses = new List<UsedClass>();
            foreach (var usedType in usedTypes)
            {
                string usedModule;
                if (knownTypesModule.TryGetValue(usedType.GetFullName(), out usedModule))
                {
                    UsedClasses.Add(new UsedClass(usedType, usedModule));
                }
            }

        }

        private IEnumerable<Type> GetMethodTypes(MethodModel method)
        {
            var result = method.Parameters.SelectMany(p => TypeUtils.GetUsedTypes(p.Type)).ToList();
            if (method.ReturnType != null)
                result.AddRange(TypeUtils.GetUsedTypes(method.ReturnType));
            return result;
        }

        public string Name { get; }
        public string Module { get; }
        public List<MethodModel> Methods { get; }

        public List<UsedClass> UsedClasses { get; }
    }
}
