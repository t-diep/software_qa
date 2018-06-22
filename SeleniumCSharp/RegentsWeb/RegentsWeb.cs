using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

/**
 * Contains the test suite for the regents web application
 */
namespace RegentsWeb
{
    [TestClass]
    public class RegentsWeb
    {
        IWebDriver driver;
        IJavaScriptExecutor jexe;


        /*PRIVATE METHODS*/

        /**
         * Helper for hitting the Continue button of the application with all blank fields
         */
        private void SkipCreateNewAccount()
        {
            driver.FindElement(By.Id("signUpId")).Click();

            IWebElement closeVideo = driver.FindElement(By.XPath("/html[1]/body[1]/div[5]/div[3]/div[1]/button[1]"));
            closeVideo.Click();

            jexe.ExecuteScript("scroll(0, 1300)");
            driver.FindElement(By.Id("startYourApplicationId")).Click();
        }

        /**
          * Helper for creating a new account on the regents web application 
          */
        private void CreateNewAccount()
        {
            IWebElement signupButton = driver.FindElement(By.Id("signUpId"));
            signupButton.Click();

            IWebElement closeVideo = driver.FindElement(By.XPath("/html[1]/body[1]/div[5]/div[3]/div[1]/button[1]"));
            closeVideo.Click();

            //Fill out initial information on login page
            driver.FindElement(By.Id("username")).SendKeys("tdrs062118");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.Name("confirm_password")).SendKeys("Welcome01");
            driver.FindElement(By.Id("firstName")).SendKeys("tdrs062118");
            driver.FindElement(By.Id("lastName")).SendKeys("tdrs062118");
            driver.FindElement(By.Id("emailAddress")).SendKeys("tdrs062118@hkconsulting.biz");
            driver.FindElement(By.Name("confirm_email")).SendKeys("tdrs062118@hkconsulting.biz");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("500-555-0006");
            driver.FindElement(By.Id("addressLine1")).SendKeys("11342 South 3275 West");
            driver.FindElement(By.Id("city")).SendKeys("South Jordan");
            driver.FindElement(By.Id("stateName")).SendKeys("Utah");
            driver.FindElement(By.Id("postalCode")).SendKeys("84095");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[3]/form[1]/div[1]/div[14]/div[1]/div[1]/label[1]/input[1]")).Click();
            driver.FindElement(By.Id("referralSourceId")).SendKeys("Utah Scholars Program");
            driver.FindElement(By.Id("startYourApplicationId")).Click();

            Thread.Sleep(1500);
        }

        /*END PRIVATE METHODS*/

        [TestInitialize]
        public void SetUpBrowser()
        {
            driver = new ChromeDriver();
            jexe = (IJavaScriptExecutor)driver;

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(70);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(70);
            driver.Navigate().GoToUrl("https://devaccount.regentsscholarship.org/login");
        }

        /**
         * Tests whether skipping the initial application works as expected
         */
        [TestMethod]
        public void AutomateTestSkipCreateNewAccount()
        {
            SkipCreateNewAccount();
        }

        /**
         * Tests whether filling out the initial application is working
         */
        [TestMethod]
        public void AutomateTestCreateNewAccount()
        {
            CreateNewAccount();
        }

        [TestCleanup]
        public void Close()
        {
            driver.Close();
        }
    }
}
