using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    public class Address
    {
        public string UnitNumber { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

        public string ToStringSingleLine()
        {
            return this.ToString().Replace("\n", "");
        }

        public bool IsPostOfficeBox
        {
            get
            {
                return (UnitNumber.ToLower().Contains("box ")) || (HouseNumber.ToLower().Contains("box ")) || (Street.ToLower().Contains("box "));
            }
        }

        public bool HasValues
        {
            get
            {
                if (
                    (!string.IsNullOrEmpty(this.Street)) ||
                    (!string.IsNullOrEmpty(this.UnitNumber)) ||
                    (!string.IsNullOrEmpty(this.City)) ||
                    (!string.IsNullOrEmpty(this.Province)) ||
                    (!string.IsNullOrEmpty(this.PostalCode))
                    )
                {
                    return true;
                }
                return false;
            }
        }

        public string ToStringHTML()
        {
            return this.ToString().Replace("\n", "<br>");
        }

        public override string ToString()
        {
            StringBuilder address = new StringBuilder();

            if (!string.IsNullOrEmpty(this.UnitNumber))
            {
                address.Append(this.UnitNumber);
                address.Append(" ");
            }

            if (!string.IsNullOrEmpty(this.HouseNumber))
            {
                address.Append(this.HouseNumber);
                address.Append(" ");
            }

            if (!string.IsNullOrEmpty(this.Street))
            {
                address.Append(this.Street);
                address.Append(", ");
            }

            if (!string.IsNullOrEmpty(this.City))
            {
                address.Append(this.City);
                address.Append(" ");
            }

            if (!string.IsNullOrEmpty(this.Province))
            {
                address.Append(this.Province);
                address.Append(" ");
            }

            if (!string.IsNullOrEmpty(this.PostalCode))
            {
                address.Append(this.PostalCode);
                address.Append(", ");
            }

            if (!string.IsNullOrEmpty(this.Country))
            {
                address.Append(this.Country);
            }

            return address.ToString();
        }



        public static implicit operator string(Address a)
        {
            return a.ToString();
        }
    }
}
