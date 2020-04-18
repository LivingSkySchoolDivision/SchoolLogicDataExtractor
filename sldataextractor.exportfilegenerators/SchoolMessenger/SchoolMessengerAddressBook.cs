using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace sldataextractor.exportfilegenerators.SchoolMessenger
{
    public class SchoolMessengerAddressBook : ExportFileGenerator, IExportFileGenerator
    {
        private const string delimiter = ",";
        private const string stringContainer = "\"";

        public SchoolMessengerAddressBook(ConfigFile ConfigFile, Dictionary<string, string> Arguments) : base(ConfigFile, Arguments) { }


        public MemoryStream Generate()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings            
            writer.Write(stringContainer + "SchoolID" + stringContainer + delimiter);
            writer.Write(stringContainer + "StudentID" + stringContainer + delimiter);
            writer.Write(stringContainer + "StudentLastName" + stringContainer + delimiter);
            writer.Write(stringContainer + "StudentFirstName" + stringContainer + delimiter);
            writer.Write(stringContainer + "Grade" + stringContainer + delimiter);
            writer.Write(stringContainer + "HomeRoom" + stringContainer + delimiter);
            writer.Write(stringContainer + "ContactLastName" + stringContainer + delimiter);
            writer.Write(stringContainer + "ContactFirstName" + stringContainer + delimiter);
            writer.Write(stringContainer + "ContactRelation" + stringContainer + delimiter);
            writer.Write(stringContainer + "ContactPriority" + stringContainer + delimiter);
            writer.Write(stringContainer + "ContactHomePhone" + stringContainer + delimiter);
            writer.Write(stringContainer + "ContactMobilePhone" + stringContainer + delimiter);
            writer.Write(stringContainer + "ContactWorkPhone" + stringContainer + delimiter);
            writer.Write(stringContainer + "ContactEmail" + stringContainer + delimiter);
            writer.Write(Environment.NewLine);

            StudentRepository _studentRepo = new StudentRepository(_configFile.DatabaseConnectionString);

            foreach (Student student in _studentRepo.GetAllActive().OrderBy(s => s.BaseSchool.DAN))
            {
                foreach (StudentContact contact in student.Contacts.Where(contact => contact.LivesWithStudent || contact.NotifyOverride))
                {
                    writer.Write(stringContainer + student.BaseSchool.GovernmentID + stringContainer + delimiter);
                    writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                    writer.Write(stringContainer + student.LastName + stringContainer + delimiter);
                    writer.Write(stringContainer + student.FirstName + stringContainer + delimiter);
                    writer.Write(stringContainer + student.Grade + stringContainer + delimiter);
                    writer.Write(stringContainer + student.Homeroom + stringContainer + delimiter);
                    writer.Write(stringContainer + contact.Contact.LastName + stringContainer + delimiter);
                    writer.Write(stringContainer + contact.Contact.FirstName + stringContainer + delimiter);
                    writer.Write(stringContainer + contact.Relation + stringContainer + delimiter);
                    writer.Write(stringContainer + contact.Priority + stringContainer + delimiter);
                    writer.Write(stringContainer + contact.Contact.HomePhone + stringContainer + delimiter);
                    writer.Write(stringContainer + contact.Contact.CellPhone + stringContainer + delimiter);
                    writer.Write(stringContainer + contact.Contact.WorkPhone + stringContainer + delimiter);
                    writer.Write(stringContainer + contact.Contact.Email + stringContainer + delimiter);
                    writer.Write(Environment.NewLine);
                }
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
