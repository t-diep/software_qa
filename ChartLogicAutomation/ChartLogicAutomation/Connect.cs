using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ChartLogicAutomation
{
	[TestClass]
	public class Connect
	{
		IWebDriver driver;
		internal const string angela = "angela";
		internal const string angelaPassword = "!!0u812";
		internal const string altPassword = "Chartlogic1";

		/// <summary>
		/// Open browser per each test case
		/// </summary>
		[TestInitialize]
		public void Navigate()
		{
			driver = new ChromeDriver();
			driver.Manage().Cookies.DeleteAllCookies();
			driver.Manage().Window.Maximize();
			driver.Navigate().GoToUrl("https://accounts.chartlogic.com/login");
		}

		/// <summary>
		/// Verifies if we can navigate to the Login container
		/// </summary>
		[TestMethod]
		public void CanNavigateToTheLoginContainer()
		{
			try
			{
				IWebElement loginContainer = driver.FindElement(By.CssSelector("body > app-root > main > login"));
				Assert.IsTrue(loginContainer != null);
			}
			catch (Exception)
			{
				Assert.Fail("Could not navigate to the Login container");
			}
		}

		/// <summary>
		/// Verifies if we can log in as a provider
		/// </summary>
		[TestMethod]
		public void CanLoginAsAProvider()
		{
			driver.FindElement(By.Id("username")).SendKeys(angela);
			driver.FindElement(By.Id("password")).SendKeys(angelaPassword);

			Assert.IsTrue(driver.Url == "https://connect.chartlogic.com/dashboard");
		}

		/// <summary>
		/// Close browser per each test case
		/// </summary>
		[TestCleanup]
		public void Close()
		{
			driver.Close();
		}
	}
}
