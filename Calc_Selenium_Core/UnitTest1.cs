using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Calc_Selenium_Core
{
    public enum Operation
    {
        Sum,
        Divide,
        Multiply,
        Substract
    }

    public class Tests : IDisposable
    {
        private IWebDriver _driver;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _driver = new ChromeDriver();
            _driver.Url = @"C:\Users\Sviatoslav_Samoilenk\Desktop\Calculator\index.html";
        }

        [SetUp]
        public void Refresh()
        {
            _driver.Navigate().Refresh();
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            _driver.Close();
        }

        [TestCase(1, 2, Operation.Sum, 3)]
        [TestCase(9, 8, Operation.Sum, 17)]
        [TestCase(4, 6, Operation.Sum, 10)]
        [TestCase(3, 5, Operation.Sum, 8)]
        [TestCase(0, 7, Operation.Sum, 7)]
        [TestCase(0, 7, Operation.Substract, -7)]
        [TestCase(9, 0, Operation.Substract, 9)]
        [TestCase(0, 0, Operation.Substract, 0)]
        [TestCase(0, 0, Operation.Multiply, 0)]
        [TestCase(0, 1, Operation.Multiply, 0)]
        [TestCase(2, 5, Operation.Multiply, 10)]
        [TestCase(0, 1, Operation.Divide, 0)]
        [TestCase(1, 1, Operation.Divide, 1)]
        [TestCase(6, 2, Operation.Divide, 3)]
        [TestCase(6, 4, Operation.Divide, 1.5)]
        public void Calc_WhenSingleKeyClicked_ShouldPerformAction(
            int a, int b, Operation operation, double expectedResult)
        {
            IWebElement element = _driver.FindElement(
                By.Id("btn"+a));
            element.Click();
            element = _driver.FindElement(
               By.Id(Map(operation)));
            element.Click();
            element = _driver.FindElement(
                By.Id("btn"+b));
            element.Click();
            
            element = _driver.FindElement(
                By.Id("btn_calc"));
            element.Click();
            
            element = _driver.FindElement(
               By.Id("calc-input"));
            var actualResult = Convert.ToDouble(element.GetAttribute("value"));
            
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(123, 345, Operation.Sum, 468)]
        [TestCase(345, 123, Operation.Substract, 222)]
        [TestCase(111, 333, Operation.Multiply, 36963)]
        [TestCase(333, 111, Operation.Divide, 3)]
        public void Calc_WhenManyKeysClicked_ShouldPerformAction(
            int a, int b, Operation operation, double expectedResult)
        {
            ClickNumbersButton(a);
            
            var element = _driver.FindElement(
               By.Id(Map(operation)));
            element.Click();
            
            ClickNumbersButton(b);

            element = _driver.FindElement(
                By.Id("btn_calc"));
            element.Click();

            element = _driver.FindElement(
               By.Id("calc-input"));
            var actualResult = Convert.ToDouble(element.GetAttribute("value"));

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Divide_WhenDivideByZero_ShouldGetTextMessage()
        {
            ClickNumbersButton(123);

            var element = _driver.FindElement(
               By.Id(Map(Operation.Divide)));
            element.Click();

            ClickNumbersButton(0);

            element = _driver.FindElement(
                By.Id("btn_calc"));
            element.Click();
            
            element = _driver.FindElement(
               By.Id("calc-input"));
            var actualResult = element.GetAttribute("value");

            Assert.AreEqual("Forbidden", actualResult);
        }
        
        private void ClickNumbersButton(int a)
        {
            foreach (var item in a.ToString())
            {
                _driver.FindElement(
                    By.Id("btn" + item)).Click();
            }
        }

        public void Dispose()
        {
            _driver.Dispose();
        }

        private string Map(Operation operation)
        {
            switch (operation)
            {
                case Operation.Sum:
                    return "btn_plus";
                case Operation.Substract:
                    return "btn_minus";
                case Operation.Multiply:
                    return "btn_multiply";
                case Operation.Divide:
                    return "btn_divide";
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }
    }
}