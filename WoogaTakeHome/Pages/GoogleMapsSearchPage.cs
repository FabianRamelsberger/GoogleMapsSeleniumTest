using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
public class GoogleMapsSearchPage
{
    private readonly IWebDriver _driver;
    private readonly string _url = "https://www.google.com/maps";
    private IWebElement SearchInput => _driver.FindElement(By.Id("searchboxinput"));
    private IWebElement SearchButton => _driver.FindElement(By.Id("searchbox-searchbutton"));


    public GoogleMapsSearchPage(IWebDriver driver)
    {
        _driver = driver;
    }

    public void Navigate()
    {
        _driver.Navigate().GoToUrl(_url);
    }

    public void EnterSearchText(string searchText)
    {
        SearchInput.SendKeys(searchText);
    }

    public void ClickSearchButton()
    {
        SearchButton.Click();
        WaitForPageLoad();
    }

    public void AcceptConsentDialogue()
    {
        // Create WebDriverWait instance with a timeout (e.g., 10 seconds)
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        try
        {
            // Wait for the consent button to be clickable
            // Adjust the selector "#buttonId" to the actual button ID or CSS selector of the consent button
            IWebElement consentButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".VfPpkd-LgbsSe.VfPpkd-LgbsSe-OWXEXe-k8QpJ.VfPpkd-LgbsSe-OWXEXe-dgl2Hf.nCP5yc.AjY5Oe.DuMIQc.LQeN7.XWZjwc")));
            // Click the consent button
            consentButton.Click();

            // Now, wait for the search input box to be visible on the page
            // Adjust the selector "#searchboxinput" if necessary
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("#searchboxinput")));
        }
        catch (WebDriverTimeoutException ex)
        {
            // Handle the case where the element was not found within the timeout period
            Console.WriteLine("Element not found within the specified wait time.");
        }
    }

    private void WaitForPageLoad()
    {
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        // This wait checks that the URL has been updated to contain the expected query.
        wait.Until(d => _driver.Url.ToLower().Contains("eiffelturm") || d.Url.ToLower().Contains("eiffel+tower") || d.Url.ToLower().Contains("eiffel%20tower"));
    }
}