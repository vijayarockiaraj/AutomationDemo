using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using MortgageAutomation.Pages;

namespace MortgageAutomation.Tests
{
    [TestFixture]
    public class MortgageFormTests
    {
        private IWebDriver? _driver;
        private MortgageFormPage? _mortgageFormPage;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            // Uncomment for headless mode in CI/CD
            // options.AddArgument("--headless=new");

            _driver = new ChromeDriver(options);
            _mortgageFormPage = new MortgageFormPage(_driver);
            _mortgageFormPage.GoToForm();
        }

        [Test]
        public void SubmitValidForm_ShouldSucceed()
        {
            try
            {
                _mortgageFormPage!.FillForm(
                    firstName: "Jane",
                    lastName: "Smith",
                    email: "jane.smith@email.com",
                    phone: "555-123-4567",
                    address: "456 Oak Avenue",
                    propertyType: "Single-family home",
                    propertyValue: "200000",
                    loanAmount: "150000",
                    termYears: "30",
                    annualIncome: "60000"
                );

                _mortgageFormPage.SubmitForm();

                string successMessage = _mortgageFormPage.GetSuccessMessage();

                Assert.That(successMessage, Is.Not.Empty, "Success message not found");
                Assert.That(successMessage.ToLower(), Does.Contain("success"), "Form submission did not succeed");

                // Capture screenshot for passed test
                _mortgageFormPage.CaptureScreenshot("SubmitValidForm_PASS.png");
            }
            catch
            {
                // Capture screenshot for failed test
                _mortgageFormPage!.CaptureScreenshot("SubmitValidForm_FAIL.png");
                throw; // Rethrow to mark test as failed
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}
