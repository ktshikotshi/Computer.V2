using System;

namespace ComputerV2_class.Exceptions
{
    class InvalidExpressionException : Exception
    {
        public InvalidExpressionException(string message) : base(message) { }
    }
}
