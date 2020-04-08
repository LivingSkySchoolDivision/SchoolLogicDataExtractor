using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class Student
    {
        /* ** Fields for the student file ** */
        public string StudentNumber { get; set; }
        public int iStudentID { get; set; }
        public School BaseSchool { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string LegalFirstName { get; set; }
        public string LegalLastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public bool isCurrentlyEnrolled { get; set; }
        public string EmailAddress { get; set; }
        public string LDAPUserName { get; set; }
        public string Role { get { return "Student"; } }
        public string Gender { get; set; }
        public string GenderInitial { get; set; }
        public string GradeUnformatted { get; set; }
        public string Grade
        {
            get
            {
                string returnMe;

                if (this.GradeUnformatted.ToLower() == "0k")
                {
                    returnMe = "K";
                }
                else if (this.GradeUnformatted.ToLower() == "k")
                {
                    returnMe = "K";
                }
                else if (this.GradeUnformatted.ToLower() == "pk")
                {
                    returnMe = "PK";
                }
                else
                {
                    returnMe = Parsers.ParseInt(this.GradeUnformatted).ToString();
                }

                return returnMe;
            }
        }

        public Address Address_Physical { get; set; }
        public Address Address_Mailing { get; set; }
        public AbsenceStatisticsResponse YearToDateAttendanceStatistics { get; set; }

        public List<StudentContact> Contacts { get; set; }
        public School PreviousSchool { get; set; }
        public string LanguageProgram { get; set; }
        public string HomeLanguage { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Homeroom { get; set; }
        public string HomeroomTeacher { get; set; }
        public decimal CreditsEarned { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string Medical { get; set; }
        public string SaskLearningNumber { get; set; }
        public string DisplayName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public string DisplayNameLastNameFirst
        {
            get
            {
                return this.LastName + ", " + this.FirstName;
            }
        }

        public Student()
        {
            this.Address_Mailing = new Address();
            this.Address_Physical = new Address();
            this.YearToDateAttendanceStatistics = new AbsenceStatisticsResponse();
            this.BaseSchool = new School();
            this.PreviousSchool = new School();
            this.Contacts = new List<StudentContact>();
        }

        public override string ToString()
        {
            return this.DisplayNameLastNameFirst + " (" + this.BaseSchool.Name + ")";
        }
    }
}
