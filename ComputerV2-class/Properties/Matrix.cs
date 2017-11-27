namespace ComputerV2_class.Properties
{
    public class Matrix
    {
        public string[] MyMatrix { get; }

        public Matrix(string expr)
        {
            MyMatrix = expr.Split('\n');
        }

        public static string ScalarMultiply(string scal, Matrix mtrx)
        {
            var mat = mtrx.MyMatrix;
            var ret = "";
            scal = MyMaths.Calc(scal);

            for (var i = 0; i < mat.Length; i++)
            {
                var m = mat[i].Split(',');
                for (var j = 0; j < m.Length; j++) m[j] = MyMaths.Calc($"{scal} * {m[j]}");
                for (var k = 0; k < m.Length; k++)
                    ret += k + 1 == m.Length ? m[k] : $"{m[k]},";
                ret = "";
            }
            foreach (var m in mat)
            {
                ret += $"[{m}]\n";
            }
            return ret;
        }
    }
}