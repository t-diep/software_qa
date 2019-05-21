using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PatientPortal
{
	[TestClass]
	public class PatientPortal
	{
		IWebDriver driver;

		/// <summary>
		/// Open browser per each test case
		/// </summary>
		[TestInitialize]
		public void Navigate()
		{
			driver = new ChromeDriver();
			driver.Navigate().GoToUrl("https://accounts.chartlogic.com/login");

		}

		/// <summary>
		/// Verifies if we can navigate to the Login container
		/// </summary>
		[TestMethod]
		public void CanNavigateToTheLoginContainer()
		{
			
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
