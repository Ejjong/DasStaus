using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DasStatus_Web
{
    public class Utilities
    {
        private static readonly ConnectionStringSettings Connection = WebConfigurationManager.ConnectionStrings["nancyDb"];
        private static readonly IConfig config = new Config();
        private static readonly string ConnectionString = config.Get("testDb");

        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            return connection;
        }

        public static string GetRelativeDateTime(DateTime now, DateTime date)
        {
            TimeSpan ts = now - date;
            if (ts.TotalMinutes < 1)//seconds ago
                return (String.Format("{0:0}초", ts.TotalSeconds));
            if (ts.TotalHours < 1)//min ago
                return (int)ts.TotalMinutes == 1 ? "1분" : (int)ts.TotalMinutes + "분";
            if (ts.TotalDays < 1)//hours ago
                return (int)ts.TotalHours == 1 ? "1시간" : (int)ts.TotalHours + "시간";
            if (ts.TotalDays < 7)//days ago
                return (int)ts.TotalDays == 1 ? "1일" : (int)ts.TotalDays + "일";

            return date.ToString("yyyy.MM.dd");
        }
    }
}