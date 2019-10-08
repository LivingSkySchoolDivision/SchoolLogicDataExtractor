using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class CourseRepository
    {
        private readonly string sql = "SELECT " +
                                        "Course.cName, " +
                                        "Course.cCourseCode, " +
                                        "Course.iGovCourseID, " +
                                        "Course.cGovernmentCode, " +
                                        "Course.iCourseID, " +
                                        "Course.lOfferedInSchool, " +
                                        "Course.lSchoolExam, " +
                                        "Grades_1.cName AS LowGrade, " +
                                        "Grades.cName AS HighGrade, " +
                                        "Course.nLowCredit, " +
                                        "Course.nHighCredit " +
                                        "FROM " +
                                        "Course LEFT OUTER JOIN " +
                                        "Grades ON Course.iHigh_GradesID = Grades.iGradesID LEFT OUTER JOIN " +
                                        "Grades AS Grades_1 ON Course.iLow_GradesID = Grades_1.iGradesID ";


        Dictionary<int, Course> _allCourses = new Dictionary<int, Course>();
        
        private Course dataReaderToCourse(SqlDataReader dataReader)
        {
            return new Course()
            {
                iCourseID = Parsers.ParseInt(dataReader["iCourseID"].ToString().Trim()),
                CourseCode = dataReader["cGovernmentCode"].ToString().Trim(),
                Name = dataReader["cName"].ToString().Trim(),
                HighGrade = dataReader["HighGrade"].ToString().Trim(),
                LowGrade = dataReader["LowGrade"].ToString().Trim(),
                HighCredit = Parsers.ParseDecimal(dataReader["nHighCredit"].ToString().Trim()),
                LowCredit = Parsers.ParseDecimal(dataReader["nLowCredit"].ToString().Trim()),
                CurrentlyOffered = Parsers.ParseBool(dataReader["lOfferedInSchool"].ToString().Trim())
            };
        }

        public CourseRepository()
        {
            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_SchoolLogic))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = sql;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            _allCourses.Add(Parsers.ParseInt(dataReader["iCourseID"].ToString().Trim()), dataReaderToCourse(dataReader));
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        public List<Course> GetAll()
        {
            return _allCourses.Values.OrderBy(x => x.Name).ToList();
        }

        public Course Get(int iCourseID)
        {
            if (_allCourses.ContainsKey(iCourseID))
            {
                return _allCourses[iCourseID];
            }

            return null;
        }

    }
}
