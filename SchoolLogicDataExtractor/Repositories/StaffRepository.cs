using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SchoolLogicDataExtractor
{
    class StaffRepository
    {
        SchoolRepository _schoolRepo = new SchoolRepository();
        private Dictionary<int, StaffMember> _allStaff = new Dictionary<int, StaffMember>();

        private readonly string SQL = "SELECT " +
                                        "Staff.iStaffID, " +
                                        "Staff.cFirstName, " +
                                        "Staff.cLastName, " +
                                        "UserStaff.dBirthDate, " +
                                        "Staff.iSchoolID, " +
                                        "Staff.cUserName, " +
                                        "LookupValues.cName, " +
                                        "Staff.lInactive, " +
                                        "Staff.lAccountLocked, " +
                                        "Staff.cLDAPName AS cRole " +
                                    "FROM            " +
                                        "LookupValues " +
                                        "RIGHT OUTER JOIN UserStaff ON LookupValues.iLookupValuesID = UserStaff.iCLEVRRoleid " +
                                        "RIGHT OUTER JOIN Staff ON  UserStaff.iStaffID = Staff.iStaffID";

        public StaffRepository()
        {
            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_SchoolLogic))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQL;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            StaffMember s = dataReaderToStaffMember(dataReader);
                            if (s != null)
                            {
                                if (!_allStaff.ContainsKey(s.ID))
                                {
                                    _allStaff.Add(s.ID, s);
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        private StaffMember dataReaderToStaffMember(SqlDataReader dataReader)
        {
            return new StaffMember()
            {
                ID = Parsers.ParseInt(dataReader["iStaffID"].ToString().Trim()),
                FirstName = dataReader["cFirstName"].ToString().Trim(),
                LastName = dataReader["cLastName"].ToString().Trim(),
                DateOfBirth = Parsers.ParseDate(dataReader["dBirthDate"].ToString().Trim()),
                LDAPUserName = dataReader["cUserName"].ToString().Trim(),
                Role = dataReader["cRole"].ToString().Trim(),
                IsEnabled = Parsers.ParseBool(dataReader["lInactive"].ToString().Trim()),
                School = _schoolRepo.Get(Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim()))
            };
        }

        public List<StaffMember> GetAll()
        {
            return _allStaff.Values.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }
    }
}
