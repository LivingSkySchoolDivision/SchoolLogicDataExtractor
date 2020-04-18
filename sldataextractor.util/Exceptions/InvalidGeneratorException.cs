using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.util.Exceptions
{
    public class InvalidGeneratorException : Exception
    {
        public InvalidGeneratorException() { }

        public InvalidGeneratorException(string message) : base(message) { }

        public InvalidGeneratorException(string message, Exception inner) : base(message, inner) { }
    }
}
