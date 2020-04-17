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
    public class StaffRepository
    {
        SchoolRepository _schoolRepo;
        List<int> ignoredStaffIDs = new List<int>()
        {
            1039, // ADMIN
            1849, // Subsec - bready
            1856, // Subsec - bcs
            1847, // Subsec - cando
            1832, // Subsec - ckes
            1822, // Subsec - ckhs
            1817, // Subsec - conn
            1840, // Subsec - hafford
            1857, // Subsec - hces
            1858, // Subsec - hcs
            1818, // Subsec - kcs
            1821, // Subsec - law
            1827, // Subsec - leo
            1834, // Subsec - luse
            1829, // Subsec - mack
            1974, // Subsec - mack 2
            1843, // Subsec - may
            1826, // Subsec - mck
            1859, // Subsec - mcl 
            1845, // Subsec - med
            1828, // Subsec - nbchs
            1820, // Subsec - nces
            1833, // Subsec - shs
            1819, // Subsec - stv
            1860, // Subsec - uchs
            1825, // Subsec - ups
            1900, // SubSSO - NBCHS
            1779, // egov
            1780, // egov
            1781, // egov
            1782, // egov
            1783, // egov
            1784, // egov
            1785, // egov
            1786, // egov
            1787, // egov
            1788, // egov
            1789, // egov
            1790, // egov
            1791, // egov
            1792, // egov
            1793, // egov
            1794, // egov
            1795, // egov
            1796, // egov
            1797, // egov
            1798, // egov
            1799, // egov
            1800, // egov
            1801, // egov
            1802, // egov
            1803, // egov
            1804, // egov
            1805, // egov
            1806, // egov
            1807, // egov
            1808, // egov
            1809, // egov
            1810, // egov
            1811, // egov
            1812, // egov
            1813, // egov
            1814, // egov
        };
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
                                        "Staff.cLDAPName, " +
                                        "UserStaff.UF_2085 as TeacherCertNum, " +
                                        "LookupValues.cName AS cRole " +
                                    "FROM            " +
                                        "LookupValues " +
                                        "RIGHT OUTER JOIN UserStaff ON LookupValues.iLookupValuesID = UserStaff.iCLEVRRoleid " +
                                        "RIGHT OUTER JOIN Staff ON  UserStaff.iStaffID = Staff.iStaffID";

        public StaffRepository(string ConnectionString)
        {
            this._schoolRepo = new SchoolRepository(ConnectionString);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                                if (!ignoredStaffIDs.Contains(s.iStaffId))
                                    if (!_allStaff.ContainsKey(s.iStaffId))
                                    {
                                        _allStaff.Add(s.iStaffId, s);
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
                iStaffId = Parsers.ParseInt(dataReader["iStaffID"].ToString().Trim()),
                FirstName = dataReader["cFirstName"].ToString().Trim(),
                LastName = dataReader["cLastName"].ToString().Trim(),
                DateOfBirth = Parsers.ParseDate(dataReader["dBirthDate"].ToString().Trim()),
                LDAPUserName = dataReader["cUserName"].ToString().Trim(),
                Role = dataReader["cRole"].ToString().Trim(),
                IsEnabled = !Parsers.ParseBool(dataReader["lInactive"].ToString().Trim()),
                School = _schoolRepo.Get(Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim())),
                TeachingCertificateNumber = dataReader["TeacherCertNum"].ToString().Trim()
            };
        }

        public List<StaffMember> GetAll()
        {
            return _allStaff.Values.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }

        public StaffMember Get(int iStaffID)
        {
            return _allStaff.ContainsKey(iStaffID) ? _allStaff[iStaffID] : null;
        }
    }
}
