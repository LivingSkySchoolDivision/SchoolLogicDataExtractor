using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.model
{
    public class Course
    {
        public int iCourseID { get; set; }
        public string CourseCode { get; set; }
        public string Name { get; set; }
        public string LowGrade { get; set; }
        public string HighGrade { get; set; }
        public decimal LowCredit { get; set; }
        public decimal HighCredit { get; set; }
        public bool CurrentlyOffered { get; set; }

        public decimal Credits
        {
            get
            {
                if (HighCredit > LowCredit)
                {
                    return HighCredit;
                }
                else
                {
                    return LowCredit;
                }
            }
        }
    }
}
