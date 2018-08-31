using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class StaffMember
    {
        public int iStaffId { get; set; }
        public School School { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string LDAPUserName { get; set; }
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

        public string Gender { get; set; }
        public string Role { get; set; }
        public bool IsEnabled { get; set; }
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
            string returnMe = this.DisplayNameLastNameFirst + " (" + this.School.Name + ")";
            if (!this.IsEnabled)
            {
                returnMe += " (INACTIVE)";
            }
            return returnMe;
        }
    }
}
