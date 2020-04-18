using sldataextractor.model;
using sldataextractor.util;
using sldataextractor.util.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace sldataextractor.data
{
    public class StudentRepository
    {
        private readonly Dictionary<int, Student> _allStudents = new Dictionary<int, Student>();
        private readonly SchoolRepository _schoolRepo;
        private readonly ContactRepository _contactRepo;
        private readonly AbsenceStatisticsRepository _absenceRepo;
        private readonly MailingAddressRepository _mailingAddressRepo;

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
                                    "Student.mEmail,  " +
                                    "Student.cGovernmentNumber, " +
                                    "Location.cPhone,  " +
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
                                    "Grades.cName AS Grade, " +
                                    "Student.mCellPhone , " +
                                    "LookupValues_5.cName AS mProgram, " +
                                    "LV_LD_Q.cName AS Quarter, " +
                                    "LV_LD_S.cName AS Section, " +
                                    "LV_LD_T.cName AS Township, " +
                                    "LV_LD_R.cName AS Range, " +
                                    "LV_LD_M.cName AS Meridian, " +
                                    "LV_LD_RL.cName AS RiverLot, " +
                                    "UserStudent.cReserveName, " +
                                    "UserStudent.cReserveHouse " +
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
                                    "LEFT OUTER JOIN LookupValues AS LookupValues_5 ON UserStudent.UF_1658=LookupValues_5.iLookupValuesID " +
                                    "LEFT OUTER JOIN LookupValues AS LV_LD_Q ON UserStudent.UF_1651=LV_LD_Q.iLookupValuesID " +
                                    "LEFT OUTER JOIN LookupValues AS LV_LD_S ON UserStudent.UF_2098=LV_LD_S.iLookupValuesID " +
                                    "LEFT OUTER JOIN LookupValues AS LV_LD_T ON UserStudent.UF_1653_1=LV_LD_T.iLookupValuesID " +
                                    "LEFT OUTER JOIN LookupValues AS LV_LD_R ON UserStudent.UF_1654_1=LV_LD_R.iLookupValuesID " +
                                    "LEFT OUTER JOIN LookupValues AS LV_LD_M ON UserStudent.UF_2093=LV_LD_M.iLookupValuesID " +
                                    "LEFT OUTER JOIN LookupValues AS LV_LD_RL ON UserStudent.UF_2096=LV_LD_RL.iLookupValuesID " +

                            "WHERE         " +
                                  "(StudentStatus.lOutsideStatus = 0)  " +
                                  "AND (StudentStatus.dInDate <= { fn CURDATE() })  " +
                                  "AND (StudentStatus.dOutDate >= { fn CURDATE() } OR StudentStatus.dOutDate < CONVERT(DATETIME, '1901-01-01 00:00:00', 102))  " +
                                  "AND (Student.iTrackID <> 0) ";

        public StudentRepository(string ConnectionString)
        {
            this._schoolRepo = new SchoolRepository(ConnectionString);
            this._contactRepo = new ContactRepository(ConnectionString);
            this._absenceRepo = new AbsenceStatisticsRepository(ConnectionString);
            this._mailingAddressRepo = new MailingAddressRepository(ConnectionString);

            _allStudents = new Dictionary<int, Student>();

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
                            Student s = dataReaderToStudent(dataReader);
                            if (!_allStudents.ContainsKey(s.iStudentID))
                            {
                                _allStudents.Add(s.iStudentID, s);
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        private Student dataReaderToStudent(SqlDataReader dataReader)
        {
            string homeroomTeacher = (dataReader["HomeroomTeacherfirstname"].ToString() + " " + dataReader["HomeroomTeacherLastname"].ToString()).Trim();

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
                GradeUnformatted = dataReader["Grade"].ToString().Trim(),
                SaskLearningNumber = dataReader["cGovernmentNumber"].ToString().Trim(),
                BaseSchool = _schoolRepo.Get(Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim())),
                Contacts = _contactRepo.GetForStudent(Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim())),
                Address_Physical = new Address()
                {
                    UnitNumber = dataReader["cApartment"].ToString().Trim().ToSingleLine(),
                    HouseNumber = dataReader["cHouseNo"].ToString().Trim().ToSingleLine(),
                    Street = dataReader["cStreet"].ToString().Trim().ToSingleLine(),
                    City = dataReader["City"].ToString().Trim().ToSingleLine(),
                    Province = dataReader["Province"].ToString().Trim().ToSingleLine(),
                    PostalCode = dataReader["cPostalCode"].ToString().Trim().ToSingleLine(),
                    Country = dataReader["Country"].ToString().Trim().ToSingleLine(),
                    Phone = dataReader["cPhone"].ToString().Trim(),
                },
                LandDescription = new LandDescription()
                {
                    Quarter = dataReader["Quarter"].ToString().Trim(),
                    Section = dataReader["Section"].ToString().Trim(),
                    Township = dataReader["Township"].ToString().Trim(),
                    Range = dataReader["Range"].ToString().Trim(),
                    Meridian = dataReader["Meridian"].ToString().Trim(),
                    RiverLot = dataReader["RiverLot"].ToString().Trim(),
                },
                ReserveName = dataReader["cReserveName"].ToString().Trim(),
                ReserveHouse = dataReader["cReserveHouse"].ToString().Trim(),
                PreviousSchool = _schoolRepo.Get(Parsers.ParseInt(dataReader["iPrevious_SchoolID"].ToString().Trim())),
                HomeLanguage = dataReader["LanguageSpokenAtHome"].ToString().Trim(),
                CountryOfOrigin = dataReader["CountryOfOrigin"].ToString().Trim(),
                LanguageProgram = dataReader["mProgram"].ToString().Trim(),
                Homeroom = dataReader["HomeroomName"].ToString().Trim(),
                HomeroomTeacher = homeroomTeacher,
                Medical = dataReader["mMedical"].ToString().Trim().ToSingleLine(),
                CreditsEarned = Parsers.ParseDecimal(dataReader["Credits"].ToString().Trim()),
                YearToDateAttendanceStatistics = _absenceRepo.Get(Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim())),
                CellPhone = dataReader["mCellPhone"].ToString().Trim(),
                HomePhone = dataReader["cPhone"].ToString().Trim(),
                Address_Mailing = _mailingAddressRepo.GetForStudent(Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim())),
                EmailAddress = dataReader["mEmail"].ToString().Trim()

            };
        }

        public List<Student> GetAll()
        {
            return _allStudents.Values.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }

        public Student Get(int iStudentID)
        {
            if (_allStudents.ContainsKey(iStudentID))
            {
                return _allStudents[iStudentID];
            }
            else
            {
                return null;
            }
        }
    }
}
