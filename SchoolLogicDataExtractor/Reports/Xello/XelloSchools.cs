using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Xello
{
    public class XelloSchools
    {
        private const string delimiter = "|";
        private const string stringContainer = "";

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("SchoolCode" + delimiter);
            writer.Write("Name" + delimiter);
            writer.Write("SchoolType");
            writer.Write(Environment.NewLine);

            SchoolRepository _schoolRepo = new SchoolRepository();

            foreach (School school in _schoolRepo.GetAll())
            {
                writer.Write(stringContainer + school.DAN + stringContainer + delimiter);
                writer.Write(stringContainer + school.Name + stringContainer + delimiter);
                writer.Write(stringContainer + (school.IsHighSchool ? 1 : 2) + stringContainer);
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
