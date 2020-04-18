using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sldataextractor.exportfilegenerators.Clever
{
    public class CleverTeachers : IExportFileGenerator
    {
        private const char delimiter = ',';
        private const string stringContainer = "\"";
        private readonly string _dbConnectionString;

        // Teachers and Staff are different things to Clever
        // Make the teacher list by first making a section  list

        public CleverTeachers(ConfigFile ConfigFile, Dictionary<string, string> Arguments)
        {
            this._dbConnectionString = ConfigFile.DatabaseConnectionString;
        }

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

            StaffRepository _staffRepo = new StaffRepository(_dbConnectionString);
            List<StaffMember> staff = _staffRepo.GetAll();
            List<string> seenTeacherCertNumbers = new List<string>();

            // Get all sections
            SchoolClassRepository screpo = new SchoolClassRepository(_dbConnectionString);
            List<SchoolClass> sections = screpo.GetAll();

            // Get all teacher assignments
            TeacherAssignmentRepository teacherAssignmentRepo = new TeacherAssignmentRepository(_dbConnectionString);
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

            // Now make the CSV
            foreach (StaffMember teacher in activeTeachers.Values)
            {
                if ((teacher.School.DAN != "0") && (teacher.School.DAN.Length > 1) && (!string.IsNullOrEmpty(teacher.TeachingCertificateNumber) && (!seenTeacherCertNumbers.Contains(teacher.TeachingCertificateNumber))))
                {
                    seenTeacherCertNumbers.Add(teacher.TeachingCertificateNumber);
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
