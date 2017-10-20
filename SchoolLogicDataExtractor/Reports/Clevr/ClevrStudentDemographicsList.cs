using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Clevr
{
    public class ClevrStudentDemographicsList
    {
        private const char delimiter = '\t';
        private readonly Encoding encoding = Encoding.ASCII;

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream, encoding);

            // Headings
            writer.Write("tenantId" + delimiter);
            writer.Write("proprietaryId" + delimiter);
            writer.Write("localId" + delimiter);
            writer.Write("lastName" + delimiter);
            writer.Write("firstName" + delimiter);
            writer.Write("birthdate" + delimiter);
            writer.Write("schoolName" + delimiter);
            writer.Write("schoolProgram" + delimiter);
            writer.Write("homeLanguage" + delimiter);
            writer.Write("countryOrigin" + delimiter);
            writer.Write("studentGradeLevel" + delimiter);
            writer.Write("homeroomTeacher" + delimiter);
            writer.Write("studentPhone" + delimiter);
            writer.Write("studentCell" + delimiter);
            writer.Write("currentCredits" + delimiter);
            writer.Write("studentStreetAddress" + delimiter);
            writer.Write("studentPOBox" + delimiter);
            writer.Write("studentCity" + delimiter);
            writer.Write("studentPostalCode" + delimiter);
            writer.Write("studentMailingAddress" + delimiter);

            for (int x = 1; x <= 3; x++)
            {
                writer.Write("parent" + x + "Name" + delimiter);
                writer.Write("parent" + x + "Email" + delimiter);
                writer.Write("parent" + x + "Phone" + delimiter);
                writer.Write("parent" + x + "Cell" + delimiter);
                writer.Write("parent" + x + "Workphone" + delimiter);
                writer.Write("parent" + x + "StreetAddress" + delimiter);
                writer.Write("parent" + x + "POBox" + delimiter);
                writer.Write("parent" + x + "City" + delimiter);
                writer.Write("parent" + x + "PostalCode" + delimiter);
                writer.Write("parent" + x + "MailingAddress" + delimiter);
                writer.Write("parent" + x + "Agency" + delimiter);
            }

            writer.Write("attendanceTotAbsences" + delimiter);
            writer.Write("attendanceTotLates" + delimiter);
            writer.Write("fundingLevel" + delimiter);
            writer.Write("fundingCategory" + delimiter);
            writer.Write("fundingRenewalDate" + delimiter);
            writer.Write("medical" + delimiter);
            writer.Write("aboriginalAcademicAchievementGrant " + delimiter);

            writer.Write(Environment.NewLine);

            StudentRepository _studentRepo = new StudentRepository();

            foreach (Student student in _studentRepo.GetAll().OrderBy(s => s.BaseSchool.Name).ThenBy(s => s.DisplayNameLastNameFirst))
            {
                writer.Write(Settings.ClevrTennantID + "" + delimiter);
                writer.Write(student.StudentNumber + delimiter);
                writer.Write(student.StudentNumber + delimiter);
                writer.Write(student.LastName + delimiter);
                writer.Write(student.FirstName + delimiter);
                writer.Write(student.DateOfBirth.ToShortDateString() + delimiter);
                writer.Write(student.BaseSchool.Name + delimiter);
                writer.Write(student.LanguageProgram + delimiter);
                writer.Write(student.HomeLanguage + delimiter);
                writer.Write(student.CountryOfOrigin + delimiter);
                writer.Write(student.Grade + delimiter);
                writer.Write(student.HomeroomTeacher + delimiter);
                writer.Write(student.Address_Physical.Phone + delimiter);
                writer.Write(student.CellPhone + delimiter);
                writer.Write(student.CreditsEarned + "" + delimiter);
                writer.Write(student.Address_Physical.Street + delimiter);
                writer.Write("" + delimiter); // P.O. Box
                writer.Write(student.Address_Physical.City + delimiter);
                writer.Write(student.Address_Physical.PostalCode + delimiter);
                writer.Write(student.Address_Mailing + delimiter); // Full mailing address

                // Make a list of exactly 3 contacts, contacts must live with, 
                // If the student doesn't have that many contacts, put a blank one in

                List<StudentContact> studentContacts = student.Contacts.Where(c=>c.Priority == 1).ToList();
                if (studentContacts.Count == 0) { studentContacts.Add(new StudentContact()); }
                if (studentContacts.Count == 1) { studentContacts.Add(new StudentContact()); }
                if (studentContacts.Count == 2) { studentContacts.Add(new StudentContact()); }
                if (studentContacts.Count > 3)
                {
                    List<StudentContact> newStudentcontacts = new List<StudentContact>()
                    {
                        studentContacts[0],
                        studentContacts[1],
                        studentContacts[2]
                    };
                    studentContacts = newStudentcontacts;
                }


                foreach (StudentContact studentContact in studentContacts)
                {
                    Contact c = studentContact.Contact;
                    writer.Write(c.DisplayName + delimiter); // name
                    writer.Write(c.Email + delimiter); // email
                    writer.Write(c.HomePhone + delimiter); // home phone
                    writer.Write("" + delimiter); // cell phone
                    writer.Write(c.WorkPhone + delimiter); // work phone
                    writer.Write(c.Address_Physical + delimiter); // street address
                    writer.Write("" + delimiter); // PO box
                    writer.Write(c.Address_Physical.City + delimiter); // city
                    writer.Write(c.Address_Physical.PostalCode + delimiter); // postal code
                    writer.Write(c.Addrses_Mailing + delimiter); // mailing address
                    writer.Write("" + delimiter); // agency
                }                               

                writer.Write(student.YearToDateAttendanceStatistics.UnexcusedAbsences + "" + delimiter);
                writer.Write(student.YearToDateAttendanceStatistics.LatesOrLeaveEarlies + "" + delimiter);
                writer.Write("" + delimiter); // funding level
                writer.Write("" + delimiter); // funding category
                writer.Write("" + delimiter); // funding renewal date
                writer.Write(student.Medical + delimiter); // medical
                writer.Write("" + delimiter); // aboriginalacademicachievementgrant

                writer.Write(Environment.NewLine);
            }


            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
