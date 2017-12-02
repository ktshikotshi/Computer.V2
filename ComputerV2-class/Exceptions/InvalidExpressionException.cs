using System;

namespace ComputerV2_class.Exceptions
{
    public class InvalidExpressionException : Exception
    {
        public InvalidExpressionException(string message) : base(message) { }
    }
}
