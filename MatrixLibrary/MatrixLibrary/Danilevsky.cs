using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixLibrary
{
    class Danilevsky
    {
        public Danilevsky(Matrix val)
        {
            Matrix original = new Matrix(val);
            A = new Matrix(val);
            Q = new Vector(val.RowCount);
            Matrix M1 = Matrix.Get_Identity_Matrix(val.RowCount);
            Matrix M = Matrix.Get_Identity_Matrix(val.RowCount);
            S = Matrix.Get_Identity_Matrix(val.RowCount);
            for (int k = 0; k < val.ColumnCount - 1; k++)
            {
                M1 = Matrix.Get_Identity_Matrix(val.RowCount);
                M = Matrix.Get_Identity_Matrix(val.RowCount);
                for (int i = 0; i < val.ColumnCount; i++)
                {
                    if (A[val.RowCount - 1 - k, val.RowCount - 2 - k] == 0)
                    {
                        throw new Exception("деление на ноль");
                    }
                    if (i != val.RowCount - 2 - k)
                    {
                        M[val.RowCount - 2 - k, i] = ((-1) * A[val.RowCount - 1 - k, i]) / A[val.RowCount - 1 - k, val.RowCount - 2 - k];
                    }
                    else
                    {
                        M[val.RowCount - 2 - k, i] = 1 / A[val.RowCount - 1 - k, val.RowCount - 2 - k];
                    }
                    M1[val.RowCount - 2 - k, i] = A[val.RowCount - k - 1, i];
                }
                S *= M;
                A = M1 * A * M;
            }
            for (int i = 0; i < val.ColumnCount; i++)
            {
                Q[i] = A[0, i];
            }
            S = MathM.Inverse_Fadeev(S);
        }
        public Matrix A
        {
            get;
            private set;
        }
        public Vector Q
        {
            get;
            private set;
        }
        public Matrix S
        {
            get;
            private set;
        }
    }
}
