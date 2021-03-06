﻿using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.model
{
    public class StaffMember
    {
        public int iStaffId { get; set; }
        public School School { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string LDAPUserName { get; set; }
        public string EmailAddress { get; set; }

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

        public string TeachingCertificateNumber { get; set; }

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
