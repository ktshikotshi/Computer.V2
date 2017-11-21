using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

namespace ComputerV2
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            //stores all the variables and their values.
            List<List<string>> variables = new List<List<string>>();
            
            //stores all the functions and their values.
            List<List<string>> functions = new List<List<string>>();
            
            //create the i variable.
            List<string> tmp = new List<string>();
            tmp.Add("i");
            tmp.Add("0");
            variables.Add(tmp);

            //variable/ function regex
            string varRegex = @"^([A-Za-z])+$";
            string funcRegex = @"^([A-Za-z])+(\(\d+\))$";
            var curr = Console.ReadLine();

            bool operationSuccess = true;
            while (curr.ToLower() != "quite")
            {
                if (curr == "--v")
                   foreach(var v in variables)
                        Console.WriteLine($"{v[0]} = {v[1]}");
                else if (!(curr.Contains("=")))
                    Console.WriteLine("missing assignment operator.");
                else              {
                    string[] sTmp = Regex.Split(curr.Replace(" ", ""), @"\=");
                    //assignment
                    if (sTmp[1] != "?")
                    {
                        string[] rhs = Regex.Split(sTmp[1], @"(\-)|(\+)|(\/)|(\*)|(\%)|(\^)|(\()|(\))");
                        
                        if (rhs.Length > 1)
                        {
                            for (var i = 0; i < rhs.Length; i++)
                            {
                                if (Regex.IsMatch(rhs[i], varRegex, RegexOptions.IgnoreCase))
                                {
                                    if (findVar(variables, rhs[i]) == -1 && !(sTmp[0].Contains($"({rhs[i]})")))
                                    {
                                        Console.WriteLine($"variable {rhs[i]} is not defined.");
                                        operationSuccess = false;
                                        break;
                                    }
                                    else
                                    {
                                        rhs[i] = variables[findVar(variables, rhs[i])][1];
                                    }
                                }
                                string str = "";
                                foreach (var s in rhs)
                                {
                                    str += s;
                                }
                                //replace variables with values
                                if (!(Regex.IsMatch(str, @"[a-zA-Z]+")))
                                    sTmp[1] = calc(str);
                                else
                                    sTmp[1] = str;
                            }
                        }
                        else if (rhs.Length == 1)
                        {
                            if (Regex.IsMatch(rhs[0], varRegex, RegexOptions.IgnoreCase))
                            {
                                if (findVar(variables, rhs[0]) == -1 && !(sTmp[0].Contains($"({rhs[0]})")))
                                {
                                    Console.WriteLine($"variable {rhs[0]} is not defined.");
                                    operationSuccess = false;
                                }
                                else { sTmp[1] = variables[findVar(variables, rhs[0])][1]; }
                            }
                        }
                        if (!(Regex.IsMatch(tmp[0], varRegex, RegexOptions.IgnoreCase))) return;
                        List<string> cVar = new List<string>();

                        if (operationSuccess)
                        {
                            if (findVar(variables, sTmp[0]) == -1)
                            {
                                cVar.Add(sTmp[0]);
                                cVar.Add(sTmp[1]);
                                cVar[1] = sTmp[1];
                                cVar[1] = sTmp[1];
                                variables.Add(cVar);
                            }
                            else
                            {
                                cVar = variables[findVar(variables, sTmp[0])];
                                variables.Remove(cVar);
                                cVar[1] = sTmp[1];
                                variables.Add(cVar);
                            }
                            Console.WriteLine(variables[variables.Count - 1][1]);
                        }
                    }
                    //resolution
                    else
                    {
                        string[] lhs = Regex.Split(sTmp[0], @"(\-)|(\+)|(\/)|(\*)|(\%)|(\^)|(\()|(\))");
                        if (lhs.Length < 2)
                        {
                            if (findVar(variables, sTmp[0]) == -1)
                                Console.WriteLine($"variable {sTmp[0]} is not defined.");
                            else
                                Console.WriteLine(variables[findVar(variables, sTmp[0])][1]);
                        }
                        else
                        {
                            for (var i = 0; i < lhs.Length; i++) {
                                if (Regex.IsMatch(lhs[i], varRegex, RegexOptions.IgnoreCase))
                                {
                                    if (findVar(variables, lhs[i]) == -1)
                                    {
                                        Console.WriteLine($"variable {lhs[i]} is not defined.");
                                        break;
                                    }
                                    else
                                    {
                                        lhs[i] = variables[findVar(variables, lhs[i])][1]; 
                                    }
                                }
                            }
                            string str = "";
                            foreach (var s in lhs)
                            {
                                str += s;
                            }
                            //replace variables with values
                            Console.WriteLine(calc(str));
                        }
                    }
                }
                curr = Console.ReadLine();
            }
        }
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
        public static int findVar(List<List<string>> variables, string str)
        {
            List<string> cVar = new List<string>();
            for (var i = 1; i < variables.Count; i++)
            {
                cVar = variables[i];
                if (string.Compare(str.ToLower(), cVar[0].ToLower()) == 0) return (i);
            }
            return -1;
        }
    }
}