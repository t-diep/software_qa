using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NewCentury
{
    [TestClass]
    public class NewCentury
    {
        IWebDriver driver;
        IJavaScriptExecutor jexe;
        const string adminPortal = "http://10.4.1.99";
        const string newCenturyWeb = "https://devaccount.newcenturyscholarship.org/login";

        [TestInitialize]
        public void ConfigureRunBrowser()
        {
            driver = new ChromeDriver();
            jexe = (IJavaScriptExecutor)driver;

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(70);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(70);
            driver.Navigate().GoToUrl(newCenturyWeb);
        }

        /**
         * Automation test for verifying the admin settings for NC Web are working correctly
         */
        [TestMethod]
        public void SAMS_882_Negative()
        {
            //Login to the Student Admin Portal
            driver.Navigate().GoToUrl(adminPortal);
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);

            IWebElement settingsNC = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/aside[1]/nav[1]/ul[1]/li[3]/ul[1]/li[8]/a[1]"));
            settingsNC.Click();

            IWebElement finalDeadlineField = driver.FindElement(By.Name("finalDeadline"));
            finalDeadlineField.Clear();
            finalDeadlineField.SendKeys("02/01/2018 23:59:59");
            finalDeadlineField.SendKeys(Keys.Enter);

            IWebElement saveSettingsButton = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default:nth-child(3) section.main-content-wrapper:nth-child(3) section.animated.fadeInRight.ng-scope div.row div.col-md-12 div.panel.panel-primary div.panel-body form:nth-child(1) div.form-group.col-md-12:nth-child(9) > button.btn.btn-primary.left-side"));
            saveSettingsButton.Click();

            driver.Navigate().GoToUrl(newCenturyWeb);

            try
            {
                IWebElement deadlineMessage = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[1]/div[1]/form[1]/div[4]/strong[1]"));
            }
            catch(Exception)
            {
                Assert.Fail("There should be a message displaying that the application is closed");
            }

            Screenshot negativeTestResult = ((ITakesScreenshot)driver).GetScreenshot();
            negativeTestResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_882_ApplicationClosed");
        }
    }
}
