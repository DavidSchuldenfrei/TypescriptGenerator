using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TypescriptGenerator.Utils;

namespace TypescriptGenerator.Writers
{
    internal static class TypeWriter
    {
        public static void WriteType(Type type, string module, Dictionary<string, string> knownTypesModule,
            string output)
        {
            var writer = new CodeWriter();
            var usedTypes = GetUsedClasses(type).ToList();
            UsedTypesWriter.Write(writer, usedTypes, knownTypesModule, module);

            WriteClass(type, knownTypesModule, writer);

            writer.SaveToFile(Path.Combine(output, StringUtils.GetFileName(type) + ".ts"));
        }

        private static void WriteClass(Type type, Dictionary<string, string> knownTypesModule, CodeWriter writer)
        {
            var fullClassName = TypeUtils.GetFullClassName(type);
            writer.AddLine($"export class {fullClassName}");
            using (writer.CreateBlock())
            {
                foreach (
                    var propertyInfo in
                        type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                {
                    writer.AddLine(
                        $"{propertyInfo.Name}: {TypeUtils.GetTsTypeName(propertyInfo.PropertyType, knownTypesModule, false)};");
                }
            }
        }

        private static IEnumerable<Type> GetUsedClasses(Type type)
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
