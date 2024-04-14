//using NUnit.Framework;

using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace SeleniumUnitTest_Lyakhov_Dmitry;
        
        //[TestFixture]
        public class SeleniumTestFor
        {
            [Test]
            public void Authorization()
            {
                var options = new ChromeOptions();
                options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
                // зайти в хром (с помощью вебдрайвера)
                var driver = new ChromeDriver(options);
                
                // перейти по URL
                driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
                Thread.Sleep(3000);
                
                // ввести логин/пароль
                var login = driver.FindElement(By.Id("Username"));
                login.SendKeys("ldr@skbkontur.ru");

                var password = driver.FindElement(By.Name("Password"));
                password.SendKeys("2109721,Fhcbr7");
                
                Thread.Sleep(3000);
                
                // нажать на кнопку войти
                var enter = driver.FindElement(By.Name("button"));
                enter.Click();
                
                Thread.Sleep(3000);
                
                // проверяем что на нужном URL
                var currentUrl = driver.Url;
                Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news");
                
                // хотим закрыть браузер после проверки и убиваем процесс драйвера
                driver.Quit();
            }
        }