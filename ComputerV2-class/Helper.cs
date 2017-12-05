using System.Text.RegularExpressions;
using ComputerV2_class.Exceptions;

namespace ComputerV2_class
{
    public static class Helper
    {
        public static string[] Split(string str)
        {
            var s = str.Replace(".", ",");
            return (Regex.Split(s.Replace(" ", ""), @"(\-)|(\+)|(\=)"));
        }

        public static string Reduce_helper1(string f)
        {
            var rgx = new Regex(@"(((\-|\+)?\d+([\.,]\d+)?)(\/|\%|\*)((\-|\+)?\d+([\.,]\d+)?))");
            var match = rgx.Match(f);
            while (match.Success)
            {
                var str = Regex.Replace(match.Value, @"(\-|\+)?\d+([\.,]\d+)?", m => string.Format(@"0{0}", m.Value));
                var tmp = Maths.Calculate(str);
                f = f.Replace(match.Value, tmp.Contains("-") ? tmp : "+" + tmp);
                match = rgx.Match(f);
            }
            return f;
        }

        public static double Reduce_helper(string[] value, string rgx)
        {
            double ret = 0d;
            var prev = "";
            foreach (var v in value)
            {
                if (Regex.IsMatch(v, rgx))
                {
                    double.TryParse(Regex.Match(prev + v, @"((\+)|(\-))?\d+([\.,]\d+)?").Value, out double tmp);
                    ret += tmp;
                }
                prev = v;
            }
            return ret;
        }
        
        public static void ValidateInput(string input)
        {
            if (Regex.IsMatch(input, @"([a-zA-Z]+(\(.*\))?)(\s+|\r+)([a-zA-Z]+(\(.*\))?)"))
                throw new InvalidExpressionException("Invalid Use of Variables."); 
            var braces = 0;
            input = input.Replace(" ", "");
            if (Regex.IsMatch(input, @"^\=\?$"))
                throw new InvalidExpressionException("Cannot query nothing."); 
            if (Regex.IsMatch(input, @"\=$"))
                throw new InvalidExpressionException("Cannot assing something to nothing"); 
            foreach (char t in input)
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
            if (Regex.IsMatch(input, @"([=]{2,})|[?]{2,}|(\=(\+|\*\/\%)|(\+|\*\/\%)\=)|(\+\-|\-\+)|\*\*\*|(\%\*\*\%|\/\*|\*\/|(\/|\%|\+|\-){2,})"))
                throw new InvalidExpressionException("Too many Repeating signs.");
        }
    }
}