using sldataextractor.data;
using sldataextractor.model;
using sldataextractor.util.Configfile;
using System;
using System.Collections.Generic;

namespace SLExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load the config file
            ConfigFile configFile = new ConfigFile("config.xml", false);

            // Load some students

            StudentRepository studentRepo = new StudentRepository(configFile.DatabaseConnectionString);

            List<Student> students = studentRepo.GetAll();

            foreach(Student s in students)
            {
                Console.WriteLine(s);
            }



        }
    }
}
