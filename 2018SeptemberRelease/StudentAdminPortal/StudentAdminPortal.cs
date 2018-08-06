using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
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
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        [TestInitialize]
        public void ConfigureRunBrowser()
        {
            driver = new ChromeDriver();
            jexe = (IJavaScriptExecutor)driver;

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(100);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(100);
            driver.Navigate().GoToUrl(adminPortal);

            //Login to the Student Admin Portal
            driver.FindElement(By.Id("username")).SendKeys("admin");
            driver.FindElement(By.Id("password")).SendKeys("Welcome01");
            driver.FindElement(By.Id("password")).SendKeys(Keys.Enter);
        }

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
         * 
         */
        [TestMethod]
        public void TestWaitForAppInfoToLoadThenClick()
        {
            driver.Navigate().GoToUrl("http://10.4.1.99/regents/appReview?stateStudentId=RS19100153");

            WebDriverWait waitForAppInfoTab = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            Actions appInfoTabActions = new Actions(driver);
            IWebElement appInfoTab = waitForAppInfoTab.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.LinkText("App Info")));
            appInfoTabActions.MoveToElement(appInfoTab);
            appInfoTabActions.Click().Build().Perform();          
        }

        /**
         * Automation test for denying a student if they submitted their FAFSA late
         */
        [TestMethod]
        public void SAMS_720()
        {
            //Log into Student Admin Portal if necessary
            if(driver.Url == "http://10.4.1.99/login")
            {
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("Welcome01");
            }

            //Directly access the account number RS19100109 
            driver.Navigate().GoToUrl("http://10.4.1.99/regents/appReview?stateStudentId=RS19100109");

            ////
            //WebDriverWait waitForAppInfoTab = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //Actions appInfoTabActions = new Actions(driver);
            //IWebElement appInfoTab = waitForAppInfoTab.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/ul[1]/li[3]/a[1]")));
            //appInfoTabActions.MoveToElement(appInfoTab);
            //appInfoTabActions.Click().Build().Perform();

            IWebElement appInfoTab = driver.FindElement(By.LinkText("App Info"));

            //Jexe
            jexe.ExecuteScript("arguments[0].click()", appInfoTab);
            jexe.ExecuteScript("arguments[0].click()", appInfoTab);

            Thread.Sleep(3500);

            IWebElement reviewStatus = driver.FindElement(By.Name("scholarshipApplication.currentReviewType"));

            Actions moveToReviewStatus = new Actions(driver);
            moveToReviewStatus.MoveToElement(reviewStatus).Build().Perform();
            moveToReviewStatus.SendKeys("Second Transcript");

            Thread.Sleep(3500);

            IWebElement reassignButton = driver.FindElement(By.Id("reassign"));
            reassignButton.Click();

            //Dismiss pop-up notifying changes have been saved
            driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/div[1]/div[1]/ul[1]/li[1]/div[1]/button[1]")).Click();
          
            //Enter in the threshold deadline for submitting FAFSA
            IWebElement fafsaSubmittedDate = driver.FindElement(By.Name("fafsaDocument.statusDate"));
            Actions moveToFafsaDateField = new Actions(driver);
            moveToFafsaDateField.MoveToElement(fafsaSubmittedDate);
            moveToFafsaDateField.SendKeys("02/02/2019");

            //Save changes
            IWebElement saveButton = driver.FindElement(By.Id("saveReview"));
            Actions moveToSaveButton = new Actions(driver);
            moveToSaveButton.MoveToElement(saveButton);
            moveToSaveButton.Click().Build().Perform();

            //Complete the review 
            IWebElement completeReviewButton = driver.FindElement(By.Id("completeReview"));
            Actions completeTheReview = new Actions(driver);
            completeTheReview.MoveToElement(completeReviewButton);
            completeTheReview.Click().Build().Perform();

            //Get the label of the current award status
            IWebElement awardStatusLabel = driver.FindElement(By.Id("awardStatusTop"));
            string currAwardStatus = awardStatusLabel.Text;

            IWebElement fafsaSubmittedLate = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/form[1]/div[2]/div[1]/div[1]/div[2]/div[2]/div[1]/select[1]/option[21]"));

            //Verify the student is denied
            Assert.IsTrue(currAwardStatus == "Denied" && fafsaSubmittedLate.Selected);
        }

        /**
         * Automation test for verifying the new award and review statuses for 2019 RS students
         */
        [TestMethod]
        public void SAMS_770()
        {
            driver.Navigate().GoToUrl("http://10.4.1.99/regents/appReview?stateStudentId=RS19100153");

            Thread.Sleep(1000);

            IWebElement appInfoTab = driver.FindElement(By.LinkText("App Info"));
            appInfoTab.Click();

            IWebElement awardReviewLabel = driver.FindElement(By.Id("awardStatusTop"));
            string awardStatus = awardReviewLabel.Text;

            IWebElement reviewStatusLabel = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/form[1]/div[1]/div[1]/div[1]/h4[1]/span[1]"));
            string reviewStatus = reviewStatusLabel.Text;

            Assert.IsTrue(awardStatus == "Application Downloaded");
            Assert.IsTrue(reviewStatus == "Not Started");

            /*TODO: Complete steps 2 - 6 */
        }

        /**
         * Automation test for pulling the New "Application By College Choice" report
         */
        [TestMethod]
        public void SAMS_785()
        {
            //Click on the "Reports" tab on the left
            IWebElement reportsTab = driver.FindElement(By.LinkText("Reports"));
            reportsTab.Click();

            //Choose the "Applications By College Choice Report" 
            IWebElement reportType = driver.FindElement(By.Name("code"));
            reportType.SendKeys("Applications By College Choice Report");

            //Choose the 2019 cohort year
            IWebElement yearField = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/div[2]/form[1]/div[1]/div[5]/div[1]/select[1]"));
            yearField.SendKeys("2019");

            //Click on "Generate" button
            IWebElement generateButton = driver.FindElement(By.Id("btnId"));
            generateButton.Click();

            //Take screenshot of result; the result should be a list of students
            Screenshot appCollegeChoiceReportResult = ((ITakesScreenshot)driver).GetScreenshot();
            appCollegeChoiceReportResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_785_AppCollegeChoiceReportResult.png", ScreenshotImageFormat.Png);

            //For verifying the exact needed columns when pulling the report
            HashSet<String> columns = new HashSet<String>();
            columns.Add("Regents Acccount Number");
            columns.Add("Last Name");
            columns.Add("First Name");
            columns.Add("Last 4 of SSN");
            columns.Add("DOB");
            columns.Add("Highschool");
            columns.Add("College Preference");
            columns.Add("EFC Amount");

            //Verifying actual column results when pulling the report
            try
            {
                IWebElement regentsAcctNum = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row:nth-child(3) div.col-lg-12 div.col-md-12:nth-child(4) div.panel.panel-success div.row div.col-lg-12 div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer thead:nth-child(1) tr:nth-child(1) > th.sorting:nth-child(2)"));
                string regentsAcctNumLabel = regentsAcctNum.Text;
                Assert.IsTrue(columns.Contains(regentsAcctNumLabel));

                IWebElement lastName = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row:nth-child(3) div.col-lg-12 div.col-md-12:nth-child(4) div.panel.panel-success div.row div.col-lg-12 div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer thead:nth-child(1) tr:nth-child(1) > th.sorting:nth-child(3)"));
                string lastNameLabel = lastName.Text;
                Assert.IsTrue(columns.Contains(lastNameLabel));

                IWebElement firstName = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row:nth-child(3) div.col-lg-12 div.col-md-12:nth-child(4) div.panel.panel-success div.row div.col-lg-12 div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer thead:nth-child(1) tr:nth-child(1) > th.sorting:nth-child(4)"));
                string firstNameLabel = firstName.Text;
                Assert.IsTrue(columns.Contains(firstNameLabel));

                IWebElement last4SSN = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row:nth-child(3) div.col-lg-12 div.col-md-12:nth-child(4) div.panel.panel-success div.row div.col-lg-12 div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer thead:nth-child(1) tr:nth-child(1) > th.sorting:nth-child(5)"));
                string last4SSNLabel = last4SSN.Text;
                Assert.IsTrue(columns.Contains(last4SSNLabel));

                IWebElement dob = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row:nth-child(3) div.col-lg-12 div.col-md-12:nth-child(4) div.panel.panel-success div.row div.col-lg-12 div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer thead:nth-child(1) tr:nth-child(1) > th.sorting:nth-child(6)"));
                string dobLabel = dob.Text;
                Assert.IsTrue(columns.Contains(dobLabel));

                IWebElement highSchool = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row:nth-child(3) div.col-lg-12 div.col-md-12:nth-child(4) div.panel.panel-success div.row div.col-lg-12 div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer thead:nth-child(1) tr:nth-child(1) > th.sorting:nth-child(7)"));
                string highSchoolLabel = highSchool.Text;
                Assert.IsTrue(columns.Contains(highSchoolLabel));

                IWebElement collegePref = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row:nth-child(3) div.col-lg-12 div.col-md-12:nth-child(4) div.panel.panel-success div.row div.col-lg-12 div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer thead:nth-child(1) tr:nth-child(1) > th.sorting:nth-child(8)"));
                string collegePrefLabel = collegePref.Text;
                Assert.IsTrue(columns.Contains(collegePrefLabel));

                IWebElement efcAmt = driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row:nth-child(3) div.col-lg-12 div.col-md-12:nth-child(4) div.panel.panel-success div.row div.col-lg-12 div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer thead:nth-child(1) tr:nth-child(1) > th.sorting:nth-child(9)"));
                string efcAmtLabel = efcAmt.Text;
                Assert.IsTrue(columns.Contains(efcAmtLabel));
            }
            catch (Exception)
            {
                Assert.Fail("Not all of the necessary columns are displayed on the report");
            }

            //Verifying if the excel spreadsheet was downloaded successfully when pressing the "Excel" button
            string pathToReport = "C:\\Users\\antho\\Downloads\\ApplicationsByCollegeChoiceReport.xls";

            if(File.Exists(pathToReport))
            {
                File.Delete(pathToReport);               
            }

            IWebElement excelButton = driver.FindElement(By.Id("excelButton"));
            excelButton.Click();

            //Take screenshot of successfully downloaded excel spreadsheet
            Screenshot excelFileDownloadResult = ((ITakesScreenshot)driver).GetScreenshot();
            excelFileDownloadResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_785_ExcelSpreadsheet.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for verifying whether the data entered in the RS Web Application matches 
         * the data in the admin portal
         */
        [TestMethod]
        public void SAMS_887()
        {
            driver.Navigate().GoToUrl(regentsWeb);

            string username = "tdrs060818b";
            string password = "Welcome01";

            IWebElement usernameField = driver.FindElement(By.Name("username"));
            usernameField.SendKeys(username);

            IWebElement passwordField = driver.FindElement(By.Name("password"));
            passwordField.SendKeys(password);

            IWebElement signInButton = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/form[1]/input[3]"));
            signInButton.Click();

            IWebElement completeApplicationButton = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/div[2]/div[1]/div[2]/div[2]/ul[1]/li[1]/div[2]/button[1]"));
            completeApplicationButton.Click();

            IWebElement educationInfoTab = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/div[1]/ul[1]/li[3]"));
            educationInfoTab.Click();

            IWebElement collegeDropDown = driver.FindElement(By.Name("collegeId"));

            Actions goToCollegeDropDown = new Actions(driver);
            goToCollegeDropDown.MoveToElement(collegeDropDown);
            goToCollegeDropDown.Click().Build().Perform();

            try
            {
                IWebElement deferOption = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[3]/section[1]/div[1]/div[1]/form[1]/div[13]/div[1]/select[1]/option[4]"));
                Assert.IsTrue(deferOption.Displayed);
            }
            catch (Exception)
            {
                Assert.Fail("Defer option should show for college/university drop down");
            }

            //Take screenshot of defer option showing up
            Screenshot deferOptionResult = ((ITakesScreenshot)driver).GetScreenshot();
            deferOptionResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_887_deferOptionShowing.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for verifying that the "Closed" review status appears on Admin Portal
         */
        [TestMethod]
        public void SAMS_908()
        {
            driver.Navigate().GoToUrl("http://10.4.1.99/regents/appReview?stateStudentId=RS19100172");

            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //WebDriverWait waitForAppInfoTab = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //IWebElement appInfoTab = waitForAppInfoTab.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.LinkText("App Info")));

            //string focusedElement = driver.SwitchTo().ActiveElement().Text;
            //Console.WriteLine(focusedElement);

            //Actions appInfoTabActions = new Actions(driver);
            //appInfoTabActions.MoveToElement(appInfoTab).Build().Perform();
            //appInfoTabActions.Click().Build().Perform();

            //focusedElement = driver.SwitchTo().ActiveElement().Text;

            //Console.WriteLine(focusedElement);

            IWebElement holderAllTabs = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/ul[1]"));

            var allTopTabs = new SelectElement(holderAllTabs);

            var allTabs = allTopTabs.Options;

            Console.WriteLine(allTabs.Count);

            //SelectElement allReviewStatuses = new SelectElement(reviewStatusDropList);

            //IList<IWebElement> reviewStatusOptions = allReviewStatuses.Options;

            //bool closedOptionExists = false;

            //foreach (IWebElement option in reviewStatusOptions)
            //{
            //    if (option.Text == "Closed")
            //    {
            //        closedOptionExists = true;
            //    }
            //}

            //Assert.IsTrue(closedOptionExists);
        }

        /**
         * Automation test for verifying the reviewer drop-down error has been fixed
         */
        [TestMethod]
        public void SAMS_915()
        {
            driver.FindElement(By.LinkText("Students")).Click();

            IWebElement acctNumberField = driver.FindElement(By.Id("accountNumber"));
            acctNumberField.SendKeys("RS19100177");
            acctNumberField.SendKeys(Keys.Enter);
            driver.FindElement(By.CssSelector("body.pace-done:nth-child(2) section.theme-default section.main-content-wrapper:nth-child(3) div.row.student-search div.col-md-12 div.panel.panel-success:nth-child(2) div.col-lg-12.background_main_bg div.bs-example div.dataTables_wrapper.no-footer table.display.dataTable.no-footer tbody:nth-child(2) tr.odd td.sorting_1:nth-child(1) > a.btn.btn-primary")).Click();

            WebDriverWait waitForAppInfo = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement appInfo = waitForAppInfo.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/ul[1]/li[3]/a[1]")));
            appInfo.Click();

            IWebElement reviewStatusLabel = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/form[1]/div[1]/div[1]/div[1]/h4[1]/span[1]"));

            if(reviewStatusLabel.Text == "Not Started")
            {
                Actions scrollDown = new Actions(driver);

                /*INITIAL HIGH SCHOOL TRANSCRIPT DATES*/
                IWebElement postMarkHSTranscript = driver.FindElement(By.Id("initialHighSchoolTranscriptDocument1.postmarkDate"));
                scrollDown.MoveToElement(postMarkHSTranscript);
                scrollDown.Perform();
                postMarkHSTranscript.Clear();
                postMarkHSTranscript.SendKeys("01/02/2019");
                IWebElement verifiedHSTranscript = driver.FindElement(By.Id("initialHighSchoolTranscriptDocument1.statusDate"));
                verifiedHSTranscript.Clear();
                verifiedHSTranscript.SendKeys("01/03/2019");
                IWebElement initialHSDeliveryMethod = driver.FindElement(By.Id("initialHighSchoolTranscriptDocument1.deliveryMethod"));
                initialHSDeliveryMethod.SendKeys("Mailed");

                /*INITIAL ACT SCORE DATES*/
                IWebElement actScoresPostmark = driver.FindElement(By.Id("actScoresDocument.postmarkDate"));
                scrollDown.MoveToElement(actScoresPostmark);
                scrollDown.Perform();
                actScoresPostmark.Clear();
                actScoresPostmark.SendKeys("01/04/2019");
                IWebElement actScoresVerified = driver.FindElement(By.Id("actScoresDocument.statusDate"));
                actScoresVerified.Clear();
                actScoresVerified.SendKeys("01/05/2019");
                IWebElement actScoresDeliveryMethod = driver.FindElement(By.Id("actScoresDocument.deliveryMethod"));
                actScoresPostmark.SendKeys("eData");

                //Click on the Save Review button
                IWebElement saveReviewButton = driver.FindElement(By.Id("saveReview"));
                saveReviewButton.Click();
            }
            
            //If necessary, click the Complete Review button to move to Second Review
            if(reviewStatusLabel.Text != "Second Transcript")
            {
                //Click on the Complete Review button
                IWebElement completeReviewButton = driver.FindElement(By.Id("completeReview"));
                completeReviewButton.Click();
            }

            IWebElement reviewerLabel = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[1]/div[1]/div[1]/div[1]/div[3]/div[1]/form[1]/div[1]/div[1]/div[1]/h4[1]/span[2]"));

            Assert.AreEqual("lsalgado", reviewerLabel.Text);

            //Take screenshot of the correct reviewer label
            Screenshot reviewerLabelResult = ((ITakesScreenshot)driver).GetScreenshot();
            reviewerLabelResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_915_ReviewerLabel.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for verifying the correct columns for the NC Annual Report columns
         */
        [TestMethod]
        public void SAMS_923()
        {
            IWebElement ncReportsTab = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/aside[1]/nav[1]/ul[1]/li[3]/ul[1]/li[7]/a[1]"));
            ncReportsTab.Click();

            IWebElement reportSelector = driver.FindElement(By.Id("code"));
            reportSelector.SendKeys("Annual Report");

            IWebElement yearSelector = driver.FindElement(By.Id("year"));
            yearSelector.SendKeys("2019");

            IWebElement generateButton = driver.FindElement(By.Id("btnId"));
            generateButton.Click();

            ArrayList columns = new ArrayList();
            columns.Add("District");
            columns.Add("High School");
            columns.Add("Total # of Applicants");
            columns.Add("Recipients");
            columns.Add("Non-Eligible Applicants");
            columns.Add("Pending");

            ArrayList actualColumns = new ArrayList();

            try
            {
                IWebElement firstColumn = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[3]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/table[1]/thead[1]/tr[1]/th[2]"));
                actualColumns.Add(firstColumn.Text);

                IWebElement secondColumn = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[3]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/table[1]/thead[1]/tr[1]/th[3]"));
                actualColumns.Add(secondColumn.Text);

                IWebElement thirdColumn = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[3]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/table[1]/thead[1]/tr[1]/th[4]"));
                actualColumns.Add(thirdColumn.Text);

                IWebElement fourthColumn = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[3]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/table[1]/thead[1]/tr[1]/th[5]"));
                actualColumns.Add(fourthColumn.Text);

                IWebElement fifthColumn = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[3]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/table[1]/thead[1]/tr[1]/th[6]"));
                actualColumns.Add(fifthColumn.Text);

                IWebElement sixColumn = driver.FindElement(By.XPath("/html[1]/body[1]/section[1]/section[1]/div[2]/div[3]/div[1]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/table[1]/thead[1]/tr[1]/th[7]"));
                actualColumns.Add(sixColumn.Text);
            }
            catch(Exception)
            {
                Assert.Fail("NC Annual Report doesn't have exact correct columns");
            }

            for(int index = 0; index < 6; index++)
            {
                Assert.IsTrue(actualColumns[index].Equals(columns[index]));
            }

            //Take screenshot of NC Annual Report and its correct columns
            Screenshot ncAnnualReportColumns = ((ITakesScreenshot)driver).GetScreenshot();
            ncAnnualReportColumns.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_923_NCAnnualReportCorrectColumns.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for verifying the Complete Review button remains disabled during the 
         * "Not Started" review status and "Application Downloaded" award status
         */
        [TestMethod]
        public void SAMS_943()
        {
            //Click on the RS "Students" tab
            IWebElement studentsTab = driver.FindElement(By.LinkText("Students"));
            studentsTab.Click();

            //Enter the first name of the RS account we are looking for
            IWebElement firstNameField = driver.FindElement(By.Id("firstName"));
            firstNameField.SendKeys("tdrs071218a");
            firstNameField.SendKeys(Keys.Enter);

            //Click on account number to access their page
            IWebElement acctNumberButton = driver.FindElement(By.LinkText("RS19100177"));
            acctNumberButton.Click();

            //Wait until the page finishes loading and shows the App Info page
            By appInfoLinkText = By.LinkText("App Info");
            IWebElement appInfoTab = driver.FindElement(appInfoLinkText);

            Actions scrollToAppInfoTab = new Actions(driver);
            new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(appInfoLinkText));
            scrollToAppInfoTab.MoveToElement(appInfoTab);

            Actions clickingOnAppInfoTab = new Actions(driver);
            clickingOnAppInfoTab.Click(appInfoTab).Build().Perform();

            //IWebElement reviewStatusSelector = driver.FindElement(reviewStatusName);
            //waitForWebElements.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(reviewStatusName));          
            //reviewStatusSelector.SendKeys("Not Started");

            //IWebElement reassignButton = driver.FindElement(By.Id("reassign"));
            //reassignButton.Click();

            //IWebElement awardStatusSelector = driver.FindElement(By.Name("scholarshipApplication.awardStatus.code"));
            //WebDriverWait waitForAwardStatus = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            //waitForAwardStatus.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(awardStatusSelector));
            //awardStatusSelector.SendKeys("Application Downloaded");

            //IWebElement saveButton = driver.FindElement(By.Id("saveReview"));
            //saveButton.Click();

            //IWebElement completeReviewButton = driver.FindElement(By.Id("completeReview"));
            //Assert.IsFalse(completeReviewButton.Enabled);

            ////Take a screenshot of the disabled complete review button
            //Screenshot disabledCompleteReviewButton = ((ITakesScreenshot)driver).GetScreenshot();
            //disabledCompleteReviewButton.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_943_DisabledCompleteReviewButton.png", ScreenshotImageFormat.Png);
        }

        /**
         * Automation test for verifying the ACT score in a student to be without decimals
         */
        [TestMethod]
        public void SAMS_945()
        {
            IWebElement rsStudentTab = driver.FindElement(By.LinkText("Students"));
            rsStudentTab.Click();

            IWebElement firstNameField = driver.FindElement(By.Id("firstName"));
            firstNameField.SendKeys("tdrs071618b");
            firstNameField.SendKeys(Keys.Enter);

            IWebElement acctNumButton = driver.FindElement(By.LinkText("RS19100184"));
            acctNumButton.Click();

            Thread.Sleep(1000);

            IWebElement appInfoTab = driver.FindElement(By.LinkText("App Info"));
            appInfoTab.Click();

            IWebElement compActField = driver.FindElement(By.Name("actComposite.score"));         
            Actions scrollToActField = new Actions(driver);
            scrollToActField.MoveToElement(compActField);
            scrollToActField.Build().Perform();
            string actComposite = compActField.Text;

            Assert.IsTrue(actComposite != "22.0");

            //Take screenshot of ACT score result
            Screenshot actScoreResult = ((ITakesScreenshot)driver).GetScreenshot();
            actScoreResult.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\SAMS_945_nonDecimalACTScoreFormat.png", ScreenshotImageFormat.Png);
        }

        /**
         * Closes each test case session
         */
        [TestCleanup]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}
