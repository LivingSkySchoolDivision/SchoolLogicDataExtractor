using System;
using System.Collections.Generic;
using System.Text;

namespace sldataextractor.model
{
    public class LandDescription
    {        
        // Example of how this is displayed
        // NW-28-37-18-W3
        // Quarter: NW
        // Section: 28
        // Township: 37
        // Range: 18
        // Meridian: W3 (As I understand this is always 'W' for West)

        public string Quarter { get; set; }
        public string Section { get; set; }
        public string Township { get; set; }
        public string Range { get; set; }
        public string Meridian { get; set; }
        public string RiverLot { get; set; }

        public override string ToString()
        {
            if (
                (string.IsNullOrEmpty(Quarter)) &&
                (string.IsNullOrEmpty(Section)) &&
                (string.IsNullOrEmpty(Township)) &&
                (string.IsNullOrEmpty(Range)) &&
                (string.IsNullOrEmpty(Meridian))
                )
            {
                return string.Empty;
            }
            else
            {
                return this.Quarter + "-" + this.Section + "-" + this.Township + "-" + this.Range + "-W" + this.Meridian;
            }
        }

    }
}
