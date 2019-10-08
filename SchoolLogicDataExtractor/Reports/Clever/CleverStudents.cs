using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Clever
{
    /*
     * To do multiple contacts for a student, duplicate the student's row and put additional contacts on the duplicated rows
     */

    public class CleverStudents
    {
        private const char delimiter = ',';
        private const string stringContainer = "\"";

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("School_id" + delimiter);
            writer.Write("Student_id" + delimiter);
            writer.Write("Student_number" + delimiter);
            writer.Write("State_id" + delimiter);
            writer.Write("Last_name" + delimiter);
            writer.Write("Middle_name" + delimiter);
            writer.Write("First_name" + delimiter);
            writer.Write("Grade" + delimiter);
            writer.Write("Gender" + delimiter);
            writer.Write("Graduation_year" + delimiter);
            writer.Write("DOB" + delimiter);
            writer.Write("Race" + delimiter);
            writer.Write("Hispanic_Latino" + delimiter);
            writer.Write("Home_language" + delimiter);
            writer.Write("Ell_status" + delimiter);
            writer.Write("Frl_status" + delimiter);
            writer.Write("IEP_status" + delimiter);
            writer.Write("Student_street" + delimiter);
            writer.Write("Student_city" + delimiter);
            writer.Write("Student_state" + delimiter);
            writer.Write("Student_zip" + delimiter);
            writer.Write("Student_email" + delimiter);
            writer.Write("Contact_relationship" + delimiter);
            writer.Write("Contact_type" + delimiter);
            writer.Write("Contact_name" + delimiter);
            writer.Write("Contact_phone" + delimiter);
            //writer.Write("Contact_phone_type" + delimiter);
            writer.Write("Contact_email" + delimiter);
            writer.Write("Contact_sis_id" + delimiter);
            writer.Write("Username" + delimiter);
            writer.Write("Password" + delimiter);
            writer.Write("Unweighted_gpa" + delimiter);
            writer.Write("Weighted_gpa" + delimiter);

            writer.Write(Environment.NewLine);

            StudentRepository _studentRepo = new StudentRepository();
            List<Student> students = _studentRepo.GetAll();
            ContactRepository _contactRepo = new ContactRepository();

            foreach (Student student in students)
            {             
                List<StudentContact> studentContacts = _contactRepo.GetForStudent(student.iStudentID).Where(x => x.CanAccessHomelogic).ToList();
                if (studentContacts.Count > 0)
                {
                    // Students with contacts   
                    foreach (StudentContact contact in studentContacts)
                    {
                        writer.Write(stringContainer + student.BaseSchool.DAN + stringContainer + delimiter);
                        writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                        writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                        writer.Write(stringContainer + student.SaskLearningNumber + stringContainer + delimiter); // State student ID (SK learning number?)
                        writer.Write(stringContainer + student.LastName + stringContainer + delimiter);
                        writer.Write(stringContainer + student.MiddleName + stringContainer + delimiter);
                        writer.Write(stringContainer + student.FirstName + stringContainer + delimiter);
                        writer.Write(stringContainer + student.Grade + stringContainer + delimiter);
                        writer.Write(stringContainer + student.GenderInitial + stringContainer + delimiter);
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Grad year
                        writer.Write(stringContainer + student.DateOfBirth.ToString("MM/dd/yyyy") + stringContainer + delimiter);
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Race
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Hispanic_latino
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Home Language
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // ELL_status
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Frl_status
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // IEP_status (SP. ED? Y/N)
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Student_street
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // City
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // State
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Zip
                        writer.Write(stringContainer + student.EmailAddress + stringContainer + delimiter); // Email
                        writer.Write(stringContainer + contact.Relation + stringContainer + delimiter); // Contact Relationship
                        writer.Write(stringContainer + "family" + stringContainer + delimiter); // Contact Type
                        writer.Write(stringContainer + contact.Contact.DisplayName + stringContainer + delimiter); // Contact name
                        writer.Write(stringContainer + contact.Contact.HomePhone + stringContainer + delimiter); // Contact phone 
                                                                                                                 //writer.Write(stringContainer + "Home" + stringContainer + delimiter); // Contact phone type [ Cell | Home | Work ]
                        writer.Write(stringContainer + contact.Contact.Email + stringContainer + delimiter); // Contact email
                        writer.Write(stringContainer + contact.Contact.iContactID + stringContainer + delimiter); // Contact SIS ID
                        writer.Write(stringContainer + student.EmailAddress + stringContainer + delimiter); // Student default username
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Student default password
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Unweighted GPA
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // Weighted GPA
                        writer.Write(Environment.NewLine);
                    }
                }
                else
                {
                    // Students without contacts
                    writer.Write(stringContainer + student.BaseSchool.DAN + stringContainer + delimiter);
                    writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                    writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                    writer.Write(stringContainer + student.SaskLearningNumber + stringContainer + delimiter); // State student ID (SK learning number?)
                    writer.Write(stringContainer + student.LastName + stringContainer + delimiter);
                    writer.Write(stringContainer + student.MiddleName + stringContainer + delimiter);
                    writer.Write(stringContainer + student.FirstName + stringContainer + delimiter);
                    writer.Write(stringContainer + student.Grade + stringContainer + delimiter);
                    writer.Write(stringContainer + student.GenderInitial + stringContainer + delimiter);
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Grad year
                    writer.Write(stringContainer + student.DateOfBirth.ToString("MM/dd/yyyy") + stringContainer + delimiter);
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Race
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Hispanic_latino
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Home Language
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // ELL_status
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Frl_status
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // IEP_status (SP. ED? Y/N)
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Student_street
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // City
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // State
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Zip
                    writer.Write(stringContainer + student.EmailAddress + stringContainer + delimiter); // Email
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Contact Relationship
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Contact Type
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Contact name
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Contact phone
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Contact email
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Contact SIS ID
                    writer.Write(stringContainer + student.EmailAddress + stringContainer + delimiter); // Student default username
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Student default password
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Unweighted GPA
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // Weighted GPA
                    writer.Write(Environment.NewLine);
                }
                
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
