using Xunit;
using Computer.V2.Lib.Exceptions;
using System;

namespace Computer.V2.Tests
{
    public class ExceptionTests
    {
        [Fact]
        public void InvalidExpressionException_WithMessage_SetsMessageCorrectly()
        {
            // Arrange
            string expectedMessage = "Test error message";
            
            // Act
            var exception = new InvalidExpressionException(expectedMessage);
            
            // Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.IsAssignableFrom<ArgumentException>(exception);
        }

        [Fact]
        public void InvalidMatrixException_WithMessage_SetsMessageCorrectly()
        {
            // Arrange
            string expectedMessage = "Invalid matrix format";
            
            // Act
            var exception = new InvalidMatrixException(expectedMessage);
            
            // Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void InvalidVariableException_DefaultConstructor_SetsDefaultMessage()
        {
            // Act
            var exception = new InvalidVariableException();
            
            // Assert
            Assert.Equal("Invalid Use of Variables.", exception.Message);
            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void InvalidExpressionException_CanBeThrown()
        {
            // Arrange
            string message = "Expression is invalid";
            
            // Act & Assert
            Assert.Throws<InvalidExpressionException>(() => 
            {
                throw new InvalidExpressionException(message);
            });
        }

        [Fact]
        public void InvalidMatrixException_CanBeThrown()
        {
            // Arrange
            string message = "Matrix is invalid";
            
            // Act & Assert
            Assert.Throws<InvalidMatrixException>(() => 
            {
                throw new InvalidMatrixException(message);
            });
        }

        [Fact]
        public void InvalidVariableException_CanBeThrown()
        {
            // Act & Assert
            Assert.Throws<InvalidVariableException>(() => 
            {
                throw new InvalidVariableException();
            });
        }

        [Fact]
        public void InvalidExpressionException_InheritsFromArgumentException()
        {
            // Arrange
            var exception = new InvalidExpressionException("test");
            
            // Act & Assert
            Assert.IsType<InvalidExpressionException>(exception);
            Assert.IsAssignableFrom<ArgumentException>(exception);
        }

        [Fact]
        public void InvalidMatrixException_InheritsFromException()
        {
            // Arrange
            var exception = new InvalidMatrixException("test");
            
            // Act & Assert
            Assert.IsType<InvalidMatrixException>(exception);
            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void InvalidVariableException_InheritsFromException()
        {
            // Arrange
            var exception = new InvalidVariableException();
            
            // Act & Assert
            Assert.IsType<InvalidVariableException>(exception);
            Assert.IsAssignableFrom<Exception>(exception);
        }
    }
}