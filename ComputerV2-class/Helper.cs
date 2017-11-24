using System.Text.RegularExpressions;

namespace ComputerV2_class
{
    public static class Helper
    {
        public static string[] Split(string str)
        {
            var s = str.Replace(".", ",");
            return (Regex.Split(s.Replace(" ", ""), @"(\-)|(\+)|(\=)"));
        }
    }
}