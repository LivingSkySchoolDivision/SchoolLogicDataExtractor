using sldataextractor.model;
using sldataextractor.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace sldataextractor.data
{
    public class EnrolledStudentRepository
    {
        private StudentRepository _studentRepo;
        private SchoolRepository _schoolRepo;
        private string _connectionString;

        private string sql = "SELECT iStudentID, dInDate, dOutDate, iSchoolID, lOutsideStatus FROM StudentStatus";

        private EnrolledStudent dataReaderToEnrolledStudent(SqlDataReader dataReader)
        {
            Student s = _studentRepo.Get(Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim()));
            if (s == null) return null;

            return new EnrolledStudent()
            {
                Student = s,
                School = _schoolRepo.Get(Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim())),
                InDate = Parsers.ParseDate(dataReader["dInDate"].ToString().Trim()),
                OutDate = Parsers.ParseDate(dataReader["dOutDate"].ToString().Trim()),
            };
        }


        public List<EnrolledStudent> GetEnrollmentsFor(DateTime from, DateTime to)
        {
            List<EnrolledStudent> returnMe = new List<EnrolledStudent>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = sql + " WHERE ((StudentStatus.dOutDate = @NULLDATE) OR (StudentStatus.dOutDate>@STARTTIME)) AND (StudentStatus.dInDate<=@ENDTIME)";
                    sqlCommand.Parameters.AddWithValue("NULLDATE", new DateTime(1900, 1, 1, 0, 0, 0));
                    sqlCommand.Parameters.AddWithValue("STARTTIME", from);
                    sqlCommand.Parameters.AddWithValue("ENDTIME", to);
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            EnrolledStudent s = dataReaderToEnrolledStudent(dataReader);
                            if (s != null)
                            {
                                returnMe.Add(s);
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }

            return returnMe;
        }

        public List<EnrolledStudent> GetEnrollmentsFor(DateTime thisday)
        {
            return GetEnrollmentsFor(thisday, thisday);
        }


        public EnrolledStudentRepository(string ConnectionString)
        {
            _connectionString = ConnectionString;
            _studentRepo = new StudentRepository(_connectionString);
            _schoolRepo = new SchoolRepository(_connectionString);
        }

    }
}
