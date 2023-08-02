using System.Collections.Specialized;
using System.Configuration;

namespace AutomationFramework.Factories
{
    public static class ConfigurationFactory
    {
        public static string GetEnvSectionValue(string key)
        {
            var keyValue = string.Empty;
            if (ConfigurationManager.GetSection(ConfigurationManager.AppSettings["environment"]) is NameValueCollection
                section)
                keyValue = section[key];

            return keyValue;
        }

        public static string GetAppSettingValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}