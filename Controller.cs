using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

class Controller 
{
    private EdgeOptions options {get; set;}
    private EdgeDriver driver {get; set;}
    private WebDriverWait wait {get; set;}
    private Data data {get; set;}
    public Controller(EdgeOptions options, EdgeDriver driver, WebDriverWait wait, Data data)
    {
        this.options = options;
        this.driver = driver;
        this.wait = wait;
        this.data = data;
    }

    public void Login()
    {
        driver.Navigate().GoToUrl("https://www.ynotlms.com/");
        IWebElement usernameField = wait.Until(driver => driver.FindElement(By.Id("mat-input-0")));
        IWebElement passwordField = wait.Until(driver => driver.FindElement(By.Id("mat-input-1")));
        usernameField.SendKeys(data.Username);
        passwordField.SendKeys(data.Password);

        IWebElement loginButton = wait.Until(driver => driver.FindElement(By.CssSelector(".btn-block.btn-lg.m-t-20.m-b-20.mat-raised-button.mat-primary")));
        loginButton.Click();

        driver.Navigate().GoToUrl("https://www.ynotlms.com/leads?bucket_type_id=3&amp;");
    }
    public void Filter()
    {
        // Filter Drop Down
        IWebElement ClickFilterDropDown = wait.Until(driver => driver.FindElements(By.ClassName("expand"))[2]);
        ClickFilterDropDown.Click();

        // Location
        IWebElement FilterByLocation = wait.Until(driver => driver.FindElement(By.Id("s2id_autogen3")));
        foreach (string item in data.Location)
        {
            FilterByLocation.SendKeys(item);
            FilterByLocation.SendKeys(Keys.Enter);
        }

        // Program Input 
        IWebElement SearchByProgramInput = wait.Until(driver => driver.FindElement(By.Id("s2id_autogen4")));
        foreach (string item in data.Programs)
        {
            SearchByProgramInput.SendKeys(item);
            SearchByProgramInput.SendKeys(Keys.Enter);
        }

        // Priority to All Buckets
        IWebElement? Bucket = driver.FindElement(By.XPath("//*[text()='Priority']"));
        Bucket?.Click();
        IWebElement? Bucket_DropDown = driver.FindElement(By.XPath("//*[text()='All Buckets']"));
        Bucket_DropDown?.Click();

        // Set NoOpenTasks
        IWebElement NoOpenTasks = wait.Until(driver => driver.FindElement(By.Id("s2id_autogen15")));      
        NoOpenTasks.SendKeys("No");
        NoOpenTasks.SendKeys(Keys.Enter);

        // Filter Lead Status
        IWebElement FilterByLeadStatus = wait.Until(driver => driver.FindElement(By.Id("s2id_autogen11")));      
        foreach (string item in data.LeadStatus)
        {
            FilterByLeadStatus.SendKeys(item);
            FilterByLeadStatus.SendKeys(Keys.Enter);
        }
        // DateRange 
        IWebElement DateRange = wait.Until(driver => driver.FindElement(By.Id("form-date-range")));
        DateRange.SendKeys(data.DateRange);
        DateRange.SendKeys(Keys.Enter);
    }
}