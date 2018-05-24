package selenium_webdriver_website_serebiidotnet;

import java.util.concurrent.TimeUnit;

import javax.swing.JOptionPane;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.edge.EdgeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.ie.InternetExplorerDriver;
import org.openqa.selenium.opera.OperaDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

public class WebDriverChrome_Serebii 
{
	static WebDriver driver;
	static JavascriptExecutor jexe;
	static final String popupMessage = "Enter a browser to use for Selenium Webdriver \n\n";
	static final String browserChoices = "chrome \nfirefox \nedge \nopera \nexplorer";
	
	public static void main(String[] args) 
	{
		String browser = (JOptionPane.showInputDialog(null, popupMessage + browserChoices)).toLowerCase();
		String userInput = JOptionPane.showInputDialog(null, "Enter test script number");
		int scriptNum = Integer.parseInt(userInput);
		
		configureBrowser(browser);
		loadWebPage();
		runTestScript(scriptNum);
	}

	/**
	 * 
	 * @param browser
	 */
	private static void configureBrowser(String browser)
	{
		//Tell system which web driver to use
		System.setProperty("webdriver.chrome.driver", "C:\\Users\\antho\\Downloads\\Selenium\\chromedriver_win32\\chromedriver.exe");
		driver = new ChromeDriver();	
	}
	
	/**
	 * 
	 */
	public static void loadWebPage()
	{
		try {
			//Make window as big as possible for visibility sake
			driver.manage().window().maximize();
			
			//Delete all cookies first
			driver.manage().deleteAllCookies();
			
			driver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
			
			driver.manage().timeouts().pageLoadTimeout(40, TimeUnit.SECONDS);
			
			driver.get("https://www.serebii.net/index2.shtml");
		} 
		catch (Exception e) 
		{
			e.printStackTrace();
		}
	}
	
	/**
	 * 
	 * @param number
	 */
	public static void runTestScript(int number)
	{
		switch(number)
		{
			case 0:
				navigateToUSUMPage();
			break;
			
			default:
				driver.close();
		}
	}
	
	/**
	 * Test script to navigate to the Pokemon Ultra Sun and Ultra Moon (USUM) webpage 
	 */
	private static void navigateToUSUMPage()
	{
		jexe = (JavascriptExecutor) driver;
		jexe.executeScript("scroll(0, 1200)");
		driver.findElement(By.linkText("Ultra Sun & Ultra Moon")).click();
		jexe.executeScript("scroll(0, 400)");
		driver.findElement(By.cssSelector("input[class='")).click();
		//String absPath = "html/head/body[1]";
		//new WebDriverWait(driver, 10).until(ExpectedConditions.elementToBeClickable(By.cssSelector("//*[@id='player_uid_946563507_1']/div[4]/div[1]/@button='player_uid_946563507_1']/div[4]/button"))).click();
		
	}
}
