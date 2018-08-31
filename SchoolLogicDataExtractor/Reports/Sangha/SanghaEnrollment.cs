using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Sangha
{
    public class SanghaEnrollment
    {
        // Staff and student enrolment



        // All active contacts who "lives with" is yes
        private const string delimiter = ",";
        private const string stringContainer = "\"";
        private readonly Encoding encoding = Encoding.ASCII;

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream, encoding);

            // Headings
            //Id (Student / Staff),SectionId,Role (Teacher / Student),SchoolId

            writer.Write("Id" + delimiter);
            writer.Write("SectionId" + delimiter);
            writer.Write("Role" + delimiter);
            writer.Write("SchoolId" + delimiter);
            writer.Write(Environment.NewLine);

            TeacherAssignmentRepository _teacherAssignmentRepo = new TeacherAssignmentRepository();
            StudentClassEnrolmentRepository _studentClassEnrolmentRepo = new StudentClassEnrolmentRepository();


            // Teachers 
            foreach (TeacherAssignment ta in _teacherAssignmentRepo.GetAll().Where(x => Helpers.SanghaSettings.AllowedSchoolGovIDs.Contains(x.Class.School.DAN)))
            {
                writer.Write(stringContainer + ta.Teacher.iStaffId + stringContainer + delimiter); // Id
                writer.Write(stringContainer + ta.Class.iClassID + stringContainer + delimiter); // SectionId
                writer.Write(stringContainer + "Teacher" + stringContainer + delimiter); // Role
                writer.Write(stringContainer + ta.Class.School.GovernmentID + stringContainer + delimiter); // Schoolid
                writer.Write(Environment.NewLine);
            }

            // Students
            foreach (StudentClassEnrolment sa in _studentClassEnrolmentRepo.GetAll().Where(x => Helpers.SanghaSettings.AllowedSchoolGovIDs.Contains(x.Class.School.DAN)))
            {
                writer.Write(stringContainer + sa.Student.StudentNumber + stringContainer + delimiter); // Id
                writer.Write(stringContainer + sa.Class.iClassID + stringContainer + delimiter); // SectionId
                writer.Write(stringContainer + "Student" + stringContainer + delimiter); // Role
                writer.Write(stringContainer + sa.Class.School.GovernmentID + stringContainer + delimiter); // Schoolid
                writer.Write(Environment.NewLine);
            }
            
            writer.Flush();
            outStream.Flush();
            return outStream;
        }

    }
}
