using SchoolLogicDataExtractor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extract_Clever_Staff
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
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
                    SchoolLogicDataExtractor.Reports.Clever.CleverStaff generator = new SchoolLogicDataExtractor.Reports.Clever.CleverStaff();
                    MemoryStream reportContents = generator.GenerateCSVFile();

                    // Save the report
                    FileHelpers.SaveFile(reportContents, filename);
                }
                else
                {
                    throw new SyntaxException("Not enough arguments");
                }
            }
            catch (SyntaxException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                SendSyntax();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
    }
}
