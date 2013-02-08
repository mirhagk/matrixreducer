using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixReducer
{
    public class Vector
    {
        double[] values;
        int length;
        public int Length
        {
            get
            {
                return length;
            }
        }
        public Vector(int length)
        {
            this.length = length;
            values = new double[length];
        }
        public double this[int index]
        {
            get
            {
                return values[index - 1];
            }
            set
            {
                values[index - 1] = value;
            }
        }
        public static double DotProduct(Vector a, Vector b)
        {
            if (a.length != b.length)
                throw new ArgumentException("Can't calculate dot product of vectors with different lengths");
            double result = 0;
            for (int i = 0; i < a.length; i++)
            {
                result += a.values[i] * b.values[i];
            }
            return result;
        }
        public static implicit operator Vector(Matrix matrix)
        {
            if (matrix.Columns == 1)
            {
                matrix = matrix.Transpose();
            }
            else if (matrix.Rows != 1)
                throw new ArgumentException("Can only convert row or column matrices to a vector");
            Vector result = new Vector(matrix.Columns);
            for (int i = 1; i <= result.length; i++)
            {
                result[i] = matrix[1, i];
            }
            return result;
        }
    }
}
