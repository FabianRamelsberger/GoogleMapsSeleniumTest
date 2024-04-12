using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[TestFixture]
public class GoogleMapsSearchTests
{
    private IWebDriver _driver;
    private GoogleMapsSearchPage _searchPage;

    [SetUp]
    public void SetUp()
    {
        _driver = new ChromeDriver(); // Ensure ChromeDriver is available in your PATH or specify its location
        _driver.Manage().Window.Maximize();
        _searchPage = new GoogleMapsSearchPage(_driver);
    }

    [Test]
    public void SearchForEiffelTower_ShouldDisplayCorrectLocation()
    {
        _searchPage.Navigate();
        _searchPage.AcceptConsentDialogue();
        _searchPage.EnterSearchText("Eiffel Tower");
        _searchPage.ClickSearchButton();
        Assert.That(_driver.Url.Contains("Eiffelturm"), "The search did not navigate to the Eiffel Tower location.");
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
    }
}