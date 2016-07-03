using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Utils
{
    public static class RegexUtils
    {
        public static Regex ToRegex(this string pattern)
        {
            return new Regex("^" + pattern
                .Replace(".", "\\.")
                .Replace("*", ".*") + "$");
        }

        public static RegexAction ToRegexAction(this string pattern)
        {
            var action = Action.Include;
            if (pattern.StartsWith("-"))
            {
                pattern = pattern.Substring(1);
                action = Action.Exclude;
            }
            return new RegexAction(action, pattern.ToRegex());
        }

        public static bool ShouldInclude(IEnumerable<RegexAction> regexActions, string pattern)
        {
            if (regexActions == null)
                return true;
            var result = true;
            foreach (var regexAction in regexActions)
            {
                if (MayChange(regexAction.Action, result))
                {
                    if (regexAction.Regex.IsMatch(pattern))
                        result = !result;
                }
            }
            return result;
        }

        private static bool MayChange(Action action, bool result)
        {
            return (action == Action.Exclude && result) || (action == Action.Include && !result);
        }
    }
}
