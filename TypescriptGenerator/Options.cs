using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TypescriptGenerator
{
    [DataContract]
    internal class Options
    {
        [DataMember(Name = "outFolder")]
        public string OutFolder { get; set; }
        [DataMember(Name = "basePath")]
        public string BasePath { get; set; }
        [DataMember(Name = "assemblies")]
        public List<AssemblyModule> Assemblies { get; set; }
    }

    [DataContract]
    internal class AssemblyModule
    {
        [DataMember(Name = "path")]
        public string Path { get; set; }
        [DataMember(Name = "module")]
        public string Module { get; set; }
        [DataMember(Name = "skipNamespaces")]
        public List<string> SkipNamespcase { get; set; }
        [DataMember(Name = "skipTypes")]
        public List<string> SkipTypes { get; set; }

        public bool ShouldSkip(Type type)
        {
            return (SkipNamespcase?.Contains(type.Namespace) ?? false)
                   || (SkipTypes?.Contains(type.Namespace + "." + type.Name) ?? false);
        }
    }
}
