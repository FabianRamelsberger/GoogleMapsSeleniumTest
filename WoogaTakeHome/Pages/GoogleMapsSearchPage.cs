using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
public class GoogleMapsSearchPage
{
    private readonly IWebDriver _driver;
    // Updated URL to ensure the interface loads in English
    private readonly string _url;
    private readonly string _urlEN = "https://www.google.com/maps?hl=en";
    private readonly string _urlDE = "https://www.google.com/maps?hl=de";
    private const float SECONDS_TO_WAIT_FOR_URL = 15f;

    private IWebElement SearchInput => _driver.FindElement(By.Id("searchboxinput"));
    private IWebElement SearchButton => _driver.FindElement(By.Id("searchbox-searchbutton"));

    public GoogleMapsSearchPage(IWebDriver driver, string languageIdentifier)
    {
        _driver = driver;
        // This could be extended in its own class, when supporting multiple languages
        if (languageIdentifier.Equals("EN"))
        {
            _url = _urlEN;
        }
        else if(languageIdentifier.Equals("DE"))
        {
            _url = _urlDE;
        }
    }

        
    public bool SearchForTerm(string searchTerm, string resultCompareTerm = "")
    {
        if (_driver == null) throw new InvalidOperationException("WebDriver is not initialized.");

        // if there is no result term given we just search for the search term in the URL
        if (resultCompareTerm.Equals(""))
        {
            resultCompareTerm = searchTerm;
        }
        Navigate();
        AcceptConsentDialogue();
        return EnterSearchTermReturnValidationCheck(searchTerm,resultCompareTerm);
    }

    private bool EnterSearchTermReturnValidationCheck(string searchTerm, string resultCompareTerm)
    {
        EnterSearchText(searchTerm);
        ClickSearchButton();
        return WaitForPageLoadReturnValidationCheck(resultCompareTerm);
    }
    
    private void Navigate()
    {
        _driver.Navigate().GoToUrl(_url);
    }

    public void EnterSearchText(string searchText)
    {
        SearchInput.SendKeys(searchText);
    }

    private void ClickSearchButton()
    {
        SearchButton.Click();
    }

    private void AcceptConsentDialogue()
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
    
    private bool WaitForPageLoadReturnValidationCheck(string resultCompareTerm)
    {
        // space is replaced with + in urls
        string multipleWordsTerm = resultCompareTerm.Replace(' ', '+').Split(',')[0];
        try
        {
            // This wait checks that the URL has been updated to contain the expected query.
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(SECONDS_TO_WAIT_FOR_URL));
            wait.Until(d => d.Url.ToLower().Contains(resultCompareTerm.ToLower()) ||
                            d.Url.ToLower().Contains(multipleWordsTerm.ToLower()));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            // for negative tests or timeouts
            return false;
        }
    }
}