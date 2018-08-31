using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Sangha
{
    public class SanghaParents
    {
        // All active contacts who "lives with" is yes
        private const string delimiter = ",";
        private const string stringContainer = "\"";
        private readonly Encoding encoding = Encoding.ASCII;

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream, encoding);

            // Headings
            //Student Id,First Name,Middle Name,Last Name,Sex,Email 1,Email 2,Tel,Mobile,Date Of Birth,SchoolId

            writer.Write("Student Id" + delimiter);
            writer.Write("First Name" + delimiter);
            writer.Write("Middle Name" + delimiter);
            writer.Write("Last Name" + delimiter);
            writer.Write("Sex" + delimiter);
            writer.Write("Email 1" + delimiter);
            writer.Write("Email 2" + delimiter);
            writer.Write("Tel" + delimiter);
            writer.Write("Mobile" + delimiter);
            writer.Write("Date Of Birth" + delimiter);
            writer.Write("SchoolId" + delimiter);
            writer.Write(Environment.NewLine);


            // For each student in the student export, get their contacts that "lives with" is on
            
            //StudentRepository _studentRepo = new StudentRepository();
            EnrolledStudentRepository _esr = new EnrolledStudentRepository();
            ContactRepository _cr = new ContactRepository();
            
            //foreach (Student student in _studentRepo.GetAll().OrderBy(s => s.BaseSchool.Name).ThenBy(s => s.DisplayNameLastNameFirst))
            foreach (EnrolledStudent es in _esr.GetEnrollmentsFor(DateTime.Now).Where(x => Helpers.SanghaSettings.AllowedSchoolGovIDs.Contains(x.School.DAN)).OrderBy(x => x.School.Name).ThenBy(x => x.Student.DisplayNameLastNameFirst))
            {
                foreach (StudentContact sc in _cr.GetForStudent(es.Student.iStudentID).Where(x => x.LivesWithStudent))
                {
                    writer.Write(stringContainer + es.Student.StudentNumber + stringContainer + delimiter); // Student ID
                    writer.Write(stringContainer + sc.Contact.FirstName + stringContainer + delimiter); // First name
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Middle name
                    writer.Write(stringContainer + sc.Contact.LastName + stringContainer + delimiter); // Last name
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Sex
                    writer.Write(stringContainer + sc.Contact.Email + stringContainer + delimiter); // Email
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Email 2
                    writer.Write(stringContainer + sc.Contact.HomePhone + stringContainer + delimiter); // Tel
                    writer.Write(stringContainer + sc.Contact.CellPhone + stringContainer + delimiter); // Mobile
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Date of birth
                    writer.Write(stringContainer + es.School.GovernmentID + stringContainer + delimiter); // SchoolId
                    writer.Write(Environment.NewLine);
                }
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
