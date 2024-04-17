using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

public class WebDriverSetup
{
    public enum BrowserType
    {
        Chrome,
        Firefox,
        Edge
    }

    public static IWebDriver InitializeDriver(BrowserType browserType)
    {
        IWebDriver driver;

        // Get the path to the directory of the executing assembly
        string driverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Drivers");
        
        switch (browserType)
        {
            case BrowserType.Chrome:
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                ChromeOptions chromeOptions = new ChromeOptions();
                // Add Chrome-specific options here
                driver = new ChromeDriver(chromeOptions);
                break;

            case BrowserType.Firefox:
                FirefoxOptions firefoxOptions = new FirefoxOptions();
                // Add Firefox-specific options here
                driver = new FirefoxDriver(firefoxOptions);
                break;

            case BrowserType.Edge:
                EdgeOptions edgeOptions = new EdgeOptions();
                // Add Edge-specific options here
                driver = new EdgeDriver(edgeOptions);
                break;

            default:
                throw new ArgumentException("Browser not supported.");
        }

        driver.Manage().Window.Maximize();
        return driver;
    }
}