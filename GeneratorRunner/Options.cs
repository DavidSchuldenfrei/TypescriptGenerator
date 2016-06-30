using System.Collections.Generic;
using TypescriptModel;

namespace GeneratorRunner
{
    internal class Options
    {
        public string OutFolder { get; set; }
        public string BasePath { get; set; }
        public List<AssemblyModule> Assemblies { get; set; }
    }
}
