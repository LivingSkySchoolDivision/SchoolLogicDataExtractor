using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class Contact
    {
        public int iContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }

        public Contact()
        {
            this.Address = new Address();
        }

        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }

    }
}
