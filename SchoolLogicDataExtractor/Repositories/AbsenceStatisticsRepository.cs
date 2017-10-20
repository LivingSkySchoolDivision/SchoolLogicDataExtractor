using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicDataExtractor
{

    class AbsenceStatisticsRepository
    {
        Dictionary<int, AbsenceStatisticsResponse> _cache = new Dictionary<int, AbsenceStatisticsResponse>();

        private readonly string SQL_ActiveStudentIDs = "SELECT DISTINCT([iStudentID]) FROM StudentStatus WHERE dInDate <= { fn CURDATE() } AND (dOutDate >= { fn CURDATE() } OR dOutDate < '1901-01-01 00:00:00') ORDER BY iStudentID";

        private const string SQLQuery = "SELECT Student.iStudentID, Absences.X as TotalAbsenceEntries, Unexcused.X as Unexcused, Excused.X as Excused, Lates.X as Late, LeaveEarlies.X as LeaveEarly, TotalLateMinutes.X as TotalLateMinutes, TotalLeaveEarlyMinutes.X as TotalLeaveEarlyMinutes FROM Student " +
                                        "LEFT JOIN (SELECT iStudentID, X=COUNT(*) FROM Attendance LEFT JOIN AttendanceStatus ON Attendance.iAttendanceStatusID=AttendanceStatus.iAttendanceStatusID LEFT JOIN AttendanceReasons ON Attendance.iAttendanceReasonsID=AttendanceReasons.iAttendanceReasonsID GROUP BY iStudentID) as Absences ON Student.istudentID=Absences.iStudentID " +
                                        "LEFT JOIN (SELECT iStudentID, X=COUNT(*) FROM Attendance LEFT JOIN AttendanceStatus ON Attendance.iAttendanceStatusID=AttendanceStatus.iAttendanceStatusID LEFT JOIN AttendanceReasons ON Attendance.iAttendanceReasonsID=AttendanceReasons.iAttendanceReasonsID WHERE AttendanceStatus.cName = 'Late' GROUP BY iStudentID) as Lates ON Student.istudentID=Lates.iStudentID " +
                                        "LEFT JOIN (SELECT iStudentID, X=COUNT(*) FROM Attendance LEFT JOIN AttendanceStatus ON Attendance.iAttendanceStatusID=AttendanceStatus.iAttendanceStatusID LEFT JOIN AttendanceReasons ON Attendance.iAttendanceReasonsID=AttendanceReasons.iAttendanceReasonsID WHERE Attendancestatus.cName = 'Leave Early' GROUP BY iStudentID) as LeaveEarlies ON Student.istudentID=LeaveEarlies.iStudentID " +
                                        "LEFT JOIN (SELECT iStudentID, X=COUNT(*) FROM Attendance LEFT JOIN AttendanceStatus ON Attendance.iAttendanceStatusID=AttendanceStatus.iAttendanceStatusID LEFT JOIN AttendanceReasons ON Attendance.iAttendanceReasonsID=AttendanceReasons.iAttendanceReasonsID WHERE AttendanceStatus.cName != 'Late' AND Attendancestatus.cName != 'Leave Early' AND lExcusable=1 GROUP BY iStudentID) as Excused ON Student.istudentID=Excused.iStudentID " +
                                        "LEFT JOIN (SELECT iStudentID, X=COUNT(*) FROM Attendance LEFT JOIN AttendanceStatus ON Attendance.iAttendanceStatusID=AttendanceStatus.iAttendanceStatusID LEFT JOIN AttendanceReasons ON Attendance.iAttendanceReasonsID=AttendanceReasons.iAttendanceReasonsID WHERE AttendanceStatus.cName != 'Late' AND Attendancestatus.cName != 'Leave Early' AND (lExcusable=0 OR lExcusable IS NULL) GROUP BY iStudentID) as Unexcused ON Student.istudentID=Unexcused.iStudentID " +
                                        "LEFT JOIN (SELECT iStudentID, X=SUM(iMinutes) FROM Attendance LEFT JOIN AttendanceStatus ON Attendance.iAttendanceStatusID=AttendanceStatus.iAttendanceStatusID LEFT JOIN AttendanceReasons ON Attendance.iAttendanceReasonsID=AttendanceReasons.iAttendanceReasonsID WHERE AttendanceStatus.cName = 'Late' GROUP BY iStudentID) as TotalLateMinutes ON Student.istudentID=TotalLateMinutes.iStudentID " +
                                        "LEFT JOIN (SELECT iStudentID, X=SUM(iMinutes) FROM Attendance LEFT JOIN AttendanceStatus ON Attendance.iAttendanceStatusID=AttendanceStatus.iAttendanceStatusID LEFT JOIN AttendanceReasons ON Attendance.iAttendanceReasonsID=AttendanceReasons.iAttendanceReasonsID WHERE Attendancestatus.cName = 'Leave Early' GROUP BY iStudentID) as TotalLeaveEarlyMinutes ON Student.istudentID=TotalLeaveEarlyMinutes.iStudentID ";

        private AbsenceStatisticsResponse SQLDataReaderToResponse(SqlDataReader dataReader)
        {
            return new AbsenceStatisticsResponse()
            {
                iStudentID = Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim()),
                TotalEntries = Parsers.ParseInt(dataReader["TotalAbsenceEntries"].ToString().Trim()),
                Lates = Parsers.ParseInt(dataReader["Late"].ToString().Trim()),
                LeaveEarlies = Parsers.ParseInt(dataReader["LeaveEarly"].ToString().Trim()),
                UnexcusedAbsences = Parsers.ParseInt(dataReader["Unexcused"].ToString().Trim()),
                ExcusedAbsences = Parsers.ParseInt(dataReader["Excused"].ToString().Trim()),
                TotalLateMinutes = Parsers.ParseInt(dataReader["TotalLateMinutes"].ToString().Trim()),
                TotalLeaveEarlyMinutes = Parsers.ParseInt(dataReader["TotalLeaveEarlyMinutes"].ToString().Trim()),
            };
        }


        public AbsenceStatisticsRepository()
        {
            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_SchoolLogic))
            {
                // Get all active student IDs
                List<int> _activeStudentIDs = new List<int>();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQL_ActiveStudentIDs;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int studentID = Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim());
                            if (studentID > 0)
                            {
                                if (!_activeStudentIDs.Contains(studentID))
                                {
                                    _activeStudentIDs.Add(studentID);
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }

                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = SQLQuery + " WHERE Student.iStudentID IN (" + _activeStudentIDs.ToCommaSeparatedString() + ") ORDER BY Student.iStudentID"
                })
                {
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            AbsenceStatisticsResponse response = SQLDataReaderToResponse(dataReader);
                            _cache.Add(response.iStudentID, response);
                        }
                    }
                }
            }
        }

        public AbsenceStatisticsResponse Get(int iStudentID)
        {
            if (_cache.ContainsKey(iStudentID))
            {
                return _cache[iStudentID];
            } else
            {
                return new AbsenceStatisticsResponse();
            }
        }
        

    }
}
