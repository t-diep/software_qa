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
         * Automation test for verifying the application should be closed after configuring the settings
         * in admin portal
         */
        [TestMethod]
        public void SAMS_882_Negative()
        {
            //Login to the Student Admin Portal
            driver.Navigate().GoToUrl(adminPortal);
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);

            //Go to the Settings tab under the New Century menu
            IWebElement settingsNC = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/aside[1]/nav[1]/ul[1]/li[3]/ul[1]/li[8]/a[1]"));
            settingsNC.Click();

            //Configure final deadline to a later date than application deadline
            IWebElement finalDeadlineField = driver.FindElement(By.Name("finalDeadline"));
            finalDeadlineField.Clear();
            finalDeadlineField.SendKeys("02/01/2018 23:59:59");
            finalDeadlineField.SendKeys(Keys.Enter);

            //Save the changes
            IWebElement saveSettingsButton = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default:nth-child(3) section.main-content-wrapper:nth-child(3) section.animated.fadeInRight.ng-scope div.row div.col-md-12 div.panel.panel-primary div.panel-body form:nth-child(1) div.form-group.col-md-12:nth-child(9) > button.btn.btn-primary.left-side"));
            saveSettingsButton.Click();

            //Switch to New Century Web
            driver.Navigate().GoToUrl(newCenturyWeb);

            //Verify that the message for closed application is displayed
            try
            {
                IWebElement deadlineMessage = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[1]/div[1]/form[1]/div[4]/strong[1]"));
                Assert.IsTrue(deadlineMessage.Displayed);
            }
            catch(Exception)
            {
                Assert.Fail("There should be a message displaying that the application is closed");
            }

            //Take screenshot of application closed displayed 
            Screenshot negativeTestResult = ((ITakesScreenshot)driver).GetScreenshot();
            negativeTestResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_882_ApplicationClosed.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for verifying the application should now be open after configuring the settings
         * in admin portal
         */
        [TestMethod]
        public void SAMS_882_Positive()
        {
            //Login to the Student Admin Portal
            driver.Navigate().GoToUrl(adminPortal);
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);

            //Go to the Settings tab under the New Century menu
            IWebElement settingsNC = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/aside[1]/nav[1]/ul[1]/li[3]/ul[1]/li[8]/a[1]"));
            settingsNC.Click();

            //Configure final deadline to a later date than application deadline
            IWebElement finalDeadlineField = driver.FindElement(By.Name("finalDeadline"));
            finalDeadlineField.Clear();
            finalDeadlineField.SendKeys("02/01/2019 23:59:59");
            finalDeadlineField.SendKeys(Keys.Enter);

            //Save the changes
            IWebElement saveSettingsButton = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default:nth-child(3) section.main-content-wrapper:nth-child(3) section.animated.fadeInRight.ng-scope div.row div.col-md-12 div.panel.panel-primary div.panel-body form:nth-child(1) div.form-group.col-md-12:nth-child(9) > button.btn.btn-primary.left-side"));
            saveSettingsButton.Click();

            //Switch to New Century Web
            driver.Navigate().GoToUrl(newCenturyWeb);

            //Verify that the message for closed application is displayed
            try
            {
                IWebElement signUpButton = driver.FindElement(By.CssSelector("body.login_bg_body:nth-child(2) div.header-content div.header-content-inner div:nth-child(1) div.col-sm-4.form-container.form-group:nth-child(3) form.login-form > button.btn.btn-success.btn-signup:nth-child(11)"));
                Assert.IsTrue(signUpButton.Displayed);
            }
            catch (Exception)
            {
                Assert.Fail("The sign up button should appear, and thus the application should be open");
            }

            //Take screenshot of application open result
            Screenshot positiveTestResult = ((ITakesScreenshot)driver).GetScreenshot();
            positiveTestResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_882_ApplicationOpen.png", ScreenshotImageFormat.Png);
        }

        /**
         * Closes current browser once per automation test performed
         */
        [TestCleanup]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}
