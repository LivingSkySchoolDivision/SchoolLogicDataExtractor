using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Xello
{
    public class XelloStudents
    {
        private const string delimiter = "|";
        private const string stringContainer = "";
        private readonly List<string> gradeList = new List<string>() { "12", "11", "10", "9", "8", "7", "09", "08", "07" };

        // https://public.careercruising.com/us/en/support/onboarding/repetitive-data-transfer-student-and-course/
        
        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings            
            writer.Write("StudentID" + delimiter);
            writer.Write("FirstName" + delimiter);
            writer.Write("LastName" + delimiter);
            writer.Write("Gender" + delimiter);
            writer.Write("DateOfBirth" + delimiter);
            writer.Write("CurrentGrade" + delimiter);
            writer.Write("CurrentSchoolCode" + delimiter);
            writer.Write("PreRegSchoolCode" + delimiter);
            writer.Write("Email" + delimiter);
            writer.Write(Environment.NewLine);

            StudentRepository _studentRepo = new StudentRepository();

            foreach (Student student in _studentRepo.GetAll().Where(s => gradeList.Contains(s.Grade)).OrderBy(s => s.BaseSchool.Name).ThenBy(s => s.DisplayNameLastNameFirst))
            {
                writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                writer.Write(stringContainer + student.FirstName + stringContainer + delimiter);
                writer.Write(stringContainer + student.LastName + stringContainer + delimiter);
                writer.Write(stringContainer + student.GenderInitial + stringContainer + delimiter);
                writer.Write(stringContainer + student.DateOfBirth.ToString("yyyy-MM-dd") + stringContainer + delimiter);
                writer.Write(stringContainer + student.Grade + stringContainer + delimiter);
                writer.Write(stringContainer + student.BaseSchool.DAN + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);  
                writer.Write(stringContainer + student.EmailAddress + stringContainer);
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
