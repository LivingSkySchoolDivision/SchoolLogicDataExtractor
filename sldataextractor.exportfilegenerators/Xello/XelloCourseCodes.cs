using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace sldataextractor.exportfilegenerators.Xello
{
    public class XelloCourseCodes : IExportFileGenerator
    {
        private const string delimiter = "|";
        private const string stringContainer = "";
        private readonly string _dbConnectionString;

        public XelloCourseCodes(ConfigFile ConfigFile, Dictionary<string, string> Arguments)
        {
            this._dbConnectionString = ConfigFile.DatabaseConnectionString;
        }

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings            
            writer.Write("SchoolCode" + delimiter);
            writer.Write("CourseCode" + delimiter);
            writer.Write("CourseName" + delimiter);
            writer.Write("CreditValue" + delimiter);
            writer.Write("GradeLower" + delimiter);
            writer.Write("GradeUpper");
            writer.Write(Environment.NewLine);

            // HIGH SCHOOL CLASSES ONLY
            // - Or more specifically, courses that offer more than zero credits

            // Get all classes
            // Make a list of all courses offered

            SchoolClassRepository classRepository = new SchoolClassRepository(_dbConnectionString);
            CourseRepository courseRepository = new CourseRepository(_dbConnectionString);
            SchoolRepository schoolRepo = new SchoolRepository(_dbConnectionString);

            List<Course> coursesWithCredits = courseRepository.GetAll().Where(x => x.Credits > 0 && x.CurrentlyOffered).ToList();

            foreach (School school in schoolRepo.GetAll().Where(x => x.IsHighSchool))
            {
                foreach (Course course in coursesWithCredits)
                {
                    writer.Write(stringContainer + school.GovernmentID + stringContainer + delimiter);
                    writer.Write(stringContainer + course.CourseCode + stringContainer + delimiter);
                    writer.Write(stringContainer + course.Name + stringContainer + delimiter);
                    writer.Write(stringContainer + course.Credits + stringContainer + delimiter);
                    writer.Write(stringContainer + course.LowGrade + stringContainer + delimiter);
                    writer.Write(stringContainer + course.HighGrade + stringContainer);
                    writer.Write(Environment.NewLine);
                }
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
