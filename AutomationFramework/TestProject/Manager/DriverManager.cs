using AutomationFramework.TestProject.Manager;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Reflection;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace AutomationFramework.TestProject.Driver
{
    class DriverManagerImpl
    {

        [ThreadStatic]
        public static IWebDriver driver;
        private static BrowserType browserType;
        

        public static IWebDriver SelectBrowser(ScenarioContext context)
        {

            // Get the value from command line
            // example--testparam:Browser=Chrome or -- TestRunParameters.Parameter(name=%22Browser%22", value=%22Chrome%22")
            // If nothing specified, test will run on Chrome browser

            var browser = TestContext.Parameters.Get("Browser", "remoteBSChrome"); // remoteBSChrome

            // This helps you to pass parameter/variables like browser type from ADO pipeline
            //string browserADO = Environment.GetEnvironmentVariable("browser", EnvironmentVariableTarget.Process); 
            Console.WriteLine("Browser to execute on: >> " + browser);
            browserType = (BrowserType)Enum.Parse(typeof(BrowserType), browser);

            switch (browserType)
            {
                case BrowserType.Chrome:
                    ChromeOptions option = new ChromeOptions();
                    option.AcceptInsecureCertificates = true;                   
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    driver = new ChromeDriver(option);

                    break;

                case BrowserType.Firefox:
                    var driverDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(driverDir, "geckodriver.exe");

                    service.HideCommandPromptWindow = true;
                    service.SuppressInitialDiagnosticInformation = true;
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AcceptInsecureCertificates = true;              

                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    driver = new FirefoxDriver(service, firefoxOptions);

                    break;


                case BrowserType.IE:
                    break;

                case BrowserType.remoteBSChrome:

                    OpenQA.Selenium.Chrome.ChromeOptions capability = new OpenQA.Selenium.Chrome.ChromeOptions();
                    capability.AddAdditionalCapability("os_version", "10", true);
                    capability.AddAdditionalCapability("acceptSslCerts", "true", true);
                    capability.AddAdditionalCapability("resolution", "1920x1080", true);
                    capability.AddAdditionalCapability("browser", "Chrome", true);
                    capability.AddAdditionalCapability("browser_version", "latest", true);
                    capability.AddAdditionalCapability("os", "Windows", true);
                    capability.AddAdditionalCapability("name", context.ScenarioInfo.Title, true); // test name
                    capability.AddAdditionalCapability("build", "Test-" + DateTime.Now.ToString("dd/MM/yyyy"), true); // CI/CD job or build name
                    capability.AddAdditionalCapability("browserstack.user", "<<YouNeedToAddYourBrowserStackAccountUsername>>", true);
                    capability.AddAdditionalCapability("browserstack.key", "<<YouNeedToAddYourBrowserStackAccountAccessKey>>", true);
                    driver = new RemoteWebDriver(
                      new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capability
                    );

                    break;

                case BrowserType.remoteBSFirefox:

                    FirefoxOptions cap = new FirefoxOptions();
                    cap.AddAdditionalCapability("os", "Windows", true);
                    cap.AddAdditionalCapability("os_version", "10", true);
                    cap.AddAdditionalCapability("browser", "Firefox", true);
                    cap.AddAdditionalCapability("browser_version", "latest-beta", true);
                    cap.AddAdditionalCapability("build", "Test-" + DateTime.Now.ToString("dd/MM/yyyy"), true);
                    cap.AddAdditionalCapability("name", context.ScenarioInfo.Title, true);
                    cap.AddAdditionalCapability("browserstack.local", "false", true);
                    cap.AddAdditionalCapability("browserstack.selenium_version", "3.141.0", true);
                    cap.AddAdditionalCapability("browserstack.user", "<<YouNeedToAddYourBrowserStackAccountUsername>>", true);
                    cap.AddAdditionalCapability("browserstack.key", "<<YouNeedToAddYourBrowserStackAccountAccessKey>>", true);

                    driver = new RemoteWebDriver(
                      new Uri("https://hub-cloud.browserstack.com/wd/hub/"), cap
                    );

                    break;

                default:
                    ChromeOptions options = new ChromeOptions();
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    driver = new ChromeDriver(options);

                    break;
            }

            driver.Navigate().GoToUrl(PortalManager.GetPortalURL());
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            return driver;

        }


    }

    enum BrowserType
    {
        Chrome,
        Firefox,
        IE,
        remoteBSChrome,
        remoteBSFirefox
    }
}
