using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class StudentContact
    {
        public Contact Contact { get; set; }
        public int iContactID { get; set; }
        public string Relation { get; set; }
        public bool LivesWithStudent { get; set; }
        public int Priority { get; set; }

        public override string ToString()
        {
            return this.Contact.ToString() + " (" + this.Relation + ")";
        }
    }
}
