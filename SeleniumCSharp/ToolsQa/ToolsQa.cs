using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Drawing;
using OpenQA.Selenium.Interactions;

namespace ToolsQa
{
	[TestClass]
	public class ToolsQa
	{
		IWebDriver driver;
		IJavaScriptExecutor jexe;

		public void JumpToElement(By by)
		{
			//string className = "#content > div.vc_row.wpb_row.vc_row-fluid.dt-default > div > div > div > div.wpb_text_column.wpb_content_element > div > form > fieldset > div:nth-child(23) > strong > label";
			//var element = driver.FindElement(By.CssSelector(className));
			var element = driver.FindElement(by);
			Actions actions = new Actions(driver);
			actions.MoveToElement(element);
			actions.Perform();
		}

		public void TestPartialLink(By by)
		{
			//Verify the link labeled "Partial Link" is working
			var partialLink = driver.FindElement(by);
			partialLink.Click();
			Assert.IsTrue(driver.Url == "https://www.toolsqa.com/automation-practice-form/");

			//Take screenshot
			Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
			screenshot.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\ToolsQA_PartialLinkTest");

			driver.Navigate().Back();
		}

		public void TestLink(By by, String url)
		{
			var link = driver.FindElement(by);
			link.Click();
			Assert.IsTrue(driver.Url == "https://www.toolsqa.com/automation-practice-table/");
			Assert.IsTrue(driver.Title == url);
			
			//Take screenshot
			Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
			screenshot.SaveAsFile("C:\\Users\\antho\\OneDrive\\Pictures\\Screenshots\\ToolsQA_LinkTest");

			//Go back to the original page
			driver.Navigate().Back();
		}

		/// <summary>
		/// Test the whole form
		/// </summary>
		[TestMethod]
		public void TestToolsQA()
		{
			driver = new ChromeDriver();

			jexe = (IJavaScriptExecutor)driver;

			driver.Manage().Cookies.DeleteAllCookies();
			driver.Manage().Window.Maximize();
			driver.Navigate().GoToUrl("https://www.toolsqa.com/automation-practice-form/");
			driver.Navigate().Refresh();

			string className = "#content > div.vc_row.wpb_row.vc_row-fluid.dt-default > div > div > div > div.wpb_text_column.wpb_content_element > div > form > fieldset > div:nth-child(23) > strong > label";
			JumpToElement(By.ClassName(className));

			TestPartialLink(By.PartialLinkText("Partial Link Test"));
			TestLink(By.LinkText("Link Test"), "Demo Table for practicing Selenium Automation");

			var firstName = driver.FindElement(By.Name("firstname"));
			firstName.SendKeys("TestClient1");

			var lastName = driver.FindElement(By.Name("lastname"));
			lastName.SendKeys("TestClient1");

			var gender = driver.FindElement(By.Id("sex-0"));
			gender.Click();

			JumpToElement(By.Id("exp-0"));

			Random rand = new Random();
			List<IWebElement> radioButtons = 

			//driver.Quit();
		}
	}
}
