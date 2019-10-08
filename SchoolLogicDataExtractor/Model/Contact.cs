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
        public Address Address_Physical { get; set; }
        public Address Address_Mailing { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string CellPhone { get; set; }
        public string DisplayName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public string DisplayNameLastNameFirst
        {
            get
            {
                return this.LastName + ", " + this.FirstName;
            }
        }

        public Contact()
        {
            this.Address_Physical = new Address();
            this.Address_Mailing = new Address();
        }

        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }

    }
}
