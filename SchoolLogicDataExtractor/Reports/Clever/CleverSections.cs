using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Clever
{
    public class CleverSections
    {
        private const char delimiter = ',';
        private const string stringContainer = "\"";

        // Don't send them empty classes

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("School_id" + delimiter);
            writer.Write("Section_id" + delimiter);
            writer.Write("Teacher_id" + delimiter);
            writer.Write("Teacher_2_id" + delimiter);
            writer.Write("Teacher_3_id" + delimiter);
            writer.Write("Teacher_4_id" + delimiter);
            writer.Write("Teacher_5_id" + delimiter);
            writer.Write("Teacher_6_id" + delimiter);
            writer.Write("Teacher_7_id" + delimiter);
            writer.Write("Teacher_8_id" + delimiter);
            writer.Write("Teacher_9_id" + delimiter);
            writer.Write("Teacher_10_id" + delimiter);
            writer.Write("Name" + delimiter);
            writer.Write("Section_number" + delimiter);
            writer.Write("Grade" + delimiter);
            writer.Write("Course_name" + delimiter);
            writer.Write("Course_number" + delimiter);
            writer.Write("Course_description" + delimiter);
            writer.Write("Period" + delimiter);
            writer.Write("Subject" + delimiter);
            writer.Write("Term_name" + delimiter);
            writer.Write("Term_start" + delimiter);
            writer.Write("Term_end" + delimiter);

            writer.Write(Environment.NewLine);

            SchoolClassRepository screpo = new SchoolClassRepository();
            List<SchoolClass> sections = screpo.GetAll();

            TeacherAssignmentRepository teacherAssignmentRepo = new TeacherAssignmentRepository();
            List<TeacherAssignment> allTeachingAssignments = teacherAssignmentRepo.GetAll();

            // Sort teacher assignments into a dictionary for easier consumption
            // Only send up classes that have teachers assigned
            Dictionary<int, List<TeacherAssignment>> assignmentsByClassID = new Dictionary<int, List<TeacherAssignment>>();

            // Don't send empty classes, so get enrolment counts for each class
            StudentClassEnrolmentRepository enrolmentRepo = new StudentClassEnrolmentRepository();
            Dictionary<int, int> enrolmentCountsByClassID = enrolmentRepo.GetEnrolmentCountsByClassID();

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

            foreach (SchoolClass sc in sections)
            {
                if (enrolmentCountsByClassID.ContainsKey(sc.iClassID)) 
                { 
                    if (enrolmentCountsByClassID[sc.iClassID] > 0)
                    {
                        if (assignmentsByClassID.ContainsKey(sc.iClassID))
                        {
                            List<TeacherAssignment> thisClassAssignments = assignmentsByClassID[sc.iClassID];
                            if (thisClassAssignments.Count > 0)
                            {
                                writer.Write(stringContainer + sc.School.DAN + stringContainer + delimiter); // School id
                                writer.Write(stringContainer + sc.iClassID + stringContainer + delimiter); // Section id
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 1 ? thisClassAssignments[0].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 1
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 2 ? thisClassAssignments[1].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 2
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 3 ? thisClassAssignments[2].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 3
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 4 ? thisClassAssignments[3].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 4
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 5 ? thisClassAssignments[4].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 5
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 6 ? thisClassAssignments[5].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 6 
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 7 ? thisClassAssignments[6].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 7
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 8 ? thisClassAssignments[7].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 8
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 9 ? thisClassAssignments[8].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 9
                                writer.Write(stringContainer + (thisClassAssignments.Count >= 10 ? thisClassAssignments[9].Teacher.TeachingCertificateNumber.ToString() : string.Empty) + stringContainer + delimiter); // Teacher 10
                                writer.Write(stringContainer + sc.Name + stringContainer + delimiter); // Name
                                writer.Write(stringContainer + sc.Section + stringContainer + delimiter); // Section number
                                writer.Write(stringContainer + "" + stringContainer + delimiter); // Grade
                                writer.Write(stringContainer + sc.Course.Name + stringContainer + delimiter); // Course Name
                                writer.Write(stringContainer + sc.Course.CourseCode + stringContainer + delimiter); // Course Number
                                writer.Write(stringContainer + "" + stringContainer + delimiter); // Course Description
                                writer.Write(stringContainer + "" + stringContainer + delimiter); // Period
                                writer.Write(stringContainer + "" + stringContainer + delimiter); // Subject
                                writer.Write(stringContainer + "" + stringContainer + delimiter); // Term name
                                writer.Write(stringContainer + "" + stringContainer + delimiter); // Term start
                                writer.Write(stringContainer + "" + stringContainer + delimiter); // Term end
                                writer.Write(Environment.NewLine);
                            }
                        }
                    }
                }                
            }            

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
