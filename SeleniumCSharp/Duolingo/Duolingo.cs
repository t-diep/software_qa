using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

/**
 * Contains all of the test scripts for automation testing using 
 * Selenium Webdriver for the Duolingo web application
 */
namespace Duolingo
{
    /**
     * Duolingo web application test automation suite
     */
    [TestClass]
    public class Duolingo
    {
        //Driver used to launch a browser
        IWebDriver driver;
        //Used for executing javascript commands
        IJavaScriptExecutor jexe;

        /*METHODS USED FOR CONVIENIENCE*/

        /**
         * Helper for configuring browser settings and loads the browser
         */
        private void LoadWebPage()
        {
            driver = new ChromeDriver();
            jexe = (IJavaScriptExecutor)driver;

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(70);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(70);

            driver.Navigate().GoToUrl("https://www.duolingo.com/");            
        }

        /**
         * Helper for clicking on the Get Started button on Duolingo's website
         */
        private void GetStarted()
        {
            IWebElement getStartedButton = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[2]/a[1]"));
            getStartedButton.Click();
        }

        /**
         * Helper for changing the site language to German 
         */
        private void ChangeSiteLanguage()
        {
            IWebElement dropDown = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/span[2]"));
            dropDown.Click();
            IWebElement deutschOption = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ul[1]/li[3]"));
            deutschOption.Click();
        }

        /**
         * Helper for selecting a desired course on Duolingo
         */
        private void SelectCourse()
        {
            IWebElement germanButton = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[1]/ul[1]/li[3]/a[1]"));
            germanButton.Click();
        }

        /**
         * Helper for starting a chosen course on Duolingo
         */
        private void StartLearningCourse()
        {
            IWebElement startLearning = driver.FindElement(By.XPath("/html[1]/body[1]/div[3]/div[1]/div[1]/div[1]/section[1]/div[2]/div[1]/div[1]/div[1]/button[1]"));
            startLearning.Click();
        }

        /**
         * Helper for setting the daily goal option to "casual"
         */
        private void SetDailyGoal()
        {
            IWebElement casualGoalOption = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/ul[1]/li[1]/label[1]"));
            casualGoalOption.Click();

            IWebElement setDailyGoalButton = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/button[1]"));
            setDailyGoalButton.Click();

            IWebElement notNowOption = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/ul[1]/li[3]/button[1]"));
            notNowOption.Click();

            IWebElement beginnerOption = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/button[1]/div[1]"));
            beginnerOption.Click();
        }

        /**
         * 
         */
        private void CompleteBasicsCourse()
        {
            bool lessonComplete = false;
            int pageNum = 1;

            while(!lessonComplete)
            {
                IWebElement commandHeader = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/h1[1]/span[1]"));

                Console.WriteLine(commandHeader.Text);

                //Retrieve the header title for a particular word to select correct option for
                int index = commandHeader.Text.IndexOf("r ");
                string targetWord = commandHeader.Text.Substring(index + 2);
                targetWord = targetWord.Replace("“", "");
                targetWord = targetWord.Replace("”", "");

                if (commandHeader.Text.Contains("Select"))
                {
                    HandleSelectCorrectWordPage(targetWord);
                    pageNum++;
                }
                else if (pageNum > 2)
                {
                    HandleWriteCorrectTranslationPage(targetWord);
                }
                else
                {
                    lessonComplete = true;

                    IWebElement lessonCompleteText = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/h1[1]"));

                    //Finish the basics course lesson by hitting "Continue" one final time
                    IWebElement next = driver.FindElement(By.CssSelector("div.BWibf._3MLiB div._3PBCS div._3giip div._3GXmV._1sntG div._1cw2r > button._3XJPq._2PaNr.ZrFol._3j92s._27uC9._2R_Yv.JnmAc"));
                    next.Click();  
                }
            }        
        }

        /**
         * Handles pages that require the user to choose the correct option
         */
        private void HandleSelectCorrectWordPage(string wordToTranslate)
        {
            Dictionary<String, String> library = ConstructLibrary();

            string xPathFirstOption = "/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/ul[1]/li[1]/label[1]/span[2]";
            string xPathSecondOption = "/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/ul[1]/li[2]/label[1]/span[2]";
            string xPathThirdOption = "/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/ul[1]/li[3]/label[1]/span[2]";

            //Get all three option web elements
            IWebElement firstOption = driver.FindElement(By.XPath(xPathFirstOption));
            IWebElement secondOption = driver.FindElement(By.XPath(xPathSecondOption));
            IWebElement thirdOption = driver.FindElement(By.XPath(xPathThirdOption));

            //Retrieve the foreign word for each option
            string firstOptionText = firstOption.Text;
            string secondOptionText = secondOption.Text;
            string thirdOptionText = thirdOption.Text;

            if (library.ContainsKey(firstOptionText) && library[firstOptionText] == wordToTranslate)
            {
                firstOption.Click();
            }
            else if (library.ContainsKey(secondOptionText) && library[secondOptionText] == wordToTranslate)
            {
                secondOption.Click();
            }
            else if (library.ContainsKey(thirdOptionText) && library[thirdOptionText] == wordToTranslate)
            {
                thirdOption.Click();
            }

            //Enter key
            IWebElement nextButton = driver.FindElement(By.CssSelector("div.BWibf._3MLiB div._3PBCS div._3giip div._3GXmV._1sntG div._1cw2r > button._3XJPq._2PaNr.ZrFol._3j92s._27uC9._2R_Yv.JnmAc"));
            nextButton.Click();
            nextButton.Click();

            Thread.Sleep(2000);
        }

        private void HandleWriteCorrectTranslationPage(string phrase)
        {

        }

        private Dictionary<String, String> ConstructLibrary()
        {
            Dictionary<String, String> library = new Dictionary<string, string>();

            var lines = File.ReadLines("C:\\Users\\antho\\Source\\Repos\\software_qa\\SeleniumCSharp\\Duolingo\\GermanLibrary.txt");

            foreach(var line in lines)
            {
                int index = line.IndexOf("|");

                string foreign = line.Substring(0, index);
                string english = line.Substring(index + 1);

                library.Add(foreign, english);
            }

            return library;
        }

        /*END OF METHOD USED FOR CONVIENIENCE*/



        /* AUTOMATION TESTS  */

        /**
         * Test for successful browser load and configurations
         */
        [TestMethod]
        public void AutomateTestLoadWebPage()
        {
            LoadWebPage();

            string expected = "https://www.duolingo.com/";
            string actual = driver.Url;

            Assert.AreEqual(expected, actual);

            Thread.Sleep(1000);
        }

        /**
         * Tests whether the user is able to register on the Duolingo website
         */
        [TestMethod]
        public void AutomateTestGetStarted()
        {
            LoadWebPage();
            GetStarted();

            string expectedURL= "https://www.duolingo.com/register";
            string actualURL = driver.Url;

            Assert.AreEqual(expectedURL, actualURL);

            Thread.Sleep(1300);
        }

        /**
         * Tests to see whether the system successfully changed the language to German
         */
         [TestMethod]
         public void AutomateTestChangeSiteLanguage()
         {
            LoadWebPage();
            ChangeSiteLanguage();

            string germanURL = "https://de.duolingo.com/";
            string actual = driver.Url;

            Assert.AreEqual(germanURL, actual);
            Thread.Sleep(1500);
         }

        /**
         * Tests for whether the German course
         */ 
        [TestMethod]
        public void AutomateTestSelectCourse()
        {
           LoadWebPage();
            SelectCourse();

           string expected = "https://www.duolingo.com/course/de/en/Learn-German-Online";
           string actual = driver.Url;

           Assert.AreEqual(expected, actual);
           Thread.Sleep(1000);
        }

        /**
         * Tests for selecting and then starting a particular language course on Duolingo
         */
        [TestMethod]
        public void AutomateTestStartLearningCourse()
        {
            LoadWebPage();
            SelectCourse();
            StartLearningCourse();      

            string expected = "https://www.duolingo.com/course/de/en/Learn-German-Online";
            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
            Thread.Sleep(1000);
        }

        /**
         * Tests for configuring the set daily goals before starting the first lesson 
         */
         [TestMethod]
         public void AutomateTestSetDailyGoal()
         {
            LoadWebPage();
            SelectCourse();
            StartLearningCourse();
            SetDailyGoal();

            string expected = "https://www.duolingo.com/skill/de/Basics-1/1";
            string actual = driver.Url;

            Assert.AreEqual(expected, actual);
            Thread.Sleep(1000);
         }

        /**
         * Tests whether the basics course is completed successfully
         */
         [TestMethod]
         public void AutomateTestCompleteBasicsCourse()
         {
            LoadWebPage();
            SelectCourse();
            StartLearningCourse();
            SetDailyGoal();
            CompleteBasicsCourse();
         }

        /**
         * Used to run one instance of the web driver per test case
         */
        [TestCleanup]
        public void Close()
        {
            driver.Close();
        }
    }
}
