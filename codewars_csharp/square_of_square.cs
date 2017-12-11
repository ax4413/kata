using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace codewars_csharp
{
    [TestFixture]
    [Ignore("Tests take too long")]
    public class square_of_square
    {
        [TestCase(11, "1 2 4 10")]
        [TestCase(20, "1 2 3 5 19")]
        [TestCase(50, "1 3 5 8 49")]
        [TestCase(50000, "2 3 7 9 316 49999")]
        public void square_of_square_test(int i, string expected)
        {
            var sos = new Decompose();
            var actual = sos.decompose(i);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    public class Decompose
    {
        public string decompose(long i)
        {
            var masterSquare = Math.Pow(i, 2);

            var squareRange = Enumerable.Range(1, (int) i - 1)
                .Reverse()
                .Select(x => Math.Pow(x, 2))
                .ToList();

            return FindSquareOfSquares(squareRange, masterSquare);
        }

        private static string FindSquareOfSquares(List<double>numbers, double target)
        {
            var results = sum_up(numbers, target);
            var resultStrings = results
                .Select(o =>
                    o.OrderBy(i => i)
                        .Select(i =>
                            Math.Sqrt(i))
                        .Aggregate("", (current, next) => current + " " + next)
                        .Trim())
                .Distinct()
                .ToList();

            return resultStrings.Take(1).Single();
        }

        private static List<List<double>> sum_up(List<double> numbers, double target)
        {
            var results = new List<List<double>>();
            sum_up_recursive(numbers, target, new List<double>(), results);

            return results;
        }

        private static bool sum_up_recursive(List<double> numbers, double target, List<double> partial, List<List<double>> results)
        {
            var s = partial.Sum();

            if (s == target)
            {
                results.Add(partial);
                return true;
            }

            if (s > target)
                return false;

            for (var i = 0; i < numbers.Count; i++)
            {
                var remaining = new List<double>();
                var n = numbers[i];

                for (var j = i + 1; j < numbers.Count; j++)
                    remaining.Add(numbers[j]);

                var partialRec = new List<double>(partial) {n};

                if(sum_up_recursive(remaining, target, partialRec, results))
                    return true;
            }
            return false;
        }
    }
}

