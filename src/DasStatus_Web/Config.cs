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
                if(ret.Contains("postgre")) return GetP(ret);
                return ret;
            }
            else
            {
                return fromConfig;
            }
        }
    public string GetP(string postgreUrl)
    {
        var uriString = postgreUrl;
        var uri = new Uri(uriString);
        var db = uri.AbsolutePath.Trim('/');
        var user = uri.UserInfo.Split(':')[0];
        var passwd = uri.UserInfo.Split(':')[1];
        var port = uri.Port > 0 ? uri.Port : 5432;
        var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}", 
            uri.Host, db, user, passwd, port);
        return connStr;
    }

    }
 

}
