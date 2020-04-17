using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.model
{
    public class EnrolledStudent
    {
        public Student Student { get; set; }
        public School School { get; set; }
        public DateTime InDate { get; set; }
        public DateTime OutDate { get; set; }

        public bool IsActive()
        {
            return IsActive(DateTime.Now);
        }

        public bool IsActive(DateTime asOfthisDate)
        {
            // If there is no outdate, or the outdate is in the future, it is active
            if (this.OutDate <= new DateTime(1920, 01, 01))
            {
                return true;
            }

            if (this.OutDate > asOfthisDate)
            {
                return true;
            }

            return false;
        }
    }
}
