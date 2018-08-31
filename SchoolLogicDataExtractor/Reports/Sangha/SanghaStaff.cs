using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor.Reports.Sangha
{
    public class SanghaStaff
    {
        private const string delimiter = ",";
        private const string stringContainer = "\"";
        private readonly Encoding encoding = Encoding.ASCII;

        public MemoryStream GenerateCSV()
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream, encoding);

            // Headings
            // Staff Id,Staff Number,Hire Date,First Name,Middle Name,Last Name,Sex,Email 1,Email 2,Tel,Mobile,Date Of Birth,SchoolId
            writer.Write("Staff Id" + delimiter);
            writer.Write("Staff Number" + delimiter);
            writer.Write("Hire Date" + delimiter);
            writer.Write("First Name" + delimiter);
            writer.Write("Middle Name" + delimiter);
            writer.Write("Last Name" + delimiter);
            writer.Write("Sex" + delimiter);
            writer.Write("Email 1" + delimiter);
            writer.Write("Email 2" + delimiter);
            writer.Write("Tel" + delimiter);
            writer.Write("Mobile" + delimiter);
            writer.Write("Date Of Birth" + delimiter);
            writer.Write("SchoolId" + delimiter);
            writer.Write(Environment.NewLine);

            StaffRepository _staffRepo = new StaffRepository();

            foreach (StaffMember s in _staffRepo.GetAll().Where(x => Helpers.SanghaSettings.AllowedSchoolGovIDs.Contains(x.School.DAN)).Where(x => x.IsEnabled).OrderBy(x => x.School.Name).ThenBy(x => x.DisplayNameLastNameFirst))
            {
                writer.Write(stringContainer + s.iStaffId + stringContainer + delimiter);
                writer.Write(stringContainer + s.iStaffId + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Hire date
                writer.Write(stringContainer + s.FirstName + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Middle name
                writer.Write(stringContainer + s.LastName + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Sex
                writer.Write(stringContainer + s.EmailAddress + stringContainer + delimiter);
                writer.Write(stringContainer + "" + stringContainer + delimiter); // Email 2
                writer.Write(stringContainer + "" + stringContainer + delimiter); // phone
                writer.Write(stringContainer + "" + stringContainer + delimiter); // cell
                writer.Write(stringContainer + "" + stringContainer + delimiter);
                writer.Write(stringContainer + s.School.GovernmentID + stringContainer + delimiter);
                writer.Write(Environment.NewLine);
            }

            writer.Flush();
            outStream.Flush();
            return outStream;
        }
    }
}
