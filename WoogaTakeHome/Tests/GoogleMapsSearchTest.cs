using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
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
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        _driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options);
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
    public void SearchForStatueOfLiberty_ShouldDisplayCorrectLocation()
    {
        var term = _searchTerms[STATUE_OF_LIBERTY_INDEX];
        SearchForTermAndAssert(term);
    }
    
    [Test]
    public void SearchForEiffelTowerUsingPartialName_ShouldDisplayCorrectLocation()
    {
        var term = "Eiffel";
        SearchForTermAndAssert(term);
    }

    [Test]
    public void SearchForStatueOfLibertyWithExtraDescription_ShouldDisplayCorrectLocation()
    {
        var term = _searchTerms[STATUE_OF_LIBERTY_INDEX] + ", near New York";
        SearchForTermAndAssert(term);
    }

    [Test]
    public void SearchForGreatWallUsingCommonName_ShouldDisplayCorrectLocation()
    {
        var term = "The Great Wall";
        SearchForTermAndAssert(term);
    }
    
    [Test]
    public void SearchForColosseum_ShouldDisplayCorrectLocation()
    {
        var term = _searchTerms[COLOSSEUM_INDEX];
        SearchForTermAndAssert(term);
    }

    [Test]
    public void SearchForGreatWallOfChina_ShouldDisplayCorrectLocation()
    {
        var term = _searchTerms[GREAT_WALL_OF_CHINA_INDEX];
        SearchForTermAndAssert(term);    
    }
    
    private void SearchForTermAndAssert(string term)
    {
        _searchPage.Navigate();
        _searchPage.AcceptConsentDialogue();
        EnterSearchClickWaitForLoad(term);
        Assert.That(_driver.PageSource.Contains(term.Split(',')[0]), $"could not find {term} in URL"); 
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