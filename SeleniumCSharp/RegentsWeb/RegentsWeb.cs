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

            Thread.Sleep(1500);
        }

        /**
          * Helper for creating a new account on the regents web application 
          */
        private void CreateNewAccount()
        {
            string testUsername = "tdrs062618b";

            driver.Manage().Cookies.DeleteAllCookies();

            IWebElement signupButton = driver.FindElement(By.Id("signUpId"));
            signupButton.Click();

            IWebElement closeVideo = driver.FindElement(By.XPath("/html[1]/body[1]/div[5]/div[3]/div[1]/button[1]"));
            closeVideo.Click();

            //Fill out initial information on login page
            driver.FindElement(By.Id("username")).SendKeys(testUsername);
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.Name("confirm_password")).SendKeys("Welcome01");
            driver.FindElement(By.Id("firstName")).SendKeys(testUsername);
            driver.FindElement(By.Id("lastName")).SendKeys(testUsername);
            driver.FindElement(By.Id("emailAddress")).SendKeys(testUsername + "@hkconsulting.biz");
            driver.FindElement(By.Name("confirm_email")).SendKeys(testUsername + "@hkconsulting.biz");
            driver.FindElement(By.Id("phoneNumber")).SendKeys("500-555-0006");
            driver.FindElement(By.Id("addressLine1")).SendKeys("1273 West Four B Lane");
            driver.FindElement(By.Id("city")).SendKeys("South Jordan");
            driver.FindElement(By.Id("stateName")).SendKeys("Utah");
            driver.FindElement(By.Id("postalCode")).SendKeys("84095");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[3]/form[1]/div[1]/div[14]/div[1]/div[1]/label[1]/input[1]")).Click();
            driver.FindElement(By.Id("referralSourceId")).SendKeys("Utah Scholars Program");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[3]/form[1]/div[1]/input[1]")).Click();

            Thread.Sleep(1500);

            //Click on the "Complete Application" button
            driver.FindElement(By.CssSelector("div.wrapper div:nth-child(2) div:nth-child(3) form:nth-child(1) div.signup > input.primary:nth-child(16)")).Click();
        }

        /**
         * Takes an incomplete student account and completes the personal information page
         */
        private void CompletePersonalInformation()
        {
            string username = "tdrs062618b";
            string password = "Welcome01";

            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);

            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]")).Click();
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/div[2]/div[1]/div[2]/div[2]/ul[1]/li[1]/div[2]/button[1]")).Click();

            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/div[1]/ul[1]/li[2]/a[1]")).Click();

            driver.FindElement(By.XPath("/html[1]/body[1]/div[3]/div[3]/div[1]/button[1]")).Click();

            jexe.ExecuteScript("scroll(0, 500)");

            driver.FindElement(By.Name("notifyOption")).Click();
            driver.FindElement(By.Id("dateOfBirthId")).SendKeys("02/02/1996");
            driver.FindElement(By.Id("ssn")).SendKeys("999-99-9999");
            driver.FindElement(By.Id("pinNumber")).SendKeys("9999");

            Thread.Sleep(2000);
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

            Assert.AreEqual("https://devaccount.regentsscholarship.org/login", driver.Url);
        }

        /**
         * Tests whether filling out the initial application is working; the program should take 
         * the user to the Home page of the Regents Web App
         */
        [TestMethod]
        public void AutomateTestCreateNewAccount()
        {
            CreateNewAccount();

            Assert.AreEqual("https://devaccount.regentsscholarship.org/home", driver.Url);
        }

        /**
         * 
         */
        [TestMethod]
        public void AutomateTestCompletePersonalInformation()
        {
            CompletePersonalInformation();

            Assert.AreEqual("https://devaccount.regentsscholarship.org/regents/personal", driver.Url);
        }

        [TestCleanup]
        public void Close()
        {
            driver.Close();
        }
    }
}
