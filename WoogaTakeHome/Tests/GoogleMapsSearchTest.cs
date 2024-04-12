using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WoogaTakeHome.Resources;

[TestFixture]
public class GoogleMapsSearchTests
{
    private IWebDriver _driver;
    private GoogleMapsSearchPage _searchPage;
    private List<string> _searchTerms;

    private static int EIFEL_TOWER_INDEX = 0;
    private static int STATUE_OF_LIBERTY_INDEX = 1;
    private static int COLOSSEUM_INDEX = 2;
    private static int GREAT_WALL_OF_CHINA_INDEX = 3;
    
    [SetUp]
    public void SetUp()
    {
        _driver = new ChromeDriver(); // Ensure ChromeDriver is available in your PATH or specify its location
        _driver.Manage().Window.Maximize();
        _searchPage = new GoogleMapsSearchPage(_driver);
        _searchTerms = TestData.LoadSearchTerms();
    }
    
    [Test]
    public void SearchForEiffelTower_ShouldDisplayCorrectLocation()
    {
        var term = _searchTerms[EIFEL_TOWER_INDEX];
        SearchForTermAndAssert(term);
    }

    [Test]
    public void Search_For_Statue_Of_Liberty()
    {
        var term = _searchTerms[STATUE_OF_LIBERTY_INDEX];
        SearchForTermAndAssert(term);
    }
    
    [Test]
    public void Search_For_Colosseum()
    {
        var term = _searchTerms[COLOSSEUM_INDEX];
        SearchForTermAndAssert(term);
    }

    [Test]
    public void Search_For_Great_Wall_Of_China()
    {
        var term = _searchTerms[GREAT_WALL_OF_CHINA_INDEX];
        SearchForTermAndAssert(term);    }
    
    private void SearchForTermAndAssert(string term)
    {
        _searchPage.Navigate();
        _searchPage.AcceptConsentDialogue();
        EnterSearchClickWaitForLoad(term);
        Assert.That(_driver.PageSource.Contains(term.Split(',')[0]), $"could not find {term} in URL"); // Simplistic check
    }

    private void EnterSearchClickWaitForLoad(string term, string additionalDescription = "")
    {
        _searchPage.EnterSearchText(term,additionalDescription);
        _searchPage.ClickSearchButton();
        _searchPage.WaitForPageLoad(term);
    }
    
    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
    }
}