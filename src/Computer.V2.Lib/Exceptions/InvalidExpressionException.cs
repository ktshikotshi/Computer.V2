using System;

namespace Computer.V2.Lib.Exceptions
{
    public class InvalidExpressionException : ArgumentException
    {
        public InvalidExpressionException(string message) : base(message) { }
    }
}
