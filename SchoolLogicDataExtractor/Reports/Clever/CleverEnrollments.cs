using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Clever
{
    public class CleverEnrollments
    {
        private const char delimiter = ',';
        private const string stringContainer = "\"";

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("School_id" + delimiter);
            writer.Write("Section_id" + delimiter);
            writer.Write("Student_id" + delimiter);
            writer.Write(Environment.NewLine);

            StudentClassEnrolmentRepository studentClassEnrolmentRepo = new StudentClassEnrolmentRepository();

            List<StudentClassEnrolment> studentEnrolments = studentClassEnrolmentRepo.GetAll();

            foreach (StudentClassEnrolment sa in studentEnrolments)
            {
                writer.Write(stringContainer + sa.Class.School.DAN + stringContainer + delimiter);
                writer.Write(stringContainer + sa.Class.iClassID + stringContainer + delimiter);
                writer.Write(stringContainer + sa.Student.StudentNumber + stringContainer + delimiter);
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
