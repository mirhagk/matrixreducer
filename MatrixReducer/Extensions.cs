using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixReducer
{
    public static class Extensions
    {
        public static Matrix ReadMatrix(this System.IO.TextReader reader)
        {
            string result = reader.ReadLine();
            int numRows = int.Parse(result.Split(' ')[0]);
            for (int i = 0; i < numRows; i++)
            {
                result += "\n" + reader.ReadLine();
            }
            return Matrix.FromString(result);
        }
        public static Vector ReadVector(this System.IO.TextReader reader)
        {
            string[] input = reader.ReadLine().Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
            Vector result = new Vector(input.Length);
            for (int i = 0; i < result.Length; i++)
            {
                result[i+1] = double.Parse(input[i]);
            }
            return result;
        }
        public static string RepeatString(this string toRepeat, int numTimes)
        {
            string result = "";
            for (int i = 0; i < numTimes; i++)
                result += toRepeat;
            return result;
        }
    }
}
