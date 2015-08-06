using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixLibrary
{
    public class Matrix
    {
        private double[,] data;

        public Matrix() : this(0, 0) { }
        public Matrix(double[,] item)
        {
            data = (double[,])item.Clone();
        }
        public Matrix(Matrix val)
        {
            data = (double[,])val.data.Clone();
        }
        public Matrix(int Row, int Column)
        {
            data = new double[Row, Column];
        }
        public double[,] To_Array
        {
            get
            {
                return data;
            }
        }
        public int RowCount
        {
            get
            {
                return data.GetLength(0);
            }
        }
        public int ColumnCount
        {
            get
            {
                return data.GetLength(1);
            }
        }
        public bool Squar
        {
            get
            {
                if ((ColumnCount == RowCount) && (ColumnCount > 0 && RowCount > 0))
                    return true;
                else
                    return false;
            }
        }
        public Matrix rearrangement_of_rows(int index1, int index2) //Перестановка строк
        {
            double temp;
            Matrix original = new Matrix(this);
            for (int i = 0; i < ColumnCount; i++)
            {
                temp = original[index1, i];
                original[index1, i] = original[index2, i];
                original[index2, i] = temp;
            }
            return original;
        }
        public Vector Get_column(int n)
        {
            Vector rez = new Vector(RowCount);
            for (int i = 0; i < RowCount; i++)
            {
                rez[i] = this[i, n];
            }
            return rez;
        }
        public Vector Get_row(int n)
        {
            Vector rez = new Vector(ColumnCount);
            for (int i = 0; i < ColumnCount; i++)
            {
                rez[i] = this[n, i];
            }
            return rez;
        }
        public Matrix AddColumn(Vector C)
        {
            Matrix Original = new Matrix(this);
            Matrix Original2 = new Matrix(Original.RowCount, Original.ColumnCount + 1);
            for (int i = 0; i < Original2.RowCount; i++)
            {
                for (int j = 0; j < Original.ColumnCount; j++)
                {
                    Original2[i, j] = Original[i, j];
                }
            }
            for (int i = 0; i < RowCount; i++)
            {
                Original2[i, Original2.ColumnCount - 1] = C[i];
            }
            return Original2;

        }
        #region Перегрузка_операций
        public double this[int index1, int index2]
        {
            get
            {
                return data[index1, index2];
            }
            set
            {
                data[index1, index2] = value;
            }
        }
        public static Matrix operator +(double op1, Matrix op2)
        {
            Matrix temp = new Matrix(op2.RowCount, op2.ColumnCount);
            for (int i = 0; i < op2.RowCount; i++)
            {
                for (int j = 0; j < op2.ColumnCount; j++)
                {
                    temp.data[i, j] = op1 + op2[i, j];
                }
            }
            return temp;

        }
        public static Matrix operator +(Matrix op1, double op2)
        {
            return op2 + op1;
        }
        public static Matrix operator +(Matrix op1, Matrix op2)
        {
            Matrix temp = new Matrix(op1.RowCount, op2.ColumnCount);
            if (op1.ColumnCount == op2.ColumnCount && op1.RowCount == op2.RowCount)
            {

                for (int i = 0; i < op1.RowCount; i++)
                {
                    for (int j = 0; j < op1.ColumnCount; j++)
                    {
                        temp.data[i, j] = op1[i, j] + op2[i, j];
                    }
                }
            }
            else
            {
                throw new Exception("Операция невозможна");

            }
            return temp;

        }

        public static Matrix operator -(Matrix op1, double op2)
        {
            return op1 + (-1) * op2;
        }
        public static Matrix operator -(Matrix op1, Matrix op2)
        {
            return op1 + (-1) * op2;
        }

        public static Matrix operator *(double op1, Matrix op2)
        {
            Matrix temp = new Matrix(op2.RowCount, op2.ColumnCount);
            for (int i = 0; i < temp.RowCount; i++)
                for (int j = 0; j < temp.ColumnCount; j++)
                {
                    temp[i, j] = op2[i, j] * op1;
                }
            return temp;
        }
        public static Matrix operator *(Matrix op1, double op2)
        {
            return op2 * op1;
        }
        public static Matrix operator *(Matrix op1, Matrix op2)
        {
            Matrix temp = new Matrix(op1.RowCount, op2.ColumnCount);
            if (op1.ColumnCount == op2.RowCount)
            {
                double res;
                for (int i = 0; i < op1.RowCount; i++)
                    for (int j = 0; j < op2.ColumnCount; j++)
                    {
                        res = 0;
                        for (int k = 0; k < op1.ColumnCount; k++)
                            res += op1[i, k] * op2[k, j];
                        temp[i, j] = res;
                    }

            }
            else
            {
                throw new Exception("Операция невозможна");
            }
            return temp;
        }
        public static Vector operator *(Vector op1, Matrix op2)
        {
            Vector temp = new Vector(op2.ColumnCount);
            if (op1.Count == op2.RowCount)
            {
                double res;
                for (int j = 0; j < op2.ColumnCount; j++)
                {
                    res = 0;
                    for (int k = 0; k < op1.Count; k++)
                        res += op1[k] * op2[k, j];
                    temp[j] = res;
                }

            }
            else
            {
                throw new Exception("Операция невозможна");
            }
            return temp;
        }
        public static Vector operator *(Matrix op1, Vector op2)
        {
            Vector temp = new Vector(op1.RowCount);
            if (op2.Count == op1.ColumnCount)
            {
                double res;
                for (int j = 0; j < op1.RowCount; j++)
                {
                    res = 0;
                    for (int k = 0; k < op2.Count; k++)
                        res += op2[k] * op1[j, k];
                    temp[j] = res;
                }

            }
            else
            {
                throw new Exception("Операция невозможна");
            }
            return temp;
        }
        #endregion
        #region Генерация определлёных матриц
        public static Matrix Get_Identity_Matrix(int RowCount)
        {
            return Get_Diag_Matrix(RowCount, 1);
        }
        public static Matrix Get_Diag_Matrix(int RowCount, double val)
        {
            Matrix rez = new Matrix(RowCount, RowCount);
            for (int i = 0; i < RowCount; i++)
            {
                rez[i, i] = val;
            }
            return rez;
        }
        public static Matrix Get_Rand_Matrix(int RowCount, int colColumn, int min = 0, int max = 10)
        {
            Matrix rez = new Matrix(RowCount, RowCount);
            Random rnd1 = new Random();
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < colColumn; j++)
                {
                    rez[i, j] = rnd1.Next(min, max);
                }
            }
            return rez;
        }
        public static Matrix Get_Rand_Matrix_real(int RowCount, int colColumn, int min = 0, int max = 1)
        {
            Matrix rez = new Matrix(RowCount, RowCount);
            Random rnd1 = new Random();
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < colColumn; j++)
                {
                    rez[i, j] = (max - min) * rnd1.NextDouble() + min; ;
                }
            }
            return rez;

        }
        #endregion
       
        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    str += Math.Round(this[i, j], 2) + " ";
                }
                str += Environment.NewLine;
            }
            return str;
        }
    }
}
