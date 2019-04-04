using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Xello
{
    class XelloSchools
    {
        private const string delimiter = "|";
        private const string stringContainer = "\"";
        private readonly Encoding encoding = Encoding.UTF8;

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream, encoding);

            // Headings
            writer.Write("SchoolCode");
            writer.Write("Name");
            writer.Write("SchoolType");
            writer.Write(Environment.NewLine);

            SchoolRepository _schoolRepo = new SchoolRepository();

            foreach (School school in _schoolRepo.GetAll())
            {
                writer.Write(stringContainer + school.DAN + stringContainer);
                writer.Write(stringContainer + school.Name + stringContainer);
                writer.Write(stringContainer + "" +stringContainer);
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
