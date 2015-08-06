using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixLibrary
{
    public class Vector
    {
        private double[] ptr;

        public Vector() : this(0) { }
        public Vector(params double[] item)
        {
            ptr = (double[])item.Clone();
        }
        public Vector(Vector val)
        {
            ptr = (double[])val.ptr.Clone();
        }
        public Vector(int Count)
        {
            ptr = new double[Count];
        }
        public int Count
        {
            get
            {

                return ptr.Length;
            }
        }
        #region Перегрузка_операций
        public double this[int index]
        {
            get
            {
                return ptr[index];
            }
            set
            {
                ptr[index] = value;
            }
        }
        static public Vector operator +(double a, Vector op)
        {
            Vector temp = new Vector(op.Count);
            for (int i = 0; i < op.Count; i++)
            {
                temp.ptr[i] = op[i] + a;
            }
            return temp;

        }
        static public Vector operator +(Vector op, double a)
        {
            return a + op;

        }
        static public Vector operator +(Vector op1, Vector op2)
        {
            Vector temp = new Vector(op1.Count);
            if (op1.Count == op2.Count)
            {

                for (int i = 0; i < op1.Count; i++)
                {
                    temp.ptr[i] = op1[i] + op2[i];
                }
            }
            else
            {
                throw new Exception("Операция невозможна т.к длины векторов не совпадают.");

            }
            return temp;

        }
        static public Vector operator -(Vector op1, Vector op2)
        {
            return op1 + (-1) * op2;
        }

        static public Vector operator *(double a, Vector op2)
        {
            Vector temp = new Vector(op2.Count);
            for (int i = 0; i < temp.Count; i++)
                temp[i] = op2[i] * a;
            return temp;
        }
        static public Vector operator *(Vector op, double a)
        {
            return a * op;
        }
        static public double operator *(Vector op1, Vector op2)//Скалярное произведение
        {
            double temp;
            if (op1.Count == op2.Count)
            {
                double res = 0;
                for (int i = 0; i < op1.Count; i++)
                    res += op1[i] * op2[i];
                temp = res;
            }
            else
            {
                throw new Exception("Операция невозможна т.к длины векторов не совпадают.");
            }
            return temp;
        }
        public static Vector vectorMul(Vector a, Vector b)//Векторное произведение
        {
            Vector temp = new Vector(3);
            if (a.Count == b.Count)
            {
                if (a.Count == 3)
                {
                    temp[0] = a[1] * b[2] - a[2] * b[1];
                    temp[1] = a[2] * b[0] - a[0] * b[2];
                    temp[2] = a[0] * b[1] - a[1] * b[0];
                }
            }
            return temp;
        }
        #endregion
        public double vectLength
        {
            get
            {
                return Math.Sqrt(this * this);
            }
        }


        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < Count; i++)
            {
                str += this[i] + "\t";
            }
            return str;
        }
    }
}
