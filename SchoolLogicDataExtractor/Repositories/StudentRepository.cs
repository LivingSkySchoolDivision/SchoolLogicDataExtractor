using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SchoolLogicDataExtractor
{
    class StudentRepository
    {
        private readonly Dictionary<int, Student> _allStudents = new Dictionary<int, Student>();
        private readonly SchoolRepository _schoolRepo = new SchoolRepository();
        private readonly ContactRepository _contactRepo = new ContactRepository();

        private string SQL = "SELECT         " +
                                  "StudentStatus.iStudentID,  " +
                                  "StudentStatus.dInDate,  " +
                                  "StudentStatus.dOutDate,  " +
                                  "StudentStatus.lOutsideStatus,  " +
                                  "StudentStatus.iSchoolID,  " +
                                  "LookupValues_1.cName AS Gender,  " +
                                  "LookupValues_1.cCode AS GenderCode,  " +
                                  "Student.cStudentNumber,  " +
                                  "Student.cFirstName,  " +
                                  "Student.cLastName,  " +
                                  "Student.cLegalFirstName,  " +
                                  "Student.cLegalLastName,  " +
                                  "Student.cLegalMiddleName,  " +
                                  "Student.dBirthdate,  " +
                                  "Student.iTrackID,  " +
                                  "Student.iSchoolID AS Expr1,  " +
                                  "Student.mMedical,  " +
                                  "Location.cApartment,  " +
                                  "Location.cHouseNo,  " +
                                  "Location.cPostalCode,  " +
                                  "Location.cStreet,  " +
                                  "LookupValues_2.cName AS City,  " +
                                  "LookupValues_3.cName AS Province,  " +
                                  "Country_1.cName AS Country,  " +
                                  "Student.cUserName,  " +
                                  "Homeroom.cName AS HomeroomName,  " +
                                  "Staff.cFirstName AS HomeroomTeacherfirstname,  " +
                                  "Staff.cLastName AS HomeroomTeacherLastname,  " +
                                  "Student.iPrevious_SchoolID, " +
                                  "(SELECT COUNT(*) AS Expr1 FROM MarksHistory WHERE (iStudentID = StudentStatus.iStudentID) AND (nCreditEarned > 0)) AS Credits,  " +
                                  "LookupValues_4.cName AS LanguageSpokenAtHome,  " +
                                  "LookupValues.cName AS CountryOfOrigin,  " +
                                  "Grades.cName AS Grade " +
                            "FROM " +
                                    "UserStudent " +
                                    "RIGHT OUTER JOIN Country AS Country_1 " +
                                    "RIGHT OUTER JOIN Location ON Country_1.iCountryID = Location.iCountryID " +
                                    "RIGHT OUTER JOIN Student ON Location.iLocationID = Student.iLocationID " +
                                    "ON UserStudent.iStudentID = Student.iStudentID " +
                                    "LEFT OUTER JOIN Grades ON Student.iGradesID = Grades.iGradesID " +
                                    "LEFT OUTER JOIN LookupValues ON UserStudent.UF_1657 = LookupValues.iLookupValuesID " +
                                    "LEFT OUTER JOIN LookupValues AS LookupValues_4 ON UserStudent.UF_1653 = LookupValues_4.iLookupValuesID " +
                                    "LEFT OUTER JOIN Homeroom " +
                                    "LEFT OUTER JOIN Staff ON Homeroom.i1_StaffID = Staff.iStaffID " +
                                    "ON Student.iHomeroomID = Homeroom.iHomeroomID " +
                                    "LEFT OUTER JOIN LookupValues AS LookupValues_3 ON Location.iLV_RegionID = LookupValues_3.iLookupValuesID " +
                                    "LEFT OUTER JOIN LookupValues AS LookupValues_2 ON Location.iLV_CityID = LookupValues_2.iLookupValuesID " +
                                    "LEFT OUTER JOIN LookupValues AS LookupValues_1 ON Student.iLV_GenderID = LookupValues_1.iLookupValuesID " +
                                    "RIGHT OUTER JOIN StudentStatus ON Student.iStudentID = StudentStatus.iStudentID " +

                            "WHERE         " +
                                  "(StudentStatus.lOutsideStatus = 0)  " +
                                  "AND (StudentStatus.dInDate <= { fn CURDATE() })  " +
                                  "AND (StudentStatus.dOutDate >= { fn CURDATE() } OR StudentStatus.dOutDate < CONVERT(DATETIME, '1901-01-01 00:00:00', 102))  " +
                                  "AND (Student.iTrackID <> 0) ";

        public StudentRepository()
        {
            _allStudents = new Dictionary<int, Student>();

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
                            _allStudents.Add(Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim()), dataReaderToStudent(dataReader));
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        private Student dataReaderToStudent(SqlDataReader dataReader)
        {
            return new Student()
            {
                StudentNumber = dataReader["cStudentNumber"].ToString().Trim(),
                iStudentID = Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim()),
                FirstName = dataReader["cFirstName"].ToString().Trim(),
                LastName = dataReader["cLastName"].ToString().Trim(),
                MiddleName = dataReader["cLegalMiddleName"].ToString().Trim(),
                LegalFirstName = dataReader["cLegalFirstName"].ToString().Trim(),
                LegalLastName = dataReader["cLegalLastName"].ToString().Trim(),
                DateOfBirth = Parsers.ParseDate(dataReader["dBirthdate"].ToString().Trim()),
                isCurrentlyEnrolled = true,
                LDAPUserName = dataReader["cUsername"].ToString().Trim(),
                Gender = dataReader["Gender"].ToString().Trim(),
                GenderInitial = dataReader["GenderCode"].ToString().Trim(),
                Grade = dataReader["Grade"].ToString().Trim(),
                BaseSchool = _schoolRepo.Get(Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim())),
                Contacts = _contactRepo.GetForStudent(Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim())),
                Address_Physical = new Address()
                {
                    UnitNumber = dataReader["cApartment"].ToString().Trim(),
                    HouseNumber = dataReader["cHouseNo"].ToString().Trim(),
                    Street = dataReader["cStreet"].ToString().Trim(),
                    City = dataReader["City"].ToString().Trim(),
                    Province = dataReader["Province"].ToString().Trim(),
                    PostalCode = dataReader["cPostalCode"].ToString().Trim(),
                    Country = dataReader["Country"].ToString().Trim(),
                },
                PreviousSchool = _schoolRepo.Get(Parsers.ParseInt(dataReader["iPrevious_SchoolID"].ToString().Trim())),
                HomeLanguage = dataReader["LanguageSpokenAtHome"].ToString().Trim(),
                CountryOfOrigin = dataReader["CountryOfOrigin"].ToString().Trim(),
                Homeroom = dataReader["HomeroomName"].ToString().Trim(),
                CreditsEarned = Parsers.ParseDecimal(dataReader["Credits"].ToString().Trim()),
            };
        }

        public List<Student> GetAll()
        {
            return _allStudents.Values.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }
    }
}
