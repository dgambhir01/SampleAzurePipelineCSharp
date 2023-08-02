using AutomationFramework.TestProject.ExampleSite.Pages;
using AutomationFramework.TestProject.Hooks;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomationFramework.TestProject.ExampleSite.Steps
{
    [Binding]
    public sealed class LoginTest
    {
        public IWebDriver driver = WebHooks.driver;
        readonly LoginPage loginPage = new LoginPage();
        readonly LandingPage landingPage = new LandingPage();

        private readonly ScenarioContext _scenarioContext;

        public LoginTest(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I am at home page of testproject portal")]
        public void GivenIAmAtHomePageOfTestproject()
        {
            loginPage.VerifySignUpImage();
        }


        [When(@"I enter a valid (.+) and (.*)")]
        public void WhenIEnterAValidHbfiservAndPassword(string username, string password)
        {
            loginPage.EnterUserName(username);

            loginPage.EnterPassword(password);
            

        }


        [When(@"I click the LogIn button")]
        public void WhenIClickTheLogInButton()
        {
            loginPage.ClickLoginBtn();
            
        }

        [Then(@"I should be able to login successfully")]
        public void ThenIShouldBeAbleToLoginSuccessfully()
        {
            landingPage.VerifyLogOutDisplay();
        }







    }
}
