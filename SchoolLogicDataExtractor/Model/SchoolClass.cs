using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class SchoolClass
    {
        public int iClassID { get; set; }
        public string Name { get; set; }
        public int iCourseID { get; set; }
        public Course Course { get; set; }
        public string Section { get; set; }
        public int iSchoolID { get; set; }
        public School School { get; set; }
        public int EnrolledStudents { get; set; }
    }
}
