using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ArgsProblem.Tests
{
    public class SearchTest : IDisposable
    {
        private readonly ChromeDriver _driver;

        public SearchTest()
        {
            var options = new ChromeOptions();
            _driver = new ChromeDriver(".", options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);
            _driver
                .Navigate()
                .GoToUrl(new Uri(
                    "https://codility-frontend-prod.s3.amazonaws.com/media/task_static/qa_csharp_search/862b0faa506b8487c25a3384cfde8af4/static/attachments/reference_page.html"));
        }

        [Fact]
        public void test_If_SearchField_Exists()
        {
            var inputTextElements = _driver.FindElements(By.Id("search-input"));
            Assert.Equal(1, inputTextElements.Count);

            var searchButtons = _driver.FindElements(By.Id("search-button"));
            Assert.Equal(1, searchButtons.Count);
        }

        [Fact]
        public void test_If_Empty_Text_Search_Is_Forbidden()
        {
            TypeInSearchBar(string.Empty);
            Search();
            ExplicitWaitForResults(TimeSpan.FromMilliseconds(100));

            var elements = _driver.FindElements(By.Id("error-empty-query"));
            Assert.Equal(1, elements.Count);
            var text = elements.First().Text;
            Assert.Equal("Provide some query", text);
        }

        [Fact]
        public void test_Atleast_One_Island_Is_Found_For_Isla()
        {
            TypeInSearchBar("isla");
            Search();
            ExplicitWaitForResults(TimeSpan.FromMilliseconds(100));

            var unOrderLists = _driver.FindElement(By.Id("search-results"));
            var lists = unOrderLists.FindElements(By.TagName("li"));
            Assert.NotEmpty(lists);
            Assert.NotEmpty(lists.First().Text);
        }

        [Fact]
        public void test_For_No_Result_Found()
        {
            // Example how to use the driver:
            TypeInSearchBar("castle");
            Search();
            ExplicitWaitForResults(TimeSpan.FromMilliseconds(100));

            var errorTextDivs = _driver.FindElements(By.Id("error-no-results"));
            Assert.Equal(1, errorTextDivs.Count);
            var errorDivText = errorTextDivs.First().Text;
            Assert.Equal("No results", errorDivText);
        }

        private void TypeInSearchBar(string text)
        {
            var inputTextElement = _driver.FindElement(By.Id("search-input"));
            inputTextElement.SendKeys(text);
        }

        private void Search()
        {
            _driver.FindElement(By.Id("search-button")).Click();
        }

        public void ExplicitWaitForResults(TimeSpan waitingTime)
        {
            var webDriverWait = new WebDriverWait(_driver, waitingTime);
            webDriverWait.Until(driver => driver.FindElement(By.Id("search-results")));
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
