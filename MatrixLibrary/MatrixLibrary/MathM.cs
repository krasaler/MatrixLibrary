using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary.Decomposition;


namespace MatrixLibrary
{
    public class MathM
    {
        public static double Determinant(Matrix val)
        {
            LU lu = new LU(val);
            double R = 1;
            Matrix temp = new Matrix(lu.L_U);
            for (int i = 0; i < temp.RowCount; i++)
            {
                R *= temp[i, i];
            }
            return Math.Round(R, 2) * Math.Pow(-1, lu.number_of_permutations);
        }
        public static Matrix Inverse_LU(Matrix val)
        {
            Matrix original = new Matrix(val);
            Matrix rez = new Matrix(val.RowCount, val.ColumnCount);
            Matrix E = Matrix.Get_Identity_Matrix(val.RowCount);
            Vector L = new Vector(val.RowCount);
            for (int i = 0; i < val.ColumnCount; i++)
            {
                L = SLAU.LU(original, E.Get_column(i));
                for (int j = 0; j < val.RowCount; j++)
                {
                    rez[j, i] = L[j];
                }
            }
            return rez;
        }
        public static Matrix Inverse_Fadeev(Matrix val)
        {
            Matrix A = new Matrix(val);
            Matrix rez = new Matrix(val.RowCount, val.ColumnCount);
            Fadeev fadeev = new Fadeev(A);
            rez = fadeev.F;
            rez = 1 / Convert.ToDouble(fadeev.Q[rez.ColumnCount - 1]) * fadeev.B_n;
            return rez;
        }
        public static double cond_1(Matrix val)
        {
            return Matrix_norm_1(val) * Matrix_norm_1(Inverse_LU(val));
        }
        public static double cond_2(Matrix val)
        {
            return Matrix_norm_2(val) * Matrix_norm_2(Inverse_LU(val));
        }
        public static double cond_INF(Matrix val)
        {

            return Matrix_norm_inf(val) * Matrix_norm_inf(Inverse_LU(val));
        }
        public static Matrix pow(Matrix val, int a)
        {
            Matrix temp = new Matrix(val);
            if (val.ColumnCount == val.RowCount && a > 0)
            {

                for (int i = 0; i < a - 1; i++)
                {
                    temp *= temp;
                }
            }
            else
            {
                throw new Exception("Операция невозможна");
            }
            return temp;

        }
        public static double Trace(Matrix val)
        {
            double sum;
            sum = 0;
            if (val.Squar)
            {
                for (int i = 0; i < val.ColumnCount && val.Squar; i++)
                {

                    sum += val[i, i];

                }
            }
            else
            {
                throw new Exception("Матрица должна быть квадратной");
            }
            return sum;
        }
        public static Matrix Transpose(Matrix val)
        {
            Matrix temp = new Matrix(val.ColumnCount, val.RowCount);
            for (int i = 0; i < temp.RowCount; i++)
            {
                for (int j = 0; j < temp.ColumnCount; j++)
                {
                    temp[i, j] = val[j, i];
                }
            }
            return temp;
        }
        static public double vect_norm_1(Vector data)
        {
            double rez = 0;
            for (int i = 0; i < data.Count; i++)
            {
                rez += Math.Abs(data[i]);
            }
            return rez;
        }

        static public double vect_norm_2(Vector data)
        {
            double rez = 0;
            for (int i = 0; i < data.Count; i++)
            {
                rez += Math.Abs(data[i]) * Math.Abs(data[i]);
            }
            rez = Math.Sqrt(rez);
            return rez;
        }

        static public double vect_norm_inf(Vector data)
        {
            double rez = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (Math.Abs(data[i]) > rez) rez = Math.Abs(data[i]);
            }
            return rez;
        }
        public static double Matrix_norm_1(Matrix val)
        {
            double rez = 0;
            if (val.Squar)
            {
                double r;
                for (int i = 0; i < val.ColumnCount; i++)
                {
                    r = 0;
                    for (int j = 0; j < val.RowCount; j++)
                    {
                        r += Math.Abs(val[j, i]);
                    }
                    if (r > rez) rez = r;
                }
            }
            else
            {
                throw new Exception("Матрица должна быть квадратной!!");
            }
            return rez;
        }
        public static double Matrix_norm_2(Matrix val)
        {
            double rez = 0;
            if (val.Squar)
            {
                Math.Round(Math.Sqrt(EigenValuesVector.MaxEigenValue(val, 0.01)), 2);
            }
            else
            {
                throw new Exception("Матрица должна быть квадратной!!");
            }
            return rez;
        }

        public static double Matrix_norm_inf(Matrix val)
        {
            double rez = 0;
            if (val.Squar)
            {
                double r;
                for (int i = 0; i < val.RowCount; i++)
                {
                    r = 0;
                    for (int j = 0; j < val.ColumnCount; j++)
                    {
                        r += Math.Abs(val[i, j]);
                    }
                    if (r > rez) rez = r;
                }
            }
            else
            {
                throw new Exception("Матрица должна быть квадратной");
            }
            return rez;
        }
        public static double Matrix_norm_Euclidian_norm(Matrix val)
        {
            double rez = 0;
            if (val.Squar)
            {
                for (int i = 0; i < val.RowCount; i++)
                {
                    for (int j = 0; j < val.ColumnCount; j++)
                    {
                        rez += val[i, j] * val[i, j];
                    }
                }
                rez = Math.Round(Math.Sqrt(rez), 2);
            }
            else
            {
                throw new Exception("Матрица должна быть квадратной");
            }
            return rez;
        }
        public static double Matrix_norm_N(Matrix val)
        {
            return Matrix_norm_Euclidian_norm(val);
        }
        public static double Matrix_norm_M(Matrix val)
        {
            double rez=0;
            if (val.Squar)
            {    
                for (int i = 0; i < val.RowCount; i++)
                {
                    for (int j = 0; j < val.ColumnCount; j++)
                    {
                        if (val[i,j] > rez) rez = val[i,j];
                    }
                }
                rez *= val.ColumnCount;
            }
            else
            {
                throw new Exception("Матрица должна быть квадратной");
            }
            return rez;
        }
    }
}
