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
            var tmp = expr.Split('\n');
            if (tmp.Length > 1)
            {
                _dimentions.Rows = tmp.Length - 1;
                var m = tmp[0].Split(',');
                _dimentions.Columns = m.Length;
            }
            IntMatrix = new Double[_dimentions.Rows, _dimentions.Columns];
            for (var j = 0; j < _dimentions.Columns; j++) {
                var rw = tmp[j].Split(',');
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
            int rowCounter = 0, columnCounter = 0;
            if (m1.Dimentions.Columns != m2.Dimentions.Rows) return (false, "Matrics cannot be multiplied", ret);
            for (var i = 0; i < m1.Dimentions.Columns; i++)
            {

                    while (rowCounter < m1.Dimentions.Rows)
                    {
                        ret += $" {(rowCounter > 0 ? "+" : "")} {m1.IntMatrix[i, rowCounter]} * {m2.IntMatrix[rowCounter, columnCounter]}";
                        rowCounter++;
                        if (rowCounter == m1.Dimentions.Rows && columnCounter < m2.Dimentions.Columns - 1)
                        {
                            ret += (",");
                            rowCounter = 0;
                            columnCounter++;
                        }
                    }
                rowCounter = 0;
                columnCounter = 0;
                ret += "\n";
            }
            ret = CalcMatrix(ret, m1.Dimentions.Rows, m2.Dimentions.Columns);
            return (true, null, ret);
        }

        private static string CalcMatrix(string met ,int rows, int columns)
        {
            var ret = "";
            var tmp = met.Split('\n');
            
            var tmp2 = new string[rows, columns];
            for (var j = 0; j < columns; j++) {
                var rw = tmp[j].Split(',');
                for (var i = 0; i < rows; i++)
                    tmp2[j, i] = MyMaths.Calc(rw[i]);
            }
            for(var i = 0; i < columns; i++)
            {
                ret += "[";
                for (var j = 0; j < rows; j++)
                {
                    ret += $"{(j > 0? ",": "")}{tmp2[i,j]}";
                }
                ret += "]\n";
            }
            return ret;
        }
    }
}