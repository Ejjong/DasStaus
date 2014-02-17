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
using DasStatus_Web.Models;
using System.ComponentModel.DataAnnotations;

namespace DasStatus_Web.Modules
{
    public class DasModule : NancyModule
    {
        private DbConnection _connection;
        public static TimeZoneInfo koreaTZI = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");

        public DasModule()
        {
            Get["/", runAsync: true] = async (_, token) =>
            {
                var model = new MainModel()
                {
                    Title = Utilities.GetTitle(),
                    SubTitle = Utilities.GetSubTitle(),
                    TwitterWidgetSrc = Utilities.GetWidgetSrc(),
                    Users = GetList().Select(u => new DasUserEx(u)).OrderByDescending(o => o.Date)
                };
                return View["index.sshtml", model];
            };

            Get["/test"] = _ =>
            {
                return "Hello World";
            };
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
}