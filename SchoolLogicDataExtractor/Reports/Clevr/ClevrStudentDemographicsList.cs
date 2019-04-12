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
        private const string stringContainer = "";
        private readonly Encoding encoding = Encoding.UTF8;

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
                writer.Write(stringContainer + Settings.ClevrTennantID + "" + stringContainer + delimiter);
                writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                writer.Write(stringContainer + student.LastName + stringContainer + delimiter);
                writer.Write(stringContainer + student.FirstName + stringContainer + delimiter);
                writer.Write(stringContainer + student.DateOfBirth.ToShortDateString() + stringContainer + delimiter);
                writer.Write(stringContainer + student.BaseSchool.Name + stringContainer + delimiter);
                writer.Write(stringContainer + student.LanguageProgram + stringContainer + delimiter);
                writer.Write(stringContainer + student.HomeLanguage + stringContainer + delimiter);
                writer.Write(stringContainer + student.CountryOfOrigin + stringContainer + delimiter);
                writer.Write(stringContainer + student.Grade + stringContainer + delimiter);
                writer.Write(stringContainer + student.HomeroomTeacher + stringContainer + delimiter);
                writer.Write(stringContainer + student.Address_Physical.Phone + stringContainer + delimiter);
                writer.Write(stringContainer + student.CellPhone + stringContainer + delimiter);
                writer.Write(stringContainer + student.CreditsEarned + "" + stringContainer + delimiter);
                writer.Write(stringContainer + student.Address_Physical.Street + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter); // P.O. Box
                writer.Write(stringContainer + student.Address_Physical.City + stringContainer + delimiter);
                writer.Write(stringContainer + student.Address_Physical.PostalCode + stringContainer + delimiter);
                writer.Write(stringContainer + student.Address_Mailing + stringContainer + delimiter); // Full mailing address

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
                    writer.Write(stringContainer + c.DisplayName + stringContainer + delimiter); // name
                    writer.Write(stringContainer + c.Email + stringContainer + delimiter); // email
                    writer.Write(stringContainer + c.HomePhone + stringContainer + delimiter); // home phone
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // cell phone
                    writer.Write(stringContainer + c.WorkPhone + stringContainer + delimiter); // work phone
                    writer.Write(stringContainer + c.Address_Physical + stringContainer + delimiter); // street address
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // PO box
                    writer.Write(stringContainer + c.Address_Physical.City + stringContainer + delimiter); // city
                    writer.Write(stringContainer + c.Address_Physical.PostalCode + stringContainer + delimiter); // postal code
                    writer.Write(stringContainer + c.Addrses_Mailing + stringContainer + delimiter); // mailing address
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // agency
                }                               

                writer.Write(stringContainer + student.YearToDateAttendanceStatistics.UnexcusedAbsences + "" + stringContainer + delimiter);
                writer.Write(stringContainer + student.YearToDateAttendanceStatistics.LatesOrLeaveEarlies + "" + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter); // funding level
                writer.Write(stringContainer + "" + stringContainer + delimiter); // funding category
                writer.Write(stringContainer + "" + stringContainer + delimiter); // funding renewal date
                writer.Write(stringContainer + student.Medical + stringContainer + delimiter); // medical
                writer.Write(stringContainer + "" + stringContainer + delimiter); // aboriginalacademicachievementgrant

                writer.Write(Environment.NewLine);
            }


            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
