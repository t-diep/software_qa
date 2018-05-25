package selenium_webdriver_hard_websites;

import java.util.concurrent.TimeUnit;

import javax.swing.JOptionPane;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

/**
 * ON HOLD FOR NOW
 * 
 * @author Tony Diep, last updated 5-25-18
 *
 */
public class WebDriverChromeAmazon 
{
	static WebDriver driver;
	
	public static void main(String[] args) 
	{
		String input = JOptionPane.showInputDialog(null, "Enter an integer to run a test script");
		int scriptNum = Integer.parseInt(input);
		invokeChromeBrowser();
		runTestScript(scriptNum);
	}
	
	public static void invokeChromeBrowser()
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
			
			driver.manage().timeouts().pageLoadTimeout(40, TimeUnit.SECONDS);
			
			driver.get("http://www.amazon.com");
		} 
		catch (Exception e) 
		{
			e.printStackTrace();
		}
	}
	
	public static void runTestScript(int number)
	{
		switch(number)
		{
			case 0:
			openAndCloseBrowser();
			break;
			
			case 1:
			checkTodaysDeals();
			break;
		}
	}
	
	/**
	 * Simple test script run to open and close browser
	 */
	private static void openAndCloseBrowser()
	{
		driver.close();
	}
	
	/**
	 * 
	 */
	private static void checkTodaysDeals()
	{
		driver.findElement(By.linkText("/gp/goldbox/ref=nav_cs_gb")).click();
	}
}
