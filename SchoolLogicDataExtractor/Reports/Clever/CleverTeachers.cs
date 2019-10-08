using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Clever
{
    public class CleverTeachers
    {
        private const char delimiter = ',';
        private const string stringContainer = "\"";

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("School_id" + delimiter);
            writer.Write("Teacher_id" + delimiter);
            writer.Write("Teacher_number" + delimiter);
            writer.Write("State_teacher_id" + delimiter);
            writer.Write("Teacher_email" + delimiter);
            writer.Write("First_name" + delimiter);
            writer.Write("Middle_name" + delimiter);
            writer.Write("Last_name" + delimiter);
            writer.Write("Title" + delimiter);
            writer.Write("Username" + delimiter);
            writer.Write("Password" + delimiter);

            writer.Write(Environment.NewLine);

            StaffRepository _staffRepo = new StaffRepository();
            List<StaffMember> staff = _staffRepo.GetAll();

            Dictionary<string, StaffMember> staffDictionary = new Dictionary<string, StaffMember>();
            foreach(StaffMember s in staff)
            {
                if (!staffDictionary.ContainsKey(s.TeachingCertificateNumber))
                {
                    staffDictionary.Add(s.TeachingCertificateNumber, s);
                }
            }

            TeacherAssignmentRepository teacherAssignmentRepository = new TeacherAssignmentRepository();

            List<string> staffWithAssignments = new List<string>();
            foreach(TeacherAssignment ta in teacherAssignmentRepository.GetAll())
            {
                if (!staffWithAssignments.Contains(ta.Teacher.TeachingCertificateNumber))
                {
                    if (ta.Teacher.School != null)
                    {
                        if ((ta.Teacher.School.isFake == false) && (ta.Teacher.School.DAN != "0") && (ta.Teacher.TeachingCertificateNumber.Length > 0))
                        {
                            staffWithAssignments.Add(ta.Teacher.TeachingCertificateNumber);
                        }
                    }
                }
            }

            List<StaffMember> teachers = new List<StaffMember>();
            foreach(string teachingCertNumber in staffWithAssignments)
            {
                if (staffDictionary.ContainsKey(teachingCertNumber))
                {
                    teachers.Add(staffDictionary[teachingCertNumber]);
                }
            }

            foreach (StaffMember teacher in teachers)
            {
                if ((teacher.School.DAN != "0") && (teacher.School.DAN.Length > 1))
                {
                    writer.Write(stringContainer + teacher.School.DAN + stringContainer + delimiter); // School id
                    writer.Write(stringContainer + teacher.TeachingCertificateNumber + stringContainer + delimiter); // Teacher id
                    writer.Write(stringContainer + teacher.TeachingCertificateNumber + stringContainer + delimiter); // Teacher number
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // State teacher id
                    writer.Write(stringContainer + teacher.EmailAddress + stringContainer + delimiter); // teacher email
                    writer.Write(stringContainer + teacher.FirstName + stringContainer + delimiter); // firstname
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // middlename
                    writer.Write(stringContainer + teacher.LastName + stringContainer + delimiter); // lastname
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // title
                    writer.Write(stringContainer + teacher.EmailAddress + stringContainer + delimiter); // username
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // password
                    writer.Write(Environment.NewLine);
                }
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
