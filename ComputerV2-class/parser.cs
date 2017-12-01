using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ComputerV2_class.Exceptions;
namespace ComputerV2_class
{
    public static class Parser
    {
        public static (bool Success, string Message, string Value) Assign(string expr, string val, ref List<List<string>> vars, ref List<List<string>> funcs)
        {
            var rgxStr = @"([a-zA-Z]+\(([a-zA-Z]+)|(\d+([\.,]\d+)?)\))";
            Regex rgx = new Regex(rgxStr);
            string fVar = "";
            if (rgx.IsMatch(expr))
            {
                fVar = Regex.Split(expr, @"\(|\)")[1];
                if (Substitute(val, funcs, vars, fVar).Success) val = Substitute(val, funcs, vars, fVar).Value;
                else return (false, Substitute(val, funcs, vars, fVar).Message, null);
                var mx = MatrixManipulation(val);
                if (mx.Found || (!mx.Found && mx.Message == null)) val = mx.Value;
                else return (false, mx.Message, null);
                val = ManageNaturalForm(val, expr);
                val = Reduce(val, expr);
                var Ass = AssignFunction(expr, val, ref funcs);
                if (Ass.Success) return (true, null, Ass.Value);
                else
                    return (false, Ass.Message, null);
            }
            rgxStr = @"[a-zA-Z]+";
            rgx = new Regex(rgxStr);
            if (rgx.IsMatch(expr))
            {
                if (expr != "i")
                {
                    if (Substitute(val, funcs, vars, fVar).Success) val = Substitute(val, funcs, vars, fVar).Value;
                    else return (false, Substitute(val, funcs, vars, fVar).Message, null);
                    var mx = MatrixManipulation(val);
                    if (mx.Found || (!mx.Found && mx.Message == null)) val = mx.Value;
                    else return (false, mx.Message, null);
                    var Ass = AssignVariable(expr, val, ref vars);
                    if (Ass.Success) return (true, null, Ass.Value);
                    else
                        return (false, Ass.Message, null);
                }
                return (false, "Variable: i is reserved.", null);
            }
            return (false, "No variable or Function found", null);
        }

        public static (bool Success, string Message, string Value) Substitute(string expr, List<List<string>> funcs, List<List<string>> vars, string fVar)
        {
            var func = MatchFunction(expr, funcs, vars);
            expr = func.Value;
            fVar = func.Variable == "" ? fVar : func.Variable;
            expr = MatchVariable(expr, vars);
            if (!(Undefined(expr, fVar).Success)) return (false, Undefined(expr, fVar).Message, null);
            return (true, null, expr);
        }

        private static (string Variable, string Value) MatchFunction(string expr, List<List<string>> funcs, List<List<string>> vars)
        {
            Regex rgx = new Regex(@"(((\-|\+)(\s+)?)?[a-zA-Z]+\((([a-zA-Z]+)|(\d+([\.,]\d+)?))\))");
            var match = rgx.Matches(expr);
            string fVar = "";
            for (var i = 0; i < match.Count; i++)
            {
                if (match[i].Success)
                {
                    string tmp = match[i].Value;
                    string[] func = Regex.Split(tmp, @"\(|\)");
                    string rplc = "";
                    foreach (var f in funcs)
                    {
                        if (f[0] == func[0])
                        {
                            fVar = func[1];
                            rplc = f[2];
                            if (Regex.IsMatch(func[1], @"^[a-zA-Z]+$"))
                            {
                                foreach (var v in vars)
                                {
                                    if (v[0] == func[1])
                                    {
                                        rplc = rplc.Replace(f[1], $"({v[1]})");
                                        rplc = Maths.Calculate(rplc);
                                        expr = expr.Replace(tmp, rplc);
                                        break;
                                    }
                                }
                                expr = expr.Replace(tmp, f[2]);
                            }
                            else
                            {
                                rplc = rplc.Replace(f[1], $"({func[1]})");
                                rplc = Maths.Calculate(rplc);
                                expr = expr.Replace(tmp, rplc);
                            }
                        }
                    }
                }
            }
            return (fVar, expr); 
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
                expr = Maths.Calculate(expr);
            return expr;
        }
        
        private static (bool Success, string Message, string Value) AssignFunction(string expr, string value, ref List<List<string>> funcs)
        {
            string[] func = Regex.Split(expr, @"\(|\)");
            var matches = Regex.Matches(value, @"[a-zA-Z]+");
            string[] var = new string[matches.Count];
            for (var i = 0; i < matches.Count; i++)
                var[i] = matches[i].Value;
            foreach (var v in var)
                if (v != func[1]) return (false, "Function should only contain one variable", null);
            for (var i = 0; i < funcs.Count; i++)
            {
                if (funcs[i][0] == func[0])
                {
                    funcs[i][1] = func[1];
                    funcs[i][2] = value;
                    return (true, null, funcs[i][2]);
                }
            }
            var newFunc = new List<string>
            {
                func[0],
                func[1],
                value
            };
            funcs.Add(newFunc);
            return (true, null, funcs[funcs.Count - 1][2]);
        }

        private static (bool Success, string Message, string Value) AssignVariable(string expr, string value, ref List<List<string>> vars)
        {
            for (var i = 0; i < vars.Count; i++)
            {
                if (vars[i][0] == expr)
                {
                    vars[i][1] = Maths.Calculate(value);
                    return (true, null, vars[i][1]);
                }
            }
            var newVar = new List<string>
            {
                expr,
                value
            };
            vars.Add(newVar);
            return (true, null, vars[vars.Count - 1][1]);
        }

        //needs to function with higher degrees
        private static string ManageNaturalForm(string f, string var)
        {
            string[] expr = Helper.Split(f);
            var = Regex.Split(var, @"\(|\)")[1];
            const string pow1 = @"^(\d+([\.,]\d+)?)?(\*)?[A-Za-z](\^[1])?$";
            const string pow2 = @"^(\d+([\.,]\d+)?)?(\*)?[A-Za-z]\^[2]$";
            const string pow0 = @"^(\d+([\.,]\d+)?)((\*)?[A-Za-z]\^[0])?$";

            for (var i = 0; i < expr.Length; i++)
            {
                if (Regex.IsMatch(expr[i], pow2, RegexOptions.IgnoreCase))
                    expr[i] = (Regex.IsMatch(expr[i], @"^\d+([\.,]\d+)?", RegexOptions.IgnoreCase) ?
                        Regex.Match(expr[i], @"^\d+([\.,]\d+)?", RegexOptions.IgnoreCase).ToString() : "1") + var + "^2";
                else if (Regex.IsMatch(expr[i], pow1, RegexOptions.IgnoreCase))
                    expr[i] = (Regex.IsMatch(expr[i], @"^\d+([\.,]\d+)?", RegexOptions.IgnoreCase) ?
                        Regex.Match(expr[i], @"^\d+([\.,]\d+)?", RegexOptions.IgnoreCase).ToString() : "1") + var + "^1";
                else if (Regex.IsMatch(expr[i], pow0, RegexOptions.IgnoreCase))
                    expr[i] = Regex.Match(expr[i], @"^\d+([\.,]\d+)?", RegexOptions.IgnoreCase).Value;
            }
            string tmp = "";
            foreach (var s in expr)
                tmp += s;
            return tmp;
        }

        //needs work for higher degrees
        private static string Reduce(string f, string var)
        {
            f = Helper.Reduce_helper1(f);
            string[] expr = Helper.Split(f);
            var = Regex.Split(var, @"\(|\)")[1];
            const string pow1 = @"^((\d+)([\.,]\d+)?)?(\*)?[A-Za-z](\^[1])?$";
            const string pow2 = @"^((\d+)([\.,]\d+)?)?(\*)?[A-Za-z]\^[2]$";
            const string pow0 = @"^\d+([\.,]\d+)?$";
            double a = 0, b = 0, c = 0;
            a = Helper.Reduce_helper(expr, pow2);
            b = Helper.Reduce_helper(expr, pow1);
            c = Helper.Reduce_helper(expr, pow0);
            string tmp = "";
            tmp += a != 0 ? (a > 0 ? a.ToString(): " " + a.ToString())  + $"*{var}^2" : "";
            tmp += b != 0 ? (b > 0 ? (tmp == ""? b.ToString() : $" + {b}") : " " + b.ToString()) + $"*{var}" : "";
            tmp += c != 0 ? (c > 0 ? (tmp == "" ? c.ToString() : $" + {c}") : " " + c.ToString()) : "";
            return tmp;
        }
        
        private static (bool Success, string Message) Undefined(string val, string fVar)
        {
            Regex rgx = new Regex(@"(((\-|\+)(\s+)?)?[a-zA-Z]+\((([a-zA-Z]+)|(\d+([\.,]\d+)?))\))", RegexOptions.None);
            var match = rgx.Match(val);
            if (match.Success) return (false, $"  Function : {match.Value} is not defined.");
            rgx = new Regex(@"[A-Za-z]+", RegexOptions.None);
            match = rgx.Match(val);
            if (match.Success && match.Value != fVar && match.Value != "i") return (false, $"  Variable : {match.Value} is not defined.");
            return (true, null);
        }

        public static (bool Found, string Message, string Value) MatrixManipulation(string expr)
        {
            if (!expr.Contains("[")) return (false, null, expr);
            if (expr.Contains("*"))
            {
                var tmp = expr.Split('*');
                if (expr.Contains("**"))
                {
                    if (SplitMatrix(tmp[0]).Valid && SplitMatrix(tmp[tmp.Length - 1]).Valid)
                    {
                        Matrix thisM = new Matrix(SplitMatrix(tmp[0]).Value);
                        Matrix this2 = new Matrix(SplitMatrix(tmp[tmp.Length - 1]).Value);
                        var mlty = Matrix.Multiply(thisM, this2);
                        if (mlty.Success)
                            expr = mlty.Value;
                        else
                        {
                            return (false, mlty.Message, null);
                        }
                        return (true, null, expr);
                    }
                    else
                        return (false, (SplitMatrix(tmp[0]).Valid
                            ? SplitMatrix(tmp[1]).Message
                            : SplitMatrix(tmp[0]).Message), null);
                }
                try
                {
                    var scalar = tmp[0].Contains(",") ? Decimal.Parse(tmp[tmp.Length - 1]) : Decimal.Parse(tmp[0]);
                    var mtrxVal = tmp[0].Contains(",") ? SplitMatrix(tmp[0]).Value : SplitMatrix(tmp[tmp.Length - 1]).Value;
                    var mtrx = new Matrix(mtrxVal);
                    expr = Matrix.ScalarMultiply(scalar.ToString(), mtrx);
                }
                catch (FormatException) { throw new InvalidMatrixException("Format of Matrix is Bad"); }

                return (true, null, expr);
                }
            var split = SplitMatrix(expr);
            //will throw an exceptoion if the numbers are not correctly formatted.
            Matrix nMatrix = new Matrix(split.Value);
            return (split.Valid, split.Message, split.Value);

        }

        private static (bool Valid, string Message, string Value) SplitMatrix(string expr)
        {
            int braces = 0;
            
            for (var i = 0; i < expr.Length; i++)
            {
                if (expr[i] == '[') braces += 1;
                if (expr[i] == ']') braces -= 1;
            }
            if (braces != 0) throw new InvalidMatrixException($"Braces are incomplete :  {expr}");
            if (Regex.IsMatch(expr, @"\]\[")) throw new InvalidMatrixException($"Missing ; :  {expr}");
            expr = Regex.Replace(expr, @"\[|\]", "");
            var nExpr = Regex.Split(expr, @"[\n]|[;]");
            expr = "";
            for (var i = 0; i < nExpr.Length; i++)
            {
                
                nExpr[i] = nExpr[i] == "" ? "" : $"[{nExpr[i]}]";
                if (Regex.IsMatch(nExpr[i], @"^\[(((\-)?\d+([\.,]\d+)?)?([,])?)+\]$") || nExpr[i] == "")
                    expr += nExpr[i] == ""? "" : $"{nExpr[i]}\n";
                else
                    return (false, $"format of matric is not correct:  {nExpr[i]}", null);
            }
            return (true, null, expr);
        }
    }
}
