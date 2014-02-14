using Nancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Dapper;
using DapperExtensions;
using System.Data.Common;

namespace DasStatus_Web
{
    public class DasModule : NancyModule
    {
        private DbConnection _connection;

        public DasModule()
        {
            var ret = GetList();
            Get["/"] = _ => View["index.sshtml"];
            Get["/status"] = _ => "Hello World";
        }

        IEnumerable<DasUser> GetList()
        {
            IEnumerable<DasUser> result;
            using (_connection = Utilities.GetOpenConnection())
            {
                result = _connection.GetList<DasUser>();
            }

            return result;
        }
    }

    [Table("DasUser")]
    public class DasUser
    {
        public int Id { get; set; }
        public int TwitterId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public DateTime? Date { get; set; }
    }
}