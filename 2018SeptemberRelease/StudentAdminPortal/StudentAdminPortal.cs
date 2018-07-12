using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

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

            //Login to the Student Admin Portal
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);
        }

        //~PRIVATE METHODS~//
       

        //~END PRIVATE METHODS~//

        //~AUTOMATION TESTS~//

        /**
         * A sample automation test to ensure that the login to admin portal was successful
         */
        [TestMethod]
        public void VerifyLogin()
        {
            Assert.IsTrue(driver.Url == "http://10.4.1.99/user/dashboard");
        }

        /**
         * Automation test for verifying the reviewer drop-down error has been fixed
         */
        [TestMethod]
        public void SAMS_915()
        {
            driver.FindElement(By.LinkText("Students")).Click();

            IWebElement acctNumberField = driver.FindElement(By.Id("accountNumber"));
            acctNumberField.SendKeys("RS19100172");
            acctNumberField.SendKeys(Keys.Enter);
            driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row.student-search div.col-md-12 div.panel.panel-success:nth-child(2) div.col-lg-12.background_main_bg div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer tbody:nth-child(2) tr.odd td.sorting_1:nth-child(1) > a.btn.btn-primary")).Click();

            WebDriverWait waitForAppInfo = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            IWebElement appInfo = waitForAppInfo.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/ul[1]/li[3]/a[1]")));
            appInfo.Click();

            //IWebElement innerScrollMenu = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/form[1]/div[2]"));
            //innerScrollMenu.Click();

            Actions scrollDown = new Actions(driver);

            IWebElement fafsaDateField = driver.FindElement(By.Id("fafsaDocument.statusDate"));
            scrollDown.MoveToElement(fafsaDateField);
            scrollDown.Perform();
        }
    }
}
