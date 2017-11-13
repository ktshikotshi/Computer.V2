using System;
using System.Text.RegularExpressions;

namespace ComputerV2_class
{
    public static class Helper
    {
        //accurate to 4 decimal points....really slow with big numbers
        public static double Sqrt(double x)
        {
            if (x <= 0)
                return (x);
            var t = 0.000001;
            while (t * t < x)
                t += 0.000001;
            return Convert.ToDouble(t.ToString("0.####"));
        }
        
        public static string[] Split(string str)
        {
            var s = str.Replace(".", ",");
            return (Regex.Split(s.Replace(" ", ""), @"(\-)|(\+)|(\=)"));
        }
    }
}