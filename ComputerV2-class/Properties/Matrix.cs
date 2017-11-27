using System;
using System.Text.RegularExpressions;

namespace ComputerV2_class.Properties
{
    public class Matrix
    {
        private readonly (int Columns, int Rows) _dimentions;
        public string[] MyMatrix { get; }

        public (int Columns, int Rows) Dimentions
        {
            get { return _dimentions; }
        }

        public Matrix(string expr)
        {
            MyMatrix = expr.Split('\n');
            if (MyMatrix.Length > 1)
            {
                _dimentions.Rows = MyMatrix.Length;
                var m = MyMatrix[0].Split(',');
                _dimentions.Columns = m.Length;
            }
        }

        public static string ScalarMultiply(string scal, Matrix mtrx)
        {
            var mat = mtrx.MyMatrix;
            var ret = "";
            scal = MyMaths.Calc(scal);

            for (var i = 0; i < mat.Length; i++)
            {
                mat[i] = Regex.Replace(mat[i], @"\[|\]", "");
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

        public static (bool Success, string Message, string Value) Multiply(Matrix m1, Matrix m2)
        {

            var ret = "";
            
            if (m1.Dimentions.Columns != m2.Dimentions.Rows) return (true, "Matrics cannot be multiplied", ret);
            
            var newMatrix = new string[m1.Dimentions.Columns, m2.Dimentions.Rows];
            
            return (true, null, ret);
        }
    }
}