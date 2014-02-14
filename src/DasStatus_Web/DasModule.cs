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

namespace DasStatus_Web
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
                    Title = "Das Fahrrad",
                    SubTitle = "Rider Status",
                    TwitterWidgetSrc = "http://platform.twitter.com/widgets/follow_button.1387492107.html#_=1389658876572&id=twitter-widget-1&lang=en&screen_name=dasfahrrad_&shoe_count_true&show_screen_name=true&size=m",
                    Users = GetList().Select(u => new DasUserEx(u))
                };
                return View["index.sshtml", model];
            };

            Get["/status"] = _ =>
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

    public class DasUserEx : DasUser
    {
        public DasUserEx(DasUser user)
        {
            this.Id = user.Id;
            this.TwitterId = user.TwitterId;
            this.Name = user.Name;
            this.Status = user.Status;
            this.Message = user.Message;
            this.Date = user.Date;
        }

        public string DisplayDate
        {
            get { return Utilities.GetRelativeDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DasModule.koreaTZI), (DateTime)Date); }
        }
    }
}