using System;
using System.Collections.Generic;

namespace TypescriptModel
{
    public class AssemblyModule
    {
        public string Path { get; set; }
        public string Module { get; set; }
        public List<string> SkipNamespaces { get; set; }
        public List<string> SkipTypes { get; set; }

        public bool ShouldSkip(Type type)
        {
            return (SkipNamespaces?.Contains(type.Namespace) ?? false)
                   || (SkipTypes?.Contains(type.Namespace + "." + type.Name) ?? false);
        }

    }
}
