using System.Text.RegularExpressions;

namespace ProiectColectiv.Core.Utils
{
    public static class StringUtils
    {
        public static string GetCamelCase(string word)
        {
            const string strRegex = @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])";
            const string strReplace = @" $1$2";

            return new Regex(strRegex, RegexOptions.None).Replace(word, strReplace);
        }
    }
}