package selenium_webdriver_basic;

import java.util.concurrent.TimeUnit;

import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

public class WebDriverBrowserCommands 
{
	//Used to run the Selenium WebDriver
	static WebDriver driver;
	
	public static void main(String[] args) 
	{
		invokeBrowser();
	}

	/**
	 * Sets up and runs the google chrome browser
	 */
	public static void invokeBrowser()
	{	
		try {
			//Tell system which web driver to use
			System.setProperty("webdriver.chrome.driver", "C:\\Users\\antho\\Downloads\\Selenium\\chromedriver_win32\\chromedriver.exe");
			
			//Create instance of chrome driver
			driver = new ChromeDriver();
			
			//Make window as big as possible for visibility sake
			driver.manage().window().maximize();
			
			//Delete all cookies first
			driver.manage().deleteAllCookies();
			
			//Configure the time for the driver to search for elements if not immediately found
			driver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
			
			//Configure the amount of time for the page to load before timing out
			driver.manage().timeouts().pageLoadTimeout(30, TimeUnit.SECONDS);
			
			executeCloseBrowser();
		} 
		catch (Exception e) 
		{
			e.printStackTrace();
		}
	}
	
	/**
	 * Simply closes the browser running on the web driver
	 */
	public static void executeCloseBrowser()
	{
		try 
		{
			//For one instance of a browser
			driver.close();
			
			//For multiple instances of browsers
			//driver.quit();
		}
		catch (Exception e) 
		{	
			e.printStackTrace();
		}	
	}
}
