using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ComputerV2_class;
using ComputerV2_class.Exceptions;

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
      
            Console.Write("> ");
            var curr = Console.ReadLine().ToLower();
            while (curr.ToLower() != "exit")
            {
                try
                {
                    if (curr == "--v")
                        foreach (var v in variables)
                            Console.WriteLine($"{v[0]} = {v[1]}");
                    else if (curr == "--f")
                        foreach (var f in functions)
                            Console.WriteLine($"{f[0]}({f[1]}) = {f[2]}");
                    else if (!(curr.Contains("=")))
                        Console.WriteLine("Missing assignment operator.");
                    else
                    {
                        Parse.ValidateInput(curr);
                        string[] sTmp = Regex.Split(curr.Replace(" ", ""), @"\=");
                        //assignment
                        if (!sTmp[1].Contains("?"))
                        {
                            var parse = Parse.Assign(sTmp[0], sTmp[1], ref variables, ref functions);
                            if (!(parse.Success)) Console.WriteLine(parse.Message);
                            else Console.WriteLine($"{parse.Value}");
                        }
                        //resolution

                        else
                        {
                            var sub = Parse.Substitute(sTmp[0], functions, variables, "");
                            if (!sub.Success) throw new InvalidExpressionException(sub.Message);
                            var subVal = "";
                            if (sTmp[1] != "?")
                            {
                                if (!sub.Success) throw new InvalidExpressionException($"Error in query : {sTmp[0]}");
                                var rhs = sTmp[1].Split('?');
                                if (rhs.Length != 2) throw new InvalidExpressionException("Error in Query : format should be x+...=y?");
                                var rhsY = Parse.Substitute(rhs[0], functions, variables, "");
                                if (!rhsY.Success) throw new InvalidExpressionException($"Error in query : {sub.Value} = {sTmp[1]}");
                                var poly = new Polynomial($"{sub.Value}={rhsY.Value}");
                                poly.PolySolve();
                                subVal = poly.GetOut();
                                Console.WriteLine(subVal.TrimEnd('\n'));                              
                            }
                            else   
                                Console.WriteLine($"{Maths.Calculate(sub.Value)}");
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.Write("> ");
                curr = Console.ReadLine().ToLower();
            }
        }
    }
}