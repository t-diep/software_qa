using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace CourseWarehouse
{
    [TestClass]
    public class CourseWarehouse
    {
        IWebDriver driver;
        IJavaScriptExecutor jexe;
        const string adminPortalDev = "http://10.4.1.99";
        const string courseWarehouseDev = "https://devcourses.regentsscholarship.org";
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        /**
         * Opens a browser for every automation test case that runs
         */
        [TestInitialize]
        public void ConfigureRunBrowser()
        {
            driver = new ChromeDriver();
            jexe = (IJavaScriptExecutor)driver;

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(100);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(100);
            driver.Navigate().GoToUrl(courseWarehouseDev);
        }

        /**
         * Automation test for verifying a requested school from a representative
         */
        [TestMethod]
        public void CW_236()
        {
            //Clicking on the "Representative click here" button
            driver.FindElement(By.CssSelector("div.navbar.navbar-fixed-top:nth-child(1) header.header.navbar-fixed-top div.container.head-left-right nav.main-nav.navbar-right div.navbar-collapse.collapse ul.nav.navbar-nav ul.nav.navbar-nav.navbar-right:nth-child(3) div.col-md-12 div.row li.dropdown.nav-item.nav-item-cta.last > a.dropdown-toggle.btn.btn-primary-outline.out_line")).Click();

            //Log onto CWH with username and password
            driver.FindElement(By.Name("username")).SendKeys("niki@hkconsulting.biz");
            driver.FindElement(By.Name("password")).SendKeys("Welcome01");

            //Click on Login button
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/header[1]/div[1]/nav[1]/div[2]/ul[1]/ul[1]/div[1]/div[1]/li[1]/ul[1]/form[1]/div[1]/div[1]/div[1]/div[1]/button[1]")).Click();

            //Click on "Add School" button
            driver.FindElement(By.CssSelector("div.sections-wrapper div.screen-bg-sn.top_pos.light_background:nth-child(2) section.bg-access-section div.container div.row div.form-box.col-sm-offset-0.xs-offset-0.col-xs-12.col-md-12 div.form-box-inner.col-sm-12.col-md-12.col-lg-12 div.screen-bg-sn.top_pos.light_background section.bg-access-section div.row div.form-box.row:nth-child(1) div.col-md-8.col-sm-12.col-xs-12 div.prorepuser-container.repuser-pink div.prorepuser-content div.prorepuser-heading h5:nth-child(1) > button.but-ajst.pull-right.add-btn.btn-primary.button_style.btn-round.addSchlBtnClass:nth-child(2)")).Click();

            //Fill out the pop-up form
            driver.FindElement(By.Id("cwhSchoolLookup.name")).SendKeys("West Jordan High School");
            driver.FindElement(By.Id("cwhSchoolLookup.address1")).SendKeys("8136 2700 W");
            driver.FindElement(By.Id("cwhSchoolLookup.stateCode")).SendKeys("Utah");
            driver.FindElement(By.Id("cwhSchoolLookup.city")).Clear();

            IWebElement schoolCity = driver.FindElement(By.Id("cwhSchoolLookup.city"));
            schoolCity.Clear();
            schoolCity.SendKeys("West Jordan");
            schoolCity.SendKeys(Keys.Enter);

            driver.FindElement(By.Id("cwhSchoolLookup.code")).SendKeys("84088");
            driver.FindElement(By.Id("cwhSchoolLookup.cwhDistrict")).SendKeys("Jordan");
            driver.FindElement(By.Id("cwhSchoolLookup.ceebCode")).SendKeys("450446");
        }

        /**
         * Closes browser for each automation test case session that runs
         */
        [TestCleanup]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}
