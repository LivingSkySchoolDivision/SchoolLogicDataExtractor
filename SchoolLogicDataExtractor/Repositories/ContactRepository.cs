using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SchoolLogicDataExtractor
{
    class ContactRepository
    {
        // Get list of currently enrolled student IDs
        // Get list of contacts associated with those students
        // Load only those contacts

        private readonly MailingAddressRepository _mailingAddressRepo = new MailingAddressRepository();

        Dictionary<int, Contact> _allContacts = new Dictionary<int, Contact>();
        Dictionary<int, List<StudentContact>> _contactRelationsByStudentID = new Dictionary<int, List<StudentContact>>();

        private readonly string SQL_ActiveStudentIDs = "SELECT DISTINCT([iStudentID]) FROM StudentStatus WHERE dInDate <= { fn CURDATE() } AND (dOutDate >= { fn CURDATE() } OR dOutDate < '1901-01-01 00:00:00') ORDER BY iStudentID";
        private readonly string SQL_ActiveContacts = "SELECT DISTINCT([iContactID]) FROM [SchoolLogicDB].[dbo].[ContactRelation]";
        private readonly string SQL_Contacts = "SELECT " +
                                                        "Contact.iContactID,  " +
                                                        "Contact.cFirstName,  " +
                                                        "Contact.cLastName,  " +
                                                        "Contact.mEmail,  " +
                                                        "Contact.mCellphone,  " +
                                                        "Contact.cBusPhone,  " +
                                                        "Location.cApartment,  " +
                                                        "Location.cHouseNo,  " +
                                                        "Location.cStreet,  " +
                                                        "LookupValues.cName AS City,  " +
                                                        "LookupValues_2.cName AS Province,  " +
                                                        "Country.cName AS Country,  " +
                                                        "Location.cPhone,  " +
                                                        "Location.cPostalCode " +
                                                    "FROM             " +
                                                        "Country  " +
                                                        "RIGHT OUTER JOIN Location ON Country.iCountryID = Location.iCountryID  " +
                                                        "LEFT OUTER JOIN LookupValues AS LookupValues_2 ON Location.iLV_RegionID = LookupValues_2.iLookupValuesID  " +
                                                        "LEFT OUTER JOIN LookupValues ON Location.iLV_CityID = LookupValues.iLookupValuesID  " +
                                                        "RIGHT OUTER JOIN Contact ON Location.iLocationID = Contact.iLocationID ";
        private readonly string SQL_ContactRelations = "SELECT " +
                                                            "ContactRelation.iContactRelationID,  " +
                                                            "ContactRelation.iContactID,  " +
                                                            "LookupValues.cName AS Relation,  " +
                                                            "ContactRelation.iStudentID,  " +
                                                            "ContactRelation.lLivesWithStudent,  " +
                                                            "ContactRelation.lMail,  " +
                                                            "ContactRelation.iContactPriority,  " +
                                                            "ContactRelation.iSchoolID " +
                                                        "FROM             " +
                                                            "ContactRelation  " +
                                                            "LEFT OUTER JOIN LookupValues ON ContactRelation.iLV_RelationID = LookupValues.iLookupValuesID ";

        public ContactRepository()
        {
            // Cache all contacts for all active staff

            // Get all active student IDs
            // Get all contact IDs for those student IDs
            // Load all of those contacts, putting them in collections by student ID
            // Get contact relations for all contacts




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

                // Load all of those contacts, putting them in collections by student ID
                _allContacts = new Dictionary<int, Contact>();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQL_Contacts + " WHERE iContactID IN (" + _activeContactIDs.ToCommaSeparatedString() + ") ORDER BY iContactID";
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int contactID = Parsers.ParseInt(dataReader["iContactID"].ToString().Trim());
                            if (contactID > 0)
                            {
                                Contact c = dataReaderToContact(dataReader);

                                if (c != null)
                                {
                                    if (!_allContacts.ContainsKey(contactID))
                                    {
                                        _allContacts.Add(contactID, c);
                                    }
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }

                // Get contact relations for all contacts
                _contactRelationsByStudentID = new Dictionary<int, List<StudentContact>>();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQL_ContactRelations + " WHERE iStudentID IN (" + _activeStudentIDs.ToCommaSeparatedString() + ") ORDER BY iStudentID"; ; ;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int studentID = Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim());
                            if (studentID > 0)
                            {
                                StudentContact sc = dataReaderToStudentContact(dataReader);
                                if (sc != null)
                                {
                                    if (!_contactRelationsByStudentID.ContainsKey(studentID))
                                    {
                                        _contactRelationsByStudentID.Add(studentID, new List<StudentContact>());
                                    }
                                    _contactRelationsByStudentID[studentID].Add(sc);
                                }
                            }

                        }
                    }
                    sqlCommand.Connection.Close();
                }

            }
        }

        private StudentContact dataReaderToStudentContact(SqlDataReader dataReader)
        {
            int contactID = Parsers.ParseInt(dataReader["iContactID"].ToString().Trim());
            Contact c = Get(contactID);
            if (c != null)
            {
                return new StudentContact()
                {
                    Contact = c,
                    iContactID = contactID,
                    Relation = dataReader["Relation"].ToString().Trim(),
                    LivesWithStudent = Parsers.ParseBool(dataReader["lLivesWithStudent"].ToString().Trim()),
                    Priority = Parsers.ParseInt(dataReader["iContactPriority"].ToString().Trim())
                };
            }
            else
            {
                return null;
            }
        }

        private Contact dataReaderToContact(SqlDataReader dataReader)
        {
            return new Contact()
            {
                iContactID = Parsers.ParseInt(dataReader["iContactID"].ToString().Trim()),
                FirstName = dataReader["cFirstName"].ToString().Trim(),
                LastName = dataReader["cLastName"].ToString().Trim(),
                Email = dataReader["mEmail"].ToString().Trim(),
                HomePhone = dataReader["cPhone"].ToString().Trim(),
                WorkPhone = dataReader["cBusPhone"].ToString().Trim(),
                CellPhone = dataReader["mCellPhone"].ToString().Trim(),
                Address_Physical = new Address()
                {
                    UnitNumber = dataReader["cApartment"].ToString().Trim().ToSingleLine(),
                    HouseNumber = dataReader["cHouseNo"].ToString().Trim().ToSingleLine(),
                    Street = dataReader["cStreet"].ToString().Trim().ToSingleLine(),
                    City = dataReader["City"].ToString().Trim().ToSingleLine(),
                    Province = dataReader["Province"].ToString().Trim().ToSingleLine(),
                    PostalCode = dataReader["cPostalCode"].ToString().Trim().ToSingleLine(),
                    Country = dataReader["Country"].ToString().Trim().ToSingleLine()
                },
                Addrses_Mailing = _mailingAddressRepo.GetForContact(Parsers.ParseInt(dataReader["iContactID"].ToString().Trim()))
            };
        }

        public Contact Get(int contactID)
        {
            if (_allContacts.ContainsKey(contactID))
            {
                return _allContacts[contactID];
            }
            else
            {
                return null;
            }
        }

        public List<StudentContact> GetForStudent(int iStudentID)
        {
            if (_contactRelationsByStudentID.ContainsKey(iStudentID))
            {
                return _contactRelationsByStudentID[iStudentID];
            }
            else
            {
                return new List<StudentContact>();
            }
        }

    }
}
