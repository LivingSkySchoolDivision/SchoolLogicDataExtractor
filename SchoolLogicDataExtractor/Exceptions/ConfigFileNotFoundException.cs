using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class ConfigFileNotFoundException : Exception
    {
        public ConfigFileNotFoundException() { }

        public ConfigFileNotFoundException(string message) : base(message) { }

        public ConfigFileNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
