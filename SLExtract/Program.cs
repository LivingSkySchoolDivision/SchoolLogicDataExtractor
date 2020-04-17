using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using sldataextractor.util.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SLExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load the config file
            ConfigFile configFile = new ConfigFile("config.xml", false);

            if (args.Any())
            {
                string filename = string.Empty;

                foreach (string argument in args)
                {
                    if (argument.ToLower().StartsWith("/filename:"))
                    {
                        filename = argument.Substring(10, argument.Length - 10);
                    }
                }

                if (string.IsNullOrEmpty(filename))
                {
                    throw new SyntaxException("Filename argument is required");
                }

                // Generate the report 

                MemoryStream reportContents = new MemoryStream();

                // Save the report
                SaveFile(reportContents, filename);
            }
            else
            {
                throw new SyntaxException("Not enough arguments");
            }

        }

        static void SendSyntax()
        {
            Console.WriteLine("SYNTAX:");
            Console.WriteLine("");
            Console.WriteLine(" REQUIRED:");
            Console.WriteLine("  /filename:filename.txt");
            Console.WriteLine("  Specify the file name");
        }

        public static void SaveFile(MemoryStream fileContent, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            if (fileName.Length <= 1) return;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (FileStream fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
            {
                fileContent.WriteTo(fileStream);
                fileStream.Flush();
            }
        }
    }
}
