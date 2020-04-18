using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sldataextractor.exportfilegenerators.Xello
{
    public class XelloSchools : IExportFileGenerator
    {
        private const string delimiter = "|";
        private const string stringContainer = "";
        private readonly string _dbConnectionString;

        public XelloSchools(ConfigFile ConfigFile, Dictionary<string, string> Arguments)
        {
            this._dbConnectionString = ConfigFile.DatabaseConnectionString;
        }

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("SchoolCode" + delimiter);
            writer.Write("Name" + delimiter);
            writer.Write("SchoolType");
            writer.Write(Environment.NewLine);

            SchoolRepository _schoolRepo = new SchoolRepository(_dbConnectionString);

            foreach (School school in _schoolRepo.GetAll())
            {
                writer.Write(stringContainer + school.DAN + stringContainer + delimiter);
                writer.Write(stringContainer + school.Name + stringContainer + delimiter);
                writer.Write(stringContainer + (school.IsHighSchool ? 1 : 2) + stringContainer);
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
