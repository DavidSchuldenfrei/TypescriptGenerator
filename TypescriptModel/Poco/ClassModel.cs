using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypescriptModel.Common;

namespace TypescriptModel.Poco
{
    public class ClassModel
    {
        public ClassModel(Type type, string module, Dictionary<string, string> knownTypesModule)
        {
            Type = type;
            TsName = TypeUtils.GetFullClassName(type);
            FileName = type.Name.Replace("`", "");
            Module = module;
            var usedTypes = GetUsedTypes(type);
            UsedClasses = new List<UsedClass>();
            foreach (var usedType in usedTypes)
            {
                string usedModule;
                if (knownTypesModule.TryGetValue(usedType.GetFullName(), out usedModule))
                {
                    UsedClasses.Add(new UsedClass(usedType, usedModule));
                }
            }
            Properties = new List<ModelProperty>();
            foreach (
                var propertyInfo in
                    type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                Properties.Add(new ModelProperty(propertyInfo, knownTypesModule));
            }
        }

        public Type Type { get; }
        public string TsName { get; }
        public string Module { get; }
        public string FileName { get; }
        public List<ModelProperty> Properties { get; }
        public List<UsedClass> UsedClasses { get; }

        private static IEnumerable<Type> GetUsedTypes(Type type)
        {
            var result = new List<Type>();
            foreach (
                var propertyInfo in
                    type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                result.AddRange(TypeUtils.GetUsedTypes(propertyInfo.PropertyType));
            }
            if (type.BaseType != null)
                result.AddRange(TypeUtils.GetUsedTypes(type.BaseType));
            return result.Distinct();
        }
    }
}
