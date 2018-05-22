package selenium_webdriver_basic;

import java.util.concurrent.TimeUnit;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

/**
 * Contains the framework of demoing the web driver navigation commands
 * 
 * This code is inspired by the Edureka tutorial on Selenium Webdriver
 * 
 * @author Tony Diep, last updated 5-21-18
 *
 */
public class WebDriverNavigateCommands 
{
	static WebDriver driver;
	
	public static void main(String[] args) 
	{
		invokeBrowser();
	}

	/**
	 * 
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
			
			driver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
			
			driver.manage().timeouts().pageLoadTimeout(30, TimeUnit.SECONDS);
			
			executeNavigationCommands();
		} 
		catch (Exception e) 
		{
			e.printStackTrace();
		}
	}
	
	/**
	 * 
	 */
	public static void executeNavigationCommands()
	{
		try 
		{
			//Get the URL 
			driver.navigate().to("https://www.serebii.net/index2.shtml");
			//driver.findElement(By.xpath("//span[starts-with(text(), 'TVs & Appliances')]")).click();
			//driver.findElement(By.xpath("//span[contains(text(), 'Samsung')]")).click();
			
			Thread.sleep(2000);
			
			driver.navigate().back();
			
			Thread.sleep(2000);
			
			driver.navigate().forward();
			
			Thread.sleep(2000);
			
			driver.navigate().refresh();
		
			Thread.sleep(2000);
		}
		catch (Exception e) 
		{	
			e.printStackTrace();
		}	
	}
}
