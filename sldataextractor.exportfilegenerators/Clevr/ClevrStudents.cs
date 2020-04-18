using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace sldataextractor.exportfilegenerators.Clevr
{
    public class ClevrStudents : ExportFileGenerator, IExportFileGenerator
    {
        private const string delimiter = "\t";
        private const string stringContainer = "";

        public ClevrStudents(ConfigFile ConfigFile, Dictionary<string, string> Arguments) : base(ConfigFile, Arguments) { }

        public MemoryStream Generate()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("tenantId" + delimiter);
            writer.Write("proprietaryId" + delimiter);
            writer.Write("localId" + delimiter);
            writer.Write("primaryLocation" + delimiter);
            writer.Write("lastName" + delimiter);
            writer.Write("firstName" + delimiter);
            writer.Write("middleName" + delimiter);
            writer.Write("commonName" + delimiter);
            writer.Write("birthdate" + delimiter);
            writer.Write("status" + delimiter);
            writer.Write("email" + delimiter);
            writer.Write("gender" + delimiter);
            writer.Write("role" + delimiter);
            writer.Write("username" + delimiter);
            writer.Write("password" + delimiter);
            writer.Write("grade");
            writer.Write(Environment.NewLine);

            StudentRepository _studentRepo = new StudentRepository(_configFile.DatabaseConnectionString);

            foreach (Student student in _studentRepo.GetAllActive().OrderBy(s => s.BaseSchool.Name).ThenBy(s => s.DisplayNameLastNameFirst))
            {
                writer.Write(_configFile.ClevrTenantID + "" + delimiter);
                writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                writer.Write(stringContainer + student.StudentNumber + stringContainer + delimiter);
                writer.Write(stringContainer + student.BaseSchool.DAN + stringContainer + delimiter);
                writer.Write(stringContainer + student.LegalLastName + stringContainer + delimiter);
                writer.Write(stringContainer + student.LegalFirstName + stringContainer + delimiter);
                writer.Write(stringContainer + student.MiddleName + stringContainer + delimiter);
                writer.Write(stringContainer + student.FirstName + stringContainer + delimiter);
                writer.Write(stringContainer + student.DateOfBirth.ToShortDateString() + stringContainer + delimiter);
                writer.Write("1" + delimiter); // status 1=active 0=inactive
                writer.Write(stringContainer + student.EmailAddress + stringContainer + delimiter);
                writer.Write(stringContainer + student.GenderInitial + stringContainer + delimiter);
                writer.Write(stringContainer + "student" + stringContainer + delimiter);
                writer.Write(stringContainer + student.LDAPUserName + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + student.Grade + stringContainer);
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
