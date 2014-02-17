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
        private SqlConnection _connection;
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

            Get["/create"] = _ =>
            {
                int ret = default(int);
                using (_connection = Utilities.GetOpenConnection())
                {
                    var sql = @"CREATE TABLE dbo.DasUser
(
  [Id]  int IDENTITY(1,1) NOT NULL,
  [TwitterId] int NOT NULL,
  [Name]  nvarchar(50)  NOT NULL,
  [Status]  nvarchar(50)  NOT NULL,
  [Message]  nvarchar(200)  NOT NULL,
  [Date] datetime,
  CONSTRAINT PK_SIS_UserMenu PRIMARY KEY ([Id], [TwitterId])
);";
                    var command = new SqlCommand(sql, _connection);
                    ret = command.ExecuteNonQuery();
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
                            Message = "메세지",
                            Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DasModule.koreaTZI)
                        });
                   _connection.Close();
                }

                return ret;
            };


            Get["/delete"] = _ =>
            {
                bool ret = false;
                using (_connection = Utilities.GetOpenConnection())
                {
                    var results = _connection.GetList<DasUser>(new { TwitterId = 123456789 });
                    foreach(var result in results)
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
            IEnumerable<DasUser> result;
            using (_connection = Utilities.GetOpenConnection())
            {
                result = _connection.GetList<DasUser>();
                _connection.Close();
            }

            return result;
        }
    }
}