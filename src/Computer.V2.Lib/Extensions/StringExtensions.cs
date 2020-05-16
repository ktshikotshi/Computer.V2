using System;
using System.Text.RegularExpressions;

using Computer.V2.Lib.Exceptions;

namespace Computer.V2.Lib.Extensions
{
    public static class StringExtensions
    {
        public static void Validate(this string str)
        {
            if (Regex.IsMatch(str, @"([a-zA-Z]+(\(.*\))?)(\s+|\r+)([a-zA-Z]+(\(.*\))?)"))
                throw new InvalidExpressionException("Invalid Use of Variables."); 
            var braces = 0;
            str = str.Replace(" ", "");
            if (Regex.IsMatch(str, @"^\=\?$"))
                throw new InvalidExpressionException("Cannot query nothing."); 
            if (Regex.IsMatch(str, @"\=$"))
                throw new InvalidExpressionException("Cannot assing something to nothing"); 
            foreach (char t in str)
            {
                switch (t)
                {
                    case '[':
                        braces += 1;
                        break;
                    case ']':
                        braces -= 1;
                        break;
                    case '(':
                        braces += 1;
                        break;
                    case ')':
                        braces -= 1;
                        break;
                }
            }
            if (braces != 0) throw new InvalidExpressionException("Opening braces must have a corresponding closing brace.");
            if (Regex.IsMatch(str, @"([=]{2,})|[?]{2,}|(\=(\+|\*\/\%)|(\+|\*\/\%)\=)|(\+\-|\-\+)|\*\*\*|(\%\*\*\%|\/\*|\*\/|(\/|\%|\+|\-){2,})"))
                throw new InvalidExpressionException("Too many Repeating signs.");
        }
        
        public static string[] SplitExpression(this string str)
        {
            var s = str.Replace(".", ",");
            return (Regex.Split(s.Replace(" ", ""), @"(\-)|(\+)|(\=)"));
        }
    }
}