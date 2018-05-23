package selenium_webdriver_basic;

import java.util.concurrent.TimeUnit;

import javax.swing.JOptionPane;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

public class WebDriver_Serebii 
{
	static WebDriver driver;
	static JavascriptExecutor jexe;
	
	public static void main(String[] args) 
	{
		String userInput = JOptionPane.showInputDialog(null, "Enter test script number");
		int scriptNum = Integer.parseInt(userInput);
		
		loadSerebii();	
		runTestScript(scriptNum);
	}

	/**
	 * 
	 */
	public static void loadSerebii()
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
				usumPage();
			break;
			
			default:
				driver.close();
		}
	}
	
	private static void usumPage()
	{
		jexe = (JavascriptExecutor) driver;
		jexe.executeScript("scroll(0, 1200)");
		driver.findElement(By.linkText("Ultra Sun & Ultra Moon")).click();
		jexe.executeScript("scroll(0, 400)");
		driver.findElement(By.cssSelector("input[class='")).click();
	}
}
