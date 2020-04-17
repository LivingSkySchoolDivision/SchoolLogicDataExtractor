using sldataextractor.model;
using sldataextractor.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace sldataextractor.data
{
    public class TeacherAssignmentRepository
    {
        // Get assignments from 2 places:
        //  Teacher resources
        //  Primary and secondary teachers for classes

        StaffRepository _staffRepo;
        SchoolClassRepository _schoolClassRepo;

        private readonly string sql_Class = "SELECT iClassID, iDefault_StaffID FROM Class";
        private readonly string sql_Resource = "SELECT iClassID, iStaffID FROM ClassResource";

        private Dictionary<int, List<int>> _classIdsWithAllTeachers = new Dictionary<int, List<int>>();

        public TeacherAssignmentRepository(string ConnectionString)
        {
            this._staffRepo = new StaffRepository(ConnectionString);
            this._schoolClassRepo = new SchoolClassRepository(ConnectionString);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                // Class table
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = sql_Class;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int istaffid = Parsers.ParseInt(dataReader["iDefault_StaffID"].ToString().Trim());
                            int iclassid = Parsers.ParseInt(dataReader["iClassID"].ToString().Trim());

                            if ((istaffid > 0) && (iclassid > 0))
                            {
                                // Add the class if it doesn't already exist
                                if (!_classIdsWithAllTeachers.ContainsKey(iclassid))
                                {
                                    _classIdsWithAllTeachers.Add(iclassid, new List<int>());
                                }

                                // Add the teacher, if they don't already exist (there will be duplicates in SchoolLogic)
                                if (!_classIdsWithAllTeachers[iclassid].Contains(istaffid))
                                {
                                    _classIdsWithAllTeachers[iclassid].Add(istaffid);
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }

                // Resources table
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = sql_Resource;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int istaffid = Parsers.ParseInt(dataReader["iStaffID"].ToString().Trim());
                            int iclassid = Parsers.ParseInt(dataReader["iClassID"].ToString().Trim());

                            if ((istaffid > 0) && (iclassid > 0))
                            {
                                // Add the class if it doesn't already exist
                                if (!_classIdsWithAllTeachers.ContainsKey(iclassid))
                                {
                                    _classIdsWithAllTeachers.Add(iclassid, new List<int>());
                                }

                                // Add the teacher, if they don't already exist (there will be duplicates in SchoolLogic)
                                if (!_classIdsWithAllTeachers[iclassid].Contains(istaffid))
                                {
                                    _classIdsWithAllTeachers[iclassid].Add(istaffid);
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }

            }
        }

        public List<TeacherAssignment> GetAll()
        {
            List<TeacherAssignment> returnMe = new List<TeacherAssignment>();

            foreach (int classid in _classIdsWithAllTeachers.Keys)
            {
                SchoolClass parsedclass = _schoolClassRepo.Get(classid);
                if (parsedclass != null)
                {
                    foreach (int staffid in _classIdsWithAllTeachers[classid])
                    {
                        StaffMember parsedstaff = _staffRepo.Get(staffid);
                        if (parsedstaff != null)
                        {
                            returnMe.Add(new TeacherAssignment()
                            {
                                Teacher = parsedstaff,
                                Class = parsedclass
                            });
                        }
                    }
                }
            }

            return returnMe;
        }


    }
}
