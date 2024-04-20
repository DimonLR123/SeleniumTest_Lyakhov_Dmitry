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
            
            [Test]
            public void Auth()
            {
                Authorization();
                
                Thread.Sleep(3000);
                
                // проверяем что на нужном URL
                var currentUrl = driver.Url;
                Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news");
                
            }

            [Test]
            public void Create_folder()
            {
                Authorization();
                Thread.Sleep(3000);
                
                driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files");
                Thread.Sleep(2000);
                var add = driver.FindElement(By.ClassName("react-ui-1f3jmd3"));
                add.Click();
                Thread.Sleep(1000);
                 
                var folder = driver.FindElement(By.CssSelector("[data-tid='CreateFolder']"));
                folder.Click();
                Thread.Sleep(1000);
                
                var namefolder = driver.FindElement(By.CssSelector("[placeholder='Новая папка']"));
                namefolder.SendKeys("New");
                Thread.Sleep(1000);
                
                var savefolder = driver.FindElement(By.CssSelector("[data-tid='SaveButton']"));
                savefolder.Click();
                Thread.Sleep(1000);
                
                var check = driver.FindElement(By.CssSelector("[data-tid='modal-content']"));
                if (check == null) Assert.That(true);
                //проверка на отстуствие модалки создания папки (знаю, странная проверка, надо бы на наличие созданной папки, но пока не понял как это делать)

            }
            
            [Test]
            public void Select_date()
            {
                Authorization();
                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                
                Thread.Sleep(2000); 
                driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
                
                Thread.Sleep(2000); 
                var datafield = driver.FindElement(By.CssSelector("[inputmode='numeric']"));
                datafield.Click();
                
                Thread.Sleep(2000); 
                
                // проверяем что календарь открывается
                var dataopen = driver.FindElement(By.CssSelector("[data-tid='MonthView__month']"));
                if (dataopen != null) Assert.That(true);
                //возможно, как-то по другому проверку на наличие элемента делать надо, но вроде работает
                
                
                
                
            }
            
            [Test]
            public void NewYearThemeOn()
            {
                Authorization();
                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                
                Thread.Sleep(2000);                 
                var avatar = driver.FindElement(By.CssSelector("[data-tid='Avatar']"));
                avatar.Click();
                
                Thread.Sleep(2000);
                var settings = driver.FindElement(By.CssSelector("[data-tid='Settings']"));
                settings.Click();
                
                Thread.Sleep(2000);
                //нажатие на тогл (приходится браться за class, понимаю что плохой вариант, но другого кликабельного элемента нет)
                var toggle = driver.FindElement(By.CssSelector("[class='react-ui-1jxed06']"));
                toggle.Click();
                
                Thread.Sleep(2000);
                var save = driver.FindElement(By.CssSelector("[class='react-ui-1m5qr6w']"));
                save.Click();
                
                Thread.Sleep(3000);
                
                // проверяем что включено
                var newyeartheme = driver.FindElement(By.CssSelector("[class='sc-dvUynV eIUTfe']"));
                if (newyeartheme != null) Assert.That(true);
                // также вопрос, корректная ли проверка
                
            }
            
            [Test]
            public void ChangePassword()
            
            {
                Authorization();
                //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                
                Thread.Sleep(2000); 
                driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/security");
                
                Thread.Sleep(2000);
                //кнопка изменить пароль
                var buttonpassword = driver.FindElement(By.CssSelector("[data-tid='ResetPassword']"));
                buttonpassword.Click();
                
                Thread.Sleep(2000);
                
                //здесь я пытался объявить элемент, который нужно будет находить (он одинаковый для нескольких полей)
                IWebElement textField = driver.FindElement(By.CssSelector("[type='password']"));
                IWebElement textField1 = driver.FindElement(By.CssSelector("[type='password']"));
                IWebElement textField2 = driver.FindElement(By.CssSelector("[type='password']"));
                
                var oldpassword = driver.FindElement(By.CssSelector("[data-tid='OldPassword']"));
                oldpassword.Click();
                //IWebDriver textfield = driver.FindElement(By.Name("password"));
                Thread.Sleep(1000);
                textField.SendKeys("2109721,Fhcbr7");
                
                Thread.Sleep(2000);

                var newpassword = driver.FindElement(By.CssSelector("[data-tid='NewPassword']"));
                newpassword.Click();
                Thread.Sleep(1000);
                textField1.SendKeys("2109721,Fhcbr8");
                
                Thread.Sleep(2000);
                
                var repeatpassword = driver.FindElement(By.CssSelector("[data-tid='RepeatPassword']"));
                repeatpassword.Click();
                Thread.Sleep(1000);
                textField2.SendKeys("2109721,Fhcbr8");
                // все это записывает почему-то в одно поле со старым паролем

                var checksave = driver.FindElement(By.CssSelector("[class='react-ui-m0adju']"));
                if (checksave != null) Assert.That(true);
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
                
                //пример явного ожидания (ждем 3 сек пока не появится эелемент с id username) - 2 строчки
                //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Username")));
                
                // ввести логин/пароль
                var login = driver.FindElement(By.Id("Username"));
                login.SendKeys("ldr@skbkontur.ru");

                var password = driver.FindElement(By.Name("Password"));
                password.SendKeys("2109721,Fhcbr7");
                
                //Thread.Sleep(3000);
                
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