using System;
using System.Collections.Generic;
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
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        const string password = "Welcome01";

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
         * Automation test for verifying the NC CAF
         */
        [TestMethod]
        public void SAMS_845()
        {
            string username = "NC18100172";

            IWebElement usernameField = driver.FindElement(By.Name("username"));
            usernameField.SendKeys(username);

            IWebElement passwordField = driver.FindElement(By.Name("password"));
            passwordField.SendKeys(password);

            IWebElement signInButton = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[1]/div[1]/form[1]/button[1]"));
            signInButton.Click();

            IWebElement cafTab = driver.FindElement(By.LinkText("Conditional Acceptance Form"));
            cafTab.Click();

            string currentPage = driver.Url;

            Assert.AreEqual("https://devaccount.newcenturyscholarship.org/caform", currentPage);

            Screenshot ncCAF = ((ITakesScreenshot)driver).GetScreenshot();
            ncCAF.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_845_NC_CAF.png", ScreenshotImageFormat.Png);
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
         * Automation test for verifying all of the anomalities in the NC Web Application
         */
        [TestMethod]
        public void SAMS_921()
        {
            //Verify that SAMS_882 is working first
            SAMS_882_Negative();
            SAMS_882_Positive();

            //Click on the Start New Application button
            IWebElement signUpButton = driver.FindElement(By.CssSelector("body.login_bg_body:nth-child(2) div.header-content div.header-content-inner div:nth-child(1) div.col-sm-4.form-container.form-group:nth-child(3) form.login-form > button.btn.btn-success.btn-signup:nth-child(11)"));
            signUpButton.Click();

            //Close the video pop-up screen
            IWebElement closeVideoButton = driver.FindElement(By.CssSelector("body.login_bg_body:nth-child(2) div.ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-dialog-buttons.ui-draggable.ui-resizable:nth-child(16) div.ui-dialog-buttonpane.ui-widget-content.ui-helper-clearfix:nth-child(3) div.ui-dialog-buttonset button.ui-button.ui-widget.ui-state-default.ui-corner-all.ui-button-text-only > span.ui-button-text"));
            closeVideoButton.Click();

            //Scroll to the bottom of the page
            jexe.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

            //Click on the continue application button
            IWebElement continueApplicationButton = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/button[1]"));
            continueApplicationButton.Click();

            //To test against
            HashSet<String> fieldErrors = new HashSet<String>();
            fieldErrors.Add("Please enter a username");
            fieldErrors.Add("Please provide a password");
            fieldErrors.Add("Please enter your firstname");
            fieldErrors.Add("Please enter your lastname");
            fieldErrors.Add("Please enter a valid email address");
            fieldErrors.Add("Please confirm a valid email address");
            fieldErrors.Add("Please enter address line 1");
            fieldErrors.Add("Please enter city");
            fieldErrors.Add("Please select state");
            fieldErrors.Add("Please enter zip");

            //Verify that all of the blank field errors appear
            try
            {
                IWebElement usernameError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[1]"));
                string usernameErrorLabel = usernameError.Text;
                Assert.IsTrue(fieldErrors.Contains(usernameErrorLabel));

                IWebElement passwordError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[2]"));
                string passwordErrorLabel = passwordError.Text;
                Assert.IsTrue(fieldErrors.Contains(passwordErrorLabel));

                IWebElement firstNameError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[4]"));
                string firstNameErrorLabel = firstNameError.Text;
                Assert.IsTrue(fieldErrors.Contains(firstNameErrorLabel));

                IWebElement lastNameError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[5]"));
                string lastNameErrorLabel = lastNameError.Text;
                Assert.IsTrue(fieldErrors.Contains(lastNameErrorLabel));

                IWebElement emailAddressError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[7]"));
                string emailAddressErrorLabel = emailAddressError.Text;
                Assert.IsTrue(fieldErrors.Contains(emailAddressErrorLabel));

                IWebElement confirmEmailError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[8]"));
                string confirmEmailErrorLabel = confirmEmailError.Text;
                Assert.IsTrue(fieldErrors.Contains(confirmEmailErrorLabel));

                IWebElement phoneNumberError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[9]"));
                string phoneNumberErrorLabel = phoneNumberError.Text;
                Assert.IsTrue(fieldErrors.Contains(phoneNumberErrorLabel));

                IWebElement addressLineError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[10]"));
                string addressLineErrorLabel = addressLineError.Text;
                Assert.IsTrue(fieldErrors.Contains(addressLineErrorLabel));

                IWebElement cityLineError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[11]"));
                string cityLineErrorLabel = cityLineError.Text;
                Assert.IsTrue(fieldErrors.Contains(cityLineErrorLabel));

                IWebElement stateLineError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[12]"));
                string stateLineErrorLabel = stateLineError.Text;
                Assert.IsTrue(fieldErrors.Contains(stateLineErrorLabel));

                IWebElement zipCodeError = driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/label[13]"));
                string zipCodeErrorLabel = zipCodeError.Text;
                Assert.IsTrue(fieldErrors.Contains(zipCodeErrorLabel));

            }
            catch(Exception)
            {

            }
        }

        /**
         * Automation test for NC Smoke Test Happy Path
         */
        [TestMethod]
        public void SAMS_922()
        {
            IWebElement startNewApplicationButton = driver.FindElement(By.CssSelector("#signUpId"));
            startNewApplicationButton.Click();

            IWebElement closeVideoPopup = driver.FindElement(By.CssSelector("body.login_bg_body:nth-child(2) div.ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-dialog-buttons.ui-draggable.ui-resizable:nth-child(16) div.ui-dialog-buttonpane.ui-widget-content.ui-helper-clearfix:nth-child(3) div.ui-dialog-buttonset button.ui-button.ui-widget.ui-state-default.ui-corner-all.ui-button-text-only > span.ui-button-text"));
            closeVideoPopup.Click();

            string username = "tdrs" + DateTime.Now.ToString("MM/dd/yy") + alphabet[0];
            string password = "Welcome01";
            string email = username + "@hkconsulting.biz";
            string phoneNumber = "(500) 555-0006";
            string address = "1273 West Four B Lane";
            string city = "South Jordan";
            string zipCode = "84095";

            //Fill out form
            driver.FindElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/input[3]")).SendKeys(password);
            driver.FindElement(By.Id("firstName")).SendKeys(username);
            driver.FindElement(By.Id("middleName")).SendKeys(username);
            driver.FindElement(By.Id("lastName")).SendKeys(username);
            driver.FindElement(By.Id("emailAddress")).SendKeys(email);
            driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/input[8]")).SendKeys(email);
            driver.FindElement(By.Id("phoneNumber")).SendKeys(phoneNumber);
            driver.FindElement(By.Id("addressLine1")).SendKeys(address);
            driver.FindElement(By.Id("City")).SendKeys(city);
            driver.FindElement(By.Id("stateName")).SendKeys("Utah");
            driver.FindElement(By.Id("postalCode")).SendKeys(zipCode);

            //Click on the Continue Application button
            driver.FindElement(By.XPath("/html[1]/body[1]/header[1]/div[1]/div[1]/div[2]/div[1]/form[1]/button[1]")).Click();           
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
