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
                        if (Regex.IsMatch(curr, @"([\=]{2,}|[\-]{2,}|[\%]{2,}|[\/]{2,}|[\+]{2,}|[\?]{2,})|[\*]{3,}"))
                            throw new InvalidExpressionException("Input Error");
                        string[] sTmp = Regex.Split(curr.Replace(" ", ""), @"\=");
                        //assignment
                        if (!sTmp[1].Contains("?"))
                        {
                            var parse = Parser.Assign(sTmp[0], sTmp[1], ref variables, ref functions);
                            if (!(parse.Success)) Console.WriteLine(parse.Message);
                            else Console.WriteLine($"{parse.Value}");
                        }
                        //resolution

                        else
                        {
                            if (sTmp[1] != "?") throw new InvalidExpressionException("Input Error");
                            var sub = Parser.Substitute(sTmp[0], functions, variables, "");
                            if (sub.Success) Console.WriteLine($"{Maths.Calculate(sub.Value)}");
                            else Console.WriteLine(Parser.Substitute(sTmp[0], functions, variables, "").Message);

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