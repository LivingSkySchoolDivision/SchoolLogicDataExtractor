using sldataextractor.model;
using sldataextractor.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace sldataextractor.data
{
    public class StudentClassEnrolmentRepository
    {
        // Keep a list of students and classes
        StudentRepository _studentRepo;
        SchoolClassRepository _schoolClassRepo;

        private string sql = "SELECT iStudentID, iClassID, dInDate, dOutDate, iLV_CompletionStatusID, LookupValues.cName as CompletionStatus FROM Enrollment LEFT OUTER JOIN LookupValues ON Enrollment.iLV_CompletionStatusID = LookupValues.iLookupValuesID ORDER BY iClassID ASC, iStudentID ASC";

        List<StudentClassEnrolment> _allEnrolments = new List<StudentClassEnrolment>();
        private Dictionary<int, List<StudentClassEnrolment>> _enrolmentsByStudentID = new Dictionary<int, List<StudentClassEnrolment>>();
        private Dictionary<int, List<StudentClassEnrolment>> _enrolmentsByClassID = new Dictionary<int, List<StudentClassEnrolment>>();

        private StudentClassEnrolment dataReaderToEnrolment(SqlDataReader dataReader)
        {
            int parsediclassid = Parsers.ParseInt(dataReader["iClassID"].ToString().Trim());
            int parsedistudentid = Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim());

            Student student = _studentRepo.Get(parsedistudentid);
            SchoolClass sclass = _schoolClassRepo.Get(parsediclassid);
            if ((student == null) || (sclass == null))
            {
                return null;
            }

            return new StudentClassEnrolment()
            {
                InDate = Parsers.ParseDate(dataReader["dInDate"].ToString().Trim()),
                OutDate = Parsers.ParseDate(dataReader["dOutDate"].ToString().Trim()),
                Status = dataReader["CompletionStatus"].ToString().Trim(),
                Student = student,
                Class = sclass
            };
        }

        public StudentClassEnrolmentRepository(string ConnectionString)
        {
            this._studentRepo = new StudentRepository(ConnectionString);
            this._schoolClassRepo = new SchoolClassRepository(ConnectionString);

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
                            StudentClassEnrolment sce = dataReaderToEnrolment(dataReader);
                            if (sce != null)
                            {
                                if ((sce.Student != null) && (sce.Class != null))
                                {

                                    // add entry by student Id                            
                                    if (!_enrolmentsByStudentID.ContainsKey(sce.Student.iStudentID))
                                    {
                                        _enrolmentsByStudentID.Add(sce.Student.iStudentID, new List<StudentClassEnrolment>());
                                    }
                                    _enrolmentsByStudentID[sce.Student.iStudentID].Add(sce);

                                    // add entry by class id
                                    if (!_enrolmentsByClassID.ContainsKey(sce.Class.iClassID))
                                    {
                                        _enrolmentsByClassID.Add(sce.Class.iClassID, new List<StudentClassEnrolment>());
                                    }
                                    _enrolmentsByClassID[sce.Class.iClassID].Add(sce);

                                    // add to general list
                                    _allEnrolments.Add(sce);
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        public List<StudentClassEnrolment> GetAll()
        {
            return _allEnrolments;
        }

        public List<StudentClassEnrolment> GetForStudent(int iStudentID)
        {
            return _enrolmentsByStudentID.ContainsKey(iStudentID) ? _enrolmentsByStudentID[iStudentID] : new List<StudentClassEnrolment>();
        }

        public List<StudentClassEnrolment> GetForClass(int iClassID)
        {
            return _enrolmentsByClassID.ContainsKey(iClassID) ? _enrolmentsByClassID[iClassID] : new List<StudentClassEnrolment>();
        }

        public Dictionary<int, int> GetEnrolmentCountsByClassID()
        {
            Dictionary<int, int> returnMe = new Dictionary<int, int>();

            foreach (int classID in _enrolmentsByClassID.Keys)
            {
                returnMe.Add(classID, _enrolmentsByClassID[classID].Count);
            }

            return returnMe;
        }

    }
}
