using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace Metabolon_Selenium_Interview
{
    public class Metabolon
    {
        string baseUrl = "https://www.saucedemo.com/";
        string[] goodUser = { "standard_user", "secret_sauce" };
        string[] badUser = { "locked_out_user", "secret_sauce" };
        By userNameSelector = By.XPath("//input[@id='user-name']");
        By passwordSelector = By.XPath("//input[@id='password']");
        By loginButtonSelector = By.XPath("//input[@id='login-button']");
        By postLoginLogo = By.XPath("//div[@class='app_logo']");
        By errorMessageDisplay = By.XPath("//div[contains(@class,'error-message-container')]/h3");
        string userBannedMessage = "Sorry, this user has been banned";

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
        public void SuccessfulLogin()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            //I am on the login page
            driver.Navigate().GoToUrl(baseUrl);
            IWebElement userNameInput = driver.FindElement(userNameSelector);
            IWebElement passwordInput = driver.FindElement(passwordSelector);
            IWebElement loginButton = driver.FindElement(loginButtonSelector);

            //I fill in the account information (good)
            userNameInput.Clear();
            passwordInput.Clear();
            userNameInput.SendKeys(goodUser[0]);
            passwordInput.SendKeys(goodUser[1]);

            //I click login
            loginButton.Click();
            wait.Until(ExpectedConditions.StalenessOf(loginButton));

            //I am redirected to the sauce demo main page
            Assert.That(driver.Url, Contains.Substring("www.saucedemo.com/inventory.html"));

            //I verify the app logo exists
            Assert.True(driver.FindElement(postLoginLogo).Displayed);
        }

        [Test]
        public void FailedLoginErrorCheck()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            //I am on the login page
            driver.Navigate().GoToUrl(baseUrl);
            IWebElement userNameInput = driver.FindElement(userNameSelector);
            IWebElement passwordInput = driver.FindElement(passwordSelector);
            IWebElement loginButton = driver.FindElement(loginButtonSelector);

            //I fill in the account information (good)
            userNameInput.Clear();
            passwordInput.Clear();
            userNameInput.SendKeys(badUser[0]);
            passwordInput.SendKeys(badUser[1]);

            //I click login
            loginButton.Click();

            //I verify the error message contains the text "Sorry, this user has been banned"
            wait.Until(ExpectedConditions.ElementIsVisible(errorMessageDisplay));
            Assert.That(driver.FindElement(errorMessageDisplay).Text, Contains.Substring(userBannedMessage));
        }
    }
}