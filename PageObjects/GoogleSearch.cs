using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace Selenium_Tests.PageObjects
{
    class GoogleSearchPage
    {
        public By searchInput = By.CssSelector("[title='Search']");
        public By searchButton = By.XPath("//input[contains(@value,'Search') and not(ancestor::div[@wfd-invisible])]"); //Unused as it's deemed "uninteractable"
        public By searchResults = By.CssSelector("a[href]");

        public void PerformGoogleSearch(IWebDriver driver, String searchTerm)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            IWebElement searchInput = driver.FindElement(this.searchInput);
            //IWebElement searchButton = driver.FindElement();
            searchInput.Clear();
            searchInput.SendKeys(searchTerm);
            searchInput.SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.StalenessOf(searchInput));
        }
    }
}
