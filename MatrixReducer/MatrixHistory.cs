using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixReducer
{
    public class MatrixHistory
    {
        public class MatrixOperation
        {
            public Matrix matrixBefore;
            public MatrixOperation(Matrix matrix)
            {
                matrixBefore = (Matrix)matrix.Clone();
            }
        }
        public class MultiplyRow:MatrixOperation
        {
            public int row;
            public double factor;
            public MultiplyRow(Matrix matrix) : base(matrix) { }
        }
        public class SwapRows : MatrixOperation
        {
            public int fromRow;
            public int toRow;
            public SwapRows(Matrix matrix) : base(matrix) { }
        }
        public class AddMultiplesToRow : MatrixOperation
        {
            public int fromRow;
            public int toRow;
            public double factor;
            public AddMultiplesToRow(Matrix matrix) : base(matrix) { }
        }
    }
}
