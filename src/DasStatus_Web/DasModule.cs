using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DasStatus_Web
{
    public class DasModule : NancyModule
    {
        public DasModule()
        {
            Get["/status"] = _ => "Hello World";
        }
    }
}