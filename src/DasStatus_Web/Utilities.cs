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
        private static readonly string ConnectionString = Connection.ConnectionString;

        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            return connection;
        }
    }
}