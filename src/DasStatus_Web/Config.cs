using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DasStatus_Web
{
    public interface IConfig
    {
        string Get(string key);
    }

    public class Config : IConfig
    {
        public string Get(string key)
        {
            var fromConfig = ConfigurationManager.AppSettings[key];
            if (String.Equals(fromConfig, "{ENV}", StringComparison.InvariantCultureIgnoreCase))
            {
                var ret = Environment.GetEnvironmentVariable(key);
                return ret;
            }
            else
            {
                return fromConfig;
            }
        }
    }

}
