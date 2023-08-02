using System;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace AutomationFramework.Utilities.Web
{
    public static class Extensions
    {
        public static IWebElement WaitForEnabled(this IWebElement element, int timeSpan = 60)
        {
            var watch = new Stopwatch();

            watch.Start();
            while (watch.Elapsed.Seconds < timeSpan)
                if (element.Enabled)
                    return element;

            throw new ElementNotInteractableException();
        }

        public static IWebElement WaitForVisible(this IWebElement element, int timeSpan = 60)
        {
            var watch = new Stopwatch();

            watch.Start();
            while (watch.Elapsed.Seconds < timeSpan)
                if (element.Displayed)
                    return element;

            throw new ElementNotVisibleException();
        }

        public static IWebElement WaitForText(this IWebElement element, int timeSpan = 60)
        {
            var watch = new Stopwatch();

            watch.Start();
            while (watch.Elapsed.Seconds < timeSpan)
                if (element.Text.Length > 0)
                    return element;

            throw new ElementNotVisibleException();
        }

        public static IWebElement WaitForText(this IWebElement element, string text, int timeSpan = 60)
        {
            var watch = new Stopwatch();

            watch.Start();
            while (watch.Elapsed.Seconds < timeSpan)
                if (element.Text == text)
                    return element;

            throw new NoSuchElementException();
        }

        public static IWebElement ClickAndWaitFor(this IWebElement element,int timeSpan = 5000)
        {
            element.Click();
            Thread.Sleep(timeSpan);
            return element;
        }

        public static void ClickAndWaitForGpccPageLoad(this IWebElement element)
        {
            element.Click();
            WaitForGpccPageLoaded(GetWebDriverFromElement(element));
        }

        public static IWebDriver GetWebDriverFromElement(this IWebElement element)
        {
            IWebDriver driver;

            if (element.GetType().ToString() == "OpenQA.Selenium.Support.PageObjects.WebElementProxy")
            {
                driver = ((IWrapsDriver)element
                    .GetType().GetProperty("WrappedElement")
                    ?.GetValue(element, null))?.WrappedDriver;
            }
            else
            {
                driver = ((IWrapsDriver)element).WrappedDriver;
            }
            return driver;
        }

        public static void WaitForGpccPageLoaded(this IWebDriver driver, int timeSpan = 60)
        {
            var watch = new Stopwatch();
            watch.Start();

            while (watch.Elapsed.Seconds < timeSpan)
            {
                Thread.Sleep(2000);
                var loaderLength = ((IJavaScriptExecutor) driver)
                    .ExecuteScript("return document.getElementsByClassName('app-loader').length");
                if (Convert.ToInt32(loaderLength) == 0)
                    return;
            }
        }
    }
}