using System;

namespace Computer.V2.Lib.Exceptions
{    
    public class InvalidMatrixException : Exception
    {
        public InvalidMatrixException(string message) : base(message) { }

    }
}
