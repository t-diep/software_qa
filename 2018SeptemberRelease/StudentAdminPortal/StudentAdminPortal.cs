using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace StudentAdminPortal
{
    [TestClass]
    public class StudentAdminPortal
    {
        IWebDriver driver;
        IJavaScriptExecutor jexe;
        const string adminPortal = "http://10.4.1.99";
        const string regentsWeb = "https://devaccount.regentsscholarship.org/login";

        [TestInitialize]
        public void ConfigureRunBrowser()
        {
            driver = new ChromeDriver();
            jexe = (IJavaScriptExecutor)driver;

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(70);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(70);
            driver.Navigate().GoToUrl(adminPortal);
        }

        //~PRIVATE METHODS~//
        
        /**
         * Helper to log onto admin portal for convienience 
         */
        private void LoginAsAdmin()
        {
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);
        }

        //~END PRIVATE METHODS~//

        //~AUTOMATION TESTS~//

        /**
         * A sample automation test to ensure that the login to admin portal was successful
         */
        [TestMethod]
        public void VerifyLogin()
        {
            LoginAsAdmin();

            Assert.IsTrue(driver.Url == "http://10.4.1.99/user/dashboard");
        }
    }
}
