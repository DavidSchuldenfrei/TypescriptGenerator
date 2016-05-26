using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypescriptGenerator.Utils;

namespace TypescriptGenerator.Writers
{
    internal static class UsedTypesWriter
    {
        public static void Write(CodeWriter writer, IEnumerable<Type> usedTypes, Dictionary<string, string> knownTypesModule, string currentModule)
        {
            var hasUsedTypes = false;
            foreach (var usedType in usedTypes)
            {
                string typeModule;
                if (knownTypesModule.TryGetValue(usedType.GetFullName(), out typeModule))
                {
                    hasUsedTypes = true;
                    var relativePath = GetRelativePath(currentModule, typeModule);
                    var name = StringUtils.GetFileName(usedType);
                    var typeName = usedType.Name;
                    var genPos = typeName.IndexOf('`');
                    if (genPos > 0)
                        typeName = typeName.Substring(0, genPos);

                    writer.AddLine($"import {{{typeName}}} from \"{relativePath}{name}\";");
                }
            }
            if (hasUsedTypes)
                writer.AddLine("");

        }

        private static string GetRelativePath(string currentModule, string typeModule)
        {
            var currentParts = currentModule.Split('.').ToList();
            var typeParts = typeModule.Split('.').ToList();
            while (typeParts.Count > 0 && currentParts.Count > 0 && typeParts[0] == currentParts[0])
            {
                typeParts.RemoveAt(0);
                currentParts.RemoveAt(0);
            }
            var sb = new StringBuilder();
            if (currentParts.Any())
            {
                for (var i = 0; i < currentParts.Count; i++)
                {
                    sb.Append("../");
                }
            }
            else
            {
                sb.Append("./");
            }
            foreach (var typePart in typeParts)
            {
                sb.Append(typePart).Append("/");
            }
            return sb.ToString();
        }
    }
}
