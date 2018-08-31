using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Sangha
{
    public class SanghaStudents
    {
        private const string delimiter = ",";
        private const string stringContainer = "\"";
        private readonly Encoding encoding = Encoding.ASCII;

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream, encoding);

            // Headings
            writer.Write("Student Id" + delimiter);
            writer.Write("Student number" + delimiter);
            writer.Write("Grade" + delimiter);
            writer.Write("First Name" + delimiter);
            writer.Write("Middle Name" + delimiter);
            writer.Write("Last Name" + delimiter);
            writer.Write("Sex" + delimiter);
            writer.Write("Email 1" + delimiter);
            writer.Write("Email 2" + delimiter);
            writer.Write("Tel" + delimiter);
            writer.Write("Mobile" + delimiter);
            writer.Write("Date Of Birth" + delimiter);
            writer.Write("Enrollment Date" + delimiter);
            writer.Write("Leave date" + delimiter);
            writer.Write("SchoolId" + delimiter);
            writer.Write(Environment.NewLine);

            //StudentRepository _studentRepo = new StudentRepository();
            EnrolledStudentRepository _esr = new EnrolledStudentRepository();

            //foreach (Student student in _studentRepo.GetAll().OrderBy(s => s.BaseSchool.Name).ThenBy(s => s.DisplayNameLastNameFirst))
            foreach (EnrolledStudent es in _esr.GetEnrollmentsFor(DateTime.Now).Where(x => Helpers.SanghaSettings.AllowedSchoolGovIDs.Contains(x.School.DAN)).OrderBy(x => x.School.Name).ThenBy(x => x.Student.DisplayNameLastNameFirst))
            {
                writer.Write(stringContainer + es.Student.StudentNumber + stringContainer + delimiter);
                writer.Write(stringContainer + es.Student.StudentNumber + stringContainer + delimiter);
                writer.Write(stringContainer + es.Student.Grade + stringContainer + delimiter);
                writer.Write(stringContainer + es.Student.FirstName + stringContainer + delimiter);
                writer.Write(stringContainer + es.Student.MiddleName + stringContainer + delimiter);
                writer.Write(stringContainer + es.Student.LastName + stringContainer + delimiter);
                writer.Write(stringContainer + es.Student.GenderInitial + stringContainer + delimiter);
                writer.Write(stringContainer + es.Student.EmailAddress + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Email 2
                writer.Write(stringContainer + es.Student.HomePhone + stringContainer + delimiter);
                writer.Write(stringContainer + es.Student.CellPhone + stringContainer + delimiter);
                writer.Write(stringContainer + es.Student.DateOfBirth.ToShortDateString() + stringContainer + delimiter);
                writer.Write(stringContainer + es.InDate.ToShortDateString() + stringContainer + delimiter); // Enrollment date
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Leave date
                writer.Write(stringContainer + es.School.GovernmentID + stringContainer + delimiter); // School ID
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
