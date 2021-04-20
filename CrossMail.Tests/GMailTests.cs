using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace CrossMail.Tests
{
    public class GMailTests: IDisposable
    {
        IWebDriver _browserDriver;
        IConfiguration _config;
        public GMailTests()
        {
            _browserDriver = new ChromeDriver("./");
            _config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
        }

        public void Dispose()
        {
            _browserDriver.Quit();
        }

        [Fact]
        public void Should_Send_Email()
        {
            _browserDriver.Navigate().GoToUrl("https://mail.google.com/");
            var userElement = _browserDriver.FindElement(By.Id("identifierId"));
            userElement.SendKeys(_config["username"]);

            _browserDriver.FindElement(By.Id("identifierNext")).Click();

            var passwordElement = _browserDriver.FindElement(By.Name("password"));
            passwordElement.SendKeys(_config["password"]);

            _browserDriver.FindElement(By.Id("passwordNext")).Click();

            var composeElement = _browserDriver.FindElement(By.XPath("//*[@role='button' and (.)='COMPOSE']"));
            composeElement.Click();

            _browserDriver.FindElement(By.Name("to")).Clear();
            _browserDriver.FindElement(By.Name("to")).SendKeys($"{_config["username"]}@gmail.com");

            var sendButtonElement = _browserDriver.FindElement(By.XPath("//*[@role='button' and text()='Send']"));
            sendButtonElement.Click();
        }
    }
}
