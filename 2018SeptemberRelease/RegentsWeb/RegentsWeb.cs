using System;
using System.Drawing.Imaging;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace RegentsWeb
{
    [TestClass]
    public class RegentsWeb
    {
        IWebDriver driver;
        IJavaScriptExecutor jexe;
        const string adminPortal = "http://10.4.1.99";
        const string regentsWeb = "https://devaccount.regentsscholarship.org/login";
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
            driver.Navigate().GoToUrl(regentsWeb);
        }

        /**
         * Automation test for negative threshold for the 2019 RS Minimum GPA requirement
         * (i.e. test for GPA to be 3.299)
         */
        [TestMethod]
        public void SAMS_671_Negative()
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
            gpaField.SendKeys(Keys.Enter);

            try
            {
                IWebElement gpaError = driver.FindElement(By.CssSelector("div.bootbox.modal.fade.danger.in:nth-child(4) div.modal-dialog div.modal-content div.modal-body div.bootbox-body > span:nth-child(1)"));
                Assert.IsTrue(gpaError.Displayed);
            }
            catch(Exception)
            {
                Assert.Fail("GPA Error popup should show for GPA less than 3.3");
            }

            //Take screenshot of result
            Screenshot minGPANegativeResult = ((ITakesScreenshot)driver).GetScreenshot();
            minGPANegativeResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_671_Negative.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for positive threshold for the 2019 RS Minimum GPA requirement
         * (i.e. test for GPA to be 3.3)
         */
        [TestMethod]
        public void SAMS_671_Positive()
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

            //Take screenshot of result
            Screenshot minGPANegativeResult = ((ITakesScreenshot)driver).GetScreenshot();
            minGPANegativeResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_671_Positive.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test to verify the new text changes for the FAFSA questions on RS Web
         */
        [TestMethod]
        public void SAMS_674()
        {
            string username = "tdrs070518a";
            string password = "Welcome01";

            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]")).Click();
            driver.FindElement(By.Id("applyScholarshipBtn")).Click();

            driver.FindElement(By.CssSelector("div:nth-child(3) div.wizard:nth-child(1) ul.steps li.black_div.page_hover_rs:nth-child(2) > a:nth-child(1)")).Click();
            driver.FindElement(By.CssSelector("body:nth-child(2) div.ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-dialog-buttons.ui-draggable.ui-resizable:nth-child(4) div.ui-dialog-buttonpane.ui-widget-content.ui-helper-clearfix:nth-child(3) div.ui-dialog-buttonset > button:nth-child(1)")).Click();

            jexe.ExecuteScript("scroll(0, 750)");

            string fafsaText = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/section[1]/div[1]/div[1]/form[1]/div[14]/div[2]/div[1]/div[1]/span[1]")).Text;
            string expected = "I understand that I must complete and submit the Free Application for Federal Student Aid (FAFSA) by (coded application deadline) or by (coded priority application deadline) to meet the priority deadline.";

            //Verify the correct fafsa text
            Assert.AreEqual(expected, fafsaText);

            //Click on the "Save and Continue" button
            driver.FindElement(By.CssSelector("section.fuelux:nth-child(3) div.step-content div.step-pane.active form:nth-child(1) div.row.button_bottom_align:nth-child(15) > button.pull-right.btn.btn-primary.position.btn-next")).Click();

            //Verify the error pop-up for not checking the fafsa checkbox is displayed
            Assert.IsTrue(driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/section[1]/div[1]/div[1]/form[1]/div[14]/div[2]/div[1]/div[1]/label[1]")).Displayed);

            //Save screenshot for negative result
            Screenshot uncheckedBoxResult = ((ITakesScreenshot)driver).GetScreenshot();
            uncheckedBoxResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_674_Negative.png", ScreenshotImageFormat.Png);

            driver.FindElement(By.Name("agreeTerms")).Click();

            //Click on the "Save and Continue" button
            driver.FindElement(By.CssSelector("section.fuelux:nth-child(3) div.step-content div.step-pane.active form:nth-child(1) div.row.button_bottom_align:nth-child(15) > button.pull-right.btn.btn-primary.position.btn-next")).Click();

            Assert.IsTrue(driver.Url == "https://devaccount.regentsscholarship.org/regents/education");

            //Save screenshot for positive result
            Screenshot checkedBoxResult = ((ITakesScreenshot)driver).GetScreenshot();
            checkedBoxResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_674_Positive.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automate test for verifying the minimum ACT composite score for 2019 RS students is 22
         * This will test for a negative threshold result (i.e. enter a ACT composite score of 21)
         */
        [TestMethod]
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
            gpaField.SendKeys(Keys.Enter);

            IWebElement gpaTooLowError = driver.FindElement(By.CssSelector("div.bootbox.modal.fade.danger.in:nth-child(4) div.modal-dialog div.modal-content div.modal-header > h4.modal-title"));           
            Assert.IsTrue(gpaTooLowError.Displayed);

            //Take screenshot of result
            Screenshot negativeTestResult = ((ITakesScreenshot)driver).GetScreenshot();
            negativeTestResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_724_Negative.png", ScreenshotImageFormat.Png);
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

            jexe.ExecuteScript("scroll(0, 750)");

            IWebElement gpaField = driver.FindElement(By.Id("cumulativeGpa"));
            gpaField.SendKeys("3.3");
            gpaField.SendKeys(Keys.Tab);

            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
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
         * Automation test for verifying for the duplicate email error not showing for a particular 2019 RS student 
         * account
         */
        [TestMethod]
        public void SAMS_793()
        {
            //Enter in username, password, and then click on the sign in button
            driver.FindElement(By.Name("username")).SendKeys("nwrs0802b");
            driver.FindElement(By.Name("password")).SendKeys("Welcome01");
            driver.FindElement(By.CssSelector("div.wrapper div:nth-child(2) div:nth-child(1) div.login:nth-child(3) form.login-form > input.primary:nth-child(6)")).Click();

            //Clicking on "Complete Now"
            driver.FindElement(By.Id("applyScholarshipBtn")).Click();

            //Clicking on "Personal Information" tab on the top 
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/div[1]/ul[1]/li[2]/a[1]")).Click();

            //Clicking on the Close button to close video pop-up
            driver.FindElement(By.CssSelector("body:nth-child(2) div.ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-dialog-buttons.ui-draggable.ui-resizable:nth-child(4) div.ui-dialog-buttonpane.ui-widget-content.ui-helper-clearfix:nth-child(3) div.ui-dialog-buttonset > button:nth-child(1)")).Click();

            //Locating the primary email address field and testing using the specified email address for the test case
            IWebElement emailAddressField = driver.FindElement(By.Name("emailAddress"));
            emailAddressField.Clear();
            emailAddressField.SendKeys("miguelrivera@ushe.edu");
            emailAddressField.SendKeys(Keys.Tab);

            //Get the email address we entered in the field
            string textValue = emailAddressField.GetAttribute("value");

            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);              
                //Search for the duplicate email address error message
                driver.FindElement(By.CssSelector("#emailAddress-error"));                
                Assert.Fail("Duplicate email error message should not show");
            }
            catch (Exception)
            {
                Assert.IsTrue(textValue == "miguelrivera@ushe.edu");
            }

            //Take screenshot of result
            Screenshot noDuplicateEmailErrorResult = ((ITakesScreenshot)driver).GetScreenshot();
            noDuplicateEmailErrorResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_793_NoDuplicateEmailError");
        }

        /**
         * Automation test for configuring RS Application settings under admin portal for 2018 RS applicants
         */
        [TestMethod]
        public void SAMS_883_2018_Cohort()
        {   
            //~NEGATIVE TEST~//

            //Go to Admin Portal and log in as admin
            driver.Navigate().GoToUrl(adminPortal);
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

            string negativeDeadline = "07/05/2018 23:59:00";

            IWebElement deadlineField = driver.FindElement(By.Name("finalDeadline"));
            deadlineField.Clear();
            deadlineField.SendKeys(negativeDeadline);
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
            Screenshot negativeResult = ((ITakesScreenshot)driver).GetScreenshot();
            negativeResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_883_2018_Negative.png", ScreenshotImageFormat.Png);

            //~END NEGATIVE TEST~//


            //~POSITIVE TEST~//
            driver.Navigate().GoToUrl(adminPortal);

            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[2]/div[1]/form[1]/span[1]/button[1]")).Click();

            //Click on the "Settings" tab on the left
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/aside[1]/nav[1]/ul[1]/li[2]/ul[1]/li[9]/a[1]")).Click();

            string positiveDeadline = "12/09/2018 23:59:00";

            IWebElement deadlineFieldAgain = driver.FindElement(By.Name("finalDeadline"));
            deadlineFieldAgain.Clear();
            deadlineFieldAgain.SendKeys(positiveDeadline);
            deadlineFieldAgain.SendKeys(Keys.Enter);

            //Save button
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/section[1]/div[1]/div[1]/div[1]/div[2]/form[1]/div[9]/button[1]")).Click();

            //Close successful notification pop-up before logging out
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/ul[1]/li[1]/div[1]/button[1]")).Click();

            //Logout button
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/header[1]/ul[2]/li[2]/a[1]")).Click();

            //Going to RS Web Application
            driver.Navigate().GoToUrl("https://devaccount.regentsscholarship.org/login");

            Assert.IsTrue(driver.FindElement(By.Id("signUpId")).Displayed);

            //Take screenshot of result
            Screenshot positiveResult = ((ITakesScreenshot)driver).GetScreenshot();
            positiveResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_883_2018_Positive.png", ScreenshotImageFormat.Png);

            //~FILL OUT INITIAL FORM AND HITTING CONTINUE~//
            driver.FindElement(By.Name("username")).SendKeys("tdrs070918e");
            driver.FindElement(By.Name("password")).SendKeys("Welcome01");
            driver.FindElement(By.CssSelector("div.wrapper div:nth-child(2) div:nth-child(1) div.login:nth-child(3) form.login-form > input.primary:nth-child(6)")).Click();
            driver.FindElement(By.Id("applyScholarshipBtn")).Click();

            string header = driver.FindElement(By.CssSelector("div:nth-child(1) div:nth-child(3) div.row.step-pane.active:nth-child(2) div.col-lg-12 > h2.color-title:nth-child(1)")).Text;

            Assert.IsTrue(header.Contains("2018"));

            //Take screenshot of result
            Screenshot positiveResult2018 = ((ITakesScreenshot)driver).GetScreenshot();
            positiveResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_883_2019_Positive.png", ScreenshotImageFormat.Png);
        }

        /**
         * Same automation test as SAMS_883_2018_Cohort except it's testing for 2019 cohort applicants
         */
        [TestMethod]
        public void SAMS_883_2019_Cohort()
        {
            //~NEGATIVE TEST~//

            //Go to Admin Portal and log in as admin
            driver.Navigate().GoToUrl(adminPortal);
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[2]/div[1]/form[1]/span[1]/button[1]")).Click();

            //Click on the "Settings" tab on the left
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/aside[1]/nav[1]/ul[1]/li[2]/ul[1]/li[9]/a[1]")).Click();

            IWebElement academicYearField = driver.FindElement(By.Name("academicYear"));
            academicYearField.Clear();
            academicYearField.SendKeys("2019");

            IWebElement applicationYearField = driver.FindElement(By.Name("applicationYear"));
            applicationYearField.Clear();
            applicationYearField.SendKeys("2019");

            string negativeDeadline = "07/05/2019 23:59:00";

            IWebElement deadlineField = driver.FindElement(By.Name("finalDeadline"));
            deadlineField.Clear();
            deadlineField.SendKeys(negativeDeadline);
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
            catch (Exception)
            {
                //New Application button not showing as expected
            }

            //Take screenshot of result
            Screenshot negativeResult = ((ITakesScreenshot)driver).GetScreenshot();
            negativeResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_883_2018_Negative.png", ScreenshotImageFormat.Png);

            //~END NEGATIVE TEST~//


            //~POSITIVE TEST~//
            driver.Navigate().GoToUrl(adminPortal);

            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[2]/div[1]/form[1]/span[1]/button[1]")).Click();

            //Click on the "Settings" tab on the left
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/aside[1]/nav[1]/ul[1]/li[2]/ul[1]/li[9]/a[1]")).Click();

            string positiveDeadline = "12/09/2019 23:59:00";

            IWebElement deadlineFieldAgain = driver.FindElement(By.Name("finalDeadline"));
            deadlineFieldAgain.Clear();
            deadlineFieldAgain.SendKeys(positiveDeadline);
            deadlineFieldAgain.SendKeys(Keys.Enter);

            //Save button
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/section[1]/div[1]/div[1]/div[1]/div[2]/form[1]/div[9]/button[1]")).Click();

            //Close successful notification pop-up before logging out
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/ul[1]/li[1]/div[1]/button[1]")).Click();

            //Logout button
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/header[1]/ul[2]/li[2]/a[1]")).Click();

            //Going to RS Web Application
            driver.Navigate().GoToUrl("https://devaccount.regentsscholarship.org/login");

            Assert.IsTrue(driver.FindElement(By.Id("signUpId")).Displayed);

            //~FILL OUT INITIAL FORM AND HITTING CONTINUE~//
            driver.FindElement(By.Name("username")).SendKeys("tdrs070918f");
            driver.FindElement(By.Name("password")).SendKeys("Welcome01");
            driver.FindElement(By.CssSelector("div.wrapper div:nth-child(2) div:nth-child(1) div.login:nth-child(3) form.login-form > input.primary:nth-child(6)")).Click();
            driver.FindElement(By.Id("applyScholarshipBtn")).Click();

            string header = driver.FindElement(By.CssSelector("div:nth-child(1) div:nth-child(3) div.row.step-pane.active:nth-child(2) div.col-lg-12 > h2.color-title:nth-child(1)")).Text;

            Assert.IsTrue(header.Contains("2019"));

            //Take screenshot of result
            Screenshot positiveResult = ((ITakesScreenshot)driver).GetScreenshot();
            positiveResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_883_2019_Positive.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for verifying the RS CAF 
         */
        [TestMethod]
        public void SAMS_844()
        {
            string acctNumber = "RS18106909";
            string password = "Welcome01";

            IWebElement usernameField = driver.FindElement(By.Name("username"));
            usernameField.SendKeys(acctNumber);

            IWebElement passwordField = driver.FindElement(By.Name("password"));
            passwordField.SendKeys(password);

            IWebElement signInButton = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]"));
            signInButton.Click();

            IWebElement cafFormTab = driver.FindElement(By.LinkText("Conditional Acceptance Form"));
            cafFormTab.Click();

            string currentPage = driver.Url;

            Assert.AreEqual("https://devaccount.regentsscholarship.org/regents/caform", currentPage);

            Screenshot rsCAF = ((ITakesScreenshot)driver).GetScreenshot();
            rsCAF.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_844_RS_CAF.png", ScreenshotImageFormat.Png);
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
         * Automate test for forgotten username functionality on RS Web Application
         */
        [TestMethod]
        public void SAMS_918_ForgotUsername()
        {
            string invalidEmail = "testinvalidemail@gmail.com";
            string validEmail = "anthony.tony.diep@gmail.com";

            //~INVALID EMAIL ADDRESS~//

            //Click on "Don't know your username?" hyperlink, then enter in credentials
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/a[1]")).Click();
            driver.FindElement(By.Id("email")).SendKeys(invalidEmail);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/form[1]/div[1]/input[1]")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("body.modal-open:nth-child(2) div.bootbox.modal.fade.bootbox-alert.in div.modal-dialog div.modal-content div.modal-body div.bootbox-body > div.text-danger")));

            //Verify that the system handles invalid email addresses with a error display pop-up
            try
            {
                string dangerText = driver.FindElement(By.CssSelector("body.modal-open:nth-child(2) div.bootbox.modal.fade.bootbox-alert.in div.modal-dialog div.modal-content div.modal-body div.bootbox-body > div.text-danger")).Text;
                string expected = "Email Id doesn't match";
                Assert.AreEqual(expected, dangerText);
            }
            catch(Exception)
            {
                Assert.Fail("Invalid email should trigger error pop up message");
            }

            Screenshot invalidEmailResult = ((ITakesScreenshot)driver).GetScreenshot();
            invalidEmailResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_918_Username_Invalid_Email.png", ScreenshotImageFormat.Png);

            //~END INVALID EMAIL ADDRESS~//

            //~VALID EMAIL ADDRESS~//

            //Close error pop-up, then enter valid email credentials
            driver.FindElement(By.CssSelector("body.modal-open:nth-child(2) div.bootbox.modal.fade.bootbox-alert.in div.modal-dialog div.modal-content div.modal-footer > button.btn.btn-primary")).Click();

            //Wait until the error-pop up closes before entering the email field again
            WebDriverWait waitToEnterEmailAgain = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("email")));

            IWebElement emailField = driver.FindElement(By.Id("email"));
            emailField.Clear();
            emailField.SendKeys(validEmail);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/form[1]/div[1]/input[1]")).Click();

            WebDriverWait waitForPopup = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html[1]/body[1]/div[5]/div[1]/div[1]/div[1]/div[1]/div[1]")));

            //Verify that the username notification shows 
            try
            {
                string notification = driver.FindElement(By.XPath("/html[1]/body[1]/div[5]/div[1]/div[1]/div[1]/div[1]/div[1]")).Text;
                string expected = "Your username will be sent to the email anthony.tony.diep@gmail.com. Please check your email.";

                Assert.AreEqual(expected, notification);
            }
            catch(Exception)
            {
                Assert.Fail("Notification to find username not showing");
            }

            Screenshot usernameNotification = ((ITakesScreenshot)driver).GetScreenshot();
            usernameNotification.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_918_Username_Valid_Email.png", ScreenshotImageFormat.Png);

            //~END VALID EMAIL ADDRESS~//
        }

        /**
         * Automate test for forgotten password functionaltity on RS Web Application
         */
        [TestMethod]
        public void SAMS_918_ForgotPassword()
        {
            //Click on the "Forgotten Password" link
            driver.FindElement(By.CssSelector("#loginForm > a:nth-child(9)")).Click();

            //Enter username and click on the reset password button
            IWebElement usernameField = driver.FindElement(By.Name("username"));
            usernameField.SendKeys("tdrs070618c");
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/div[1]/input[1]")).Click();

            //Verify that the forgotten password instructions show
            try
            {
                string forgotPassword = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/h6[1]")).Text;
                string expected = "Please check your email for additional instructions to reset your password.";

                Assert.IsTrue(driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/h6[1]")).Displayed);
                Assert.AreEqual(expected, forgotPassword);
            }
            catch (Exception)
            {
                Assert.Fail("Forgot password instructions should display");
            }

            //Take screenshot of result
            Screenshot forgottenPasswordResult = ((ITakesScreenshot)driver).GetScreenshot();
            forgottenPasswordResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_918_Password.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for testing the proper text displayed for 2018 RS Student
         * 
         * (TODO) Finish writing test case
         */
        [TestMethod]
        [Ignore]
        public void SAMS_939()
        {
            IWebElement usernameField = driver.FindElement(By.Name("username"));
            usernameField.SendKeys("RS18100120");

            IWebElement passwordField = driver.FindElement(By.Name("password"));
            passwordField.SendKeys(password);

            IWebElement signInButton = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]"));
            signInButton.Click();

            IWebElement completeNowButton = driver.FindElement(By.Id("applyScholarshipBtn"));
            completeNowButton.Click();

            IWebElement courseWorkTab = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/div[1]/ul[1]/li[4]/a[1]"));
            courseWorkTab.Click();

            IWebElement closeVideoButton = driver.FindElement(By.CssSelector("body:nth-child(2) div.ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-dialog-buttons.ui-draggable.ui-resizable:nth-child(3) div.ui-dialog-buttonpane.ui-widget-content.ui-helper-clearfix:nth-child(3) div.ui-dialog-buttonset > button:nth-child(1)"));
            closeVideoButton.Click();
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
