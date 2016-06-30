using System.Text.RegularExpressions;

namespace Utils
{
    public class RegexAction
    {
        public RegexAction(Action action, Regex regex)
        {
            Action = action;
            Regex = regex;
        }

        public Action Action { get; }
        public Regex Regex { get; }
    }
}