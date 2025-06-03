using Xunit;
using Computer.V2.Lib;
using Computer.V2.Lib.Exceptions;

namespace Computer.V2.Tests
{
    public class ParserTests
    {
        [Fact]
        public void Parse_VariableAssignment_ShouldStoreVariable()
        {
            // Arrange
            var parser = new Parser();
            // Act
            parser.Parse("x = 10");
            var result = parser.Evaluate("x");
            // Assert
            Assert.Equal("10", result);
        }

        [Fact]
        public void Parse_FunctionAssignment_ShouldStoreFunction()
        {
            // Arrange
            var parser = new Parser();
            // Act
            parser.Parse("f(x) = x * x");
            var result = parser.Evaluate("f(5)");
            // Assert
            Assert.Equal("25", result);
        }

        [Fact]
        public void Substitute_VariablesInExpression_ShouldReturnExpressionWithValues()
        {
            // Arrange
            var parser = new Parser();
            parser.Parse("x = 3");
            // Act
            var substituted = parser.Substitute("x + 2");
            // Assert
            Assert.Equal("3 + 2", substituted);
        }

        [Fact]
        public void Parse_UnknownVariable_ShouldThrowInvalidVariableException()
        {
            // Arrange
            var parser = new Parser();
            // Act & Assert
            Assert.Throws<InvalidVariableException>(() => parser.Evaluate("y + 2"));
        }
    }
}