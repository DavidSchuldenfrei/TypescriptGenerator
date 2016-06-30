using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace TypescriptModel
{
    public class AssemblyModule
    {
        private IEnumerable<RegexAction> _regexActions;
        public string Path { get; set; }
        public string Module { get; set; }
        public List<string> Patterns { get; set; }

        public void Init()
        {
            _regexActions = Patterns?.Select(pattern => pattern.ToRegexAction()).ToList() ?? new List<RegexAction>();
        }

        public bool ShouldSkip(Type type)
        {
            var typeName = type.Namespace + "." + type.Name;
            return !RegexUtils.ShouldInclude(_regexActions, typeName);
        }

    }
}
