package selenium_webdriver_basic;

import java.util.concurrent.TimeUnit;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

public class WebDriverGetCommands 
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
			
			executeGetCommands();
		} 
		catch (Exception e) 
		{
			e.printStackTrace();
		}
	}
	
	/**
	 * 
	 */
	public static void executeGetCommands()
	{
		try 
		{
			//Get the URL 
			driver.get("https://www.amazon.in");
			
			//Get title of current webpage
			String title = driver.getTitle();
			System.out.println("Current title page is " + title);
			
			driver.findElement(By.linkText("Today's Deals")).click();
			
			String currentURL = driver.getCurrentUrl();
			
			System.out.println("Current URL is " + currentURL);
			
			System.out.println("The current page source is: " + driver.getPageSource());
		} 
		catch (Exception e) 
		{	
			e.printStackTrace();
		}
	
		
	}
}
