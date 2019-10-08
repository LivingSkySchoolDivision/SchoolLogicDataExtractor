using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Helpers
{
    class SanghaSettings
    {
        // Filter all reports to only include school gov Ids on this list
        public static List<string> AllowedSchoolGovIDs = new List<string>()
        {
            "5810211", //Battleford Central School
            "5850201", //Bready Elementary School
            "5010213", //Cando Community School
            "5910111", //Cut Knife Elementary School
            "5910123", //Cut Knife High School
            "5850401", //Connaught Elementary School
            "5710213", //Hafford Central School
            "6410721", //Hartley Clark Elementary School
            "5894003", //Heritage Christian School
            "4410223", //Kerrobert Composite School
            "5850501", //Lawrence Elementary School
            "6410313", //Leoville Central School
            "4410323", //Luseland School
            "4410413", //Macklin School
            "5810713", //Maymont Central School
            "5850601", //McKitrick Community School
            "5910923", //McLurg High School
            "6694003", //Meadow Lake Christian Academy
            "6410513", //Medstead Central School
            "5910911", //Norman Carter Elementary School
            "5850904", //North Battleford Comprehensive High School
            "6410713", //Spiritwood High School
            "5810221", //St. Vital Catholic School
            "5910813", //Unity Composite High School
            "5910711", //Unity Public School
        };
    }
}
