using Xunit;
using Computer.V2.Lib;
using Computer.V2.Lib.Exceptions;

namespace Computer.V2.Tests
{
    public class FunctionsTests
    {
        [Fact]
        public void NormaliseFunc_SimplePolynomial_ReturnsNormalizedForm()
        {
            // Arrange
            string expression = "2*x^2 + 3*x + 1";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            Assert.Contains("2*x^2", result);
            Assert.Contains("3*x", result);
            Assert.Contains("1", result);
        }

        [Fact]
        public void NormaliseFunc_PolynomialWithSameTerms_CombinesTerms()
        {
            // Arrange
            string expression = "x^2 + 2*x^2 + 3*x";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            Assert.Contains("3*x^2", result);
            Assert.Contains("3*x", result);
        }

        [Fact]
        public void NormaliseFunc_PolynomialWithNegativeTerms_HandlesNegatives()
        {
            // Arrange
            string expression = "x^2 - 2*x + 1";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            Assert.Contains("1*x^2", result);
            Assert.Contains("-2*x", result);
            Assert.Contains("1", result);
        }

        [Fact]
        public void NormaliseFunc_PolynomialWithZeroCoefficient_ExcludesZeroTerms()
        {
            // Arrange
            string expression = "0*x^2 + 3*x + 5";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            Assert.DoesNotContain("0*x^2", result);
            Assert.Contains("3*x", result);
            Assert.Contains("5", result);
        }

        [Fact]
        public void NormaliseFunc_ConstantExpression_ReturnsConstant()
        {
            // Arrange
            string expression = "42";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            Assert.Equal("42", result);
        }

        [Fact]
        public void NormaliseFunc_ExpressionWithoutVariables_CalculatesResult()
        {
            // Arrange
            string expression = "2 + 3 * 4";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            Assert.Equal("14", result);
        }

        [Fact]
        public void NormaliseFunc_PolynomialWithImplicitCoefficients_AddsCoefficients()
        {
            // Arrange
            string expression = "x^2 + x + 1";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            Assert.Contains("1*x^2", result);
            Assert.Contains("1*x", result);
        }

        [Fact]
        public void NormaliseFunc_HighDegreePolynomial_OrdersTermsCorrectly()
        {
            // Arrange
            string expression = "1 + x + x^3 + x^2";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            int posX3 = result.IndexOf("x^3");
            int posX2 = result.IndexOf("x^2");
            int posX = result.IndexOf("*x");

            Assert.True(posX3 < posX2);
            Assert.True(posX2 < posX);
        }

        [Fact]
        public void NormaliseFunc_InvalidPowerFormat_ThrowsInvalidExpressionException()
        {
            // Arrange
            string expression = "x^-1 + 2";

            // Act & Assert
            Assert.Throws<InvalidExpressionException>(() => Functions.NormaliseFunc(expression));
        }

        [Theory]
        [InlineData("2*x + 3", "2*x+3")]
        [InlineData("x^2 - 5*x + 6", "1*x^2-5*x+6")]
        [InlineData("-x^2 + 4", "-1*x^2+4")]
        public void NormaliseFunc_VariousPolynomials_ReturnsExpectedForm(string input, string expected)
        {
            // Act
            string result = Functions.NormaliseFunc(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NormaliseFunc_PolynomialWithMatrixElements_PreservesMatrixElements()
        {
            // Arrange
            string expression = "x^2 + [1,2]\n[3,4]";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            Assert.Contains("x^2", result);
            Assert.Contains("[1,2]", result);
            Assert.Contains("[3,4]", result);
        }

        [Fact]
        public void NormaliseFunc_PolynomialWithImaginaryNumbers_PreservesImaginaryParts()
        {
            // Arrange
            string expression = "x^2 + 2*i";

            // Act
            string result = Functions.NormaliseFunc(expression);

            // Assert
            Assert.Contains("x^2", result);
            Assert.Contains("2*i", result);
        }
    }
}