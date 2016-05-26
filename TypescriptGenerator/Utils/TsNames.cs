using System;
using System.IO;

namespace TypescriptGenerator.Utils
{
    internal static class StringUtils
    {
        public static string GetCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "";
            return char.ToLower(name[0]) + name.Substring(1, name.Length - 1);
        }

        public static string GetPascalCase(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "";
            return char.ToUpper(name[0]) + name.Substring(1, name.Length - 1);
        }
       
        public static string GetFileName(Type type)
        {
            return type.Name.Replace("`", "");
        }

        public static string GetFolder(string outputFolder, string module)
        {
            var parts = module.Split('.');
            foreach (var part in parts)
            {
                outputFolder = Path.Combine(outputFolder, part);
            }
            return outputFolder;
        }
    }
}
