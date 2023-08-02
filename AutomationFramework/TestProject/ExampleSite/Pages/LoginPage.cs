using AutomationFramework.TestProject.Hooks;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AutomationFramework.TestProject.ExampleSite.Pages
{
    public class LoginPage
    {
        public IWebDriver driver;


        private readonly By landingPageSignUpImage = By.XPath("//a[@class='sign-up']");
        private readonly By userNameTxtField = By.Id("name");
        private readonly By passwordTxtField = By.Id("password");
        private readonly By loginBtn = By.Id("login");
        



        public LoginPage()
        {
            driver = WebHooks.driver;
        }

        public void VerifySignUpImage()
        {
            Assert.True(driver.FindElement(landingPageSignUpImage).Displayed, "Login Page is not displayed correctly.");
            Serilog.Log.Debug("Login Page is displayed correctly.");
        }


        public void EnterUserName(string username)
        {
            driver.FindElement(userNameTxtField).SendKeys(username);
            Serilog.Log.Debug("Entered username:{0} on Login page.", username);
        }

        public void EnterPassword(string password)
        {
            driver.FindElement(passwordTxtField).SendKeys(password);
            Serilog.Log.Debug("Entered password:{0} on Login page.", password);
        }

        public void ClickLoginBtn()
        {
            driver.FindElement(loginBtn).Click();
            Serilog.Log.Debug("Clicked on Login button on Login page.");
        }

        
    }
}
