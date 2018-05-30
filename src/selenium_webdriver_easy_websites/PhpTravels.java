package selenium_webdriver_easy_websites;

import java.util.concurrent.TimeUnit;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

/**
 * Contains the test setups and test scripts for the sample demo website
 * "PHPTravels.com"
 * 
 * @author Tony Diep, last updated 5-30-18
 */
public class PhpTravels 
{
	static WebDriverWait waiting;
	static WebDriver driver;
	static JavascriptExecutor jexe;
	
	public static void main(String[] args) 
	{
		configureBrowser();
		loadWebPage();
		handleInitialPopUp();
		register();
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
		//Make window as big as possible for visibility sake
		driver.manage().window().maximize();

		//Delete all cookies first
		driver.manage().deleteAllCookies();

		//Wait until the browser can load all web elements
		driver.manage().timeouts().implicitlyWait(70, TimeUnit.SECONDS);

		//Give some time for the browser to load all elements before timing out
		driver.manage().timeouts().pageLoadTimeout(70, TimeUnit.SECONDS);

		//Go to a specific URL to perform test scripts on
		driver.get("https://phptravels.com/demo/");

		//Set up JavaScript executor for scrolling 
		jexe = (JavascriptExecutor) driver;
	}
	
	
	/**
	 * Dismisses the initial alert pop-up when visiting the website
	 */
	private static void handleInitialPopUp()
	{
		try
		{
			waiting = new WebDriverWait(driver, 10);
			WebElement dismissPopUp = waiting.until(ExpectedConditions.visibilityOfElementLocated(By.cssSelector("#onesignal-popover-cancel-button")));
			dismissPopUp.click();
		}
		catch(Exception e)
		{			
			e.printStackTrace();
		}	
	}
	
	/**
	 * Registers a new account onto PHPTravels.org
	 */
	public static void register()
	{
		//driver.switchTo().frame(driver.findElement(By.xpath("//*[@id='main-menu']")));
		driver.findElement(By.linkText("http://phptravels.org")).click();
	}
}
