using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ArgsProblem.Tests
{
    public class SearchTest : IDisposable
    {
        private ChromeDriver driver;

        public SearchTest()
        {
            var options = new ChromeOptions();
            driver = new ChromeDriver(".", options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);
            driver
                .Navigate()
                .GoToUrl(new Uri(
                    "https://codility-frontend-prod.s3.amazonaws.com/media/task_static/qa_csharp_search/862b0faa506b8487c25a3384cfde8af4/static/attachments/reference_page.html"));
        }

        [Fact]
        public void test_If_SearchField_Exists()
        {
            var inputTextElements = driver.FindElements(By.Id("search-input"));
            Assert.Equal(1, inputTextElements.Count);

            var searchButtons = driver.FindElements(By.Id("search-button"));
            Assert.Equal(1, searchButtons.Count);
        }

        [Fact]
        public void test_If_Empty_Text_Search_Is_Forbidden()
        {
            // Example how to use the driver:
            TypeInSearchBar(string.Empty);
            Search();
            ExplicitWaitForResults(TimeSpan.FromMilliseconds(100));

            var elements = driver.FindElements(By.Id("error-empty-query"));
            Assert.Equal(1, elements.Count);
            var text = elements.First().Text;
            Assert.Equal("Provide some query", text);
        }

        private void TypeInSearchBar(string text)
        {
            var inputTextElement = driver.FindElement(By.Id("search-input"));
            inputTextElement.SendKeys(text);
        }

        private void Search()
        {
            driver.FindElement(By.Id("search-button")).Click();
        }

        public void ExplicitWaitForResults(TimeSpan waitingTime)
        {
            var webDriverWait = new WebDriverWait(driver, waitingTime);
            webDriverWait.Until(driver => driver.FindElement(By.Id("search-results")));
        }

        public void Dispose()
        {
            driver?.Dispose();
        }
    }
}
