using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.model
{
    public class StudentContact
    {
        public Contact Contact { get; set; }
        public int iContactID { get; set; }
        public string Relation { get; set; }
        public bool LivesWithStudent { get; set; }
        public int Priority { get; set; }
        public bool CanAccessHomelogic { get; set; }

        public StudentContact()
        {
            this.Contact = new Contact();
        }

        public override string ToString()
        {
            return this.Contact.ToString() + " (" + this.Relation + ")";
        }
    }
}
