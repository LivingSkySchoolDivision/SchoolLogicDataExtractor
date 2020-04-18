using sldataextractor.data;
using sldataextractor.exportfilegenerators;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using sldataextractor.util.Exceptions;
using sldataextractor.util.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace SLExtract
{
    class Program
    {
        private static readonly string _appName = Assembly.GetExecutingAssembly().GetName().Name.ToString();
        private const string _appDescription = "Extracts data from a SchoolLogic database into standard formats, for import into third party systems.";
        private static readonly Dictionary<string, string> _commands = new Dictionary<string, string>()
        {
            { "/filename, /f", "Name of the file to create. Existing files will be overwritten." },
            { "/config, /c", "Filename for the configuration file. If the file does not exist, an example configuration file will be created with the specified name. Defaults to \"config.xml\" if not specified." },
            { "/export, /e", "Which export file to create. See /list for possible options." },
            { "/list, /l", "List available integration files that this program can output." },
            { "/help, /h, /?", "Display this help message." }
        };

        private static readonly Dictionary<string, System.Type> _validReports = new Dictionary<string, System.Type>()
        {
            { "cleverenrollments", typeof(sldataextractor.exportfilegenerators.Clever.CleverEnrollments) },
            { "cleverschools", typeof(sldataextractor.exportfilegenerators.Clever.CleverSchools) },
            { "cleversections", typeof(sldataextractor.exportfilegenerators.Clever.CleverSections) },
            { "cleverstaff", typeof(sldataextractor.exportfilegenerators.Clever.CleverStaff) },
            { "cleverstudents", typeof(sldataextractor.exportfilegenerators.Clever.CleverStudents) },
            { "cleverteachers", typeof(sldataextractor.exportfilegenerators.Clever.CleverTeachers) },
            { "clevrstaff", typeof(sldataextractor.exportfilegenerators.Clevr.ClevrStaffList) },
            { "clevrstudents", typeof(sldataextractor.exportfilegenerators.Clevr.ClevrStudents) },
            { "clevrstudentdemographics", typeof(sldataextractor.exportfilegenerators.Clevr.ClevrStudentDemographics) },
            { "xellocoursecodes", typeof(sldataextractor.exportfilegenerators.Xello.XelloCourseCodes) },
            { "xelloschools", typeof(sldataextractor.exportfilegenerators.Xello.XelloSchools) },
            { "xellostudentcourses", typeof(sldataextractor.exportfilegenerators.Xello.XelloStudentCourses) },
            { "xellostudents", typeof(sldataextractor.exportfilegenerators.Xello.XelloStudents) }
        };

        static void Main(string[] args)
        {
            Dictionary<string, string> Arguments = extractArguments(args);
            ConfigFile configFile = null;
            string destinationFilename = string.Empty;
            string configFileName = "config.xml";
            string reportName = string.Empty;

            try
            {
                if (!Arguments.Any())
                {
                    throw new SyntaxException("Missing: Everything.");
                }

                // Parse arguments
                if (Arguments.ContainsKey("/f")) { destinationFilename = Arguments["/f"]; }
                if (Arguments.ContainsKey("/filename")) { destinationFilename = Arguments["/filename"]; }
                if (Arguments.ContainsKey("/c")) { configFileName = Arguments["/c"]; }
                if (Arguments.ContainsKey("/config")) { configFileName = Arguments["/config"]; }
                if (Arguments.ContainsKey("/export")) { reportName = Arguments["/export"]; }
                if (Arguments.ContainsKey("/e")) { reportName = Arguments["/e"]; }
                if (Arguments.ContainsKey("/list")) { SendAvailableReports(); return; }
                if (Arguments.ContainsKey("/l")) { SendAvailableReports(); return; }
                if (Arguments.ContainsKey("/help")) { SendSyntax(); return; }
                if (Arguments.ContainsKey("/h")) { SendSyntax(); return; }
                if (Arguments.ContainsKey("/?")) { SendSyntax(); return; }

                if (Arguments.ContainsKey("/debug"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" Arguments:");
                    foreach(KeyValuePair<string, string> kvp in Arguments)
                    {
                        Console.WriteLine("  \"" + kvp.Key + "\":\"" + kvp.Value + "\"");
                    }

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" Destination filename: " + destinationFilename);
                    Console.WriteLine(" Config filename: " + configFileName);
                    Console.WriteLine(" Export name: " + reportName);
                    Console.ResetColor();
                }

                // Try to load the config file
                configFile = new ConfigFile(configFileName, true);

                if (Arguments.ContainsKey("/testall"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" TESTING ALL EXPORTS");
                    Stopwatch overallStopwatch = new Stopwatch();
                    Stopwatch perExportStopwatch = new Stopwatch();
                    overallStopwatch.Start();
                    foreach (KeyValuePair<string, System.Type> kvp in _validReports)
                    {
                        perExportStopwatch.Start();
                        Console.Write(" >" + kvp.Key + " (" + kvp.Value + ") ");
                        IExportFileGenerator generator = (IExportFileGenerator)Activator.CreateInstance(kvp.Value,configFile, Arguments);

                        // Try to make the file?
                        MemoryStream exportFileContents = generator.GenerateCSV();
                        SaveFile(exportFileContents, kvp.Key);
                        perExportStopwatch.Stop();
                        Console.WriteLine(perExportStopwatch.Elapsed);
                        perExportStopwatch.Reset();
                    }
                    overallStopwatch.Stop();
                    Console.WriteLine(" Total elapsed: " + overallStopwatch.Elapsed);
                    Console.ResetColor();
                    return;
                }

                if (configFile == null)
                {
                    throw new Exception("Could not open or create config file");
                }

                if (Arguments.ContainsKey("/debug"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" Clevr Tenant ID: " + configFile.ClevrTenantID);
                    Console.WriteLine(" Database Connection String: " + configFile.DatabaseConnectionString);
                    Console.ResetColor();
                }

                if (string.IsNullOrEmpty(destinationFilename))
                {
                    throw new SyntaxException("Missing: Destination filename.");
                }

                if (string.IsNullOrEmpty(reportName))
                {
                    throw new SyntaxException("Missing: Export name.");
                } else
                {
                    if (!_validReports.ContainsKey(reportName.Trim().ToLower()))
                    {
                        throw new InvalidGeneratorException("No exporter with name \"" + reportName + "\" exists");
                    } else
                    {
                        // Try to load the generator 
                        Type generatorType = _validReports[reportName.Trim().ToLower()];
                        if (generatorType != null)
                        {
                            if (typeof(IExportFileGenerator).IsAssignableFrom(generatorType))
                            {
                                IExportFileGenerator generator = (IExportFileGenerator)Activator.CreateInstance(generatorType, new[] { configFile });

                                // Try to make the file?
                                MemoryStream exportFileContents = generator.GenerateCSV();
                                SaveFile(exportFileContents, destinationFilename);                                
                            } else
                            {
                                throw new Exception("The generator type specified in code is not valid. Type must implement IExportFileGenerator. This is an internal error requiring a code change - please alert a developer. Specified type was " + generatorType);
                            }
                        } 
                    }
                }
            }
            catch (InvalidGeneratorException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            } catch (SyntaxException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                SendSyntax();
            } catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        private static Dictionary<string, string> extractArguments(string[] args)
        {
            Dictionary<string, string> returnMe = new Dictionary<string, string>();
            
            for(int x = 0; x < args.Length; x++)
            {
                if (args[x].StartsWith('/'))
                {
                    if (args.Length > x+1)
                    {
                        if (!args[x+1].StartsWith('/'))
                        {
                            returnMe.Add(args[x].Trim().ToLower(), args[x + 1].Trim());
                        } else
                        {
                            returnMe.Add(args[x].Trim().ToLower(), string.Empty);
                        }
                    } else
                    {
                        returnMe.Add(args[x].Trim().ToLower(), string.Empty);
                    }
                }
            }

            return returnMe;
        }

        private static void SendSyntax()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(_appName);
            Console.WriteLine(_appDescription);
            Console.WriteLine("");
            Console.WriteLine("Parameters:");

            // Find the max characters the parameter names use up
            int maxChars = _commands.Keys.Max(x => x.Length);
            int descriptionMaxLength = Console.WindowWidth - maxChars - 8;

            foreach (KeyValuePair<string, string> availableCommand in _commands.OrderBy(x => x.Key))
            {
                Console.Write(" " + availableCommand.Key);

                // Split the description into chunks of a certain number of characters
                foreach (string description in availableCommand.Value.SplitIntoLines(descriptionMaxLength))
                {
                    Console.SetCursorPosition(maxChars + 3, Console.CursorTop);
                    Console.WriteLine(description);
                }
                //Console.WriteLine("");
            }

            Console.ResetColor();
        }

        private static void SendAvailableReports()
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Available exports:");

            // Find the max characters the parameter names use up
            int maxChars = _validReports.Keys.Max(x => x.Length);
            int descriptionMaxLength = Console.WindowWidth - maxChars - 8;

            foreach (string availableReport in _validReports.Keys.OrderBy(x => x))
            {
                if (!string.IsNullOrEmpty(availableReport))
                {
                    Console.WriteLine(" " + availableReport);
                }
            }

            Console.ResetColor();
        }

        private static void SaveFile(MemoryStream fileContent, string fileName)
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
