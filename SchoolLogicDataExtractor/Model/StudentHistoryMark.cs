using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Model
{
    public class StudentHistoryMark
    {
        public int iHistoryMarkID { get; set; }
        public int iStudentID { get; set; }
        public int iSchoolID { get; set; }

        public int CourseID { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }

        public decimal NumericMark { get; set; }
        public string AlphaMark { get; set; }
        public decimal CreditsEarned { get; set; }
        public decimal CreditsPossible { get; set; }
        public string Grade { get; set; }
        public DateTime CompletionDate { get; set; }
        
    }
}
