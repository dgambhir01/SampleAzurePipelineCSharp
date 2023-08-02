using AutomationFramework.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutomationFramework.Utilities
{
    public static class DynamicStepValues
    {
        public static string ReplaceDynamicUrl(string url)
        {
            Regex rg = new Regex(@"\{\w+\}");
            MatchCollection matchedEnvironmentVariable = rg.Matches(url);
            for (int count = 0; count < matchedEnvironmentVariable.Count; count++)
            {
                string key = matchedEnvironmentVariable[count].Value.Replace("{", "").Replace("}", "").Trim();
                url = url.Replace(matchedEnvironmentVariable[count].Value, ConfigurationFactory.GetEnvSectionValue(key));
            }
                
            return url;
        }
    }
}
