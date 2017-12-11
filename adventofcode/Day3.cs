using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace adventofcode
{
    [TestFixture]
    class Day3
    {
        [TestCase(10, 5, 2)]
        [TestCase(123, 10, 3)]
        [TestCase(123, 21, 4)]
        [TestCase(123, 51, 6)]
        [TestCase(325489, 325489, 552)]
        public void test_day_3_part_3(int input, int value, int expected)
        {
            var matrix = GenerateCenterOutSpiral(input);
            var actual = GetManhatanDistance(matrix, value);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(100, 1, 2)]
        [TestCase(100, 2, 4)]
        [TestCase(100, 5, 10)]
        [TestCase(100, 10, 11)]
        [TestCase(100, 21, 23)]
        [TestCase(100, 51, 54)]
        [TestCase(325489, 325489, 330785)]
        public void test_day_3_part_2(int input, int value, int expected)
        {
            var actual = CalculateFirstCellValueGreaterThanValue(input, value);
            Assert.That(actual, Is.EqualTo(expected));
        }

        private int GetManhatanDistance(int[,] matrix, int value)
        {
            var start = matrix.CoordinatesOf(1);
            var end = matrix.CoordinatesOf(value);

            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }

        private int[,] GenerateCenterOutSpiral(int input)
        {
            // +2 should stop out of bounds exceptions
            var size = (int) Math.Ceiling(Math.Sqrt(input)) + 2;

            var zero = (int) (size / 2.0);
            var matrix = new int[size, size];
            var cell = new Vector() {X = zero, Y = zero};
            matrix[cell.Y, cell.X] = 1;

            for (int i = 2; i <= input; i++)
            {
                var l = cell.Left(1);
                var r = cell.Right(1);
                var u = cell.Up(1);
                var d = cell.Down(1);
                var lu = cell.Left(1).Up(1);
                var ld = cell.Left(1).Down(1);
                var ru = cell.Right(1).Up(1);
                var rd = cell.Right(1).Down(1);

                /*   | | | | | |
                 *   | | | | | |
                 *   | | |1|*| |
                 *   | | | | | |
                 * y | | | | | |
                 *    x
                 */
                if (i == 2)
                    cell = r;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      
                 *   | | | | | | |      | | | |?| | |       | | | |*| | |      
                 *   | | |1|2|?| |      | | |1|2| | |       | | |1|2| | |      
                 *   | | | | | | |      | | | | | | |       | | | | | | |      
                 * y | | | | | | |      | | | | | | |       | | | | | | |      
                 *    x
                 */
                else if (matrix[l.Y, l.X] != 0 && matrix[u.Y, u.X] == 0)
                    cell = u;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      
                 *   | | |?|3| | |      | | | |3| | |       | | |*|3| | |      
                 *   | | |1|2| | |      | | |!|2| | |       | | |1|2| | |      
                 *   | | | | | | |      | | | | | | |       | | | | | | |      
                 * y | | | | | | |      | | | | | | |       | | | | | | |      
                 *    x
                 */
                else if (matrix[l.Y, l.X] == 0 && matrix[ld.Y, ld.X] != 0)
                    cell = l;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      
                 *   | |?|4|3| | |      | | |4|3| | |       | |*|4|3| | |      
                 *   | | |1|2| | |      | | |!|2| | |       | | |1|2| | |      
                 *   | | | | | | |      | | | | | | |       | | | | | | |      
                 * y | | | | | | |      | | | | | | |       | | | | | | |      
                 *    x
                 */
                else if (matrix[l.Y, l.X] == 0 && matrix[d.Y, d.X] != 0)
                    cell = l;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 *   |?|5|4|3| | |      | |5|4|3| | |       | |5|4|3| | |      | |5|4|3| | |
                 *   | | |1|2| | |      | |?|1|2| | |       | | |!|2| | |      | |*|1|2| | |
                 *   | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 * y | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 *    x
                 */
                else if (matrix[l.Y, l.X] == 0 && matrix[d.Y, d.X] == 0 && matrix[rd.Y, rd.X] != 0)
                    cell = d;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 *   | |5|4|3| | |      | |5|4|3| | |       | |5|4|3| | |      | |5|4|3| | |
                 *   |?|6|1|2| | |      | |6|1|2| | |       | |6|1|2| | |      | |6|1|2| | |
                 *   | | | | | | |      | |?| | | | |       | | |?| | | |      | |*| | | | |
                 * y | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 *    x
                 */
                else if (matrix[l.Y, l.X] == 0 && matrix[d.Y, d.X] == 0
                         && matrix[rd.Y, rd.X] == 0
                         && matrix[r.Y, r.X] != 0)
                    cell = d;

                else
                    cell = r;

                matrix[cell.Y, cell.X] = i;
            }
            return matrix;
        }

        private int CalculateFirstCellValueGreaterThanValue(int input, int maxValue)
        {
            // +2 should stop out of bounds exceptions
            var size = (int)Math.Ceiling(Math.Sqrt(input)) + 2;

            var zero = (int)(size / 2.0);
            var matrix = new int[size, size];
            var currentValue = 0;
            var cell = new Vector() { X = zero, Y = zero };
            matrix[cell.Y, cell.X] = 1;

            for (int i = 2; i <= input; i++)
            {
                var l = cell.Left(1);
                var r = cell.Right(1);
                var u = cell.Up(1);
                var d = cell.Down(1);
                var lu = cell.Left(1).Up(1);
                var ld = cell.Left(1).Down(1);
                var ru = cell.Right(1).Up(1);
                var rd = cell.Right(1).Down(1);

                /*   | | | | | |
                 *   | | | | | |
                 *   | | |1|*| |
                 *   | | | | | |
                 * y | | | | | |
                 *    x
                 */
                if (i == 2)
                    cell = r;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      
                 *   | | | | | | |      | | | |?| | |       | | | |*| | |      
                 *   | | |1|2|?| |      | | |1|2| | |       | | |1|2| | |      
                 *   | | | | | | |      | | | | | | |       | | | | | | |      
                 * y | | | | | | |      | | | | | | |       | | | | | | |      
                 *    x
                 */
                else if (matrix[l.Y, l.X] != 0 && matrix[u.Y, u.X] == 0)
                    cell = u;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      
                 *   | | |?|3| | |      | | | |3| | |       | | |*|3| | |      
                 *   | | |1|2| | |      | | |!|2| | |       | | |1|2| | |      
                 *   | | | | | | |      | | | | | | |       | | | | | | |      
                 * y | | | | | | |      | | | | | | |       | | | | | | |      
                 *    x
                 */
                else if (matrix[l.Y, l.X] == 0 && matrix[ld.Y, ld.X] != 0)
                    cell = l;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      
                 *   | |?|4|3| | |      | | |4|3| | |       | |*|4|3| | |      
                 *   | | |1|2| | |      | | |!|2| | |       | | |1|2| | |      
                 *   | | | | | | |      | | | | | | |       | | | | | | |      
                 * y | | | | | | |      | | | | | | |       | | | | | | |      
                 *    x
                 */
                else if (matrix[l.Y, l.X] == 0 && matrix[d.Y, d.X] != 0)
                    cell = l;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 *   |?|5|4|3| | |      | |5|4|3| | |       | |5|4|3| | |      | |5|4|3| | |
                 *   | | |1|2| | |      | |?|1|2| | |       | | |!|2| | |      | |*|1|2| | |
                 *   | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 * y | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 *    x
                 */
                else if (matrix[l.Y, l.X] == 0 && matrix[d.Y, d.X] == 0 && matrix[rd.Y, rd.X] != 0)
                    cell = d;

                /*   | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 *   | |5|4|3| | |      | |5|4|3| | |       | |5|4|3| | |      | |5|4|3| | |
                 *   |?|6|1|2| | |      | |6|1|2| | |       | |6|1|2| | |      | |6|1|2| | |
                 *   | | | | | | |      | |?| | | | |       | | |?| | | |      | |*| | | | |
                 * y | | | | | | |      | | | | | | |       | | | | | | |      | | | | | | |
                 *    x
                 */
                else if (matrix[l.Y, l.X] == 0 && matrix[d.Y, d.X] == 0
                         && matrix[rd.Y, rd.X] == 0
                         && matrix[r.Y, r.X] != 0)
                    cell = d;

                else
                    cell = r;

                currentValue = matrix.GetSumOfSurroundingSquares(cell);
                
                matrix[cell.Y, cell.X] = currentValue;
                
                if (currentValue > maxValue)
                    return currentValue;
            }
            return currentValue;

        }
    }

    public class Vector
    {
        /// <summary>
        /// Row
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Column
        /// </summary>
        public int X { get; set; }

        public Vector Up(int i) => new Vector() {X = this.X, Y = this.Y - i};
        public Vector Down(int i) => new Vector() { X = this.X, Y = this.Y + i };
        public Vector Left(int i) => new Vector() { X = this.X - 1, Y = this.Y };
        public Vector Right(int i) => new Vector() { X = this.X + 1, Y = this.Y };
    }

    public static class ArrayUtils
    {
        public static Vector CoordinatesOf<T>(this T[,] matrix, T value)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return new Vector() {X = x, Y = y};
                }
            }

            return new Vector() { X = -1, Y = -1 };
        }

        public static int GetSumOfSurroundingSquares(this int[,] matrix, int value)
        {
            var c = matrix.CoordinatesOf(value);
            return matrix.GetSumOfSurroundingSquares(c);
        }

        public static int GetSumOfSurroundingSquares(this int[,] matrix, Vector cell)
        {
            var adjesentCells = new List<Vector>
            {
                cell.Left(1),
                cell.Right(1),
                cell.Up(1),
                cell.Down(1),
                cell.Left(1).Up(1),
                cell.Left(1).Down(1),
                cell.Right(1).Up(1),
                cell.Right(1).Down(1)
            };
            return adjesentCells.Sum(v => matrix[v.Y, v.X]);
        }
    }
}

