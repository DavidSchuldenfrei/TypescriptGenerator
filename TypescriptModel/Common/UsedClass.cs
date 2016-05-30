using System;
using System.Linq;
using System.Text;

namespace TypescriptModel.Common
{
    public class UsedClass
    {
        public UsedClass(Type type, string location)
        {
            var typeName = type.Name;
            var genPos = typeName.IndexOf('`');
            if (genPos > 0)
                typeName = typeName.Substring(0, genPos);

            Name = typeName;
            Location = location;
            FileName = type.Name.Replace("`", "");
        }

        public string Name { get; }
        public string FileName { get; }
        public string Location { get; }

        public string GetRelativePath(string currentModule)
        {
            var currentParts = currentModule.Split('.').ToList();
            var typeParts = Location.Split('.').ToList();
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
