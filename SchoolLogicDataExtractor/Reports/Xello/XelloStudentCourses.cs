using SchoolLogicDataExtractor.Model;
using SchoolLogicDataExtractor.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Xello
{
    public class XelloStudentCourses
    {
        private const string delimiter = "|";
        private const string stringContainer = "";
        private readonly List<string> gradeList = new List<string>() { "12", "11", "10", "9", "8", "7", "09", "08", "07" };


        // This file contains:
        //  Student history marks
        //  Student currently enroled classes that have a credit

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings            
            writer.Write("StudentID" + delimiter);
            writer.Write("CourseCode" + delimiter);
            writer.Write("CourseName" + delimiter);
            writer.Write("GradeLevel" + delimiter);
            writer.Write("GradeMark" + delimiter);
            writer.Write("CreditValue" + delimiter);
            writer.Write("DateCourseComplete" + delimiter);
            writer.Write("HSCourse" + delimiter);
            writer.Write("CoursePart");
            writer.Write(Environment.NewLine);

            StudentRepository studentRepo = new StudentRepository();
            StudentClassEnrolmentRepository enrolmentRepo = new StudentClassEnrolmentRepository();
            StudentHistoryMarkRepository historyMarkRepo = new StudentHistoryMarkRepository();


            // Load all enrolments into a dictionary for easier parsing
            Dictionary<int, List<StudentClassEnrolment>> enrolmentsByStudent = new Dictionary<int, List<StudentClassEnrolment>>();
            foreach(StudentClassEnrolment e in enrolmentRepo.GetAll())
            {
                if (!enrolmentsByStudent.ContainsKey(e.Student.iStudentID))
                {
                    enrolmentsByStudent.Add(e.Student.iStudentID, new List<StudentClassEnrolment>());
                }

                enrolmentsByStudent[e.Student.iStudentID].Add(e);
            }

            foreach(Student student in studentRepo.GetAll().Where(s => gradeList.Contains(s.Grade)))
            {
                // ************************************
                // History marks
                // ************************************
                List<StudentHistoryMark> studentHistoryMarks = historyMarkRepo.GetForStudent(student).Where(x => x.CreditsPossible > 0).ToList();                

                if (studentHistoryMarks.Count > 0)
                {
                    foreach(StudentHistoryMark mark in studentHistoryMarks)
                    {
                        writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                        writer.Write(stringContainer + mark.CourseCode+ stringContainer + delimiter);
                        writer.Write(stringContainer + mark.CourseName + stringContainer + delimiter);
                        writer.Write(stringContainer + mark.Grade + stringContainer + delimiter);
                        writer.Write(stringContainer + mark.NumericMark + stringContainer + delimiter); // In progress courses have 0 grades
                        writer.Write(stringContainer + mark.CreditsEarned + stringContainer + delimiter);
                        writer.Write(stringContainer + mark.CompletionDate.ToString("yyyy-MM-dd") + stringContainer + delimiter);
                        writer.Write(stringContainer + ((mark.CreditsEarned > 0) ? 1 : 0) + stringContainer + delimiter);
                        writer.Write(stringContainer + "" + stringContainer); // This column isn't really used
                        writer.Write(Environment.NewLine);
                    }                    
                }

                // ************************************
                // Currently enrolled classes
                // ************************************
                if (enrolmentsByStudent.ContainsKey(student.iStudentID))
                {
                    foreach (StudentClassEnrolment enrolment in enrolmentsByStudent[student.iStudentID].Where(e => e.Class.Course.Credits > 0))
                    {
                        writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                        writer.Write(stringContainer + enrolment.Class.Course.CourseCode + stringContainer + delimiter);
                        writer.Write(stringContainer + enrolment.Class.Course.Name + stringContainer + delimiter);
                        writer.Write(stringContainer + student.Grade + stringContainer + delimiter);
                        writer.Write(stringContainer + "0" + stringContainer + delimiter); // In progress courses have 0 grades
                        writer.Write(stringContainer + enrolment.Class.Course.Credits  + stringContainer + delimiter);
                        writer.Write(stringContainer + "" + stringContainer + delimiter); // In progress courses have empty dates
                        writer.Write(stringContainer + ((enrolment.Class.Course.Credits > 0) ? 1 : 0) + stringContainer + delimiter);
                        writer.Write(stringContainer + "" + stringContainer); // This column isn't really used
                        writer.Write(Environment.NewLine);
                    }
                }
            }
            
            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
