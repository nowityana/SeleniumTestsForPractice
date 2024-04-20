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
    
    [Test]
    public void EditProfileAdress() // тест на редактирование адреса в профиле 
    {
        var buttonActions = driver.FindElement(By.CssSelector("[data-tid='Actions']")); 
        buttonActions.Click(); // нажать на профиль
        
        var buttonEdit = driver.FindElement(By.CssSelector("[data-tid='ProfileEdit']"));
        buttonEdit.Click(); // нажать на "Редактировать"
        
        var adressEdit = driver.FindElement(By.CssSelector("textarea.react-ui-r3t2bi"));
        adressEdit.SendKeys("Дворцовая пл., 2, Санкт-Петербург"); // ввод адреса
        
        var buttonSave = driver.FindElement(By.CssSelector("button.sc-juXuNZ.kVHSha"));   
        buttonSave.Click(); // нажать на "Сохранить"
        
        // без явного ожидания иногда тест падает из-за того, что не успевает перейти на страницу профиля
        Thread.Sleep(1000);
        
        // проверка, что после сохранения находимся на странице профиля пользователя
        driver.Url.Should().Be("https://staff-testing.testkontur.ru/profile/b060adf8-1282-49c4-8ef6-56afee9a4df8");
        
        // проверка, что поле "Адрес" содержит то, что ввели
        var adressValue = driver.FindElement(By.CssSelector("div.sc-exqIPC.leuDbu")).Text;
        adressValue.Should().Be("Дворцовая пл., 2, Санкт-Петербург");
    }
    
    [Test]
    public void MakeFolder() // тест на создание папки
    {
        // без явного ожидания иногда тест падает из-за того, что не успевает перейти на страницу файлов
        Thread.Sleep(1000);
        
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files"); // переход на страницу "Файлы"
     
        var buttonMake = driver.FindElement(By.CssSelector("[class='react-ui-1mhlayz']"));
        buttonMake.Click(); // нажать на "Добавить"
     
        var folder = driver.FindElement(By.CssSelector("[class='sc-iGkqmO ckwDLe react-ui-yqskr3']"));
        folder.Click(); // нажать на "Папку"
     
        var folderInputName = driver.FindElement(By.CssSelector("[data-tid='Input']"));
        folderInputName.SendKeys("For_autotests"); // ввод названия папки
     
        var buttonSave = driver.FindElement(By.CssSelector("[data-tid='SaveButton']"));
        buttonSave.Click(); // нажать на "Сохранить"
     
        driver.FindElement(By.XPath("//*[contains(text(), 'For_autotest')]")); // проверка, что созданная папка есть в списке
    }
    
    [Test]
    public void SearchEmployee() // тест на поиск сотрудника и переход на страницу его профиля
    {
        var searchBar = driver.FindElements(By.CssSelector("[data-tid='SearchBar']")).First(element => element.Displayed);
        searchBar.Click(); // нажать по панель поиска
     
        var inputName = driver.FindElement(By.XPath("//*[@id='root']/div/header/div/div[2]/div/span/label/span[2]/input"));
        inputName.SendKeys("Тюльпанова Анна Сергеевна"); // ввод имени сотрудника
     
        // без явного ожидания иногда тест падает из-за того, что не успевает кликнуть по всплывающему полю
        Thread.Sleep(2000);
        
        var chooseEmployee = driver.FindElement(By.CssSelector("[data-tid='Item']"));
        chooseEmployee.Click(); // нажать на всплывающее поле с именем сотрудника
     
        // проверка, что находимся на странице профиля сотруднка
        driver.Url.Should().Be("https://staff-testing.testkontur.ru/profile/0ec5f832-f306-467f-a0d5-1d142b79fad9");
        
        // проверка, что имя сотрудника совпадает с тем, что ввели
        var adressValue = driver.FindElement(By.CssSelector("[data-tid='EmployeeName']")).Text;
        adressValue.Should().Be("Тюльпанова Анна Сергеевна");
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