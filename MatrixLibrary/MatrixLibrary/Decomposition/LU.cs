using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixLibrary.Decomposition
{
    public class LU
    {
        /// <summary>
        /// Исходная матрица
        /// </summary>
        private Matrix val;

        /// <summary>
        /// Точность
        /// </summary>
        private const double e = 0.001;
        public LU(Matrix val)
        {
            this.val = new Matrix(val);
            decom();
        }
        private void decom()
        {
            int i, j;
            number_of_permutations = 0;
            P = Matrix.Get_Identity_Matrix(val.RowCount);
            L_U = new Matrix(val.RowCount, val.ColumnCount);
            Matrix A1 = val * P;
            Matrix[] M_i = new Matrix[val.ColumnCount - 1];
            Matrix A = new Matrix(A1);
            M = Matrix.Get_Identity_Matrix(val.RowCount);
            for (i = 0; i < val.ColumnCount - 1; i++)
            {
                M_i[i] = Matrix.Get_Identity_Matrix(val.RowCount);
                if (Math.Abs(A[i, i]) > e)
                {
                    for (j = i + 1; j < val.RowCount; j++)
                    {
                        M_i[i][j, i] = (-1) * (A[j, i] / A[i, i]);
                    }
                }
                else
                {
                    double pivotValue = 0;
                    int pivot = -1;
                    for (int k = i; k < A.ColumnCount; k++)
                    {
                        if (Math.Abs(A[k, i]) > pivotValue)
                        {
                            pivotValue = Math.Abs(A[k, i]);
                            pivot = k;
                        }
                    }
                    if (pivotValue < e)
                    {
                        throw new Exception("Разложение данным методом невозможно!!!");
                    }
                    P.rearrangement_of_rows(pivot, i);
                    A.rearrangement_of_rows(pivot, i);
                    L_U.rearrangement_of_rows(pivot, i);
                    number_of_permutations++;
                    for (j = i + 1; j < val.RowCount; j++)
                    {
                        M_i[i][j, i] = (-1) * (A[j, i] / A[i, i]);
                    }
                }
                A1 = M_i[i] * A;
                A = new Matrix(A1);
                for (j = i + 1; j < val.RowCount; j++)
                {
                    L_U[j, i] = (-1) * M_i[i][j, i];
                }
            }
            L_U = L_U + A1;
            for (int k = M_i.Length - 1; k >= 0; k--)
            {
                M *= M_i[k];
            }
        }
        /// <summary>
        /// LU матрица
        /// </summary>
        public Matrix L_U
        {
            get;
            private set;
        }
        /// <summary>
        /// Матрица преобразований
        /// </summary>
        public Matrix M
        {
            get;
            private set;
        }
        /// <summary>
        /// Матрица перестановок
        /// </summary>
        public Matrix P
        {
            get;
            private set;
        }
        /// <summary>
        /// Число перестановок
        /// </summary>
        public int number_of_permutations
        {
            get;
            private set;
        }
    }
}
