using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RegentsWeb
{
    [TestClass]
    public class RegentsWeb
    {
        IWebDriver driver;
        IJavaScriptExecutor jexe;

        [TestInitialize]
        public void ConfigureRunBrowser()
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
         * Automate test for inputting an incorrect phone number without stack trace errors
         */
        [TestMethod]
        public void SAMS_915()
        {
            string currDate = DateTime.Today.ToString("MMddyy");

            string testUsername = "tdrs" + currDate;

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
            driver.FindElement(By.Id("phoneNumber")).SendKeys("123-456-7890");
            driver.FindElement(By.Id("addressLine1")).SendKeys("1273 West Four B Lane");
            driver.FindElement(By.Id("city")).SendKeys("South Jordan");
            driver.FindElement(By.Id("stateName")).SendKeys("Utah");
            driver.FindElement(By.Id("postalCode")).SendKeys("84095");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[3]/form[1]/div[1]/div[14]/div[1]/div[1]/label[1]/input[1]")).Click();
            driver.FindElement(By.Id("referralSourceId")).SendKeys("Utah Scholars Program");
   
            //Click on the "Complete Application" button
            driver.FindElement(By.Id("startYourApplicationId")).Click();
        }
    }
}
