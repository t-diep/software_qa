using System;
using System.Threading;
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
        public void SAMS_907()
        {
            //Test student account credentials
            string username = "tdrs070318b";
            string password = "Welcome01";

            //Enter given username and password, then click on "Sign in" button
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]")).Click();

            //Account settings
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/ul[1]/li[5]/a[1]")).Click();
            
            //Delete current phone number and enter in a new phone number
            driver.FindElement(By.Name("phoneNumber")).Clear();

            //Locate the phone number field and enter in the test incorrect phone number
            IWebElement phoneNumField = driver.FindElement(By.Name("phoneNumber"));
            phoneNumField.SendKeys("123-765-4321");

            //Close the email or both email and text message pop-up
            driver.FindElement(By.Id("notifyOption2")).Click();

            Thread.Sleep(3000);

            driver.FindElement(By.XPath("/html[1]/body[1]/div[2]/div[1]/div[1]/div[3]/button[1]")).Click();

            jexe.ExecuteScript("scroll(0, 600)");

            Thread.Sleep(3000);

            //Click on save button to save changes
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/section[1]/form[1]/div[3]/button[1]")).Click();

            jexe.ExecuteScript("scroll(0, 0)");

            Assert.IsTrue(driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/div[1]/div[1]/ul[1]/li[1]/div[1]")).Displayed);
            Assert.AreEqual("(123) 765-4321", driver.FindElement(By.Name("phoneNumber")).GetAttribute("value"));
        }
    }
}
