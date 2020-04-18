using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.exportfilegenerators
{
    public abstract class ExportFileGenerator
    {
        protected ConfigFile _configFile;
        protected Dictionary<string, string> _arguments;

        public ExportFileGenerator(ConfigFile ConfigFile, Dictionary<string, string> Arguments) 
        {
            this._configFile = ConfigFile;
            this._arguments = Arguments;
        }
    }
}
