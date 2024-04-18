using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using FluentAssertions;

namespace Seleniumtests_novitckaia;

public class SeleniumTestsForPractice
{
    public ChromeDriver driver;
    
    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        // зайти в хром (с помощью вебдрайвера)
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2); // неявное ожидание
        
        // перейти по урлу https://staff-testing.testkontur.ru
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        
        // ввести логин и пароль
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("user");

        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("1q2w3e4r%T");
        
        // нажать на кнопку "Войти"
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();

        var news = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        
        // проверяем, что мы находимся на нужной странице
        var currentUrl = driver.Url;
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/news");
    }
    
    [TearDown]
    public void TearDown()
    {
        // закрываем браузер и убиваем процесс драйвера
        driver.Quit(); 
    }
}