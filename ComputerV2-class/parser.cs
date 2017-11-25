using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ComputerV2_class
{
    public static class Parser
    {
        public static bool Assign(string expr, string val, ref List<List<string>> vars, ref List<List<string>> funcs)
        {
            var rgx_str = @"([a-zA-Z]+\(([a-zA-Z]+)|(\d+)\))";
            Regex rgx = new Regex(rgx_str);
            if (rgx.IsMatch(expr))
            {
                val = Substitute(val, funcs, vars);
                val = ManageNaturalForm(val, expr);
                val = reduce(val, expr);
                if (AssignFunction(expr, val, ref funcs)) return true;
            }
            rgx_str = @"[a-zA-Z]+";
            rgx = new Regex(rgx_str);
            if (rgx.IsMatch(expr))
            {
                val = Substitute(val, funcs, vars);
                if (AssignVariable(expr, val, ref vars)) return true;
            }
            return false;
        }

        public static string Substitute(string expr, List<List<string>> funcs, List<List<string>> vars)
        {
            expr = MatchFunction(expr, funcs, vars);
            expr = MatchVariable(expr, vars);
            return expr;
        }

        private static string MatchFunction(string expr, List<List<string>> funcs, List<List<string>> vars)
        {
            Regex rgx = new Regex(@"(((\-|\+)(\s+)?)?[a-zA-Z]+\((([a-zA-Z]+)|(\d+))\))", RegexOptions.None);
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
                                    rplc = rplc.Replace(f[1], $"({v[1]})");
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
                            rplc = rplc.Replace(f[1], $"({func[1]})");
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
        private static string MatchVariable(string expr, List<List<string>> vars)
        {
            Regex rgx = new Regex(@"[A-Za-z]+");
            var match = rgx.Matches(expr);
            for (var i = 0; i < match.Count; i++)
            {
                foreach (var v in vars)
                {
                    if (v[0] == match[i].Value)
                    {
                        expr = expr.Replace(match[i].Value, v[1]);
                        break;   
                    } 
                }
            }
            if (!Regex.IsMatch(expr, @"[A-zA-Z]+"))
                expr = MyMaths.Calc(expr);
            return expr;
        }
        
        private static bool AssignFunction(string expr, string value, ref List<List<string>> funcs)
        {
            string[] func = Regex.Split(expr, @"\(|\)");
            var matches = Regex.Matches(value, @"[a-zA-Z]+");
            string[] var = new string[matches.Count];
            for (var i = 0; i < matches.Count; i++)
                var[i] = matches[i].Value;
            foreach (var v in var)
                if (v != func[1]) return false;
            for (var i = 0; i < funcs.Count; i++)
            {
                if (funcs[i][0] == func[0])
                {
                    if (funcs[i][1] != func[1])
                        funcs[i][1] = func[1];
                    funcs[i][2] = value;
                    return true;
                }
            }
            var newFunc = new List<string>();
            newFunc.Add(func[0]);
            newFunc.Add(func[1]);
            newFunc.Add(value);
            funcs.Add(newFunc);
            return true;
        }

        private static bool AssignVariable(string expr, string value, ref List<List<string>> vars)
        {
            for (var i = 0; i < vars.Count; i++)
            {
                if (vars[i][0] == expr)
                {
                    vars[i][1] = MyMaths.Calc(value);
                    return true;
                }
            }
            var newVar = new List<string>();
            newVar.Add(expr);
            newVar.Add(value);
            vars.Add(newVar);
            return true;
        }
        private static string ManageNaturalForm(string f, string var)
        {
            string[] expr = Helper.Split(f);
            var = Regex.Split(var, @"\(|\)")[1];
            const string pow1 = @"^(\d+)?(\*)?[A-Za-z](\^[1])?$";
            const string pow2 = @"^(\d+)?(\*)?[A-Za-z]\^[2]$";
            const string pow0 = @"^(\d+)((\*)?[A-Za-z]\^[0])?$";

            for (var i = 0; i < expr.Length; i++)
            {
                if (Regex.IsMatch(expr[i], pow2, RegexOptions.IgnoreCase))
                    expr[i] = (Regex.IsMatch(expr[i], @"^(\d+\,)?\d+", RegexOptions.IgnoreCase) ?
                        Regex.Match(expr[i], @"^(\d+\,)?\d+", RegexOptions.IgnoreCase).ToString() : "1") + "*" + var + "^2";
                else if (Regex.IsMatch(expr[i], pow1, RegexOptions.IgnoreCase))
                    expr[i] = (Regex.IsMatch(expr[i], @"^(\d+\,)?\d+", RegexOptions.IgnoreCase) ?
                        Regex.Match(expr[i], @"^(\d+\,)?\d+", RegexOptions.IgnoreCase).ToString() : "1") + "*" + var + "^1";
                else if (Regex.IsMatch(expr[i], pow0, RegexOptions.IgnoreCase))
                    expr[i] = Regex.Match(expr[i], @"^(\d+\,)?\d+", RegexOptions.IgnoreCase).Value;
            }
            string tmp = "";
            foreach (var s in expr)
                tmp += s;
            return tmp;
        }
        private static string reduce(string f, string var)
        {
            string[] expr = Helper.Split(f);
            var = Regex.Split(var, @"\(|\)")[1];
            const string pow1 = @"^(\d+)?(\*)?[A-Za-z](\^[1])?$";
            const string pow2 = @"^(\d+)?(\*)?[A-Za-z]\^[2]$";
            const string pow0 = @"^\d+$";
            double a = 0, b = 0, c = 0;
            a = reduce_helper(expr, pow2);
            b = reduce_helper(expr, pow1);
            c = reduce_helper(expr, pow0);
            string tmp = "";
            tmp += a != 0 ? (a > 0 ? $"+{a}": a.ToString())  + $"*{var}^2" : "";
            tmp += b != 0 ? (b > 0 ? $"+{b}" : b.ToString()) + $"*{var}" : "";
            tmp += c != 0 ? (c > 0 ? $"+{c}" : c.ToString()) : "";
            return tmp;
        }
        private static double reduce_helper(string[] value, string rgx)
        {
            double ret =0d;
            var prev = ""; 
            foreach (var v in value)
            {
                if (Regex.IsMatch(v, rgx))
                {
                    double tmp = 0d;
                    double.TryParse(Regex.Match(prev + v, @"((\+)|(\-))?\d+").Value, out tmp);
                    ret += tmp;
                }
                prev = v;
            }
            return ret;
        }
    }
}
