using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixReducer
{
    class Test
    {
        class TestMatrix
        {
            public double[,] Dimensions;
            public double Determinant;
            public TestMatrix(double[,] dimensions, double determinant)
            {
                Dimensions = dimensions;
                Determinant = determinant;
            }
        }
        static TestMatrix[] testmatrices = new TestMatrix[]{
            new TestMatrix(new double[,]{
                                {4,6},
                                {3,8}
                            },14),
            new TestMatrix(new double[,]{
                {6,1,1},
                {4,-2,5},
                {2,8,7}
            },-306)
        };
        static void TestDeterminant()
        {
            foreach (var test in testmatrices)
            {
                var matrix = new Matrix(test.Dimensions);
                Console.WriteLine(matrix.Determinant);
                System.Diagnostics.Trace.Assert(matrix.Determinant == test.Determinant);
            }
        }
        public static void RunAllTests()
        {
            TestDeterminant();
            Console.ReadKey();
        }
    }
}
