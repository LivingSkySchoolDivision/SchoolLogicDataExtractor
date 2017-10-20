using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Clevr
{
    public class ClevrStaffList
    {
        private const char delimiter = '\t';
        private readonly Encoding encoding = Encoding.ASCII;

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream, encoding);
            
            // Headings
            writer.Write("" + delimiter);
            writer.Write(Environment.NewLine);


            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
