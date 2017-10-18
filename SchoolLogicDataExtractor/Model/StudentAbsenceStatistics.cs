using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class StudentAbsenceStatistics
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalAbsences { get; set; }
        public int UnexcusedAbsences { get; set; }
        public int ExcusedAbsences { get; set; }
        public int TotalLates { get; set; }
        public int UnexcusedLates { get; set; }
        public int ExcusedLates { get; set; }
        public int TotalLateMinutes { get; set; }
        public int UnexcusedLateMinutes { get; set; }
        public int ExcusedLateMinutes { get; set; }
    }
}
