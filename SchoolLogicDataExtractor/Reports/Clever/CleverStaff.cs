using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Clever
{
    public class CleverStaff
    {
        private const char delimiter = ',';
        private const string stringContainer = "\"";

        // Teachers and Staff are different things to Clever
        // Make the teacher list by first making a section  list
        // Make the staff list by first making the teacher list, and including everybody else

        public MemoryStream GenerateCSVFile()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("School_id" + delimiter);
            writer.Write("Staff_id" + delimiter);
            writer.Write("Staff_email" + delimiter);
            writer.Write("First_name" + delimiter);
            writer.Write("Last_name" + delimiter);
            writer.Write("Department" + delimiter);
            writer.Write("Title" + delimiter);
            writer.Write("Username" + delimiter);
            writer.Write("Password" + delimiter);

            writer.Write(Environment.NewLine);

            StaffRepository _staffRepo = new StaffRepository();
            List<StaffMember> allStaff = _staffRepo.GetAll();
            List<string> seenTeacherCertNumbers = new List<string>();

            // Get all sections
            SchoolClassRepository screpo = new SchoolClassRepository();
            List<SchoolClass> sections = screpo.GetAll();

            // Get all teacher assignments
            TeacherAssignmentRepository teacherAssignmentRepo = new TeacherAssignmentRepository();
            List<TeacherAssignment> allTeachingAssignments = teacherAssignmentRepo.GetAll();

            // Sort teacher assignments into a dictionary for easier consumption
            // Only send up classes that have teachers assigned
            Dictionary<int, List<TeacherAssignment>> assignmentsByClassID = new Dictionary<int, List<TeacherAssignment>>();

            foreach (TeacherAssignment ta in allTeachingAssignments)
            {
                if (ta.Class != null)
                {
                    if (!assignmentsByClassID.ContainsKey(ta.Class.iClassID))
                    {
                        assignmentsByClassID.Add(ta.Class.iClassID, new List<TeacherAssignment>());
                    }
                    if (ta.Teacher.TeachingCertificateNumber.Length > 0)
                    {
                        assignmentsByClassID[ta.Class.iClassID].Add(ta);
                    }
                }
            }

            // Now make a list of teachers we need
            Dictionary<int, StaffMember> activeTeachers = new Dictionary<int, StaffMember>();

            foreach (int classID in assignmentsByClassID.Keys)
            {
                foreach (TeacherAssignment ta in assignmentsByClassID[classID])
                {
                    if (ta.Teacher != null)
                    {
                        if (!activeTeachers.ContainsKey(ta.Teacher.iStaffId))
                        {
                            activeTeachers.Add(ta.Teacher.iStaffId, ta.Teacher);
                        }
                    }
                }
            }

            // Now find all the teachers who aren't on the above list
            List<StaffMember> activeStaffWithoutClasses = new List<StaffMember>();
            foreach (StaffMember teacher in allStaff)
            {
                if ((!activeTeachers.ContainsKey(teacher.iStaffId)) && teacher.IsEnabled)
                {
                    activeStaffWithoutClasses.Add(teacher);
                }
            }

                // Now make the CSV
            foreach (StaffMember staff in activeStaffWithoutClasses)
            {
                if (!string.IsNullOrEmpty(staff.EmailAddress))
                {
                    string schoolID = staff.School.DAN;
                    if (staff.School.isFake)
                    {
                        schoolID = "DEFAULT_DISTRICT_OFFICE";
                    }

                    writer.Write(stringContainer + schoolID + stringContainer + delimiter); // School id
                    writer.Write(stringContainer + staff.EmailAddress + stringContainer + delimiter); // Staff id
                    writer.Write(stringContainer + staff.EmailAddress + stringContainer + delimiter); // staff email
                    writer.Write(stringContainer + staff.FirstName + stringContainer + delimiter); // firstname
                    writer.Write(stringContainer + staff.LastName + stringContainer + delimiter); // lastname
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // department
                    writer.Write(stringContainer + "" + stringContainer + delimiter); // title
                    writer.Write(stringContainer + staff.EmailAddress + stringContainer + delimiter); // username
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
