using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatrixLibrary.Decomposition;

namespace MatrixLibrary
{
    public class EigenValuesVector
    {

        public static Matrix QR_Values(Matrix A, double Eps = 0.001)
        {
            if (MathM.Determinant(A) == 0)
            {
                throw new Exception("Матрица вырождена");
            }
            Matrix Q1 = new Matrix(A.RowCount, A.ColumnCount);
            Matrix R1 = new Matrix(A.RowCount, A.ColumnCount);
            QR qr = new QR(Hessenberg(A));
            R1 = qr.R;
            Q1 = qr.Q;

            Matrix A1 = new Matrix(A.RowCount, A.ColumnCount);
            A1 = R1 * Q1;

            Matrix K = new Matrix(A1);
            for (int i = 0; i < K.ColumnCount; i++)
                K[i, i] = 0;

            if (MathM.Matrix_norm_inf(K) > Eps)
                A1 = QR_Values(A1);

            return A1;
        }
        public static Vector Fadeev_Values(Matrix val, double e = 0.001)
        {
            Fadeev fadeev = new Fadeev(val);
            Console.WriteLine("Q=" + fadeev.Q);
            double temp = 0;
            Vector eigenvalues = new Vector(val.ColumnCount);
            Vector I = Intervals(val, fadeev.Q);
            for (int i = 0; i < val.ColumnCount; i++)
            {
                temp = method_bisection(fadeev.Q, I[i], I[i + 1], e);
                eigenvalues[i] = temp;
            }

            return eigenvalues;
        }
        public static Vector Danilevsky_Values(Matrix val, double e = 0.001)
        {
            Danilevsky dan = new Danilevsky(val);
            Console.WriteLine("Q=" + dan.Q);
            double temp = 0;
            Vector eigenvalues = new Vector(val.ColumnCount);
            Vector I = new Vector(Intervals(val, dan.Q));
            for (int i = 0; i < val.ColumnCount; i++)
            {
                temp = method_bisection(dan.Q, I[i], I[i + 1], e);
                eigenvalues[i] = temp;
            }

            return eigenvalues;
        }
        public Matrix Danilevsky_Vectors(Matrix val)
        {
            Matrix A = new Matrix(val);
            Danilevsky dan = new Danilevsky(A);
            Matrix Y = new Matrix(val.RowCount, val.ColumnCount);
            Matrix T = dan.S;
            Vector R = new Vector(val.RowCount);
            R = Danilevsky_Values(dan.A);
            for (int i = 0; i < val.ColumnCount; i++)
            {
                for (int j = 0; j < val.RowCount; j++)
                {
                    Y[val.RowCount - 1 - j, i] = Math.Pow(R[i], j);
                }
            }
            A = T * Y;
            double a = 0;
            for (int i = 0; i < val.ColumnCount; i++)
            {
                a = MathM.vect_norm_2(A.Get_column(i));
                for (int j = 0; j < val.RowCount; j++)
                {
                    A[j, i] /= a;
                }
            }
            return A;
        }
        public static double MaxEigenValue(Matrix val, double E = 0.01)
        {
            Matrix C = new Matrix(val);
            C *= MathM.Transpose(C);
            double rez = 0;
            Vector X = new Vector(val.ColumnCount);
            X[0] = 1;
            Vector Y;
            Vector X1 = new Vector(X);
            if (val.Squar)
            {
                double m;
                do
                {
                    X = X1;
                    Y = C * X;
                    m = 0;
                    for (int i = 0; i < Y.Count;i++ )
                        {
                            if (Math.Abs(Y[i]) > m) m = Math.Abs(Y[i]);
                        }
                    X1 = (1 / m) * Y;
                }
                while (MathM.vect_norm_inf(X - X1) > E);
                rez = m;
            }
            else
            {
                throw new Exception("Матрица должна быть квадратной");
            }
            return rez;
        }
        #region Вспомогательные функции
        private static Matrix Hessenberg(Matrix A, int t = 0)
        {
            if (MathM.Determinant(A) == 0)
            {
                throw new Exception("Матрица вырождена");
            }

            int l = A.ColumnCount - 1;
            Matrix T = new Matrix(l - t, l - t);
            T = Matrix.Get_Identity_Matrix(T.ColumnCount);
            Vector a = new Vector(l - t);
            Vector d = new Vector(l - t);
            for (int i = 1 + t; i <= l; i++)
                a[i - 1 - t] = A[i, t];

            double c = ((a[0] - MathM.vect_norm_2(a)) * (a[0] - MathM.vect_norm_2(a)));
            for (int i = 1; i < l - t; i++)
                c += a[i] * a[i];
            c = Math.Sqrt(c); c = 1 / c;

            d[0] = c * (a[0] - MathM.vect_norm_2(a));
            for (int i = 1; i < l - t; i++)
                d[i] = c * a[i];
            T = T - ((2 * d) * d);



            Matrix S = new Matrix(l + 1, l + 1);


            for (int i = 0; i < l + 1; i++)
                for (int j = 0; j < l + 1; j++)
                {
                    S[i, j] = 0;
                }
            for (int i = 0; i <= t; i++)
                S[i, i] = 1;


            for (int i = 1 + t; i < l + 1; i++)
                for (int j = 1 + t; j < l + 1; j++)
                    S[i, j] = T[i - 1 - t, j - 1 - t];

            T = S * A;
            T = T * S;
            t++;

            if (t != l - 1)
                T = Hessenberg(T, t);

            return T;
        }
        private static double the_characteristic_polynomial(Vector Q, double x)
        {
            double rez = Math.Pow(x, Q.Count);
            for (int i = Q.Count - 1, j = 0; i >= 0; i--, j++)
            {
                rez += (-1) * Q[j] * Math.Pow(x, i);
            }
            return rez;
        }
        private static double method_bisection(Vector Q, double A, double B, double E)
        {
            double c;
            if (the_characteristic_polynomial(Q, A) == 0)
            {
                return A;
            }
            if (the_characteristic_polynomial(Q, B) == 0)
            {
                return B;
            }
            if (the_characteristic_polynomial(Q, A) * the_characteristic_polynomial(Q, B) > 0)
            {
                return double.NaN;
            }
            do
            {
                c = (A + B) / 2;
                if (the_characteristic_polynomial(Q, A) * the_characteristic_polynomial(Q, c) < 0)
                {
                    B = c;
                }
                else
                {
                    A = c;
                }
            }
            while (Math.Abs(A - B) > E);
            return (A + B) / 2;
        }
        private static Vector Intervals(Matrix val, Vector Q)
        {
            Matrix A = new Matrix(val);
            Vector X = new Vector(val.RowCount + 1);
            double a, b;
            int j = 0;
            a = (-1) * MathM.Matrix_norm_inf(A);
            b = MathM.Matrix_norm_inf(A);
            X[0] = a;
            for (double i = a; i < b; i += 0.5)
            {
                if (the_characteristic_polynomial(Q, X[j]) * the_characteristic_polynomial(Q, i) < 0)
                {
                    X[++j] = i;
                }
            }
            X[val.RowCount] = b;
            return X;
        }
        #endregion


    }
}
