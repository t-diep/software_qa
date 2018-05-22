package selenium_webdriver_basic;

import java.util.concurrent.TimeUnit;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

public class UseElementLocatorTechniques 
{
	static WebDriver driver;
	
	public static void main(String[] args) 
	{
		//elementLocatorAmazon();
		elementLocatorFacebook();
	}
	
	/**
	 * 
	 * @param url
	 */
	public static void invokeBrowser(String url)
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
				
				driver.get(url);
			} 
			catch (Exception e) 
			{
				e.printStackTrace();
			}
	}
	
	public static void elementLocatorAmazon()
	{
		//Go to Amazon.in (Amazon India)
		invokeBrowser("http://www.amazon.in");
		//Type in "Lenovo Laptops" to the search bar
		driver.findElement(By.id("twotabsearchtextbox")).sendKeys("Lenovo Laptops");
		//Click on the search button to confirm
		driver.findElement(By.className("nav-input")).click();
			
		driver.findElement(By.linkText("Your Amazon.in")).click();
		
		driver.navigate().back();
		
		driver.findElement(By.linkText("Today's Deals")).click();
		
		driver.findElement(By.partialLinkText("Amazon Pay")).click();
	}
	
	public static void elementLocatorFacebook()
	{
		invokeBrowser("http://www.facebook.com");
		
		driver.findElement(By.name("firstname")).sendKeys("Tony");

		driver.findElement(By.cssSelector("input#"));
	}
}
