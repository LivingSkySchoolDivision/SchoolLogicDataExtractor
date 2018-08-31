using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Sangha
{
    public class SanghaSections
    {
        // All classes
        // Should be just a list of courses

        private const string delimiter = ",";
        private const string stringContainer = "\"";
        private readonly Encoding encoding = Encoding.ASCII;

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream, encoding);

            // Headings
            // Section Id,Class Id,Section Number,Period,Start,End,SchoolId
            writer.Write("Section Id" + delimiter);
            writer.Write("Class Id" + delimiter);
            writer.Write("Section Number" + delimiter);
            writer.Write("Period" + delimiter);
            writer.Write("Start" + delimiter);
            writer.Write("End" + delimiter);
            writer.Write("SchoolId" + delimiter);
            writer.Write(Environment.NewLine);

            SchoolClassRepository _scr = new SchoolClassRepository();



            foreach (SchoolClass sc in _scr.GetAll().Where(x => Helpers.SanghaSettings.AllowedSchoolGovIDs.Contains(x.School.DAN)))
            {
                writer.Write(stringContainer + sc.iClassID + stringContainer + delimiter); // Section ID
                writer.Write(stringContainer + sc.iCourseID + stringContainer + delimiter); // Class ID
                writer.Write(stringContainer + sc.Section + stringContainer + delimiter); // Section Number
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Period
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Start
                writer.Write(stringContainer + "" + stringContainer + delimiter); // End
                writer.Write(stringContainer + sc.School.GovernmentID + stringContainer + delimiter); // SchoolID
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
