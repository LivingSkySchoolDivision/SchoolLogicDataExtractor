using sldataextractor.util.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace sldataextractor.util.Configfile
{
    public class ConfigFile
    {
        public string DatabaseConnectionString { get; set; }
        public string ClevrTenantID { get; set; }

        public ConfigFile(string FileName, bool CreateIfDoesntExist)
        {
            if (File.Exists(FileName))
            {
                XElement main = XElement.Load(FileName);

                // Connection String
                foreach (var result in 
                    from item in main.Descendants("Database")
                        select new
                        {
                            connectionString = item.Element("ConnectionString").Value
                        })
                {
                    DatabaseConnectionString = result.connectionString;
                }

                // Clevr Tenant ID
                foreach (var result in
                    from item in main.Descendants("Clevr")
                    select new
                    {
                        connectionString = item.Element("TenantId").Value
                    })
                {
                    ClevrTenantID = result.connectionString;
                }
            } else
            {
                if (CreateIfDoesntExist)
                {
                    CreateNewConfigFile(FileName);
                } else
                {
                    throw new ConfigurationFileNotFoundException();
                }
            }
        }

        private void CreateNewConfigFile(string FileName)
        {
            string[] configFileLines =
            {
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?>",
                "<Settings>",
                "  <Database>",
                "    <ConnectionString>data source=HOSTNAME;initial catalog=DATABASE;user id=USERNAME;password=PASSWORD;Trusted_Connection=false</ConnectionString>",
                "  </Database>",
                "  <Clevr>",
                "    <TenantId>000</TenantId>",
                "  </Clevr>",
                "</Settings>"
            };

            System.IO.File.WriteAllLines(FileName, configFileLines);
        }

    }
}
