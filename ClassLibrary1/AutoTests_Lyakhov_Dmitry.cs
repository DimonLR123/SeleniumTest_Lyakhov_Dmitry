// прошу проверить тесты на логичность и правильность;
// в тестах будет много thread.sleep - это временно! Потом сделаю оптимальные ожидания) пока не успел
// прошу проверить отдельно тест ChangePassword, он падает из-за того что вводит значения в одно поле. Не понимаю, что не так.

using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutoTest_Lyakhov_Dmitry;

        [TestFixture]
        public class Авторизация
        {
            public ChromeDriver driver;

            [SetUp]
            public void Setup_Auth()
            {
                Authorization();
            }
            
            [Test]
            public void Auth()
            // проверка авторизации
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='PageHeader']")));
                
                var currentUrl = driver.Url;
                Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news"
                    , "мы ожидали получить https://staff-testing.testkontur.ru/news, а получили" + currentUrl);
                
            }

            [Test]
            public void Create_folder()
            // проверка открытия модалки создания папки
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='PageHeader']")));
                
                driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files");
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='DropdownButton']")));
                
                var add = driver.FindElement(By.ClassName("react-ui-1f3jmd3"));
                add.Click();
                
                var folder = driver.FindElement(By.CssSelector("[data-tid='CreateFolder']"));
                folder.Click();
                
                var check = driver.FindElement(By.CssSelector("[data-tid='modal-content']"));
                check.Should().NotBeNull();

            }
            
            [Test]
            public void Select_date()
                // проверяем, что в настройках профиля, в поле выбора даты рождения открывается календарь
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='PageHeader']")));
                driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Birthday']")));
                var datafield = driver.FindElement(By.CssSelector("[inputmode='numeric']"));
                datafield.Click();
                
                var dataopen = driver.FindElement(By.CssSelector("[data-tid='MonthView__month']"));
                dataopen.Should().NotBeNull();
            }
            
            [Test]
            public void NewYearThemeOn()
            // проверка, что новогодняя тема включается (по умолчанию отключена после авторизации)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='PageHeader']")));               
                var avatar = driver.FindElement(By.CssSelector("[data-tid='Avatar']"));
                avatar.Click();
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Settings']"))); 
                var settings = driver.FindElement(By.CssSelector("[data-tid='Settings']"));
                settings.Click();
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='SettingsHotkeys']"))); 
                var toggle = driver.FindElement(By.CssSelector("[class='react-ui-1jxed06']"));
                toggle.Click();
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='SettingsHotkeys']")));
                var save = driver.FindElement(By.CssSelector("[class='react-ui-1m5qr6w']"));
                save.Click();
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='PageHeader']")));
                
                // проверяем что новогодняя тема включена (есть картинка в фоне)
                var newyeartheme = driver.FindElement(By.CssSelector("[class='sc-kizEQm sc-kmIPcE irzWzB htFMXJ']"));
                newyeartheme.Should().NotBeNull();
                
            }
            
            [Test]
            public void ChangePassword()
            // проверка валидации при пустом поле для старого пароля (выходит сообщение "поле не должно быть пустым")
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='PageHeader']")));
                driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/security");
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='ResetPassword']")));
                var buttonpassword = driver.FindElement(By.CssSelector("[data-tid='ResetPassword']"));
                buttonpassword.Click();
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='modal-content']")));
                
                var oldpassword = driver.FindElement(By.CssSelector("[data-tid='OldPassword']"));
                oldpassword.Click();
                var newpassword = driver.FindElement(By.CssSelector("[data-tid='PasswordInputEyeIcon']"));
                newpassword.Click();
                var checksave = driver.FindElement(By.CssSelector("[data-tid='PopupContent']"));
                checksave.Should().NotBeNull();
            }

            public void Authorization()
            {
                var options = new ChromeOptions();
                options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
                // зайти в хром (с помощью вебдрайвера)
                driver = new ChromeDriver(options);
                
                //неявное ожидание
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
                
                // перейти по URL
                driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
                
                // ввести логин/пароль
                var login = driver.FindElement(By.Id("Username"));
                login.SendKeys("ldr@skbkontur.ru");

                var password = driver.FindElement(By.Name("Password"));
                password.SendKeys("2109721,Fhcbr7");
                
                // нажать на кнопку войти
                var enter = driver.FindElement(By.Name("button"));
                enter.Click();
            }
            
            //закрываем браузер и убиваем процесс
            [TearDown]
            public void TearDown()
            {
                driver.Quit();
            }
            

        }