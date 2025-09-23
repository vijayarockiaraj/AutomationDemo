using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using MortgageAutomation.Pages;

namespace MortgageAutomation.Tests
{
    public class UnitTest1
    {
        private IWebDriver? _driver;
        private MortgageFormPage? _mortgageFormPage;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            // Uncomment for headless mode:
            // options.AddArgument("--headless");

            _driver = new ChromeDriver(options);
            _mortgageFormPage = new MortgageFormPage(_driver);
            _mortgageFormPage.GoToForm();
        }

        [Test]
        public void SubmitFullForm_ShouldSucceed()
        {
            _mortgageFormPage!.FillForm(
                firstName: "Jane",
                lastName: "Smith",
                email: "jane.smith@email.com",
                phone: "555-123-4567",
                address: "456 Oak Avenue",
                propertyType: "Single-family home", // must match HTML value
                propertyValue: "200000",
                loanAmount: "150000",
                termYears: "30",
                annualIncome: "60000"
            );

            _mortgageFormPage.SubmitForm();

            string successMessage = _mortgageFormPage.GetSuccessMessage();
            Assert.That(successMessage, Is.Not.Empty, "Success message not found");
            Assert.That(successMessage.ToLower(), Does.Contain("success"), "Form submission did not succeed");
        }

        [Test]
        public void MultiStepNavigation_Test()
        {
            // Step 1
            _mortgageFormPage!.FillStep1("John", "Doe", "john.doe@email.com", "555-987-6543");
            Assert.IsTrue(_mortgageFormPage.IsOnStep(2), "Should be on Step 2 after clicking Next");

            // Step 2
            _mortgageFormPage.FillStep2("789 Pine Street", "Condo", "250000");
            Assert.IsTrue(_mortgageFormPage.IsOnStep(3), "Should be on Step 3 after clicking Next");

            // Step 3
            _mortgageFormPage.FillStep3("200000", "15");
            Assert.IsTrue(_mortgageFormPage.IsOnStep(4), "Should be on Step 4 after clicking Next");

            // Go back to Step 3
            _mortgageFormPage.GoBackToPreviousStep();
            Assert.IsTrue(_mortgageFormPage.IsOnStep(3), "Should be back on Step 3 after clicking Back");

            // Go back to Step 2
            _mortgageFormPage.GoBackToPreviousStep();
            Assert.IsTrue(_mortgageFormPage.IsOnStep(2), "Should be back on Step 2 after clicking Back");

            // Go back to Step 1
            _mortgageFormPage.GoBackToPreviousStep();
            Assert.IsTrue(_mortgageFormPage.IsOnStep(1), "Should be back on Step 1 after clicking Back");
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
