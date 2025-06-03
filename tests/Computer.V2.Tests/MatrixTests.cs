using Xunit;
using Computer.V2.Lib;
using Computer.V2.Lib.Exceptions;

namespace Computer.V2.Tests
{
    public class MatrixTests
    {
        [Fact]
        public void MatrixManipulation_WithValidMatrix_ReturnsTrue()
        {
            // Arrange
            string validMatrix = "[1,2,3]\n[4,5,6]";

            // Act
            var result = Matrix.MatrixManipulation(validMatrix);

            // Assert
            Assert.True(result.Found);
            Assert.Contains("[", result.Value);
        }

        [Fact]
        public void MatrixManipulation_WithoutMatrix_ReturnsFalse()
        {
            // Arrange
            string nonMatrix = "2 + 3";

            // Act
            var result = Matrix.MatrixManipulation(nonMatrix);

            // Assert
            Assert.False(result.Found);
            Assert.Equal(nonMatrix, result.Value);
        }

        [Fact]
        public void MatrixManipulation_WithScalarMultiplication_ReturnsCorrectResult()
        {
            // Arrange
            string scalarMatrix = "2*[1,2]\n[3,4]";

            // Act
            var result = Matrix.MatrixManipulation(scalarMatrix);

            // Assert
            Assert.True(result.Found);
            Assert.Contains("2", result.Value);
            Assert.Contains("4", result.Value);
        }

        [Fact]
        public void MatrixManipulation_WithInvalidMatrix_ThrowsInvalidMatrixException()
        {
            // Arrange
            string invalidMatrix = "[1,abc,3]\n[4,5,6]";

            // Act & Assert
            Assert.Throws<InvalidMatrixException>(() => Matrix.MatrixManipulation(invalidMatrix));
        }

        [Fact]
        public void MatrixManipulation_WithMatrixMultiplication_ReturnsCorrectResult()
        {
            // Arrange
            string matrixMultiplication = "[1,2]*[3]\n[4]";

            // Act
            var result = Matrix.MatrixManipulation(matrixMultiplication);

            // Assert
            Assert.True(result.Found);
            // Matrix multiplication: [1,2] * [3;4] = [1*3+2*4] = [11]
            Assert.Contains("11", result.Value);
        }

        [Fact]
        public void MatrixManipulation_WithIncompatibleMatrices_ThrowsInvalidMatrixException()
        {
            // Arrange
            string incompatibleMatrices = "[1,2,3]*[1]\n[2]";

            // Act & Assert
            Assert.Throws<InvalidMatrixException>(() => Matrix.MatrixManipulation(incompatibleMatrices));
        }

        [Fact]
        public void MatrixManipulation_WithMissingSemicolon_ThrowsInvalidMatrixException()
        {
            // Arrange
            string invalidFormat = "[1,2][3,4]";

            // Act & Assert
            Assert.Throws<InvalidMatrixException>(() => Matrix.MatrixManipulation(invalidFormat));
        }

        [Theory]
        [InlineData("[1]")]
        [InlineData("[1,2]\n[3,4]")]
        [InlineData("[1,2,3]\n[4,5,6]\n[7,8,9]")]
        public void MatrixManipulation_WithValidMatrixFormats_ReturnsTrue(string matrix)
        {
            // Act
            var result = Matrix.MatrixManipulation(matrix);

            // Assert
            Assert.True(result.Found);
        }
    }
}