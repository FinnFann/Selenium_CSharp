using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium_Tests.PageObjects;
using System;
using System.Collections.Generic;

namespace Example_Tests
{
    public class GoogleExampleTest
    {
        GoogleSearchPage googlePage = new GoogleSearchPage();
        string baseUrl = "https://www.google.com/";
        string searchTerm = "Metabolon";
        string expectedHref = "www.metabolon.com";
        IWebDriver driver;

        [SetUp]
        public void Start_Browser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void Exit_Browser()
        {
            driver.Quit();
        }

        [Test]
        public void PerformGoogleSearchVerifyUrl()
        {
            driver.Navigate().GoToUrl(baseUrl);
            googlePage.PerformGoogleSearch(driver, searchTerm);
            Assert.That(driver.Url, Contains.Substring("search?q=" + searchTerm + "&"));
        }

        [Test]
        public void PerformGoogleSearchVerifyUrl_Should_Not_Contain_Metabolona()
        {
            driver.Navigate().GoToUrl(baseUrl);
            googlePage.PerformGoogleSearch(driver, searchTerm);
            Assert.IsFalse((driver.Url.Contains("search?q=" + searchTerm + "a&")));
        }

        [Test]
        public void VerifyFirstPageContainsHrefMatch()
        {
            int numberOfChecks = 1;
            driver.Navigate().GoToUrl(baseUrl);
            googlePage.PerformGoogleSearch(driver, searchTerm);
            IList<IWebElement> searchResults = driver.FindElements(googlePage.searchResults);
            foreach (IWebElement result in searchResults)
            {
                if (result.GetAttribute("href").ToString().Contains(expectedHref))
                {
                    Assert.Pass("Found a matching href link on the " + numberOfChecks + " element: " + result.GetAttribute("href").ToString() + 
                        Environment.NewLine + "Terminating before checking remainder." + 
                        Environment.NewLine + "Total elements found: " + searchResults.Count);
                    return;
                }
                numberOfChecks++;
            }
            Assert.Fail("No result contained the expected URL on the first page");
        }
    }
}