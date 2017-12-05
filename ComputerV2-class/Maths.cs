using System.Text.RegularExpressions;
using ComputerV2_class.Exceptions;

namespace ComputerV2_class
{
    public class Maths
    {
        //throws InvalidExpressionException
        public static string Calculate(string expression)
        {
            var matrix = Parse.MatrixManipulation(expression);
            if (!matrix.Found && matrix.Message == null)
            {
                if (Regex.IsMatch(expression, @"[\*]{2,}"))
                    throw new InvalidExpressionException("Input Error");
                expression = expression.Replace(" ", "");
                Brackets(ref expression);
                Pow(ref expression);
                DivideMultiply(ref expression);
                Imaginary(ref expression);
                Addition(ref expression);
                expression = expression.Replace(" ", "");
                if (expression.Length > 0)
                {
                    if (expression[0] == '+')
                        expression = expression.Substring(1);
                    if (expression[expression.Length - 1] == '+')
                        expression = expression.Substring(0, expression.Length - 1);
                }
            }
            else expression = matrix.Value;
            return (expression);
        }

        private static void Brackets(ref string expression)
        {
            var regex = new Regex(@"(?<=\()([^a-zA-z].*)(?=\))");
            while (regex.IsMatch(expression))
            {
                var match = regex.Match(expression).Value;
                expression = expression.Replace ($"({match})", $"1*{Calculate(match)}");
            }
        }

        private static void Pow(ref string expression)
        {
            var regex = new Regex(@"((\-)?\d+([\.,]\d+)?(?=\*i))\^((\-)?\d+([\.,]\d+)?)");
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
                    double res = value > 0? value: -1 * value;
                    for (var i = 1; i < count; i++) res *= value;
                    //prevent exponential notation to a degree, to big of a number will have a result of infinity;
                    expression = expression.Replace(match, res.ToString("0." + new string('#', 9999)));
                }
                catch(System.FormatException)
                {
                    throw new InvalidExpressionException("Power is of wrong format");
                }              
            }
        }

        private static void DivideMultiply(ref string expression)
        {
            var regex = new Regex(@"((\-)?\d+([\.,]\d+)?)(\*|\%|\/)((\-)?\d+([\.,]\d+)?)");
            expression = ManageNegative(expression);
            while(regex.IsMatch(expression))
            {
                var match = regex.Match(expression).Value;
                var m2 = ReplaceDecimalPoint(match);
                var exp = Regex.Split(m2, @"(\*)|(\/)|(\%)");
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
                expression = expression.Replace(match, $"{res}");
            }
        }

        private static void Imaginary(ref string expression)
        {
            var regex = new Regex(@"((\-|\+)?\d+([\.,]\d+)?)(\*)?i");
            if (regex.IsMatch(expression))
            {
                var res = 0d;
                while(regex.IsMatch(expression))
                {
                    var match = regex.Match(expression).Value;
                    var nmb = Regex.Match(match, @"((\-|\+)?\d+([\.,]\d+)?)").Value;
                    res += double.Parse(nmb);
                    expression = expression.Replace(match, "");
                }
                if (res != 0)
                {
                   expression = expression =="" || Regex.IsMatch(expression,@"^(\+)|(\-)")? $"{res}*i" + expression: $"{res}*i+" + expression;
                }
            }
        }

        private static void Addition(ref string expression)
        {
            var regex = new Regex(@"(?<![\^])((\-)?\d+([\.,]\d+)?)\+((\-)?\d+([\.,]\d+)?)");
            var rgx = new Regex(@"(\-|\+)?\(.*\)(\^(\d+([\.,]\d+)?))?");
            var braceMatches = rgx.Matches(expression);
            if (braceMatches.Count > 0)
                expression = rgx.Replace(expression, "");
            if (Regex.IsMatch(expression, @"(\d+([\.,]\d+)?)(\-)?(\-)(\d+([\.,]\d+)?)"))
                expression = ManageNegative(expression);
            while(regex.IsMatch(expression))
            {
                var match = regex.Match(expression).Value;
                if (match[0] == '+')
                    match = match.Substring(1);
                var m2 = ReplaceDecimalPoint(match);
                var exp = Regex.Split(m2, @"\+");
                try
                {
                    double n1 = double.Parse(exp[0]), n2 = double.Parse(exp[1]), res = 0;
                    res = n1 + n2;
                    expression = expression.Replace(match, $"{res}");
                }
                catch(System.FormatException)
                {
                    throw new InvalidExpressionException("Too many Signs in expression");
                }
                
            }
            var expr = " ";
            if (braceMatches.Count > 0)
            {
                for(var i = braceMatches.Count - 1; i >= 0; i--)
                {
                    expr = expr[0] != '-' && expr[0] != '+' ? $"{braceMatches[i].Value}+{expr}" : braceMatches[i].Value + expr;
                }
            }
            if (expression.Length > 0)
            {
                if (expression[expression.Length - 1] != '%' && expression[expression.Length - 1] != '/' && expression[expression.Length - 1] != '*')
                {
                    expression = expression[0] != '-' && expression[0] != '+' ? $"{expr}+{expression}" : expression + expr;
                }
                else expression += expr;
            }
        }

        private static string ManageNegative(string expression)
        {
            var regex = new Regex(@"(\d+([\.]\d+)?)(\-)?(\-)(\d+([\.]\d+)?)");
            while (regex.IsMatch(expression))
                expression = regex.Replace(expression, "$1+$4$5");
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
