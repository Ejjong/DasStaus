using DasStatus_Web.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DasStatus_Web.Models
{
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

        public string Label
        {
            get
            {
                switch(Status)
                {
                    case "Riding" :
                        return "label-danger";
                    case "Online" :
                        return "label-success";
                    default :
                        return "";
                }
            }
        }

    }
}