using System;
using NUnit.Framework;

namespace codewars_csharp
{
    [TestFixture]
    public class BinarySumTest
    {
        [TestCase(1, 2, "11")]
        [TestCase(2, 5, "111")]
        [TestCase(9, 5, "1110")]
        [TestCase(100, 576, "1010100100")]
        public void BinarySumTester(int a, int b, string expected)
        {
            var actual = BinarySum(a, b);
            Assert.That(actual, Is.EqualTo(expected));
        }

        public static string BinarySum(int a, int b)
        {
            return Convert.ToString(a + b, 2);
        }
    }
}
