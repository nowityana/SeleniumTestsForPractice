using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using FluentAssertions;
using System.Drawing;

namespace Seleniumtests_novitckaia;

public class SeleniumTestsForPractice
{
    public ChromeDriver driver;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        driver = new ChromeDriver(options); // зайти в хром (с помощью вебдрайвера)
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2); // неявное ожидание
        
        Autorization(); // авторизация
    }
    
    [Test]
    public void AutorizationTest() // тест на авторизацию
    {
        var news = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        
        var currentUrl = driver.Url; 
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/news"); // проверяем урл
    }

    [Test]
    public void SideBarMenuTest() // тест на вызов бокового меню и переход из него на страницу Сообщества 
    {
        // установка маленького размера,чтобы у экранов с большим разрешением не падал тест из-за отсуствия бокового меню
        driver.Manage().Window.Size = new Size(800, 600); 
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']")); 
        sideMenu.Click(); // нажать на боковое меню
        
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']")).First(element => element.Displayed);
        community.Click(); // нажать на "Сообщества"
        
        // проверяем что сообщества есть на странице
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']")); 
        driver.Url.Should().Be("https://staff-testing.testkontur.ru/communities"); // проверяем урл
    }
    
    public void Autorization() // метод авторизации
    {
        // перейти по урлу https://staff-testing.testkontur.ru
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru"); 
        
        var login = driver.FindElement(By.Id("Username")); // ввести логин
        login.SendKeys("novitsckaja.yana@yandex.ru");

        var password = driver.FindElement(By.Name("Password")); // ввести пароль
        password.SendKeys("Lnfffjd5fd3kdf!");
        
        var enter = driver.FindElement(By.Name("button")); // нажать на кнопку "Войти"
        enter.Click();
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Close(); // закрываем браузер
        driver.Quit(); // убиваем процесс драйвера
    }
}