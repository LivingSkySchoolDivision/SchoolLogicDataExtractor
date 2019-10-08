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
            writer.Write("School_id" + delimiter);
            writer.Write("School_name" + delimiter);
            writer.Write("School_number" + delimiter);

            writer.Write("State_id" + delimiter);
            writer.Write("Low_grade" + delimiter);
            writer.Write("High_grade" + delimiter);
            writer.Write("Principal" + delimiter);
            writer.Write("Principal_email" + delimiter);
            writer.Write("School_address" + delimiter);
            writer.Write("School_city" + delimiter);
            writer.Write("School_state" + delimiter);
            writer.Write("School_zip" + delimiter);
            writer.Write("School_phone" + delimiter);

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
