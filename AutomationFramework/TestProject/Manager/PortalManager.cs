using AutomationFramework.Factories;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.TestProject.Manager
{
    class PortalManager
    {
        public static string portalURL;
        public static string GetPortalURL()
        {
            string portalToUse = Environment.GetEnvironmentVariable("portal", EnvironmentVariableTarget.Process);
            Console.WriteLine("portalToUse: >> " + portalToUse);


            if (string.IsNullOrEmpty(portalToUse))
            {
                portalURL = ConfigurationFactory.GetEnvSectionValue("TestPortalUrl");
            }
            else
            {

                switch (portalToUse.ToLower())
                {
                    case "helzberg":
                        portalURL = ConfigurationFactory.GetEnvSectionValue("TestPortalUrl");
                        break;

                    case "destiny":
                        portalURL = ConfigurationFactory.GetEnvSectionValue("TestPortalUrl");
                        break;

                    default:
                        portalURL = ConfigurationFactory.GetEnvSectionValue("TestPortalUrl");
                        break;
                }
            }

            Serilog.Log.Debug("Running test(s) on URL: >> {0}.", portalURL);
            return portalURL;

        }
    }
}
