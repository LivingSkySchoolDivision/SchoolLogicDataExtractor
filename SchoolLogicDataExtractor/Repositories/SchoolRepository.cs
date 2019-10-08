using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{
    class SchoolRepository
    {
        private readonly Dictionary<int, School> _cache;
        private readonly Dictionary<int, string> _IDsToGovernmentIDs;

        // Manually hide some of the schools, for now
        //private const string SQLQuery = "SELECT * FROM School WHERE iDistrictID=1 ORDER BY cName";
        private const string SQLQuery = "SELECT School.*, LowGrade.cName as LowGrade, HighGrade.cName as HighGrade FROM School LEFT OUTER JOIN Grades as LowGrade ON School.iLow_GradesID=LowGrade.iGradesID LEFT OUTER JOIN Grades as HighGrade ON School.iHigh_GradesID=HighGrade.iGradesID WHERE (School.cName <> 'Zinactive') AND (lInactive=0) AND (iDistrictID=1)   ORDER BY cName";

        public SchoolRepository()
        {
            _cache = new Dictionary<int, School>();
            _IDsToGovernmentIDs = new Dictionary<int, string>();

            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_SchoolLogic))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQLQuery;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            _cache.Add(Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim()), dataReaderToSchool(dataReader));
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }

            // Map database IDs to Government IDs
            foreach (School school in _cache.Values)
            {
                if (!string.IsNullOrEmpty(school.GovernmentID))
                {
                    _IDsToGovernmentIDs.Add(school.iSchoolID, school.GovernmentID);
                }
            }

        }

        private School dataReaderToSchool(SqlDataReader dataReader)
        {
            int iSchoolID = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim());

            return new School()
            {
                Name = dataReader["cName"].ToString().Trim(),
                iSchoolID = iSchoolID,
                GovernmentID = dataReader["cCode"].ToString().Trim(),
                isFake = false,
                LowGrade = dataReader["LowGrade"].ToString().Trim(),
                HighGrade = dataReader["HighGrade"].ToString().Trim()
            };
        }

        public static School CreateDummySchool()
        {
            return new School()
            {
                Name = "",
                iSchoolID = 0,
                GovernmentID = "0",
                isFake = true
            };
        }

        public int GetSchoolDatabaseID(string schoolGovernmentNumber)
        {
            foreach (int schoolID in _IDsToGovernmentIDs.Keys)
            {
                if (_IDsToGovernmentIDs[schoolID] == schoolGovernmentNumber)
                {
                    return schoolID;
                }
            }
            return -1;
        }

        public School Get(int iSchooliD)
        {
            return _cache.ContainsKey(iSchooliD) ? _cache[iSchooliD] : CreateDummySchool();
        }

        public School GetByGovernmentID(string schoolGovernmentID)
        {
            int foundSchoolID = GetSchoolDatabaseID(schoolGovernmentID);

            return foundSchoolID != -1 ? _cache[foundSchoolID] : CreateDummySchool();
        }

        public List<School> GetByGovernmentID(List<string> schoolGovernmentIDs)
        {
            return schoolGovernmentIDs.Select(GetByGovernmentID).Where(foundSchool => !foundSchool.isFake).OrderBy(s => s.Name).ToList();
        }

        public List<School> Get(List<int> iSchoolIDs)
        {
            return iSchoolIDs.Select(Get).Where(foundSchool => !foundSchool.isFake).OrderBy(s => s.Name).ToList();
        }

        public List<School> GetAll()
        {
            return _cache.Values.OrderBy(s => s.Name).ToList();
        }

        public bool DoesSchoolIDExist(int schoolID)
        {
            return _cache.ContainsKey(schoolID);
        }

        public bool DoesSchoolGovIDExist(string schoolGovernmentID)
        {
            return GetSchoolDatabaseID(schoolGovernmentID) != -1;
        }
    }
}
