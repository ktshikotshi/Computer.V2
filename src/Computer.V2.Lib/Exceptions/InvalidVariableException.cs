using System;

namespace Computer.V2.Lib.Exceptions
{
    public class InvalidVariableException : Exception
    {
        public InvalidVariableException()
         : base("Invalid Use of Variables.")
        {
        }
    }
}