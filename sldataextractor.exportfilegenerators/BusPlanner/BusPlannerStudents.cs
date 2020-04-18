using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sldataextractor.exportfilegenerators.BusPlanner
{
    public class BusPlannerStudents : IExportFileGenerator
    {
        private const char separator = '\t';
        private const string stringContainer = "\"";
        private readonly string _dbConnectionString;

        public BusPlannerStudents(ConfigFile ConfigFile, Dictionary<string, string> Arguments)
        {
            this._dbConnectionString = ConfigFile.DatabaseConnectionString;
        }

        public MemoryStream GenerateCSV()
        {
            MemoryStream csvFile = new MemoryStream();
            StreamWriter writer = new StreamWriter(csvFile, Encoding.UTF8);

            int maxContactsToDisplay = 2;

            // Headings

            StringBuilder headingLine = new StringBuilder();
            headingLine.Append("\"SchoolDAN\"" + separator);
            headingLine.Append("\"StudentNumber\"" + separator);
            headingLine.Append("\"FirstName\"" + separator);
            headingLine.Append("\"LastName\"" + separator);
            headingLine.Append("\"BirthDate\"" + separator);
            headingLine.Append("\"Gender\"" + separator);
            headingLine.Append("\"Grade\"" + separator);
            headingLine.Append("\"Phone\"" + separator);
            headingLine.Append("\"House\"" + separator);
            headingLine.Append("\"Street\"" + separator);
            headingLine.Append("\"City\"" + separator);
            headingLine.Append("\"Province\"" + separator);
            headingLine.Append("\"Country\"" + separator);
            headingLine.Append("\"PostalCode\"" + separator);
            headingLine.Append("\"ReserveName\"" + separator);
            headingLine.Append("\"ReserveHouse\"" + separator);
            headingLine.Append("\"LandLocation\"" + separator);
            headingLine.Append("\"Quarter\"" + separator);
            headingLine.Append("\"Section\"" + separator);
            headingLine.Append("\"Township\"" + separator);
            headingLine.Append("\"Range\"" + separator);
            headingLine.Append("\"Meridian\"" + separator);
            headingLine.Append("\"RiverLot\"" + separator);

            for (int x = 1; x <= maxContactsToDisplay; x++)
            {
                headingLine.Append("\"Contact" + x + "ID\"" + separator);
                headingLine.Append("\"Contact" + x + "FirstName\"" + separator);
                headingLine.Append("\"Contact" + x + "LastName\"" + separator);
                headingLine.Append("\"Contact" + x + "Relation\"" + separator);
                headingLine.Append("\"Contact" + x + "HomePhone\"" + separator);
                headingLine.Append("\"Contact" + x + "WorkPhone\"" + separator);
                headingLine.Append("\"Contact" + x + "CellPhone\"" + separator);
                headingLine.Append("\"Contact" + x + "Email\"" + separator);
            }

            writer.WriteLine(headingLine.ToString());



            StudentRepository repo = new StudentRepository(_dbConnectionString);

            foreach (Student student in repo.GetAll())
            {
                StringBuilder csvLine = new StringBuilder();
                csvLine.Append("\"" + student.BaseSchool.DAN); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.StudentNumber); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.FirstName); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LastName); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.DateOfBirth.ToShortDateString()); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Gender); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Grade); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.HomePhone); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address_Physical.HouseNumber); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address_Physical.Street); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address_Physical.City); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address_Physical.Province); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address_Physical.Country); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address_Physical.PostalCode); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.ReserveName); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.ReserveHouse); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Quarter); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Section); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Township); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Range); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Meridian); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.RiverLot); csvLine.Append("\"" + separator);

                // Figure out how many contacts we should display
                int contactsToDisplay = student.Contacts.Count;
                if (contactsToDisplay > maxContactsToDisplay)
                {
                    contactsToDisplay = maxContactsToDisplay;
                }

                if (contactsToDisplay > 0)
                {
                    for (int x = 0; x < contactsToDisplay; x++)
                    {
                        StudentContact sc = student.Contacts[x];
                        if (sc != null)
                        {
                            Contact c = sc.Contact;
                            csvLine.Append("\"" + c.iContactID + "\"" + separator);
                            csvLine.Append("\"" + c.FirstName + "\"" + separator);
                            csvLine.Append("\"" + c.LastName + "\"" + separator);
                            csvLine.Append("\"" + sc.Relation + "\"" + separator);
                            csvLine.Append("\"" + c.HomePhone + "\"" + separator);
                            csvLine.Append("\"" + c.WorkPhone + "\"" + separator);
                            csvLine.Append("\"" + c.CellPhone + "\"" + separator);
                            csvLine.Append("\"" + c.Email + "\"" + separator);
                        }
                    }
                }

                if (csvLine.Length > 0)
                {
                    writer.WriteLine(csvLine.ToString());
                }
            }

            writer.Flush();
            csvFile.Flush();
            return csvFile;
        }
    }
    
}
