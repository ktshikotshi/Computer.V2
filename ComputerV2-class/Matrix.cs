using System;
using System.Text.RegularExpressions;
using System.Threading;
using ComputerV2_class.Exceptions;

namespace ComputerV2_class
{
    public class Matrix
    {
        private readonly (int Columns, int Rows) _dimentions;
        private string[] MyMatrix { get; }
        private double[,] IntMatrix { get; }

        private (int Columns, int Rows) Dimentions => _dimentions;

        private Matrix(string expr)
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

        private static string ScalarMultiply(string scal, Matrix mtrx)
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

        private static string Multiply(Matrix m1, Matrix m2)
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
            return (ret);
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
        
        public static (bool Found, string Value) MatrixManipulation(string expr)
        {
            if (!expr.Contains("[")) return (false, expr);
            expr = ManMatrix(expr);
            var split = SplitMatrix(expr);
            //will throw an exceptoion if the numbers are not correctly formatted.
            {
                var nMatrix = new Matrix(split);
            }
            return (true, expr);
        }

        private static string ManMatrix(string expression)
        {
            var regex = new Regex(@"((\d+([\.,]\d+)?)(\*)(\[(.*?)[\]]{1,}(\n)?)+)");
            if (Regex.IsMatch(expression, @"[1]\*"))
                expression = expression.Replace("1*", "");
            while (regex.IsMatch(expression))
            {
                var match = regex.Match(expression).Value;
                var  m2 = match;
                var tmp = m2.Split('*');
                var scalar = tmp[0].Contains("[") ? Decimal.Parse(tmp[tmp.Length - 1]) : Decimal.Parse(tmp[0]);
                var mtrxVal = tmp[0].Contains("[") ? SplitMatrix(tmp[0]) : SplitMatrix(tmp[tmp.Length - 1]);
                var mtrx = new Matrix(mtrxVal);
                expression = expression.Replace(match, Matrix.ScalarMultiply(scalar.ToString(), mtrx));
                
            }
            regex = new Regex(@"((((\[(.*?)[\]]{1,}(\n|;)?)+)[*]{2})((\[(.*?)[\]]{1,})(\n|\;)?)+)");
            while (regex.IsMatch(expression))
            {
                var match = regex.Match(expression).Value;
                var  m2 = match;
                m2 = m2.Remove(m2.IndexOf('*'), 1);
                var tmp = m2.Split('*');
                var thisM = new Matrix(SplitMatrix(tmp[0]));
                var this2 = new Matrix(SplitMatrix(tmp[tmp.Length - 1]));
                var mlty = Multiply(thisM, this2);
                expression = expression.Replace(match, mlty);
            }
            return (expression);
        }
        private static string SplitMatrix(string expr)
        {
            if (Regex.IsMatch(expr, @"\]\[")) throw new InvalidMatrixException($"Missing ; :  {expr}");
            expr = Regex.Replace(expr, @"\[|\]", "");
            var nExpr = Regex.Split(expr, @"[\n]|\;");
            expr = "";
            for (var i = 0; i < nExpr.Length; i++)
            {
                
                nExpr[i] = nExpr[i] == "" ? "" : $"[{nExpr[i]}]";
                if (Regex.IsMatch(nExpr[i], @"^\[(((\-)?\d+([\.,]\d+)?)?([,])?)+\]$") || nExpr[i] == "")
                    expr += nExpr[i] == ""? "" : $"{nExpr[i]}\n";
                else
                    throw new InvalidMatrixException($"format of matric is not correct:  {nExpr[i]}");
            }
            expr = expr.TrimEnd('\n');
            return (expr);
        }
    }
}