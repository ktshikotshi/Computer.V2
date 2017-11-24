using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ComputerV2_class;
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
            string funcRegex = @"^([A-Za-z]+)(\((\d+|[a-zA-Z])\))$";
            var curr = "";
            
            bool operationSuccess = true;
            bool varType = true;
            while (curr.ToLower() != "quite")
            {
                curr = Console.ReadLine();
                if (curr == "--v")
                   foreach(var v in variables)
                        Console.WriteLine($"{v[0]} = {v[1]}");
                else if (curr == "--f")
                    foreach(var v in functions)
                        Console.WriteLine($"{v[0]} = {v[1]}");
                else if (!(curr.Contains("=")))
                    Console.WriteLine("missing assignment operator.");
                else
                {
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
                                    if (FindVar(variables, rhs[i]) == -1 && !(sTmp[0].Contains($"({rhs[i]})")))
                                    {
                                        Console.WriteLine($"variable {rhs[i]} is not defined.");
                                        operationSuccess = false;
                                        break;
                                    }
                                    else
                                        rhs[i] = variables[FindVar(variables, rhs[i])][1];
                                }
                                else if (Regex.IsMatch(rhs[i], funcRegex, RegexOptions.IgnoreCase))
                                {
                                    if (FindVar(functions, rhs[i]) == -1 && !(sTmp[0].Contains($"({rhs[i]})")))
                                    {
                                        Console.WriteLine($"function {rhs[i]} is not defined.");
                                        operationSuccess = false;
                                        varType = false;
                                        break;
                                    }
                                    else
                                        rhs[i] = functions[FindVar(functions, rhs[i])][1];
                                }
                                string str = "";
                                foreach (var s in rhs)
                                    str += s;
                                //replace variables with values
                                if (!(Regex.IsMatch(str, @"[a-zA-Z]+")))
                                    sTmp[1] = MyMaths.Calc(str);
                                else
                                    sTmp[1] = str;
                            }
                        }
                        else if (rhs.Length == 1)
                        {
                            if (Regex.IsMatch(sTmp[0], funcRegex, RegexOptions.IgnoreCase))
                                varType = false;
                            if (Regex.IsMatch(rhs[0], varRegex, RegexOptions.IgnoreCase))
                            {
                                if (FindVar(variables, rhs[0]) == -1 && !(sTmp[0].Contains($"({rhs[0]})")))
                                {
                                    Console.WriteLine($"variable {rhs[0]} is not defined.");
                                    operationSuccess = false;
                                }
                                else { sTmp[1] = variables[FindVar(variables, rhs[0])][1]; }
                            }
                        }
                        
                        if (!(Regex.IsMatch(tmp[0], varRegex, RegexOptions.IgnoreCase))) return;
                        List<string> cVar = new List<string>();
                        
                        if (operationSuccess)
                        {
                            if (FindVar(variables, sTmp[0]) == -1)
                            {
                                cVar.Add(sTmp[0]);
                                cVar.Add(sTmp[1]);
                                cVar[1] = sTmp[1];
                                cVar[1] = sTmp[1];
                                if (varType == true)
                                    variables.Add(cVar);
                                else
                                    functions.Add((cVar));
                            }
                            else
                            {
                                cVar = variables[FindVar(variables, sTmp[0])];
                                variables.Remove(cVar);
                                cVar[1] = sTmp[1];
                                if (varType == true)
                                    variables.Add(cVar);
                                else
                                    functions.Add(cVar);
                            }
                            Console.WriteLine(varType ? variables[variables.Count - 1][1] : functions[functions.Count - 1][1]);
                        }
                    }
                    //resolution
                    
                    else
                    {
                        string[] lhs = Regex.Split(sTmp[0], @"(\-)|(\+)|(\/)|(\*)|(\%)|(\^)|(\()|(\))");
                        if (lhs.Length < 2)
                        {
                            if (Regex.IsMatch(sTmp[0], varRegex, RegexOptions.IgnoreCase))
                            {
                                if (FindVar(variables, sTmp[0]) == -1)
                                    Console.WriteLine($"variable {sTmp[0]} is not defined.");
                                else
                                    Console.WriteLine(variables[FindVar(variables, sTmp[0])][1]);
                            }
                            else
                            {
                                Console.WriteLine(sTmp[0]);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < lhs.Length; i++) {
                                if (Regex.IsMatch(lhs[i], varRegex, RegexOptions.IgnoreCase))
                                {
                                    if (FindVar(variables, lhs[i]) == -1)
                                    {
                                        Console.WriteLine($"variable {lhs[i]} is not defined.");
                                        break;
                                    }
                                    else
                                    {
                                        lhs[i] = variables[FindVar(variables, lhs[i])][1]; 
                                    }
                                }
                            }
                            string str = "";
                            foreach (var s in lhs)
                            {
                                str += s;
                            }
                            //replace variables with values
                            Console.WriteLine(MyMaths.Calc(str));
                        }
                    }
                }
            }
        }
        public static int FindVar(List<List<string>> variables, string str)
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