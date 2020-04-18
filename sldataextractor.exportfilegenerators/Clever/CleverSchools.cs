using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace sldataextractor.exportfilegenerators.Clever
{
    public class CleverSchools : IExportFileGenerator
    {
        private const char delimiter = ',';
        private const string stringContainer = "\"";
        private readonly string _dbConnectionString;

        public CleverSchools(ConfigFile ConfigFile, Dictionary<string, string> Arguments)
        {
            this._dbConnectionString = ConfigFile.DatabaseConnectionString;
        }

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("School_id" + delimiter);
            writer.Write("School_name" + delimiter);
            writer.Write("School_number" + delimiter);

            writer.Write("State_id" + delimiter);
            writer.Write("Low_grade" + delimiter);
            writer.Write("High_grade" + delimiter);
            writer.Write("Principal" + delimiter);
            writer.Write("Principal_email" + delimiter);
            writer.Write("School_address" + delimiter);
            writer.Write("School_city" + delimiter);
            writer.Write("School_state" + delimiter);
            writer.Write("School_zip" + delimiter);
            writer.Write("School_phone" + delimiter);

            writer.Write(Environment.NewLine);

            SchoolRepository _schoolRepo = new SchoolRepository(_dbConnectionString);
            List<School> schools = _schoolRepo.GetAll();

            foreach (School school in schools.Where(x => x.isFake == false))
            {
                writer.Write(stringContainer + school.DAN + stringContainer + delimiter);
                writer.Write(stringContainer + school.Name + stringContainer + delimiter);
                writer.Write(stringContainer + school.DAN + stringContainer + delimiter);

                writer.Write(stringContainer + "" + stringContainer + delimiter); // State ID

                // Parse K and PreK into what Clever expects
                string lowGrade = school.LowGrade;
                if (lowGrade.ToLower() == "pk")
                {
                    lowGrade = "PreKindergarten";
                }

                if ((lowGrade.ToLower() == "0k") || (lowGrade.ToLower() == "k"))
                {
                    lowGrade = "Kindergarten";
                }

                if (string.IsNullOrEmpty(lowGrade))
                {
                    lowGrade = "Ungraded";
                }

                string highGrade = school.HighGrade;
                if (highGrade.ToLower() == "pk")
                {
                    highGrade = "PreKindergarten";
                }

                if ((highGrade.ToLower() == "0k") || (highGrade.ToLower() == "k"))
                {
                    highGrade = "Kindergarten";
                }

                if (string.IsNullOrEmpty(highGrade))
                {
                    highGrade = "Ungraded";
                }

                writer.Write(stringContainer + lowGrade + stringContainer + delimiter); // Low grade
                writer.Write(stringContainer + highGrade + stringContainer + delimiter); // High greade
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Principal
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Principal email
                writer.Write(stringContainer + "" + stringContainer + delimiter); // School address
                writer.Write(stringContainer + "" + stringContainer + delimiter); // School city
                writer.Write(stringContainer + school.Region + stringContainer + delimiter); // School state
                writer.Write(stringContainer + "" + stringContainer + delimiter); // School ZIP
                writer.Write(stringContainer + "" + stringContainer + delimiter); // School phone

                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
