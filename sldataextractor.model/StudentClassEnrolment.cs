using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.model
{
    public class StudentClassEnrolment
    {
        public SchoolClass Class { get; set; }
        public Student Student { get; set; }

        public DateTime InDate { get; set; }
        public DateTime OutDate { get; set; }
        public string Status { get; set; }
        public bool IsCurrent { get; set; }
    }
}
