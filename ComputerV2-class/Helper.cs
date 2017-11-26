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

        public static string Reduce_helper1(string f)
        {
            var rgx = new Regex(@"(((\-|\+)?\d+([\.,]\d+)?)(\/|\%|\*)((\-|\+)?\d+([\.,]\d+)?))");
            var match = rgx.Match(f);
            while (match.Success)
            {
                var str = Regex.Replace(match.Value, @"(\-|\+)?\d+([\.,]\d+)?", m => string.Format(@"0{0}", m.Value));
                var tmp = MyMaths.Calc(str);
                f = f.Replace(match.Value, tmp.Contains("-") ? tmp : "+" + tmp);
                match = rgx.Match(f);
            }
            return f;
        }

        public static double Reduce_helper(string[] value, string rgx)
        {
            double ret = 0d;
            var prev = "";
            foreach (var v in value)
            {
                if (Regex.IsMatch(v, rgx))
                {
                    double tmp = 0d;
                    double.TryParse(Regex.Match(prev + v, @"((\+)|(\-))?\d+([\.,]\d+)?").Value, out tmp);
                    ret += tmp;
                }
                prev = v;
            }
            return ret;
        }
    }
}