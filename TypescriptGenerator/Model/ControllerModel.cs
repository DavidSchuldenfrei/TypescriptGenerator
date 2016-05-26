using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TypescriptGenerator.Model
{
    public class ControllerModel
    {
        public string Name { get; }
        public Type Type { get; }
        public List<MethodModel> Methods { get; }

        public ControllerModel(Type controllerType)
        {
            Name = controllerType.Name;
            Type = controllerType;
            var controllerName = Name;
            if (controllerName.EndsWith("Controller"))
                controllerName = controllerName.Substring(0, controllerName.Length - 10);
            var controllerRoutePrefix = $"api/{controllerName}";
            Methods = controllerType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(methodInfo => new MethodModel(methodInfo, controllerRoutePrefix))
                .ToList();
        }

        public IEnumerable<Type> GetUsedClasses()
        {
            var result = Methods.SelectMany(method => method.GetUsedClasses());
            return result.Distinct();
        }

    }
}
