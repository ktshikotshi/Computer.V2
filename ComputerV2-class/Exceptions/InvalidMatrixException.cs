using System;

namespace ComputerV2_class.Exceptions
{
    public class InvalidMatrixException : Exception
    {
        public InvalidMatrixException(string message) : base(message) { }

    }
}
