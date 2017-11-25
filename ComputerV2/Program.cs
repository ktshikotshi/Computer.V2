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
      
            var curr = "";
            while (curr.ToLower() != "quite")
            {
                curr = Console.ReadLine().ToLower();
                if (curr == "--v")
                   foreach(var v in variables)
                        Console.WriteLine($"{v[0]} = {v[1]}");
                else if (curr == "--f")
                    foreach(var f in functions)
                        Console.WriteLine($"{f[0]}({f[1]}) = {f[2]}");
                else if (!(curr.Contains("=")))
                    Console.WriteLine("missing assignment operator.");
                else
                {
                    string[] sTmp = Regex.Split(curr.Replace(" ", ""), @"\=");
                    //assignment
                    if (sTmp[1] != "?")
                    {
                        if (!(Parser.Assign(sTmp[0], sTmp[1], ref variables, ref functions))) Console.WriteLine("Error in input.");
                    }
                    //resolution
                    
                    else
                    {
                        var sub = Parser.Substitute(sTmp[0], functions, variables);
                        Console.WriteLine($"{MyMaths.Calc(sub)}");
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