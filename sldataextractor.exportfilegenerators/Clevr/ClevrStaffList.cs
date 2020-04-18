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
    public class ClevrStaffList : IExportFileGenerator
    {
        private const char delimiter = '\t';
        private const string stringContainer = "";
        private readonly string _dbConnectionString;
        private readonly string _clevrTenantID = string.Empty;

        public ClevrStaffList(ConfigFile ConfigFile, Dictionary<string, string> Arguments)
        {
            this._dbConnectionString = ConfigFile.DatabaseConnectionString;
            this._clevrTenantID = ConfigFile.ClevrTenantID;
        }

        public MemoryStream GenerateCSV()
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
            writer.Write(Environment.NewLine);

            StaffRepository _staffRepo = new StaffRepository(_dbConnectionString);
            List<StaffMember> allStaff = _staffRepo.GetAll();
            foreach (StaffMember staff in allStaff.Where(s => s.IsEnabled))
            {
                writer.Write(_clevrTenantID + "" + delimiter);
                writer.Write(stringContainer + staff.iStaffId + stringContainer + delimiter);
                writer.Write(stringContainer + staff.iStaffId + stringContainer + delimiter);
                writer.Write(stringContainer + staff.School.DAN + stringContainer + delimiter);
                writer.Write(stringContainer + staff.LastName + stringContainer + delimiter);
                writer.Write(stringContainer + staff.FirstName + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + staff.FirstName + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write((staff.IsEnabled ? 1 : 0) + "" + delimiter); // status 1=active 0=inactive
                writer.Write(stringContainer + staff.EmailAddress + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + staff.Role + stringContainer + delimiter);
                writer.Write(stringContainer + staff.LDAPUserName + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(Environment.NewLine);
            }


            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
