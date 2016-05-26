using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypescriptGenerator.Utils
{
    internal static class TypeUtils
    {
        public static string GetTsTypeName(Type type, Dictionary<string, string> knownTypesModule, bool forceSuccess)
        {
            if (type == typeof(string) || IsEnum(type))
                return "string";
            if (type == typeof(int) || type == typeof(int?) ||
                type == typeof(long) || type == typeof(long?) ||
                type == typeof(double) || type == typeof(double?) ||
                type == typeof(decimal) || type == typeof(decimal?))
                return "number";
            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return "Date";
            if (type == typeof(bool) || type == typeof(bool?))
                return "boolean";
            if (type.IsGenericParameter)
                return type.Name;
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                foreach (var intfc in type.GetInterfaces())
                {
                    if (intfc.IsGenericType && intfc.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        var inner = intfc.GenericTypeArguments[0];
                        return GetTsTypeName(inner, knownTypesModule, forceSuccess) + "[]";
                    }
                }
                return "any[]";
            }
            if (type.IsGenericType)
            {
                var typeParts = GetParts(type).ToList();
                if (!forceSuccess && !IsKnown(typeParts[0], knownTypesModule))
                    return "any";
                var sb = new StringBuilder();
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                var genericTypeDefinitionName = RemoveGenericMarker(genericTypeDefinition);
                sb.Append(genericTypeDefinitionName).Append("<");
                foreach (var genericTypeArgument in type.GetGenericArguments())
                {
                    sb.Append(GetTsTypeName(genericTypeArgument, knownTypesModule, forceSuccess)).Append(", ");
                }

                sb.Remove(sb.Length - 2, 2);
                sb.Append(">");
                return sb.ToString();
            }
            return forceSuccess || knownTypesModule != null && knownTypesModule.ContainsKey(type.GetFullName()) ? type.Name : "any";
        }

        public static string RemoveGenericMarker(Type type)
        {
            var name = type.Name;
            var genPos = name.IndexOf('`');
            if (genPos > 0)
                name = name.Substring(0, genPos);
            return name;
        }

        public static string GetFullClassName(Type type)
        {
            var result = GetTsTypeName(type, null, true);
            if (type.BaseType != null && type.BaseType != typeof(object))
                result = result + " extends " + GetTsTypeName(type.BaseType, null, true);
            return result;
        }

        public static IEnumerable<Type> GetUsedTypes(Type type)
        {
            List<Type> result = new List<Type>();
            if (type == typeof (string) || IsEnum(type))
            {
                result.Add(typeof(string));
                return result;
            }
            if (typeof (IEnumerable).IsAssignableFrom(type))
            {
                foreach (var intfc in type.GetInterfaces())
                {
                    if (intfc.IsGenericType && intfc.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        var inner = intfc.GenericTypeArguments[0];
                        result.AddRange(GetUsedTypes(inner));
                        return result;
                    }
                }
            }
            if (type.IsGenericType)
            {
                result.AddRange(GetParts(type));
            }
            else
            {
                result.Add(type);
            }
            return result;
        }

        public static string GetFullName(this Type type) => type.Namespace + "." + type.Name;

        private static bool IsEnum(Type type)
        {
            if (type.IsEnum)
                return true;
            if (!type.IsGenericType)
                return false;
            if (type.GetGenericTypeDefinition() == typeof (Nullable<>))
                return type.GetGenericArguments()[0].IsEnum;
            return false;
        }
        private static bool IsKnown(Type type, Dictionary<string, string> knownTypesModule)
        {
            if (type == typeof(string) ||
                type == typeof(int) || type == typeof(int?) ||
                type == typeof(long) || type == typeof(long?) ||
                type == typeof(double) || type == typeof(double?) ||
                type == typeof(decimal) || type == typeof(decimal?) ||
                type == typeof(DateTime) || type == typeof(DateTime?) ||
                type == typeof(bool) || type == typeof(bool?) ||
                type.IsGenericParameter)
                return true;
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                foreach (var intfc in type.GetInterfaces())
                {
                    if (intfc.IsGenericType && intfc.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        var inner = intfc.GenericTypeArguments[0];
                        return IsKnown(inner, knownTypesModule);
                    }
                }
                return false;
            }
            return knownTypesModule.ContainsKey(type.GetFullName());
        }
        private static IEnumerable<Type> GetParts(Type type)
        {
            var parts = new List<Type> { type.GetGenericTypeDefinition() };
            foreach (var genericTypeArgument in type.GenericTypeArguments)
            {
                if (genericTypeArgument.IsGenericType)
                    parts.AddRange(GetParts(genericTypeArgument));
                else
                    parts.Add(genericTypeArgument);
            }
            return parts;
        }

    }
}
