using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.util.Exceptions
{
    public class SyntaxException : Exception
    {
        public SyntaxException() { }

        public SyntaxException(string message) : base(message) { }

        public SyntaxException(string message, Exception inner) : base(message, inner) { }
    }
}
