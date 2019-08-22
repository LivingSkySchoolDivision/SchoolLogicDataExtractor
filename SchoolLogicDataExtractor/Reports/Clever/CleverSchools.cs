using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Clever
{
    public class CleverSchools
    {
        private const char delimiter = ',';
        private const string stringContainer = "\"";

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("School id" + delimiter);
            writer.Write("School name" + delimiter);
            writer.Write("School number" + delimiter);

            writer.Write("State id" + delimiter);
            writer.Write("Low grade" + delimiter);
            writer.Write("High grade" + delimiter);
            writer.Write("Principal" + delimiter);
            writer.Write("Principal email" + delimiter);
            writer.Write("School address" + delimiter);
            writer.Write("School city" + delimiter);
            writer.Write("School state" + delimiter);
            writer.Write("School zip" + delimiter);
            writer.Write("School phone" + delimiter);

            writer.Write(Environment.NewLine);

            SchoolRepository _schoolRepo = new SchoolRepository();
            List<School> schools = _schoolRepo.GetAll();

            foreach (School school in schools.Where(x => x.isFake == false))
            {
                writer.Write(stringContainer + school.DAN + stringContainer + delimiter);
                writer.Write(stringContainer + school.Name + stringContainer + delimiter);
                writer.Write(stringContainer + school.DAN + stringContainer + delimiter);

                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);

                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
