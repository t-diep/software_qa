package selenium_webdriver_easy_websites;

import java.util.concurrent.TimeUnit;

import javax.swing.JOptionPane;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

/**
 * Performs automation tests for a sample web application containing a sample
 * application form
 * 
 * @author Tony Diep, last updated 5-25-18 
 */
public class SimpleAutomationForm 
{
	//Used to create a specific browser for a website (i.e. Chrome)
	static WebDriver driver;
	//Used to perform javascript commands such as scrolling 
	static JavascriptExecutor jexe;
	
	public static void main(String[] args) 
	{
		configureBrowser();
		loadWebPage();
		fillOutApplicationForm();
	}
	
	/**
	 * Configures and invokes the browser 
	 */
	public static void configureBrowser()
	{
		//Tell system which web driver to use
		System.setProperty("webdriver.chrome.driver", "C:\\Users\\antho\\Downloads\\Selenium\\chromedriver_win32\\chromedriver.exe");
		driver = new ChromeDriver();	
	}
	
	/**
	 * Visits the sample web page containing the sample web application form
	 */
	public static void loadWebPage()
	{
		try {
			//Make window as big as possible for visibility sake
			driver.manage().window().maximize();
			
			//Delete all cookies first
			driver.manage().deleteAllCookies();
			
			//Wait until the browser can load all web elements
			driver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
			
			//Give some time for the browser to load all elements before timing out
			driver.manage().timeouts().pageLoadTimeout(40, TimeUnit.SECONDS);
			
			//Go to a specific URL to perform test scripts on
			driver.get("http://toolsqa.com/automation-practice-form/");
			
			//Set up JavaScript executor for scrolling 
			jexe = (JavascriptExecutor) driver;
		} 
		catch (Exception e) 
		{
			e.printStackTrace();
		}
	}
	
	/**
	 * Performs a test script to filling out the sample application form using Selenium Webdriver
	 */
	public static void fillOutApplicationForm()
	{
		//Scroll down to the form
		jexe.executeScript("scroll(0, 600)");
		
		//Locate the "Partial Link Test" element, then click on it
		driver.findElement(By.xpath("//*[@id='content']/div[1]/div/div/div/div[2]/div/form/fieldset/div[2]/a/strong")).click();
		driver.navigate().back();
		
		//Locate the "Link Test" element, then click on it
		driver.findElement(By.xpath("//*[@id='content']/div[1]/div/div/div/div[2]/div/form/fieldset/div[5]/a/strong")).click();
		driver.navigate().back();
		
		//Locate the "First Name" field, then enter in first name
		driver.findElement(By.name("firstname")).sendKeys("Tony");
		
		//Locate the "Last Name" field, then enter in last name
		driver.findElement(By.name("lastname")).sendKeys("Diep");
		
		//Choose the "Male" radio button option
		driver.findElement(By.id("sex-0")).click();
		
		//Choose the maximum amount of experience (i.e. "7")
		driver.findElement(By.id("exp-6")).click();
		
		//Enter in today's date
		driver.findElement(By.id("datepicker")).sendKeys("5/24/2018");
		
		//Select both the "Manual" and "Automation" tester options
		driver.findElement(By.id("profession-0")).click();
		driver.findElement(By.id("profession-1")).click();
		
		//Scroll down some more to see the other elements within the application form
		jexe.executeScript("scroll(600, 1250)");
		
		//Upload a photo from local drive
		driver.findElement(By.id("photo")).sendKeys("C:\\Users\\antho\\OneDrive\\Pictures\\tonys\\tony_icon.JPG");
		
		//Click on download links
		driver.findElement(By.linkText("Selenium Automation Hybrid Framework")).click();
		driver.findElement(By.linkText("Test File to Download")).click();
		
		//Select the "Selenium WebDriver" checkbox
		driver.findElement(By.id("tool-2")).click();
		
		//Select a specific continent 
		driver.findElement(By.xpath("//*[@id='continents']/option[6]")).click();
		
		//Select a specific command from the commands scroll bar
		driver.findElement(By.xpath("//*[@id='selenium_commands']/option[5]")).click();
		
		//Click on the submit button
		driver.findElement(By.id("submit")).click();
		
		//Confirm that the test using Selenium WebDriver is complete and that everything is working
		JOptionPane.showMessageDialog(null, "Verified Application Form.  All aspects working correctly");
	}
}
