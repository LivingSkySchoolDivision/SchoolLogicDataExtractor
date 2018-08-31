using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Sangha
{
    public class SanghaClasses
    {
        // Should be just a list of courses

        private const string delimiter = ",";
        private const string stringContainer = "\"";
        private readonly Encoding encoding = Encoding.ASCII;

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream, encoding);

            // Headings
            // Class Id,Class Number,Title,SchoolId
            writer.Write("Class Id" + delimiter);
            writer.Write("Class Number" + delimiter);
            writer.Write("Title" + delimiter);
            writer.Write("SchoolId" + delimiter);
            writer.Write(Environment.NewLine);

            CourseRepository _courseRepo = new CourseRepository();

            foreach (Course c in _courseRepo.GetAll())
            {

                writer.Write(stringContainer + c.iCourseID + stringContainer + delimiter);
                writer.Write(stringContainer + c.CourseCode + stringContainer + delimiter);
                writer.Write(stringContainer + c.Name + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;        
        }
    }
}
