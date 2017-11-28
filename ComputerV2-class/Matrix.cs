using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace ComputerV2_class.Properties
{
    public class Matrix
    {
        private readonly (int Columns, int Rows) _dimentions;
        public string[] MyMatrix { get; }
        public double[,] IntMatrix { get; }

        public (int Columns, int Rows) Dimentions
        {
            get { return _dimentions; }
        }

        public Matrix(string expr)
        {
            expr = Regex.Replace(expr, @"\[|\]", "");
            MyMatrix = expr.Split('\n');
            if (MyMatrix.Length > 1)
            {
                _dimentions.Rows = MyMatrix.Length - 1;
                var m = MyMatrix[0].Split(',');
                _dimentions.Columns = m.Length;
            }
            IntMatrix = new Double[_dimentions.Rows, _dimentions.Columns];
            for (var j = 0; j < _dimentions.Columns; j++) {
                var rw = MyMatrix[j].Split(',');
                for (var i = 0; i < _dimentions.Rows; i++)
                    IntMatrix[j, i] = Convert.ToDouble(rw[i]);
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
            var count = 0;
            var count1 = 0;
            var str = "";
            for (var j = 0; j < m1.Dimentions.Rows; j++) {
                for (var i = 0; i < m1.Dimentions.Columns; i++)
                {
                    if (count < m2.Dimentions.Rows)
                    {
                        str += 
                        count++;
                    }
                }
            }
            return (true, null, ret);
        }
    }
}