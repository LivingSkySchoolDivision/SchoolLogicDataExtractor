using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.model
{
    public class School
    {
        public int iSchoolID { get; set; }
        public string DAN { get { return this.GovernmentID; } }
        public string Name { get; set; }
        public string GovernmentID { get; set; }
        public bool isFake { get; set; }
        public string LowGrade { get; set; }
        public string HighGrade { get; set; }

        public string Address { get; set; }

        /// <summary>
        /// Province/State
        /// </summary>
        public string Region { get; set; }

        public bool IsHighSchool
        {
            get
            {
                return this.HighGrade == "12";
            }
        }
    }
}
