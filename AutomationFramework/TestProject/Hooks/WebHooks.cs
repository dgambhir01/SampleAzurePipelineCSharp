using AutomationFramework.Factories;
using AutomationFramework.TestProject.Driver;
using AutomationFramework.TestProject.Variables;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using BoDi;
using BrowserStack;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using static NPOI.HSSF.Util.HSSFColor;

//To execute in parallel
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(2)]

namespace AutomationFramework.TestProject.Hooks
{
    [Binding]
    public class WebHooks
    {
        // To execute in parallel need thread safe driver
        [ThreadStatic]
        public static IWebDriver driver;

        static AventStack.ExtentReports.ExtentReports extent;
        [ThreadStatic]
        static AventStack.ExtentReports.ExtentTest feature;
        [ThreadStatic]
        static AventStack.ExtentReports.ExtentTest scenario;
        [ThreadStatic]
        static AventStack.ExtentReports.ExtentTest step;


        static string reportPath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\net48", "")
                + @"Results\TestReport_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.DirectorySeparatorChar;

        // Get the value from command line
        // example--testparam:testMode=Web or -- TestRunParameters.Parameter(name=%22testMode%22", value=%22Web%22")
        // If nothing specified, test will run for Web
        private static readonly string testMode = TestContext.Parameters.Get("testMode", "Web");

        [BeforeTestRun]
        public static void StartBrowserStackLocal()
        {
            foreach (var process in Process.GetProcessesByName("BrowserStackLocal"))
            {
                Console.WriteLine(" I am killing existing BrowserStackLocal");
                process.Kill();
            }
            var browser = ConfigurationFactory.GetAppSettingValue("browser");
            if (browser.ToLower() != "browserstack") return;
            var browserStackLocal = new Local();
            List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("key", "<<YouNeedToAddYourBrowserStackAccountAccessKey>>"),
                new KeyValuePair<string, string>("forcelocal", "true")
            };
            Console.WriteLine(" I am killing configuring BrowserStackLocal");
            browserStackLocal.start(bsLocalArgs);
            bool isRunning = Process.GetProcessesByName("BrowserStackLocal").Any();
            Console.WriteLine("Local Running: " + isRunning);

        }


        [AfterTestRun]
        public static void KillBrowserStackLocal()
        {
            foreach (var process in Process.GetProcessesByName("BrowserStackLocal"))
            {
                process.Kill();
            }
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            SetUpExtentReport();
            SetUpLogger();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext context)
        {
            feature = extent.CreateTest(context.FeatureInfo.Title);
            Log.Information("Selecting feature file {0} to run", context.FeatureInfo.Title);

        }

        [BeforeScenario("@UI")]
        public void BeforeScenarioWithTag(ScenarioContext context)
        {
  
            driver = DriverManagerImpl.SelectBrowser(context);
           
        }

        [BeforeScenario(Order =0)]
        public void BeforeScenario(ScenarioContext context)
        {

            scenario = feature.CreateNode(context.ScenarioInfo.Title);
            Log.Information("Selecting scenario {0} to run", context.ScenarioInfo.Title);

        }

        [BeforeStep]
        public void BeforeStep()
        {
            step = scenario;
        }

        [AfterStep]
        public void AfterStep(ScenarioContext context)
        {
            if (context.TestError == null)
            {
                step.Log(Status.Pass, context.StepContext.StepInfo.Text);
            }
            else if (context.TestError != null)
            {

                Log.Error("Test Step Failed | " + context.TestError.Message);
                step.Log(Status.Fail, "Test Step Failed | " + context.StepContext.StepInfo.Text + " | " + context.TestError.Message);
                
            }
        }

        [AfterStep("@UI")]
        public void AfterStepWithTag(ScenarioContext context)
        {
           if (context.TestError != null)
            {
                string base64 = GetScreenshot();
                step.Log(Status.Fail, "Screenshot: ", MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build());
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" There are some issues with this test case.\"}}");
                driver.Quit();
                
            }
        }


        [AfterFeature]
        public static void AfterFeature()
        {
            extent.Flush();
        }

        [AfterScenario("@UI")]
        public void AfterScenario()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;

            if (status)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Test is passed!\"}}");

            }

            driver.Quit();
          
        }


        public string GetScreenshot()
        {
            return ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;
        }

        public static void SetUpExtentReport()
        {
            //Console.WriteLine("Report will generate at:>> " + reportPath);
            ExtentHtmlReporter htmlReport = new ExtentHtmlReporter(reportPath);
            htmlReport.Config.DocumentTitle = "TestReport";
            htmlReport.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;

            extent = new AventStack.ExtentReports.ExtentReports();
            extent.AttachReporter(htmlReport);
        }

        public static void SetUpLogger()
        {
            LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.File(reportPath + @"\Logs",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} | {Level:u3}|{Message} {NewLine}",
                rollingInterval: RollingInterval.Day).CreateLogger();
        }

    }

}