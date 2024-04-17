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
    
   //test cases loaded via Json

   #region JSON Test cases

   [Test]
   public void SearchForEiffelTower_ShouldDisplayCorrectLocation()
   {
       var term = _searchTerms[EIFEL_TOWER_INDEX];
       bool foundTermInUrl = SearchForTerm(term);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
   }

   #endregion

   #region Multiple annotation test cases
   [TestCase("Eiffel Tower", ExpectedResult = true)]
   [TestCase("Eiffel", ExpectedResult = false)]
   [TestCase("Eiffel Towwer", ExpectedResult = true)] // Intentional misspelling -> should correct itself
   public bool SearchForEiffelTower_ShouldDisplayCorrectLocation(string searchTerm)
   {
       string resultCompareTerm = "Eiffel Tower";
       bool serchTermInURL = SearchForTerm(searchTerm,resultCompareTerm);
       return serchTermInURL;
   }
   
   [TestCase("Olympic park", ExpectedResult = false)] // to many results
   [TestCase("Berlin Olympic parc", ExpectedResult = true)] // Intentional misspelling -> should correct itself
   public bool SearchForOlympicParkBerlin_ShouldDisplayCorrectLocation(string searchTerm)
   {
       string resultCompareTerm = "Berlin Olympic park";
       bool foundTermInUrl = SearchForTerm(searchTerm, resultCompareTerm);
       return foundTermInUrl;
   }
   
   // positive test
   [Test]
   public void SearchForStatueOfLibertyWithExtraDescription_ShouldDisplayCorrectLocation()
   {
       var term = _searchTerms[STATUE_OF_LIBERTY_INDEX] + ", near New York"; // does deliver
       bool foundTermInUrl = SearchForTerm(term);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
   }
   
   // Edgecase multiple locatoins test
   [Test]
   public void SearchForStatueOfLiberty_ShouldDisplayMultipleLocations()
   {
       var term = _searchTerms[STATUE_OF_LIBERTY_INDEX];
       string resultCompareTerm = term.Replace(" ", "+");
       bool foundTermInUrl = SearchForTerm(term, resultCompareTerm);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
   }
   
   [Test]
   public void SearchForGreatWallOfChina_ShouldDisplayCorrectLocation()
   {
       var term = _searchTerms[GREAT_WALL_OF_CHINA_INDEX];
       bool foundTermInUrl = SearchForTerm(term);
       Assert.That(foundTermInUrl, $"could not find {term} in URL");   
   }
   
   [Test]
   public void SearchForGreatWallUsingCommonName_ShouldDisplayCorrectLocation()
   {
       string term = "Chinas Great Wall";
       string resultCompareTerm = _searchTerms[GREAT_WALL_OF_CHINA_INDEX];
       bool foundTermInUrl = SearchForTerm(term, resultCompareTerm);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
   }

   [Test]
   public void SearchForColosseum_ShouldDisplayCorrectLocation()
   {
       var term = _searchTerms[COLOSSEUM_INDEX];
       bool foundTermInUrl = SearchForTerm(term);
       Assert.That(foundTermInUrl, $"could not find {term} in URL"); 
   }
   
   #endregion

   #region Helper functions

   private bool SearchForTerm(string searchTerm, string resultCompareTerm = "")
   {
       if (_driver == null) throw new InvalidOperationException("WebDriver is not initialized.");

       // if there is no result term given we just search for the search term in the URL
       if (resultCompareTerm.Equals(""))
       {
           resultCompareTerm = searchTerm;
       }
       _searchPage.Navigate();
       _searchPage.AcceptConsentDialogue();
       return EnterSearchTermReturnValidationCheck(searchTerm,resultCompareTerm);
   }

   #endregion
    
    private bool EnterSearchTermReturnValidationCheck(string searchTerm, string resultCompareTerm)
    {
        _searchPage.EnterSearchText(searchTerm);
        _searchPage.ClickSearchButton();
       return _searchPage.WaitForPageLoadReturnValidationCheck(resultCompareTerm);
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