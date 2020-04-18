using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.util.Exceptions
{
    class ConfigurationFileNotFoundException : Exception
    {
        public ConfigurationFileNotFoundException() { }

        public ConfigurationFileNotFoundException(string message) : base(message) { }

        public ConfigurationFileNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
