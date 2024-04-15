using NUnit.Framework;
using OpenQA.Selenium;
using WoogaTakeHome.Resources;

[TestFixture(WebDriverSetup.BrowserType.Firefox)]
[TestFixture(WebDriverSetup.BrowserType.Chrome)]
public class GoogleMapsSearchTests
{
    private IWebDriver _driver;
    private GoogleMapsSearchPage _searchPage;
    private List<string> _searchTerms;
    private WebDriverSetup.BrowserType _browserType;

    private static int EIFEL_TOWER_INDEX = 0;
    private static int STATUE_OF_LIBERTY_INDEX = 1;
    private static int COLOSSEUM_INDEX = 2;
    private static int GREAT_WALL_OF_CHINA_INDEX = 3;
    
    public GoogleMapsSearchTests(WebDriverSetup.BrowserType browserType)
    {
        _browserType = browserType;
    }

    [SetUp]
    public void Setup()
    {
        try
        {
            _driver = WebDriverSetup.InitializeDriver(_browserType);
            _searchPage = new GoogleMapsSearchPage(_driver);
            _searchTerms = TestData.LoadSearchTerms();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to initialize WebDriver: " + ex.Message);
            throw;  // Re-throwing is important to prevent further execution with invalid state.
        }
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
        if (_driver == null) throw new InvalidOperationException("WebDriver is not initialized.");
        
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
    
    [OneTimeTearDown]
    public void GlobalTearDown()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver = null;  // Nullify after quitting to prevent misuse.
        }
    }
}