using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixLibrary
{
    class Fadeev
    {
        public Fadeev(Matrix val)
        {
            double q;
            Matrix B = Matrix.Get_Identity_Matrix(val.RowCount);
            Matrix B1 = new Matrix(val.RowCount, val.ColumnCount);
            Matrix A1 = new Matrix();
            Matrix A = new Matrix(val);
            Matrix rez = new Matrix(val);
            Matrix tmp = new Matrix(B);
            Q = new Vector(val.RowCount);
            for (int i = 0; i < val.ColumnCount; i++)
            {
                A1 = A * B;
                q = MathM.Trace(A1) / Convert.ToDouble(i + 1);
                B1 = new Matrix(B);
                B = A1 - q * tmp;
                Q[i] = q;

            }
            B_n = B1;
            F = rez;
        }
        public Matrix F
        {
            get;
            private set;
        }
        public Matrix B_n
        {
            get;
            private set;
        }
        public Vector Q
        {
            get;
            private set;
        }
    }
}
