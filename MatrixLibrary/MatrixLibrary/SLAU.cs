using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary.Decomposition;
namespace MatrixLibrary
{
    public class SLAU
    {
        /// <summary>
        /// Точность
        /// </summary>
        const double e = 0.001;
        public static Vector Kramer(Matrix A, Vector b)
        {
            Vector X = new Vector(A.ColumnCount);
            Matrix temp = new Matrix(A.RowCount, 0);
            for (int i = 0; i < A.ColumnCount; i++)
            {
                temp = new Matrix(A.RowCount, 0);
                for (int j = 0; j < A.ColumnCount; j++)
                {
                    if (j == i)
                    {
                        temp = temp.AddColumn(b);
                    }
                    else
                    {
                        temp = temp.AddColumn(A.Get_column(j));
                    }
                }
                X[i] = MathM.Determinant(temp) / MathM.Determinant(A);
            }
            return X;
        }
        public static Vector LU(Matrix A, Vector b)
        {
            double sum;
            LU lu = new LU(A);
            Matrix Rez = lu.L_U;
            Vector B_i = lu.M * b;
            Vector X = new Vector(A.ColumnCount);
            for (int i = A.ColumnCount - 1; i >= 0; i--)
            {
                if (i == A.ColumnCount - 1)
                {
                    X[i] = B_i[i] / Rez[i, i];
                }
                else
                {
                    sum = 0;
                    for (int j = i; j < A.ColumnCount; j++)
                    {
                        sum += Rez[i, j] * X[j];
                    }
                    X[i] = (B_i[i] - sum) / Rez[i, i];
                }
            }
            return X;
        }
        public static Vector The_method_of_simple_iteration(Matrix A, Vector b)
        {
            Matrix original = new Matrix(A);
            Matrix B = new Matrix(A.RowCount, A.ColumnCount);
            Vector T;
            Vector X = new Vector(A.ColumnCount);
            Vector X1 = new Vector(A.ColumnCount);
            Vector d = new Vector(A.ColumnCount);
            B = Matrix.Get_Identity_Matrix(A.RowCount) - original;
            if (MathM.Matrix_norm_inf(B) < 1)
            {
                X1 = new Vector(b);
                d = new Vector(X1);
                double k;
                double sum = 0;
                do
                {
                    X = new Vector(X1);
                    for (int i = 0; i < B.RowCount; i++)
                    {
                        sum = 0;
                        for (int j = 0; j < B.ColumnCount; j++)
                        {
                            sum += B[i, j] * X[j];
                        }
                        X1[i] = sum + d[i];
                    }
                    T = X1 - X;
                    k = MathM.vect_norm_inf(T);
                }
                while (k >= e);
                return X1;
            }
            else
            {
                throw new Exception("Итерационный процес не сходится");
            }
        }
    }
}
