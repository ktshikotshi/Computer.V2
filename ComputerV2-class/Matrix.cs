using System;
using System.Text.RegularExpressions;
using ComputerV2_class.Exceptions;

namespace ComputerV2_class
{
    public class Matrix
    {
        private readonly (int Columns, int Rows) _dimentions;
        public string[] MyMatrix { get; }
        public double[,] IntMatrix { get; }

        public (int Columns, int Rows) Dimentions => _dimentions;

        public Matrix(string expr)
        {
            expr = Regex.Replace(expr, @"\[|\]", "");
            var tmp = expr.Split('\n');
            MyMatrix = tmp;
            if (tmp.Length > 1)
            {
                _dimentions.Rows = tmp.Length;
                var m = tmp[0].Split(',');
                _dimentions.Columns = m.Length;
            }
            IntMatrix = new Double[_dimentions.Rows, _dimentions.Columns];
            for (var j = 0; j < _dimentions.Rows; j++) {
                var rw = tmp[j].Split(',');
                for (var i = 0; i < _dimentions.Columns; i++)
                {
                    try
                    {
                        IntMatrix[j, i] = Convert.ToDouble(rw[i]);
                    }catch (FormatException)
                    {
                        throw new InvalidMatrixException($"Matric value is not valid : {rw[i]}");
                    }
                }
            }

        }

        public static string ScalarMultiply(string scal, Matrix mtrx)
        {
            var mat = mtrx.MyMatrix;
            var ret = "";
            scal = Maths.Calculate(scal);
            for (var i = 0; i < mat.Length; i++)
            {
                mat[i] = Regex.Replace(mat[i], @"\[|\]", "");
                var m = mat[i].Split(',');
                for (var j = 0; j < m.Length; j++) m[j] = m[j] == "" ? "" : Maths.Calculate($"{scal} * {m[j]}");
                for (var k = 0; k < m.Length; k++)
                    ret += k + 1 == m.Length ? m[k] : $"{m[k]},";
                mat[i] = ret;
                ret = "";
            }
            foreach (var m in mat)
            {
                ret += m != ""?$"[{m}]\n":"";
            }
            ret = ret.TrimEnd('\n');
            return ret;
        }
        
        public static (bool Success, string Message, string Value) Multiply(Matrix m1, Matrix m2)
        {

            var ret = "";
            int rowCounter = 0, columnCounter = 0;
            if (m1.Dimentions.Columns != m2.Dimentions.Rows) throw new InvalidMatrixException("Matrices cannot be multipled together.");
            for (var i = 0; i < m1.Dimentions.Rows; i++)
            {

                    while (rowCounter < m1.Dimentions.Columns)
                    {
                        ret += $"{(rowCounter > 0 ? "+" : "")} {m1.IntMatrix[i, rowCounter]}*{m2.IntMatrix[rowCounter, columnCounter]}";
                        rowCounter++;
                        if (rowCounter == m1.Dimentions.Columns && columnCounter < m2.Dimentions.Rows)
                        {
                            ret += (",");
                            rowCounter = 0;
                            columnCounter++;
                        }
                    if (columnCounter == m2.Dimentions.Columns)
                        break;
                    }
                rowCounter = 0;
                columnCounter = 0;
                ret += "\n";
            }
            ret = CalcMatrix(ret, m1.Dimentions.Rows, m2.Dimentions.Columns);
            return (true, null, ret);
        }
        /*
         * a = [[0,3,5];[5,5,2]]
         * b = [[3,4];[3, -2];[4, -2]]
         * a=[[4,-1];[2,-1]]
         * b=[[3,1,0];[2,1,-2]]
         */

        private static string CalcMatrix(string met ,int rows, int columns)
        {
            var ret = "";
            var tmp = met.Split('\n');
            
            var tmp2 = new string[rows, columns];
            for (var j = 0; j < rows; j++) {
                var rw = tmp[j].Split(',');
                for (var i = 0; i < columns; i++)
                    if (rw.Length > 1)
                    tmp2[j, i] = Maths.Calculate(rw[i]);
            }
            for(var i = 0; i < rows; i++)
            {
                ret += "[";
                for (var j = 0; j < columns; j++)
                {
                    ret += $"{(j > 0? ",": "")}{tmp2[i,j]}";
                }
                ret += "]\n";
            }
            return ret;
        }
    }
}