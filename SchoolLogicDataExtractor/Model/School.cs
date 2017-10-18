using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class School
    {
        public int iSchoolID { get; set; }
        public string DAN { get; set; }
        public string Name { get; set; }
        public string GovernmentID { get; set; }


        public bool isFake { get; set; }
    }
}
