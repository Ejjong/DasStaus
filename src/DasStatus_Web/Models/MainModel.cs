using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DasStatus_Web.Models
{
    public class MainModel
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string TwitterWidgetSrc { get; set; }

        public IEnumerable<DasUser> Users { get; set; }
    }
}