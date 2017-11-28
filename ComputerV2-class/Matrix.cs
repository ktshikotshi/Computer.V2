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
           
        /*
         * 1 2 3  | 1 2 3  |  1 * 1 + 2 * 4 + 3 * 7 , 1 * 2 + 2 * 5 + 3 * 8 , 1 * 3 + 2 * 6 + 3 * 9  
         * 4 5 6  | 4 5 6  |  4 * 1 + 5 * 4 + 6 * 7 , 4 * 2 + 5 * 5 + 6 * 8 , 4 * 3 + 5 * 6 + 6 * 9
         * 7 8 9  | 7 8 9  |  7 * 1 + 8 * 4 + 9 * 7 , 7 * 2 + 8 * 5 + 9 * 8 , 7 * 3 + 8 * 6 + 9 * 9
         */
        
        public static (bool Success, string Message, string Value) Multiply(Matrix m1, Matrix m2)
        {

            var ret = "";
            int rowCounter = 0, columnCounter = 0;
            if (m1.Dimentions.Columns != m2.Dimentions.Rows) return (true, "Matrics cannot be multiplied", ret);
            for (var i = 0; i < m1.Dimentions.Columns; i++)
            {

                    while (rowCounter < m1.Dimentions.Rows)
                    {
                        //to be continued
                        ret += (
                            $" {(rowCounter > 0 ? "+" : "")} {m1.IntMatrix[i, rowCounter]} * {m2.IntMatrix[rowCounter, columnCounter]}");
                        rowCounter++;
                        if (rowCounter == m1.Dimentions.Rows && columnCounter < m2.Dimentions.Columns - 1)
                        {
                            ret += (";");
                            rowCounter = 0;
                            columnCounter++;
                        }
                    }
                rowCounter = 0;
                columnCounter = 0;
                ret += "\n";
                //a = [[1,2,3];[4,5,6];[7,8,9]]
            }
            
            return (true, null, ret);
        }
    }
}