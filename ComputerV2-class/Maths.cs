using System.Text.RegularExpressions;
using ComputerV2_class.Exceptions;
namespace ComputerV2_class
{
    public class Maths
    {
        //throws InvalidExpressionException
        public static string Calculate(string expression)
        {
            expression = expression.Replace(" ", "");
            Brackets(ref expression);
            Pow(ref expression);
            DivideMultiply(ref expression);
            Imaginary(ref expression);
            Addition(ref expression);
            return (expression);
        }

        private static void Brackets(ref string expression)
        {
            var regex = new Regex(@"(?<=\()([^a-zA-z]*)(?=\))");
            while (regex.IsMatch(expression))
            {
                var match = regex.Match(expression).Value;
                expression = expression.Replace ($"({match})", Calculate(match));
            }
        }

        private static void Pow(ref string expression)
        {
            var regex = new Regex(@"((\-)?\d+([\.]\d+)?)\^((\-)?\d+([\.]\d+)?)");
            while (regex.IsMatch(expression))
            {
                var match = regex.Match(expression).Value;
                match = ReplaceDecimalPoint(match);
                var exp = match.Split('^');
                try
                {
                    double value = double.Parse(exp[0]);
                    //will throw formateException if number is not whole and positive.
                    int count = int.Parse(exp[1]);
                    if (count < 0)
                        throw new System.FormatException();
                    double res = value;
                    for (var i = 1; i < count; i++) res *= value;
                    //prevent exponential notation to a degree, to big of a number will have a result of infinity;
                    expression = expression.Replace(match, ReplaceDecimalPoint(res.ToString("0." + new string('#', 9999))));
                }
                catch(System.FormatException)
                {
                    throw new InvalidExpressionException("Power is of wrong format");
                }              
            }
        }

        private static void DivideMultiply(ref string expression)
        {
            var regex = new Regex(@"((\-)?\d+([\.]\d+)?)(\*|\%|\/)((\-)?\d+([\.]\d+)?)");
            while(regex.IsMatch(expression))
            {
                var match = regex.Match(expression).Value;
                match = ReplaceDecimalPoint(match);
                var exp = Regex.Split(match, @"(\*)|(\/)|(\%)");
                double n1 = double.Parse(exp[0]), n2 = double.Parse(exp[2]), res = 0;
                if (n2 != 0 || exp[1] == "*")
                {
                    switch (exp[1])
                    {
                        case "*": res = n1 * n2; break;
                        case "%": res = n1 % n2; break;
                        default: res = n1 / n2; break;
                    }
                }
                else
                {
                    throw new InvalidExpressionException("Attempted to divide by 0");
                }
                expression = expression.Replace(match, ReplaceDecimalPoint($"{res}"));
            }
        }

        private static void Imaginary(ref string expression)
        {
            var regex = new Regex(@"((\-|\+)?\d+([\.]\d+)?)(\*)?i");
            if (regex.IsMatch(expression))
            {
                var res = 0d;
                while(regex.IsMatch(expression))
                {
                    var match = regex.Match(expression).Value;
                    var nmb = Regex.Match(match, @"((\-|\+)?\d+([\.]\d+)?)").Value;
                    res += double.Parse(nmb);
                    expression = expression.Replace(match, "");
                }
                if (res != 0)
                    expression = $"{res}*i+0+" + expression;
            }
        }

        private static void Addition(ref string expression)
        {
            var regex = new Regex(@"[^\^]*((\-)?\d+([\.]\d+)?)\+((\-)?\d+([\.]\d+)?)");
            if (Regex.IsMatch(expression, @"(\d+([\.]\d+)?)(\-)(\d+([\.]\d+)?)"))
                expression = ManageNegative(expression);
            while(regex.IsMatch(expression))
            {
                var match = regex.Match(expression).Value;
                if (match[0] == '+')
                    match = match.Substring(1);
                var m2 = ReplaceDecimalPoint(match);
                var exp = Regex.Split(m2, @"\+");
                double n1 = double.Parse(exp[0]), n2 = double.Parse(exp[1]), res = 0;
                res = n1 + n2;
                expression = expression.Replace(match, $"{res}");
            }
        }

        private static string ManageNegative(string expression)
        {
            var regex = new Regex(@"(\d+([\.]\d+)?)(\-)(\d+([\.]\d+)?)");
            while (regex.IsMatch(expression))
                expression = regex.Replace(expression, "$1+$3$4");
            return (expression);
        }

        private static string ReplaceDecimalPoint(string expression)
        {
            if (Regex.IsMatch(expression, @"([\.])(\d+)"))
                expression = Regex.Replace(expression, @"([\.])(\d+)", ",$2");
            return (expression);
        }
    }
}
