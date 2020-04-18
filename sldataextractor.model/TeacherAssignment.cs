using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.model
{
    public class TeacherAssignment
    {
        public StaffMember Teacher { get; set; }
        public SchoolClass Class { get; set; }

    }
}
