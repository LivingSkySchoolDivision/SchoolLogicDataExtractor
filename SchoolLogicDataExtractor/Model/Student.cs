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
        public string EmailAddress
        {
            get
            {
                if (string.IsNullOrEmpty(this.LDAPUserName))
                {
                    return string.Empty;
                }
                else
                {
                    return this.LDAPUserName + Settings.EmailDomain;
                }
            }
        }
        public string LDAPUserName { get; set; }
        public string Role { get { return "Student"; } }
        public string Gender { get; set; }
        public string GenderInitial { get; set; }
        public string GradeUnformatted { get; set; }
        public string Grade { get; set; }
                
        public Address Address_Physical { get; set; }

        public List<StudentContact> Contacts { get; set; }
        public School PreviousSchool { get; set; }
        public string HomeLanguage { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Homeroom { get; set; }
        public decimal CreditsEarned { get; set; }

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

        public override string ToString()
        {
            return this.DisplayNameLastNameFirst + " (" + this.BaseSchool.Name + ")";
        }
    }
}
