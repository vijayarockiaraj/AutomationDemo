using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Threading;

namespace MortgageAutomation.Pages
{
    public class MortgageFormPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public MortgageFormPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // Navigate to form
        public void GoToForm()
        {
            _driver.Navigate().GoToUrl("https://vijayarockiaraj.github.io/mortgage-application-form/");
            _wait.Until(d => d.FindElement(By.Id("stepLabel")));
        }

        // Click button using JS (Next/Back)
        private void ClickButtonById(string id)
        {
            var button = _driver.FindElement(By.Id(id));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", button);
            Thread.Sleep(500);
        }

        // Check current step
        public bool IsOnStep(int stepNumber)
        {
            try
            {
                var stepLabelElement = _driver.FindElement(By.Id("stepLabel"));
                return stepLabelElement.Displayed && stepLabelElement.Text == stepNumber.ToString();
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void GoBackToPreviousStep()
        {
            ClickButtonById("backBtn");
        }

        public void FillStep1(string firstName, string lastName, string email, string phone)
        {
            _wait.Until(d => d.FindElement(By.Id("firstName"))).SendKeys(firstName);
            _driver.FindElement(By.Id("lastName")).SendKeys(lastName);
            _driver.FindElement(By.Id("email")).SendKeys(email);
            _driver.FindElement(By.Id("phone")).SendKeys(phone);
            ClickButtonById("nextBtn");
        }

        public void FillStep2(string address, string propertyType, string propertyValue)
        {
            _wait.Until(d => d.FindElement(By.Id("address"))).SendKeys(address);

            var select = new SelectElement(_driver.FindElement(By.Id("propertyType")));
            select.SelectByValue(propertyType.ToLower().Replace(" ", "-"));

            _driver.FindElement(By.Id("value")).SendKeys(propertyValue);
            ClickButtonById("nextBtn");
        }

        public void FillStep3(string loanAmount, string termYears)
        {
            _wait.Until(d => d.FindElement(By.Id("amount"))).SendKeys(loanAmount);
            _driver.FindElement(By.Id("term")).SendKeys(termYears);
            ClickButtonById("nextBtn");
        }

        public void FillStep4(string annualIncome)
        {
            _wait.Until(d => d.FindElement(By.Id("income"))).SendKeys(annualIncome);
        }

        public void FillForm(string firstName, string lastName, string email, string phone,
                             string address, string propertyType, string propertyValue,
                             string loanAmount, string termYears, string annualIncome)
        {
            FillStep1(firstName, lastName, email, phone);
            FillStep2(address, propertyType, propertyValue);
            FillStep3(loanAmount, termYears);
            FillStep4(annualIncome);
        }

        public void SubmitForm()
        {
            ClickButtonById("nextBtn");
            Thread.Sleep(1000);
        }

        public string GetSuccessMessage()
        {
            try
            {
                IAlert alert = _wait.Until(ExpectedConditions.AlertIsPresent());
                string message = alert.Text!;
                alert.Accept();
                return message;
            }
            catch
            {
                return "No success alert found";
            }
        }

        // ---------------- Capture screenshot ----------------
        public void CaptureScreenshot(string fileName)
	{
	    var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
	    var screenshotsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Screenshots");
	    Directory.CreateDirectory(screenshotsFolder);
	    var filePath = Path.Combine(screenshotsFolder, fileName);

	    // Save screenshot using byte array
	    File.WriteAllBytes(filePath, screenshot.AsByteArray);
	}

    }
}
