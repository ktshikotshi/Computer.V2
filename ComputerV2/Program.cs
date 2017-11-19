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
            
            //create the i variable.
            List<string> tmp = new List<string>();
            tmp.Add("i");
            tmp.Add("0");
            variables.Add(tmp);

            //variable/ function regex
            string varRegex = @"^([A-Za-z])+(\(\d+\))?$";

            if (args.Length > 0)
            {
                var curr = args[0];
                if (!(curr.Contains("=")))
                {
                    Console.WriteLine("missing assignment operator.");
                    return;
                }
                else
                {
                    string[] sTmp = Regex.Split(curr, @"\=");
                    if (sTmp[1] != "?")
                    {
                        if (!(Regex.IsMatch(tmp[0], varRegex, RegexOptions.IgnoreCase))) return;
                        bool isMatch = false;
                        for (var i = 1; i < variables.Count; i++)
                        {
                            List<string> cVar = variables[i];
                            if (string.Compare(sTmp[0].ToLower(), cVar[0].ToLower()) == 0)
                            {

                                isMatch = true;
                            }
                        }
                    }
                }
            }
        }
    }
}