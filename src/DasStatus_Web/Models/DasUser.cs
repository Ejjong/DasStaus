using DasStatus_Web.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.DataAnnotations;

namespace DasStatus_Web.Models
{
    [Alias("dasuser")]
    public class DasUser
    {
        [PrimaryKey]
        public int TwitterId { get; set; }
        [StringLength(40)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        [StringLength(280)]
        public string Message { get; set; }
        public DateTime? Date { get; set; }
    }

    public class DasUserEx : DasUser
    {
        public DasUserEx(DasUser user)
        {
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

        public string Label
        {
            get
            {
                switch (Status)
                {
                    case "Riding":
                        return "label-danger";
                    case "Online":
                        return "label-success";
                    default:
                        return "";
                }
            }
        }

    }
}