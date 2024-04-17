using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

public class WebDriverSetup
{
    public enum BrowserType
    {
        Chrome,
        Firefox
    }

    public static IWebDriver InitializeDriver(BrowserType browserType)
    {
        IWebDriver driver;
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

            default:
                throw new ArgumentException("Browser not supported.");
        }

        driver.Manage().Window.Maximize();
        return driver;
    }
}