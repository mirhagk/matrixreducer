using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixReducer
{
    public class Matrix:ICloneable
    {
        /// <summary>
        /// a 2D array to store the matrix in double form. Rows by columns, just as normal mathematical convention
        /// </summary>
        double[,] values;
        int rows;
        int columns;
        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }
        /// <summary>
        /// Public access to elements inside the matrix, accepts the arguments in 1-based notation (first row is 1)
        /// </summary>
        /// <param name="r">Which row to use</param>
        /// <param name="c">Which column to use</param>
        /// <returns></returns>
        public double this[int r, int c]
        {
            get
            {
                return values[r - 1, c - 1];
            }
            set
            {
                values[r - 1, c - 1] = value;
            }
        }
        public bool IsSquare
        {
            get
            {
                return rows == columns;
            }
        }
        public Matrix(int rows, int columns, bool identity = false)
        {
            this.rows = rows;
            this.columns = columns;
            values = new double[rows, columns];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    values[r, c] = 0;
                }
            }
            if (identity)
                if (IsSquare)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        values[i, i] = 1;
                    }
                }
                else
                    throw new ArgumentException("Identity matrices must be square matrices");
        }

        public bool RecordingHistory
        {
            get
            {
                return operationHistory != null;
            }
            set
            {
                if (value)
                {
                    if (!RecordingHistory)
                        operationHistory = new List<MatrixHistory.MatrixOperation>();
                }
                else
                    operationHistory = null;
            }
        }

        public object Clone()
        {
            //may not be efficient, but it certainly is a clean solution
            Matrix result = Transpose();
            return result.Transpose();
        }
        public List<MatrixHistory.MatrixOperation> operationHistory;
        public void SwapRows(int row1, int row2)
        {
            if (row1 == row2)
                return;
            if (RecordingHistory)
                operationHistory.Add(new MatrixHistory.SwapRows(this) { fromRow = row1, toRow = row2 });
            for (int c = 0; c < Columns; c++)
            {
                double temp = values[row1, c];
                values[row1, c] = values[row2, c];
                values[row2, c] = temp;
            }
        }
        public void MultiplyRow(int row, double factor)
        {
            if (factor == 1)
                return;
            if (RecordingHistory)
                operationHistory.Add(new MatrixHistory.MultiplyRow(this) { row = row, factor = factor });
            for (int c = 0; c < Columns; c++)
            {
                values[row, c] = values[row, c] * factor;
            }
        }
        public void AddMultiplesOfRow(int row1, int row2, double factor)
        {
            if (RecordingHistory)
                operationHistory.Add(new MatrixHistory.AddMultiplesToRow(this) { fromRow = row1, toRow = row2, factor=factor });
            for (int c = 0; c < Columns; c++)
            {
                values[row2, c] += values[row1, c] * factor;
            }
        }
        public void RREF()
        {
            int firstRow = 0;
            for (int c = 0; c < Columns; c++)
            {
                //reduce it
                //step 1, ensure that the first non-zero row is the first row (or there are no non-zero rows)
                int nonZeroRow = -1;
                for (int r = firstRow; r < Rows; r++)
                {
                    if (values[r, c] != 0)
                    {
                        nonZeroRow = r;
                        break;
                    }
                }
                if (nonZeroRow==-1)
                    continue;
                
                SwapRows(nonZeroRow,firstRow);
                //Console.WriteLine("Swapping rows {0} and {1}", nonZeroRow + 1, firstRow + 1);

                //step 2, divide the first row so that it starts with a 1
                double factor = 1 / values[firstRow, c];
                MultiplyRow(firstRow, factor);
                //Console.WriteLine("Multiplying row {0} by {1}", firstRow + 1, factor);

                //step 3, add multiples of the first row to other rows until only the first row starts with a non-zero value
                for (int r = 0; r < Rows; r++)
                {
                    if (r == firstRow)
                        continue;
                    if (values[r, c] != 0)
                    {
                        double multFactor = -values[r, c] / values[firstRow, c];
                        AddMultiplesOfRow(firstRow, r, multFactor);
                        //Console.WriteLine("Add {0} times row {1} to row {2}", multFactor, firstRow + 1, r + 1);
                    }
                }

                //step 4, cover up the first row and repeat with the rest of the rows
                firstRow++;
            }

                
        }
        public Matrix ColumnMatrix(int c)
        {
            Matrix result = new Matrix(Rows, 1);
            for (int r = 1; r <= Rows; r++)
            {
                result[r,1] = this[r, c];
            }
            return result;
        }
        public Matrix RowMatrix(int r)
        {
            Matrix result = new Matrix(1, Columns);
            for (int c = 1; c <= Columns; c++)
            {
                result[1, c] = this[r, c];
            }
            return result;
        }
        public Matrix Transpose()
        {
            Matrix result = new Matrix(Columns, Rows);
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    result.values[c, r] = values[r, c];
                }
            }
            return result;
        }
        public override string ToString()
        {
            string result = "";
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    result += values[r, c].ToString() + " ";
                }
                result += "\n";
            }
            return result;
        }
        public static Matrix FromString(string input)
        {
            string[] rows = input.Split('\n');
            Matrix result = new Matrix(int.Parse(rows[0].Split(' ')[0]), int.Parse(rows[0].Split(' ')[1]));
            for (int r = 1; r <= result.rows; r++)
            {
                string[] values = rows[r].Split(' ');
                for (int c = 1; c <= result.Columns; c++)
                {
                    result[r, c] = double.Parse(values[c - 1]);
                }
            }
            return result;
        }
        public static implicit operator Matrix(double[,] array)
        {
            Matrix result = new Matrix(array.GetLength(0), array.GetLength(1));
            result.values = array;
            return result;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (!CanMultiply(a, b))
                throw new ArgumentException("Matrices can't be multiplied");
            int indenticalSize = b.Columns;
            Matrix result = new Matrix(a.Rows, b.Columns);
            for (int r = 1; r <= result.Rows; r++)
            {
                for (int c = 1; c <= result.Columns; c++)
                {
                    result[r, c] = Vector.DotProduct(b.ColumnMatrix(c), a.RowMatrix(r));
                }
            }
            return result;
        }
        public static Matrix operator *(Matrix matrix, double scalar)
        {
            Matrix result = new Matrix(matrix.Rows, matrix.Columns);
            for (int r = 1; r <= result.Rows; r++)
            {
                for (int c = 1; c <= result.Columns; c++)
                {
                    result[r, c] = matrix[r, c] * scalar;
                }
            }
            return result;
        }
        public static Matrix operator *(double scalar, Matrix matrix) { return matrix * scalar; }
        public static bool CanMultiply(Matrix a, Matrix b)
        {
            return a.Rows == b.Columns;
        }
    }
}
