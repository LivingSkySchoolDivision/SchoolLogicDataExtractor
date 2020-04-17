using sldataextractor.model;
using sldataextractor.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace sldataextractor.data
{
    public class SchoolClassRepository
    {
        private CourseRepository _courseRepo;
        private SchoolRepository _schoolRepo;

        private readonly string sql = "SELECT iClassID, iCourseID, cName, cSection, iSchoolID, (SELECT COUNT(*) FROM Enrollment WHERE Enrollment.iClassID=Class.iClassID AND iLV_CompletionStatusID=0) as NumEnrolled FROM Class";

        private Dictionary<int, SchoolClass> _allClasses = new Dictionary<int, SchoolClass>();

        private SchoolClass dataReaderToSchoolClass(SqlDataReader dataReader)
        {
            int parsediCourseID = Parsers.ParseInt(dataReader["iCourseID"].ToString().Trim());
            Course parsedCourse = _courseRepo.Get(parsediCourseID);

            int schoolID = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim());
            School parsedSchool = _schoolRepo.Get(schoolID);

            return new SchoolClass()
            {
                iClassID = Parsers.ParseInt(dataReader["iClassID"].ToString().Trim()),
                Name = dataReader["cName"].ToString().Trim(),
                iCourseID = parsediCourseID,
                Section = dataReader["cSection"].ToString().Trim(),
                EnrolledStudents = Parsers.ParseInt(dataReader["NumEnrolled"].ToString().Trim()),
                Course = parsedCourse,
                iSchoolID = schoolID,
                School = parsedSchool
            };
        }

        public SchoolClassRepository(string ConnectionString)
        {
            this._courseRepo = new CourseRepository(ConnectionString);
            this._schoolRepo = new SchoolRepository(ConnectionString);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                            _allClasses.Add(Parsers.ParseInt(dataReader["iClassID"].ToString().Trim()), dataReaderToSchoolClass(dataReader));
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        public List<SchoolClass> GetAll()
        {
            return _allClasses.Values.ToList();
        }

        public SchoolClass Get(int iClassID)
        {
            return _allClasses.ContainsKey(iClassID) ? _allClasses[iClassID] : null;
        }

    }
}
