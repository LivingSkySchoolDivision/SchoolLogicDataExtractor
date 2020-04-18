using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sldataextractor.exportfilegenerators.Xello
{
    public class XelloSchools : ExportFileGenerator, IExportFileGenerator
    {
        private const string delimiter = "|";
        private const string stringContainer = "";

        public XelloSchools(ConfigFile ConfigFile, Dictionary<string, string> Arguments) : base(ConfigFile, Arguments) { }

        public MemoryStream Generate()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);

            // Headings
            writer.Write("SchoolCode" + delimiter);
            writer.Write("Name" + delimiter);
            writer.Write("SchoolType");
            writer.Write(Environment.NewLine);

            SchoolRepository _schoolRepo = new SchoolRepository(_configFile.DatabaseConnectionString);

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
