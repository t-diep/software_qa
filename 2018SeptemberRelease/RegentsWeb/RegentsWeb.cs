﻿using System;
using System.Drawing.Imaging;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;

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
         * Automate test for verifying the minimum ACT composite score for 2019 RS students is 22
         * This will test for a negative threshold result (i.e. enter a ACT composite score of 21)
         */
        [TestMethod]
        [Ignore]
        public void SAMS_672_Negative()
        {
            //The test account credentials to log into
            string username = "tdrs070518a";
            string password = "Welcome01";

            //Log on to RS Web App for given test account credentials 
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]")).Click();

            driver.FindElement(By.Id("applyScholarshipBtn")).Click();

            //Click on the "Educational Information" tab on the top
            driver.FindElement(By.CssSelector("div:nth-child(1) div:nth-child(3) div.wizard:nth-child(1) ul.steps > li.page_hover_rsnt:nth-child(3)")).Click();
        
            jexe.ExecuteScript("window.scrollTo(0, 750)");

            //Test for negative result; verify that the minimum ACT composite score violation pop-up shows
            IWebElement actField = driver.FindElement(By.CssSelector("section.fuelux:nth-child(4) div.step-content div.step-pane.active form:nth-child(1) div.row:nth-child(9) div.col-sm-2:nth-child(1) > input.form-control:nth-child(2)"));
            actField.SendKeys("21");
            actField.SendKeys(Keys.Tab);

            //Take screenshot of result
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_672_Negative.png", ScreenshotImageFormat.Png);

            Assert.IsTrue(driver.FindElement(By.CssSelector("body:nth-child(2) div.bootbox.modal.fade.danger.in:nth-child(4) div.modal-dialog > div.modal-content")).Displayed);
        }

        /**
          * Automate test for verifying the minimum ACT composite score for 2019 RS students is 22
          * This will test for a positive threshold result (i.e. enter a ACT composite score of 22)
          */
        [TestMethod]
        public void SAMS_672_Positive()
        {
            //The test account credentials to log into
            string username = "tdrs070518a";
            string password = "Welcome01";

            //Log on to RS Web App for given test account credentials 
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]")).Click();

            driver.FindElement(By.Id("applyScholarshipBtn")).Click();

            //Click on the "Educational Information" tab on the top
            driver.FindElement(By.CssSelector("div:nth-child(1) div:nth-child(3) div.wizard:nth-child(1) ul.steps > li.page_hover_rsnt:nth-child(3)")).Click();

            jexe.ExecuteScript("window.scrollTo(0, 750)");

            //Test for negative result; verify that the minimum ACT composite score violation pop-up shows
            IWebElement actField = driver.FindElement(By.CssSelector("section.fuelux:nth-child(4) div.step-content div.step-pane.active form:nth-child(1) div.row:nth-child(9) div.col-sm-2:nth-child(1) > input.form-control:nth-child(2)"));
            actField.SendKeys("22");
            actField.SendKeys(Keys.Tab);

            //Take screenshot of result
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_672_Positive.png", ScreenshotImageFormat.Png);

            //Verifying if the error-message doesn't show for entering an ACT Composite score of 22
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                driver.FindElement(By.CssSelector("body:nth-child(2) div.bootbox.modal.fade.danger.in:nth-child(4) div.modal-dialog > div.modal-content"));
                Assert.Fail("Error pop-up message should not show");
            }
            catch(Exception)
            {
                //The minimum ACT score error-pop up did not show, which is correct
            }          
        }

        /**
         * Automate test for the negative result for the minimum GPA requirement for 2019 RS students 
         * (i.e. test for negative threshold of 2.999 GPA)
         */
        [TestMethod]
        public void SAMS_724_Negative()
        {
            //The test account credentials to log into
            string username = "tdrs070518a";
            string password = "Welcome01";

            //Log on to RS Web App for given test account credentials 
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]")).Click();

            driver.FindElement(By.Id("applyScholarshipBtn")).Click();

            //Click on the "Educational Information" tab on the top
            driver.FindElement(By.CssSelector("div:nth-child(1) div:nth-child(3) div.wizard:nth-child(1) ul.steps > li.page_hover_rsnt:nth-child(3)")).Click();

            jexe.ExecuteScript("window.scrollTo(0, 750)");

            IWebElement gpaField = driver.FindElement(By.Id("cumulativeGpa"));
            gpaField.SendKeys("3.299");

            Assert.IsTrue(driver.FindElement(By.Id("cumulativeGpa-error")).Displayed);

            //Take screenshot of result
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_724_Negative.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automate test for the positive result for the minimum GPA requirement 
         * (i.e. minimum GPA positive threshold is at 3.3 GPA)
         */
        [TestMethod]
        public void SAMS_724_Positive()
        {
            //The test account credentials to log into
            string username = "tdrs070518a";
            string password = "Welcome01";

            //Log on to RS Web App for given test account credentials 
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]")).Click();

            driver.FindElement(By.Id("applyScholarshipBtn")).Click();

            //Click on the "Educational Information" tab on the top
            driver.FindElement(By.CssSelector("div:nth-child(1) div:nth-child(3) div.wizard:nth-child(1) ul.steps > li.page_hover_rsnt:nth-child(3)")).Click();

            jexe.ExecuteScript("window.scrollTo(0, 750)");

            IWebElement gpaField = driver.FindElement(By.Id("cumulativeGpa"));
            gpaField.SendKeys("3.3");
            gpaField.SendKeys(Keys.Tab);

            try
            {
                driver.FindElement(By.Id("cumulativeGpa-error"));
                Assert.Fail("Cumulative GPA error should not show");
            }
            catch(Exception)
            {
                
            }

            //Take screenshot of result
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_724_Positive.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for configuring RS Application settings under admin portal for 2018 RS applicants
         */
        [TestMethod]
        public void SAMS_883_2018_Cohort_Negative()
        {   
            //Go to Admin Portal and log in as admin
            driver.Navigate().GoToUrl("http://10.4.1.99");
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[2]/div[1]/form[1]/span[1]/button[1]")).Click();

            //Click on the "Settings" tab on the left
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/aside[1]/nav[1]/ul[1]/li[2]/ul[1]/li[9]/a[1]")).Click();

            IWebElement academicYearField = driver.FindElement(By.Name("academicYear"));
            academicYearField.Clear();
            academicYearField.SendKeys("2018");

            IWebElement applicationYearField = driver.FindElement(By.Name("applicationYear"));
            applicationYearField.Clear();
            applicationYearField.SendKeys("2018");

            string deadline = "07/05/2018 23:59:00";

            IWebElement deadlineField = driver.FindElement(By.Name("finalDeadline"));
            deadlineField.Clear();
            deadlineField.SendKeys(deadline);
            deadlineField.SendKeys(Keys.Enter);

            //Save button
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/section[1]/div[1]/div[1]/div[1]/div[2]/form[1]/div[9]/button[1]")).Click();

            //Close successful notification pop-up before logging out
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/ul[1]/li[1]/div[1]/button[1]")).Click();

            //Logout button
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/header[1]/ul[2]/li[2]/a[1]")).Click();

            //Going to RS Web Application
            driver.Navigate().GoToUrl("https://devaccount.regentsscholarship.org/login");

            //Verify that the New Scholarship button is not there
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                driver.FindElement(By.Id("signUpId"));
                Assert.Fail("New Application button should not be shown");
            }
            catch(Exception)
            {
                //New Application button not showing as expected
            }

            //Take screenshot of result
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_883_2018_Negative.png", ScreenshotImageFormat.Png);
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

            //Take screenshot of result
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_907.png", ScreenshotImageFormat.Png);

            Assert.IsTrue(driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/div[1]/div[1]/ul[1]/li[1]/div[1]")).Displayed);
            Assert.AreEqual("(123) 765-4321", driver.FindElement(By.Name("phoneNumber")).GetAttribute("value"));
        }

        /**
         * Closes the browser for each automated test after running to clear for the next automation test
         */
        [TestCleanup]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}
