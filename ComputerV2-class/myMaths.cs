using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ComputerV2_class
{
    public static  class myMaths
    {
        public static string calc(string expr)
        {
            string[] arr = Regex.Split(expr, @"(\-)|(\+)|(\/)|(\*)|(\%)|(\^)|(\()|(\))");
            List<string> numbs = new List<string>();
            foreach (var n in arr)
            {
                if (n != "")
                    numbs.Add(n);
            }
            brackets(ref numbs);
            power(ref numbs);
            multDiv(ref numbs);
            for (var i = 0; i < numbs.Count; i++)
            {
                if (numbs[i] == "+" )
                {
                    var n = Convert.ToDouble(numbs[i - 1]) + Convert.ToDouble(numbs[i + 1]);
                    numbs[i - 1] = n.ToString();
                    numbs.RemoveRange(i, 2);
                    i = 0;
                }
                else if (numbs[i] == "-")
                {
                    var n = Convert.ToDouble(numbs[i - 1]) - Convert.ToDouble(numbs[i + 1]);
                    numbs[i - 1] = n.ToString();
                    numbs.RemoveRange(i, 2);
                    i = 0;
                }
            }
            return numbs[0];
        }
        public static void brackets(ref List<string> expr)
        {
            int countBraces = 0;
            bool fBrace = false;
            int[] braceLoc = { 0, 0 };
            for(var i = 0; i < expr.Count; i++)
            {
                if (expr[i] == "(")
                {
                    if (countBraces == 0)
                        braceLoc[0] = i + 1;
                    countBraces += 1;
                    fBrace = true;
                }
                else if (expr[i] == ")")
                {
                    countBraces -= 1;
                    if (fBrace == true && countBraces == 0)
                    {
                        braceLoc[1] = i;
                    }
                }
                if (fBrace == true && countBraces == 0)
                {
                    string tmp = "";
                    for(var j = braceLoc[0]; j < braceLoc[1]; j++)
                    {
                        tmp += expr[j];
                    }
                    tmp = calc(tmp);
                    expr[braceLoc[0] - 1] = tmp;
                    expr.RemoveRange(braceLoc[0], (braceLoc[1] - braceLoc[0]) + 1);
                    i = 0;
                    fBrace = false;
                }
            }
        }
        public  static void power(ref List<string> expr)
        {
            double pow = 0, val = 0;
            for (var j = 0; j < expr.Count; j++)
            {
                if (expr[j] == "^")
                {
                    pow = Convert.ToDouble(expr[j + 1]);
                    val = Convert.ToDouble(expr[j - 1]);
                    double output = pow > 0? val : 0;
                    for (var i = 1; i < pow; i++)
                        output *= (val*i);
                    expr[j - 1] = output.ToString();
                    expr.RemoveRange(j, 2);
                    j = 0;
                }
            }
        }
        public static void multDiv(ref List<string> numbs)
        {
            for (var i = 0; i < numbs.Count; i++)
            {
                if (numbs[i] == "/")
                {
                    var n = Convert.ToDouble(numbs[i - 1]) / Convert.ToDouble(numbs[i + 1]);
                    numbs[i - 1] = n.ToString();
                    numbs.RemoveRange(i, 2);
                    i = 0;
                }
                else if (numbs[i] == "*")
                {
                    var n = Convert.ToDouble(numbs[i - 1]) * Convert.ToDouble(numbs[i + 1]);
                    numbs[i - 1] = n.ToString();
                    numbs.RemoveRange(i, 2);
                    i = 0;
                }
                else if (numbs[i] == "%")
                {
                    var n = Convert.ToDouble(numbs[i - 1]) % Convert.ToDouble(numbs[i + 1]);
                    numbs[i - 1] = n.ToString();
                    numbs.RemoveRange(i, 2);
                    i = 0;
                }
            }
        }
    }
}