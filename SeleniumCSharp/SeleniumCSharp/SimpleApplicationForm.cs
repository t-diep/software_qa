using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCSharp
{
    class SimpleApplicationForm
    {
        static IWebDriver driver;
        static IJavaScriptExecutor jexe;

        static void Main(string[] args)
        {
            LoadWebPage();
            FillOutApplicationForm();
        }

        /**
         * Loads web page by going to the desired url
         */
        private static void LoadWebPage()
        {
            driver = new ChromeDriver();
            driver.Url = "http://www.toolsqa.com/automation-practice-form/";

            jexe = (IJavaScriptExecutor)driver;
            jexe.ExecuteScript("scroll(0, 600)");

            if (driver.Title != "TOOLS QA | Free QA Automation Tools Tutorials")
            {
                Console.WriteLine("Problem with title of webpage...");
            }
        }

        /**
         * Performs the test script to fill out the sample application form 
         */
        private static void FillOutApplicationForm()
        {
            jexe.ExecuteScript("scroll(0, 600)");

            driver.FindElement(By.XPath("//*[@id='content']/div[1]/div/div/div/div[2]/div/form/fieldset/div[2]/a/strong")).Click();
            driver.Navigate().Back();

            driver.FindElement(By.XPath("//*[@id='content']/div[1]/div/div/div/div[2]/div/form/fieldset/div[5]/a/strong")).Click();
            driver.Navigate().Back();

            driver.FindElement(By.Name("firstname")).SendKeys("Tony");
            driver.FindElement(By.Name("lastname")).SendKeys("Diep");

            driver.FindElement(By.Id("sex-0")).Click();
            driver.FindElement(By.Id("exp-0")).Click();

            driver.FindElement(By.Id("datepicker")).SendKeys("5/25/2018");
            driver.FindElement(By.Id("profession-0")).Click();
            driver.FindElement(By.Id("profession-1")).Click();

            jexe.ExecuteScript("scroll(600, 1250)");

            driver.FindElement(By.Id("photo")).SendKeys("C:\\Users\\antho\\OneDrive\\Pictures\\tonys\\tony_icon.JPG");

            driver.FindElement(By.XPath("//*[@id='content']/div[1]/div/div/div/div[2]/div/form/fieldset/div[25]/a")).Click();
            driver.FindElement(By.XPath("//*[@id='content']/div[1]/div/div/div/div[2]/div/form/fieldset/div[26]/a")).Click();

            driver.FindElement(By.Id("tool-2")).Click();
            driver.FindElement(By.XPath("//*[@id='continents']/option[7]")).Click();
            driver.FindElement(By.XPath("//*[@id='selenium_commands']/option[2]")).Click();

            driver.FindElement(By.Id("submit")).Click();
        }
    }
}
