using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ComputerV2_class
{
    public static class Parser
    {
        public static string MatchFunction(string expr, List<List<string>> funcs, List<List<string>> vars)
        {
            Regex rgx = new Regex(@"(((\-|\+)(\s+)?)?[a-zA-Z]+\(([a-zA-Z])|(\d+)\))");
            var match = rgx.Match(expr);
            while (match.Success)
            {
                string tmp = match.Value;
                string[] func = Regex.Split(tmp, @"\(|\)");
                string rplc = "";
                foreach (var f in funcs)
                {
                    if (f[0] == func[0])
                    {
                        rplc = f[2];
                        if (Regex.IsMatch(func[1], @"^[a-zA-Z]+$"))
                        {
                            foreach (var v in vars)
                            {
                                if (v[0] == func[1])
                                {
                                    rplc = rplc.Replace(func[1], $"*({v[1]})");
                                    rplc = MyMaths.Calc(rplc);
                                    expr = expr.Replace(tmp, rplc);
                                    break;
                                }
                            }
                            if (rplc == "")
                            {
                                expr = expr.Replace(tmp, f[2]);
                            }
                        }
                        else
                        {
                            rplc = rplc.Replace(func[1], $"*({func[1]})");
                            rplc = MyMaths.Calc(rplc);
                            expr = expr.Replace(tmp, rplc);   
                        }
                    }
                }
                match = rgx.Match(expr);
            }
            return expr; 
        }

        //find the value of the variable
        public static string MatchVariable(string expr, List<List<string>> vars)
        {
            Regex rgx = new Regex(@"^([A-Za-z])+$");
            var match = rgx.Match(expr);
            while (match.Success)
            {
                foreach (var v in vars)
                {
                    if (v[0] == match.Value)
                    {
                        expr = expr.Replace(match.Value, v[1]);
                        break;   
                    } 
                }
                match = rgx.Match(expr);
            }
            return expr;
        }

        public static bool AssignFunction(string expr, List<List<string>> funcs)
        {
            Regex rgx = new Regex(@"(((\-|\+)(\s+)?)?[a-zA-Z]+\(([a-zA-Z])|(\d+)\))");
            if (rgx.IsMatch(expr))
            {
                string[] func = Regex.Split(expr, @"\(|\)");
                for (var i = 0; i < funcs.Count; i++)
                {
                    if (funcs[i][1] == func[0])
                    {
                        //funcs
                    }
                }
  
            }
            return false;
        }
    }
}
