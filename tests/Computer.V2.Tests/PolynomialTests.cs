using Xunit;
using Computer.V2.Lib;

namespace Computer.V2.Tests
{
    public class PolynomialTests
    {
        [Fact]
        public void PolySolve_QuadraticWithTwoRealSolutions_ReturnsCorrectSolutions()
        {
            // Arrange
            var polynomial = new Polynomial("x^2 - 5*x + 6 = 0");
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains("Solutions on R", result);
            Assert.Contains("2", result);
            Assert.Contains("3", result);
        }

        [Fact]
        public void PolySolve_QuadraticWithComplexSolutions_ReturnsComplexSolutions()
        {
            // Arrange
            var polynomial = new Polynomial("x^2 + 1 = 0");
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains("Solutions C", result);
            Assert.Contains("i", result);
        }

        [Fact]
        public void PolySolve_QuadraticWithOneSolution_ReturnsOneSolution()
        {
            // Arrange
            var polynomial = new Polynomial("x^2 - 2*x + 1 = 0");
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains("A solution on R", result);
            Assert.Contains("1", result);
        }

        [Fact]
        public void PolySolve_LinearEquation_ReturnsCorrectSolution()
        {
            // Arrange
            var polynomial = new Polynomial("2*x - 4 = 0");
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains("Solution on R", result);
            Assert.Contains("2", result);
        }

        [Fact]
        public void PolySolve_LinearEquationWithZeroCoefficient_ReturnsUndefined()
        {
            // Arrange
            var polynomial = new Polynomial("0*x + 5 = 0");
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains("undefined", result);
        }

        [Fact]
        public void PolySolve_ConstantEquationWithZero_ReturnsAllRealNumbers()
        {
            // Arrange
            var polynomial = new Polynomial("0 = 0");
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains("All real numbers", result);
        }

        [Fact]
        public void PolySolve_ConstantEquationWithNonZero_ReturnsNoSolution()
        {
            // Arrange
            var polynomial = new Polynomial("5 = 0");
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains("no solutuins", result);
        }

        [Fact]
        public void PolySolve_HighDegreePolynomial_ReturnsCannotSolve()
        {
            // Arrange
            var polynomial = new Polynomial("x^3 + x^2 + x + 1 = 0");
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains("degree is stricly greater than 2", result);
            Assert.Contains("can't solve", result);
        }

        [Theory]
        [InlineData("x^2 - 4 = 0", "2", "-2")]
        [InlineData("x^2 - 9 = 0", "3", "-3")]
        [InlineData("2*x^2 - 8 = 0", "2", "-2")]
        public void PolySolve_QuadraticEquations_ReturnsExpectedSolutions(string equation, string solution1, string solution2)
        {
            // Arrange
            var polynomial = new Polynomial(equation);
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains(solution1, result);
            Assert.Contains(solution2, result);
        }

        [Fact]
        public void PolySolve_NaturalFormConversion_WorksCorrectly()
        {
            // Arrange
            var polynomial = new Polynomial("x*x - 3*x + 2 = 0");
            
            // Act
            polynomial.PolySolve();
            string result = polynomial.GetOut();
            
            // Assert
            Assert.Contains("x^2", result.ToLower());
            Assert.Contains("Solutions on R", result);
        }
    }
}