using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ComputerV2_class;
using ComputerV2_class.Exceptions;

namespace ComputerV2
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            //stores all the variables and their values.
            var variables = new List<List<string>>(); 
            //stores all the functions and their values.
            var functions = new List<List<string>>();
            Console.Write("> ");
            var curr = Console.ReadLine()?.ToLower();
            while (curr.ToLower() != "exit")
            {
                try
                {
                    switch (curr)
                    {
                        case "--v":
                            PrintVars(variables);
                            break;
                        case "--f":
                            PrintFuncs(functions);
                            break;
                        case "--a":
                            PrintVars(variables);
                            PrintFuncs(functions);
                            break;
                        default:
                            if (!(curr.Contains("=")))
                                Console.WriteLine("Missing assignment operator.");
                            else
                            {
                                Helper.ValidateInput(curr);
                                var sTmp = Regex.Split(curr.Replace(" ", ""), @"\=");
                                //assignment
                                if (!sTmp[1].Contains("?"))
                                {
                                    var parse = Parser.Assign(sTmp[0], sTmp[1], ref variables, ref functions);
                                    Console.WriteLine($"{parse}");
                                }
                                //resolution
                                else
                                {
                                    var sub = Parser.Substitute(sTmp[0], functions, variables, "");
                                    if (sTmp[1] != "?")
                                    {
                                        var rhs = sTmp[1].Split('?');
                                        if (rhs.Length != 2) throw new InvalidExpressionException("Error in Query : format should be x+...=y?");
                                        var rhsY = Parser.Substitute(rhs[0], functions, variables, "");
                                        var poly = new Poly($"{sub}={rhsY}");
                                        poly.Calculate();                                        
                                    }
                                    else Console.WriteLine($"{Maths.Calculate(sub)}");
                                }
                            }
                            break;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.Write("> ");
                curr = Console.ReadLine()?.ToLower();
            }
        }
        
        private static void PrintVars(List<List<string>> variables)
        {
            Console.WriteLine("Variables :");
            if (variables.Count > 0)
            {
                foreach (var v in variables)
                    Console.WriteLine($"{v[0]} = {v[1]}");
            }
            else Console.WriteLine("No Variables yet.");
        }
        
        private static void PrintFuncs(List<List<string>> functions)
        {
            Console.WriteLine("Functions :");
            if (functions.Count > 0)
            {
                foreach (var f in functions)
                    Console.WriteLine($"{f[0]}({f[1]}) = {f[2]}");
            }
            else Console.WriteLine("No Functions yet.");
        }
    }
}