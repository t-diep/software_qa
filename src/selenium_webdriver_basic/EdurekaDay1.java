package selenium_webdriver_basic;

import java.util.concurrent.TimeUnit;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

/**
 * 
 * @author antho
 *
 */
public class EdurekaDay1 
{	
	static WebDriver webDriver;
	static JavascriptExecutor jExe;
	
	public static void main(String[] args) 
	{
		invokeChromeBrowser();
	}
	
	/*
	 * Opens an instance of a chrome browser for automation
	 */
	public static void invokeChromeBrowser()
	{
		try {
			System.setProperty("webdriver.chrome.driver", "C:\\Users\\antho\\Downloads\\Selenium\\chromedriver_win32\\chromedriver.exe");
			
			//Load chrome browser
			webDriver = new ChromeDriver();
			
			//Browser should delete cookies before loading
			webDriver.manage().deleteAllCookies();
			
			//Maximize window
			webDriver.manage().window().maximize();

			//Page synchronization; sync code and web browser together
			webDriver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
			
			//Manage page loading with code
			webDriver.manage().timeouts().pageLoadTimeout(30, TimeUnit.SECONDS);

			//Fetch URL for this browser
			webDriver.get("https://www.edureka.co");
			
			searchCourse();
		} 
		catch (Exception e) 
		{
			e.printStackTrace();
		}
	}
	
	/**
	 * Searches a course in the Edureka website
	 */
	private static void searchCourse()
	{	
		try 
		{
			//Enter input
			webDriver.findElement(By.id("homeSearchBar")).sendKeys("Java");
			
			Thread.sleep(3000);
			
			//Clicking a button
			webDriver.findElement(By.id("homeSearchBarIcon")).click();
			
			//Scroll down command
			jExe = (JavascriptExecutor) webDriver; 
			jExe.executeScript("scroll(0, 1000)");
		} 
		catch (Exception e) 
		{
			e.printStackTrace();
		}
	}
}
