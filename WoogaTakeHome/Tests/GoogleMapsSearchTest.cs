using NUnit.Framework;
using OpenQA.Selenium;
using WoogaTakeHome.Resources;

[TestFixture(WebDriverSetup.BrowserType.Firefox, "DE")]
[TestFixture(WebDriverSetup.BrowserType.Chrome, "EN")]
public class GoogleMapsSearchTests
{
    private IWebDriver _driver;
    private GoogleMapsSearchPage _searchPage;
    private List<string> _resultCompareTerms;
    private WebDriverSetup.BrowserType _browserType;
    private readonly string _languageIdentifier;

    private const int EIFEL_TOWER_INDEX = 0;
    private const int STATUE_OF_LIBERTY_INDEX = 1;
    private const int COLOSSEUM_INDEX = 2;
    private const int GREAT_WALL_OF_CHINA_INDEX = 3;
    private const int BERLIN_OLYMPIC_PAR_SEARCH_TERM = 4;

    public GoogleMapsSearchTests(WebDriverSetup.BrowserType browserType, string languageIdentifier)
    {
        _browserType = browserType;
        _languageIdentifier = languageIdentifier;
    }

    [SetUp]
    public void Setup()
    {
        try
        {
            _driver = WebDriverSetup.InitializeDriver(_browserType);
            _searchPage = new GoogleMapsSearchPage(_driver, _languageIdentifier);
            _resultCompareTerms = TestDataConverter.LoadResultCompareTerms(_languageIdentifier);
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
       var term = _resultCompareTerms[EIFEL_TOWER_INDEX];
       bool foundTermInUrl =  _searchPage.SearchForTerm(term);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
   }

   // Multiple annotation test cases
   [TestCase("Eiffel Tower", ExpectedResult = true)]
   [TestCase("Eiffel", ExpectedResult = false)]
   [TestCase("Eiffel Towwer", ExpectedResult = true)] // Intentional misspelling -> should correct itself
   public bool SearchForEiffelTower_ShouldDisplayCorrectLocation(string searchTerm)
   {
       string resultCompareTerm =_resultCompareTerms[EIFEL_TOWER_INDEX];
       bool serchTermInURL =  _searchPage.SearchForTerm(searchTerm,resultCompareTerm);
       return serchTermInURL;
   }
   
   [TestCase("Olympic park", ExpectedResult = false)] // to many results
   [TestCase("Berlin Olympic parc", ExpectedResult = true)] // Intentional misspelling -> should correct itself
   public bool SearchForOlympicParkBerlin_ShouldDisplayCorrectLocation(string searchTerm)
   {
       string resultCompareTerm = _resultCompareTerms[BERLIN_OLYMPIC_PAR_SEARCH_TERM];
       bool foundTermInUrl =  _searchPage.SearchForTerm(searchTerm, resultCompareTerm);
       return foundTermInUrl;
   }

   // positive test
   [Test]
   public void SearchForStatueOfLibertyWithExtraDescription_ShouldDisplayCorrectLocation()
   {
       var term = _resultCompareTerms[STATUE_OF_LIBERTY_INDEX] + ", near New York"; // does deliver
       bool foundTermInUrl =  _searchPage.SearchForTerm(term);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
   }
   
   // Edgecase multiple locatoins test
   [Test]
   public void SearchForStatueOfLiberty_ShouldDisplayMultipleLocations()
   {
       var term = _resultCompareTerms[STATUE_OF_LIBERTY_INDEX];
       string resultCompareTerm = term.Replace(" ", "+");
       bool foundTermInUrl =  _searchPage.SearchForTerm(term, resultCompareTerm);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
   }
   
   [Test]
   public void SearchForGreatWallOfChina_ShouldDisplayCorrectLocation()
   {
       var term = _resultCompareTerms[GREAT_WALL_OF_CHINA_INDEX];
       bool foundTermInUrl =  _searchPage.SearchForTerm(term);
       Assert.That(foundTermInUrl, $"could not find {term} in URL");   
   }
   
   [Test]
   public void SearchForGreatWallUsingCommonName_ShouldDisplayCorrectLocation()
   {
       string term = _resultCompareTerms[GREAT_WALL_OF_CHINA_INDEX];
       string resultCompareTerm = _resultCompareTerms[GREAT_WALL_OF_CHINA_INDEX];
       bool foundTermInUrl =  _searchPage.SearchForTerm(term, resultCompareTerm);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
   }

   [Test]
   public void SearchForColosseum_ShouldDisplayCorrectLocation()
   {
       var term = _resultCompareTerms[COLOSSEUM_INDEX];
       bool foundTermInUrl = _searchPage.SearchForTerm(term);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
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