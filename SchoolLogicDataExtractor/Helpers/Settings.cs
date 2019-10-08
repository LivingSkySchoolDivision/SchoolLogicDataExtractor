using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchoolLogicDataExtractor
{
    static class Settings
    {
        public static int ClevrTennantID { get { return 148; } }
        public static string EmailDomain { get { return "@lskysd.ca"; } }


        private static string _connectionString = string.Empty;
        public static string dbConnectionString_SchoolLogic
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = LoadConnectionString();
                }

                return _connectionString;
            }
        }

        public static readonly string configFileName = "Config.xml";
        
        private static string LoadConnectionString()
        {
            string returnMe = string.Empty;

            if (!string.IsNullOrEmpty(configFileName))
            {
                if (ConfigFileExists())
                {
                    XElement main = XElement.Load(configFileName);
                    var linqResults = from item in main.Descendants("Database")
                                      select new
                                      {
                                          connectionString = item.Element("ConnectionString").Value
                                      };

                    foreach (var result in linqResults)
                    {
                        returnMe = result.connectionString;
                    }
                }
                else
                {
                    CreateNewConfigFile();
                    throw new ConfigFileNotFoundException("Configuration file does not exist - creating " + configFileName);
                }
            }
            return returnMe;
        }

        public static bool ConfigFileExists()
        {
            if (File.Exists(configFileName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void CreateNewConfigFile()
        {
            string[] configFileLines =
            {
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?>",
                "<Settings>",
                "  <Database>",
                "    <ConnectionString>data source=HOSTNAME;initial catalog=DATABASE;user id=USERNAME;password=PASSWORD;Trusted_Connection=false</ConnectionString>",
                "  </Database>",
                "</Settings>"
            };

            System.IO.File.WriteAllLines(configFileName, configFileLines);
        }

    }
}
