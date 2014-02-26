using Nancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.Common;
using DasStatus_Web.Models;
using System.ComponentModel.DataAnnotations;
using Npgsql;
using System.Data;
using ServiceStack.OrmLite;

namespace DasStatus_Web.Modules
{
    public class DasModule : NancyModule
    {
        private IDbConnection _connection;
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
                    Users = GetList().Where(u=> u.Status != "Offline").Select(u => new DasUserEx(u)).OrderByDescending(o => o.Date)
                };

                return View["index.sshtml", model];
            };

            Get["/test"] = _ =>
            {
                return "Hello World";
            };

            Get["/new"] = _ =>
            {
                int ret = default(int);
                using (_connection = Utilities.GetOpenConnection())
                {
                    _connection.DropAndCreateTable<DasUser>();
                    _connection.Close();
                }

                return ret;
            };

            Get["/sample"] = _ =>
            {
                dynamic ret;
                using (_connection = Utilities.GetOpenConnection())
                {
                    ret = _connection.Insert(
                        new DasUser
                        {
                            TwitterId = 123456789,
                            Status = "Online",
                            Name = "테스터",
                            Message = "메세지메세지",
                            Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DasModule.koreaTZI)
                        });
                    _connection.Close();
                }

                return ret;
            };


            Get["/delete"] = _ =>
            {
                int ret = default(int);
                using (_connection = Utilities.GetOpenConnection())
                {
                    var results = _connection.Where<DasUser>(new { TwitterId = 123456789 });
                    foreach (var result in results)
                    {
                        ret = _connection.Delete(result);
                    }
                    _connection.Close();
                }

                return ret;
            };
        }

        IEnumerable<DasUser> GetList()
        {
            IEnumerable<DasUser> result = null;
            using (_connection = Utilities.GetOpenConnection())
            {
                result = _connection.Select<DasUser>();
                _connection.Close();
            }

            return result;
        }

    }
}