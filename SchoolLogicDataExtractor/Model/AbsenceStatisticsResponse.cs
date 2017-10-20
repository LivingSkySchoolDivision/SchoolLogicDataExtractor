using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    public class AbsenceStatisticsResponse
    {
        public int iStudentID { get; set; }
        public int TotalEntries { get; set; }
        public int Lates { get; set; }
        public int LeaveEarlies { get; set; }
        public int UnexcusedAbsences { get; set; }
        public int ExcusedAbsences { get; set; }

        public int TotalLateMinutes { get; set; }
        public int TotalLeaveEarlyMinutes { get; set; }

        public int Absences => this.UnexcusedAbsences + this.ExcusedAbsences;
        public int LatesOrLeaveEarlies => this.Lates + this.LeaveEarlies;


        public AbsenceStatisticsResponse(int studentID)
        {
            this.iStudentID = studentID;
        }

        public AbsenceStatisticsResponse() { }
    }
}
