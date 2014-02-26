using Npgsql;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using ServiceStack.OrmLite;

namespace DasStatus_Web
{
    public class Utilities
    {
        private static readonly IConfig config = new Config();
        private static readonly string ConnectionString = config.Get("connectionString");

        public static IDbConnection GetOpenConnection()
        {
            string connString = string.Empty;
            IOrmLiteDialectProvider provider = null;
            if (ConnectionString.StartsWith("postgres://"))
            {
                connString = GenerateConnectionStringForPostgreSQL(connString);
                provider = PostgreSqlDialect.Provider;
            }
            else if (ConnectionString.StartsWith("sqlserver://"))
            {
                connString = GenerationConnectionStringForSqlServer(connString);
                provider = SqlServerDialect.Provider;
            }
            else
            {
                connString = ConnectionString;
                var providerString = config.Get("provider");
                switch (providerString)
                {
                    case "postgres":
                        provider = PostgreSqlDialect.Provider;
                        break;
                    case "sqlserver":
                        provider = SqlServerDialect.Provider;
                        break;
                    default:
                        break;
                }
            }
            var dbFactory = new OrmLiteConnectionFactory(connString, provider);

            return dbFactory.OpenDbConnection();
        }

        internal static string GenerateConnectionStringForPostgreSQL(string postgreUrl)
        {
            var uriString = postgreUrl;
            var uri = new Uri(uriString);
            var db = uri.AbsolutePath.Trim('/');
            var user = uri.UserInfo.Split(':')[0];
            var passwd = uri.UserInfo.Split(':')[1];
            var port = uri.Port > 0 ? uri.Port : 5432;
            var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
                uri.Host, db, user, passwd, port);

            return connStr;
        }

        internal static string GenerationConnectionStringForSqlServer(string sqlServerUrl)
        {
            var uri = new Uri(sqlServerUrl);
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = uri.Host,
                InitialCatalog = uri.AbsolutePath.Trim('/'),
                UserID = uri.UserInfo.Split(':').First(),
                Password = uri.UserInfo.Split(':').Last(),
            }.ConnectionString;

            return connectionString;
        }


        public static string GetTitle()
        {
            return config.Get("title");
        }

        public static string GetSubTitle()
        {
            return config.Get("subTitle");
        }

        public static string GetWidgetSrc()
        {
            var screenNm = config.Get("screenName");
            return string.IsNullOrWhiteSpace(screenNm)? null : @"http://platform.twitter.com/widgets/follow_button.1387492107.html#_=1389658876572&id=twitter-widget-1&lang=en&screen_name=" + screenNm + "&shoe_count_true&show_screen_name=true&size=m";
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