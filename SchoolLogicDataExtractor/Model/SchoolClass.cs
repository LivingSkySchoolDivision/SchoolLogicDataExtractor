using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Model
{
    class SchoolClass
    {
        public int iClassID { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string GovCourseCode { get; set; }
        public decimal CreditValue { get; set; }
        public string GradeLower { get; set; }
        public string GradeUpper { get; set; }

    }
}
