﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixReducer
{
    class Program
    {
        static void SwapRows(int row1, int row2, ref double[,] matrix, ref string LaTeXCode)
        {
            if (row1 == row2)
                return;
            DrawMatrixToLatex(ref matrix, ref LaTeXCode);
            Console.WriteLine("Swapping rows {0} and {1}", row1 + 1, row2 + 1);

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                double temp = matrix[x, row1];
                matrix[x, row1] = matrix[x,row2];
                matrix[x, row2] = temp;
            }
            LaTeXCode += String.Format("Swap rows {0} and {1}\n", row1 + 1, row2 + 1);
        }
        static void MultiplyRow(int row, double factor, ref double[,] matrix, ref string LaTeXCode)
        {
            if (factor==1)
                return;
            LaTeXCode += String.Format("Multiply row {0} by {1}\n", row + 1, factor);
            Console.WriteLine("Multiplying row {0} by factor {1}", row+1, factor);
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                matrix[x, row] *= factor;
            }
            DrawMatrixToLatex(ref matrix, ref LaTeXCode);
        }
        static string repeatString(string toRepeat, int numTimes)
        {
            string result = "";
            for (int i = 0; i < numTimes; i++)
                result += toRepeat;
            return result;
        }
        static void DrawMatrixToLatex(ref double[,] matrix, ref string LaTeXCode)
        {
            int w=matrix.GetLength(0);
            int h = matrix.GetLength(1);
            LaTeXCode += @"\begin{equation}
\left\{
    \begin{array}{" + repeatString("c", w-1)+"|cl" + @"}";

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    LaTeXCode += matrix[x, y].ToString() + "&";
                }
                LaTeXCode += "\\\\";
            }

            LaTeXCode += @"\end{array}
\right\}
\end{equation}";

        }
        static void AddMultiplesOfRow(int row1, int row2, double factor, ref double[,] matrix, ref string LaTeXCode)
        {
            LaTeXCode += string.Format("Add {0} times row {1} to row {2}\n", factor, row1 + 1, row2 + 1);
            Console.WriteLine("Add {0} times row {1} to row {2}", factor, row1+1, row2+1);
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                matrix[x, row2] += matrix[x, row1] * factor;
            }
            DrawMatrixToLatex(ref matrix, ref LaTeXCode);
        }
        static string LatexHeader=@"\documentclass[11pt]{article} % use larger type; default would be 10pt
\usepackage[utf8]{inputenc} % set input encoding (not needed with XeLaTeX)
\usepackage{geometry} % to change the page dimensions
\usepackage{amssymb,amsmath}

\title{Solution to your matrix}
\author{autogenerated by Nathan Jervis' Matrix Solver}

\begin{document}
\maketitle
";
        static string LatexFooter = @"\end{document}";
        static void Main(string[] args)
        {
            string LaTeXCode = LatexHeader;
            Console.WriteLine("Enter x, space y then press enter for a matrix of that size. Ex: 4 4");
            string input = Console.ReadLine();
            int w = int.Parse(input.Split(' ')[0]);
            int l = int.Parse(input.Split(' ')[1]);
            double[,] matrix = new double[w, l];

            Console.WriteLine("Enter your matrix, with columns seperated with spaces, and rows seperated by newlines, ex:");
            Console.WriteLine("1 2 3");
            Console.WriteLine("3 5 6\n\n");
            for (int y = 0; y < l; y++)
            {
                string[] xs = Console.ReadLine().Split(' ');
                for (int x = 0; x < w; x++)
                {
                    matrix[x, y] = double.Parse(xs[x]);
                }
            }
            LaTeXCode += "The original matrix as entered by the user:\n";
            DrawMatrixToLatex(ref matrix, ref LaTeXCode);

            int firstRow = 0;
            for (int x = 0; x < w-1; x++)
            {
                //reduce it
                //step 1, ensure that the first non-zero row is the first row (or there are no non-zero rows)
                int nonZeroRow = -1;
                for (int y = firstRow; y < l; y++)
                {
                    if (matrix[x, y] != 0)
                    {
                        nonZeroRow = y;
                        break;
                    }
                }
                if (nonZeroRow == -1)
                    continue;
                SwapRows(firstRow, nonZeroRow, ref matrix, ref LaTeXCode);

                //step 2, divide the first row so that it starts with a 1
                double factor = 1 / matrix[x, firstRow];
                MultiplyRow(x, factor, ref matrix, ref LaTeXCode);

                //step 3, add multiples of the first row to other rows until only the first row starts with a non-zero value
                for (int y = 0; y < l; y++)
                {
                    if (y == firstRow)
                        continue;
                    if (matrix[x, y] != 0)
                    {
                        double multFactor = -matrix[x, y] / matrix[x, firstRow];
                        AddMultiplesOfRow(firstRow, y, multFactor, ref matrix, ref LaTeXCode);
                    }
                }

                //step 4, cover up the first row and repeat with the rest of the rows
                firstRow++;
            }
            LaTeXCode += "The final reduced matrix:\n";
            DrawMatrixToLatex(ref matrix, ref LaTeXCode);
            
            LaTeXCode += LatexFooter;
            System.IO.File.WriteAllText("output.ltx", LaTeXCode);


            Console.WriteLine("\n\nYour reduced matrix:");
            for (int y = 0; y < l; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Console.Write("{0} ", matrix[x,y]);
                }
                Console.WriteLine();
            }
            //stage 2: uncover covered rows
            Console.ReadKey();
        }
    }
}
