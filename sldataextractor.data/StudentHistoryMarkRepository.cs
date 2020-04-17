using sldataextractor.model;
using sldataextractor.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace sldataextractor.data
{
    public class StudentHistoryMarkRepository
    {
        private readonly Dictionary<int, List<StudentHistoryMark>> _cache;

        private const string SQLQuery = "SELECT " +
                                            "iMarksHistoryID, " +
                                            "iStudentID, " +
                                            "MarksHistory.iSchoolID, " +
                                            "iCourseID, " +
                                            "cCourseCode, " +
                                            "cCourseDesc as cCourseName, " +
                                            "nFinalMark, " +
                                            "cAlphaMark, " +
                                            "nCreditEarned, " +
                                            "nCreditPossible, " +
                                            "Grades.cName as Grade, " +
                                            "dEndDate " +
                                          "FROM MarksHistory  " +
                                            "LEFT OUTER JOIN Grades ON MarksHistory.iGradesID=Grades.iGradesID	 " +
                                          "ORDER BY iStudentID ";


        private StudentHistoryMark dataReaderToHistoryMark(SqlDataReader dataReader)
        {
            return new StudentHistoryMark()
            {
                iHistoryMarkID = Parsers.ParseInt(dataReader["iMarksHistoryID"].ToString().Trim()),
                iStudentID = Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim()),
                iSchoolID = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim()),
                CourseID = Parsers.ParseInt(dataReader["iCourseID"].ToString().Trim()),
                CourseCode = dataReader["cCourseCode"].ToString().Trim(),
                CourseName = dataReader["cCourseName"].ToString().Trim(),
                NumericMark = Parsers.ParseDecimal(dataReader["nFinalMark"].ToString().Trim()),
                AlphaMark = dataReader["cAlphaMark"].ToString().Trim(),
                CreditsEarned = Parsers.ParseDecimal(dataReader["nCreditEarned"].ToString().Trim()),
                CreditsPossible = Parsers.ParseDecimal(dataReader["nCreditPossible"].ToString().Trim()),
                Grade = dataReader["Grade"].ToString().Trim(),
                CompletionDate = Parsers.ParseDate(dataReader["dEndDate"].ToString().Trim()),
            };
        }

        public StudentHistoryMarkRepository(string ConnectionString)
        {
            _cache = new Dictionary<int, List<StudentHistoryMark>>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQLQuery;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            StudentHistoryMark shm = dataReaderToHistoryMark(dataReader);
                            if (shm != null)
                            {
                                if (!_cache.ContainsKey(shm.iStudentID))
                                {
                                    _cache.Add(shm.iStudentID, new List<StudentHistoryMark>());
                                }
                                _cache[shm.iStudentID].Add(shm);
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        public List<StudentHistoryMark> GetForStudent(int iStudentID)
        {
            if (_cache.ContainsKey(iStudentID))
            {
                return _cache[iStudentID];
            }

            return new List<StudentHistoryMark>();
        }

        public List<StudentHistoryMark> GetForStudent(Student student)
        {
            return GetForStudent(student.iStudentID);
        }



    }
}
