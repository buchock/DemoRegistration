using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Demo.Common
{
    public static class Utility
    {
        public static string GetAppSetting(string key)
        {
            return "";
        }

        public static string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}
