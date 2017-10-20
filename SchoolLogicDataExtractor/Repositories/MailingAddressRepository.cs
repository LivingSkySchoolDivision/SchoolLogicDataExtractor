using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class MailingAddressRepository
    {
        private Dictionary<int, List<Address>> _cache_Students = new Dictionary<int, List<Address>>();
        private Dictionary<int, List<Address>> _cache_Contacts = new Dictionary<int, List<Address>>();

        private readonly string SQL_ActiveStudentIDs = "SELECT DISTINCT([iStudentID]) FROM StudentStatus WHERE dInDate <= { fn CURDATE() } AND (dOutDate >= { fn CURDATE() } OR dOutDate < '1901-01-01 00:00:00') ORDER BY iStudentID";
        private readonly string SQL_ActiveContacts = "SELECT DISTINCT([iContactID]) FROM [SchoolLogicDB].[dbo].[ContactRelation]";

        private readonly string SQL_StudentAddresses = "SELECT " +
                                                            "LookupValues.cName, " +
                                                            "StudentAddress.iStudentID, " +
                                                            "Location.*, " +
                                                            "Country.cName as Country, " +
                                                            "LV_Province.cName as Province, " +
                                                            "LV_City.cName as City " +
                                                       "FROM " +
                                                            "StudentAddress " +
                                                            "LEFT OUTER JOIN Location ON StudentAddress.iLocationID = Location.iLocationID " +
                                                            "LEFT OUTER JOIN LookupValues ON StudentAddress.iLV_AddressTypeID = LookupValues.iLookupValuesID " +
                                                            "LEFT OUTER JOIN Country ON Location.iCountryID = Country.iCountryID " +
                                                            "LEFT OUTER JOIN LookupValues AS LV_Province ON Location.iLV_RegionID = LV_Province.iLookupValuesID " +
                                                            "LEFT OUTER JOIN LookupValues AS LV_City ON Location.iLV_CityID = LV_City.iLookupValuesID ";

        private readonly string SQL_ContactAddresses = "SELECT " +
                                                            "LookupValues.cName, " +
                                                            "ContactAddress.iContactID, " +
                                                            "Location.*, " +
                                                            "Country.cName as Country, " +
                                                            "LV_Province.cName as Province, " +
                                                            "LV_City.cName as City " +
                                                       "FROM " +
                                                           "ContactAddress " +
                                                           "LEFT OUTER JOIN Location ON ContactAddress.iLocationID = Location.iLocationID " +
                                                           "LEFT OUTER JOIN LookupValues ON ContactAddress.iLV_AddressTypeID = LookupValues.iLookupValuesID " +
                                                            "LEFT OUTER JOIN Country ON Location.iCountryID = Country.iCountryID " +
                                                            "LEFT OUTER JOIN LookupValues AS LV_Province ON Location.iLV_RegionID = LV_Province.iLookupValuesID " +
                                                            "LEFT OUTER JOIN LookupValues AS LV_City ON Location.iLV_CityID = LV_City.iLookupValuesID ";
        public MailingAddressRepository()
        {
            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_SchoolLogic))
            {
                // Get all active student IDs
                List<int> _activeStudentIDs = new List<int>();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQL_ActiveStudentIDs;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int studentID = Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim());
                            if (studentID > 0)
                            {
                                if (!_activeStudentIDs.Contains(studentID))
                                {
                                    _activeStudentIDs.Add(studentID);
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }

                // Get all contact IDs for those student IDs
                List<int> _activeContactIDs = new List<int>();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQL_ActiveContacts + " WHERE iStudentID IN (" + _activeStudentIDs.ToCommaSeparatedString() + ") ORDER BY iContactID"; ;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int contactID = Parsers.ParseInt(dataReader["iContactID"].ToString().Trim());
                            if (contactID > 0)
                            {
                                if (!_activeContactIDs.Contains(contactID))
                                {
                                    _activeContactIDs.Add(contactID);
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }

                // Cache student addresses
                _cache_Students = new Dictionary<int, List<Address>>();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQL_StudentAddresses + " WHERE iStudentID IN (" + _activeStudentIDs.ToCommaSeparatedString() + ") ORDER BY iStudentID"; ;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int id = Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim());
                            if (id > 0)
                            {
                                Address a = dataReaderToAddress(dataReader);
                                if (a != null)
                                {
                                    if (!_cache_Students.ContainsKey(id))
                                    {
                                        _cache_Students.Add(id, new List<Address>());
                                    }
                                    _cache_Students[id].Add(a);
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }

                // Cache contact addresses
                _cache_Contacts = new Dictionary<int, List<Address>>();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQL_ContactAddresses + " WHERE iContactID IN (" + _activeContactIDs.ToCommaSeparatedString() + ") ORDER BY iContactID"; ;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int id = Parsers.ParseInt(dataReader["iContactID"].ToString().Trim());
                            if (id > 0)
                            {
                                Address a = dataReaderToAddress(dataReader);
                                if (a != null)
                                {
                                    if (!_cache_Contacts.ContainsKey(id))
                                    {
                                        _cache_Contacts.Add(id, new List<Address>());
                                    }
                                    _cache_Contacts[id].Add(a);
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }


            }
        }


        private Address dataReaderToAddress(SqlDataReader dataReader)
        {
            return new Address()
            {
                UnitNumber = dataReader["cApartment"].ToString().Trim().ToSingleLine(),
                HouseNumber = dataReader["cHouseNo"].ToString().Trim().ToSingleLine(),
                Street = dataReader["cStreet"].ToString().Trim().ToSingleLine(),
                City = dataReader["City"].ToString().Trim().ToSingleLine(),
                Province = dataReader["Province"].ToString().Trim().ToSingleLine(),
                PostalCode = dataReader["cPostalCode"].ToString().Trim().ToSingleLine(),
                Country = dataReader["Country"].ToString().Trim().ToSingleLine(),
                Phone = dataReader["cPhone"].ToString().Trim()
            };
        }

        public Address GetForStudent(int iStudentID)
        {
            if (_cache_Students.ContainsKey(iStudentID))
            {
                return _cache_Students[iStudentID].First();
            }
            else
            {
                return new Address();
            }
        }

        public Address GetForContact(int iContactID)
        {
            if (_cache_Contacts.ContainsKey(iContactID))
            {
                return _cache_Contacts[iContactID].First();
            }
            else
            {
                return new Address();
            }
        }

    }
}
