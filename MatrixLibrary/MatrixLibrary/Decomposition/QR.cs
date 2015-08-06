using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixLibrary.Decomposition
{
    public class QR
    {
        public QR(Matrix val)
        {
            Q = Matrix.Get_Identity_Matrix(val.RowCount);
            decom(val);
        }
        
        public Matrix Q
        {
            get;
            private set;
        }
        public Matrix R
        {
            get;
            private set;
        }
        public Matrix decom(Matrix A, int t=0)
        {
            if (MathM.Determinant(A) == 0)
            {
                throw new Exception("Матрица вырождена");
            }
            double t1 = A[0 + t, 0 + t] / A[1 + t, 0 + t];
            double c1 = 1 / Math.Sqrt(1 + t1 * t1);
            double s1 = c1 * t1;
            Matrix P = new Matrix(A.RowCount, A.ColumnCount);
            for (int i = 0; i < P.RowCount; i++)
                for (int j = 0; j < P.ColumnCount; j++)
                    if (i == j)
                        P[i, j] = 1;
                    else
                        P[i, j] = 0;
            P[0 + t, 0 + t] = s1; P[0 + t, 1 + t] = c1;
            P[1 + t, 0 + t] = -c1; P[1 + t, 1 + t] = s1;

            Q = Q * MathM.Transpose(P);
            A = P * A;
            t++;

            if (t < A.RowCount - 1)
                A = decom(A, t);
            R = new Matrix(A);
            return A;
        }
    }
}
