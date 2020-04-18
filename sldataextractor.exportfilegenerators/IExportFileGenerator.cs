using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sldataextractor.exportfilegenerators
{
    public interface IExportFileGenerator
    {
        public MemoryStream Generate();
    }
}
