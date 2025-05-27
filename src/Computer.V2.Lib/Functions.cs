using System;
using System.Text.RegularExpressions;
using Computer.V2.Lib.Exceptions;

namespace Computer.V2.Lib
{
    public static class Functions
    {
        public static string NormaliseFunc(string expression)
        {
            var rgx = new Regex(@"((\*)?(\[.*\])\n(\*)?)|(((\-)|(\+))?(d+([\.,]\d+)?)?\*[i])");
            var braceMatches = rgx.Matches(expression);
            if (braceMatches.Count > 0)
                expression = rgx.Replace(expression, "");
            var pow = HighestPow(ref expression);
            var newExpression = "";
            if (pow != -1)
            {
                for (var i = pow; i > 0; i--)
                {
                    var regex = new Regex(@"(?<!(\())((\-|\+)?(\d+([\.]\d+)?)?(\*)?[A-Za-z]+(\^" + i + @"))(?!\))");
                    var coeff = 0d;
                    var variable = "";
                    while (regex.IsMatch(expression))
                    {
                        var match = regex.Match(expression).Value;
                        var r2 = new Regex(@"((\-)?\d+([\.]\d+)?)(?=(\*)?[a-zA-Z]+)");
                        var tmp = r2.Match(match).Value != ""? r2.Match(match).Value : "1" ;
                        coeff += double.Parse(tmp);
                        variable = Regex.Match(match, @"[A-Za-z]+").Value;
                        expression = regex.Replace(expression, "", 1);
                    }
                    if (i != 1) newExpression += coeff != 0? $"{(coeff > 0 ? (newExpression != ""? $"+{coeff}": $"{coeff}") : $"{coeff}")}*{variable}^{i}" : "";
                    else newExpression += coeff != 0 ? $"{(coeff > 0 ? (newExpression != "" ? $"+{coeff}" : $"{coeff}") : $"{coeff}")}*{variable}" : "";
                }
            }
            expression = Maths.Calculate(expression);
            expression = newExpression != "" ? $"{newExpression}{(expression != ""? (expression[0] != '+' && expression[0] != '-'? "+": "") :"")}{expression}" : expression;
            if (braceMatches.Count <= 0) return (expression);
            {
                var expr = "";
                for (var i = braceMatches.Count - 1; i >= 0; i--)
                {
                    expr += braceMatches[i].Value;
                }
                if (expr.Length <= 0) return (expression);
                if (expr[0] == '*') { expression += expr; }
                else { expression = expr + expression; }
            }
            return (expression);
        }

        private static int HighestPow(ref string expression)
        {
            var regex = new Regex(@"((\-|\+)?\d+([\.]\d+)?)?(\*)?[A-Za-z]+(\^)?((\-)?\d+([\.]\d+)?)?");
            if (!regex.IsMatch(expression)) return (-1);
            var pow = 0;
            //manage powers of 0 and 1
            {
                var pow0Regex = new Regex(@"(\d+([\.]\d+)?)((\*)?[A-Za-z]\^[0])");
                //while (pow0Regex.IsMatch(expression))
                expression = pow0Regex.Replace(expression, "$1");
                var pow1Regex = new Regex(@"(((\-)?\d+([\.]\d+)?)?(\*)?[A-Za-z]+)(\-|\+|$)");
                //while (pow1Regex.IsMatch(expression))
                expression = pow1Regex.Replace(expression, "$1^1$6");
            }
            var matches = regex.Matches(expression);
            for(var i = 0; i < matches.Count; i++)
            {
                try
                {
                    var tmpStr = Regex.Match(matches[i].Value, @"((?<=\^)((\-)?\d+([\.]\d+)?))").Value;
                    //throws format error if the number is not whole and positive.
                    if (tmpStr == "") continue;
                    var tmp int.Parse(tmpStr);
                    if (tmp < 0)
                        throw new FormatException();
                    if (tmp > pow)
                        pow = tmp;
                }
                catch(FormatException)
                {
                    throw new InvalidExpressionException("Power of Polinomial is not of correct format");
                }
            }
            return (pow);
        }
    }
}