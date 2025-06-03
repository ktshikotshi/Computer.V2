using Xunit;
using Computer.V2.Lib;
using Computer.V2.Lib.Exceptions;

namespace Computer.V2.Tests
{
    public class MathsTests
    {
        [Theory]
        [InlineData("2+3", "5")]
        [InlineData("10-4", "6")]
        [InlineData("3*4", "12")]
        [InlineData("15/3", "5")]
        public void Calculate_BasicOperations_ReturnsCorrectResult(string expression, string expected)
        {
            string result = Maths.Calculate(expression);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2^3", "8")]
        [InlineData("4^2", "16")]
        [InlineData("5^0", "1")]
        public void Calculate_PowerOperations_ReturnsCorrectResult(string expression, string expected)
        {
            string result = Maths.Calculate(expression);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Calculate_WithBrackets_ReturnsCorrectResult()
        {
            string expression = "(2+3)*4";
            string result = Maths.Calculate(expression);
            Assert.Equal("20", result);
        }

        [Fact]
        public void Calculate_ComplexExpression_ReturnsCorrectResult()
        {
            string expression = "2*(3+4)-5";
            string result = Maths.Calculate(expression);
            Assert.Equal("9", result);
        }

        [Fact]
        public void Calculate_DivisionByZero_ThrowsInvalidExpressionException()
        {
            Assert.Throws<InvalidExpressionException>(() => Maths.Calculate("10/0"));
        }

        [Fact]
        public void Calculate_InvalidPowerFormat_ThrowsInvalidExpressionException()
        {
            Assert.Throws<InvalidExpressionException>(() => Maths.Calculate("2^-1"));
        }

        [Fact]
        public void Calculate_DoubleAsterisk_ThrowsInvalidExpressionException()
        {
            Assert.Throws<InvalidExpressionException>(() => Maths.Calculate("2**3"));
        }

        [Theory]
        [InlineData("2*i", "2*i")]
        [InlineData("3*i+4*i", "7*i")]
        [InlineData("5*i-2*i", "3*i")]
        public void Calculate_ImaginaryNumbers_ReturnsCorrectResult(string expression, string expected)
        {
            string result = Maths.Calculate(expression);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("i^0", "1")]
        [InlineData("i^1", "1*i")]
        [InlineData("i^2", "-1")]
        [InlineData("i^3", "-1*i")]
        [InlineData("i^4", "1")]
        public void Calculate_ImaginaryPowers_ReturnsCorrectResult(string expression, string expected)
        {
            string result = Maths.Calculate(expression);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 2)]
        [InlineData(9, 3)]
        [InlineData(16, 4)]
        [InlineData(25, 5)]
        [InlineData(0, 0)]
        public void Sqrt_ValidInput_ReturnsCorrectResult(double input, double expected)
        {
            double result = Maths.Sqrt(input);
            Assert.Equal(expected, result, 4);
        }

        [Fact]
        public void Sqrt_NegativeInput_ReturnsInput()
        {
            double input = -4;
            double result = Maths.Sqrt(input);
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData("10%3", "1")]
        [InlineData("15%4", "3")]
        [InlineData("20%5", "0")]
        public void Calculate_ModuloOperation_ReturnsCorrectResult(string expression, string expected)
        {
            string result = Maths.Calculate(expression);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Calculate_NestedBrackets_ReturnsCorrectResult()
        {
            string expression = "((2+3)*4)-5";
            string result = Maths.Calculate(expression);
            Assert.Equal("15", result);
        }

        [Theory]
        [InlineData("2.5+1.5", "4")]
        [InlineData("3.7*2", "7.4")]
        [InlineData("10.5/2.1", "5")]
        public void Calculate_DecimalNumbers_ReturnsCorrectResult(string expression, string expected)
        {
            string result = Maths.Calculate(expression);
            Assert.Equal(expected, result);
        }
    }
}