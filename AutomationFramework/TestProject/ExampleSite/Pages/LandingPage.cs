using AutomationFramework.TestProject.ExampleSite.Pages;
using AutomationFramework.TestProject.Hooks;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace AutomationFramework.TestProject.ExampleSite.Pages
{

    public class LandingPage
    {

        public IWebDriver driver;

        private readonly By logoutBtn = By.XPath("logout");

        public LandingPage()
        {
            driver = WebHooks.driver;
        }

        public void VerifyLogOutDisplay()
        {
            Assert.True(driver.FindElement(logoutBtn).Displayed, "Logout button not displayed correctly.");
            Serilog.Log.Debug("Logout button not displayed correctly.");
        }
 


    }
}
