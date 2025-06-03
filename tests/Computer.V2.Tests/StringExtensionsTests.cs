using Xunit;
using Computer.V2.Lib.Extensions;
using Computer.V2.Lib.Exceptions;

namespace Computer.V2.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("2 + 3")]
        [InlineData("x^2 - 5*x + 6")]
        [InlineData("(2 + 3) * 4")]
        [InlineData("[1,2]\n[3,4]")]
        public void Validate_ValidExpressions_DoesNotThrow(string expression)
        {
            var exception = Record.Exception(() => expression.Validate());
            Assert.Null(exception);
        }

        [Fact]
        public void Validate_MismatchedBrackets_ThrowsInvalidExpressionException()
        {
            string expression = "(2 + 3";
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }

        [Fact]
        public void Validate_MismatchedSquareBrackets_ThrowsInvalidExpressionException()
        {
            string expression = "[1,2,3";
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }

        [Fact]
        public void Validate_TooManyEqualSigns_ThrowsInvalidExpressionException()
        {
            string expression = "x == 5";
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }

        [Fact]
        public void Validate_TooManyQuestionMarks_ThrowsInvalidExpressionException()
        {
            string expression = "x ??";
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }

        [Fact]
        public void Validate_InvalidOperatorCombination_ThrowsInvalidExpressionException()
        {
            string expression = "2 +- 3";
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }

        [Fact]
        public void Validate_RepeatingOperators_ThrowsInvalidExpressionException()
        {
            string expression = "2 ++ 3";
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }

        [Fact]
        public void Validate_InvalidVariableUsage_ThrowsInvalidExpressionException()
        {
            string expression = "x y";
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }

        [Fact]
        public void Validate_EmptyQuery_ThrowsInvalidExpressionException()
        {
            string expression = "=?";
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }

        [Fact]
        public void Validate_EmptyAssignment_ThrowsInvalidExpressionException()
        {
            string expression = "x =";
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }

        [Theory]
        [InlineData("2+3-1", new[] { "2", "+", "3", "-", "1" })]
        [InlineData("x=5", new[] { "x", "=", "5" })]
        [InlineData("a-b+c", new[] { "a", "-", "b", "+", "c" })]
        public void SplitExpression_ValidExpression_ReturnsSplitArray(string expression, string[] expected)
        {
            string[] result = expression.SplitExpression();
            Assert.Equal(expected.Length, result.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        [Fact]
        public void SplitExpression_ExpressionWithDecimalPoints_ConvertsToCommas()
        {
            string expression = "2.5+3.7";
            string[] result = expression.SplitExpression();
            Assert.Contains("2,5", result);
            Assert.Contains("3,7", result);
        }

        [Fact]
        public void SplitExpression_RemovesSpaces_ReturnsCleanArray()
        {
            string expression = " 2 + 3 ";
            string[] result = expression.SplitExpression();
            Assert.All(result, item => Assert.DoesNotContain(" ", item));
        }

        [Theory]
        [InlineData("((2+3))")]
        [InlineData("[[1,2],[3,4]]")]
        [InlineData("(([1,2]))")]
        public void Validate_NestedBrackets_DoesNotThrow(string expression)
        {
            var exception = Record.Exception(() => expression.Validate());
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("(]")]
        [InlineData("[)")]
        [InlineData("([)]")]
        public void Validate_MismatchedBracketTypes_ThrowsInvalidExpressionException(string expression)
        {
            Assert.Throws<InvalidExpressionException>(() => expression.Validate());
        }
    }
}