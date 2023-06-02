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

        Thread.Sleep(1000);

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
    public void Text()
    {
        Thread.Sleep(1000);
        IWebElement CaretDropDown = driver.FindElement(By.CssSelector(".btn.btn-xs.btn-inverse.dropdown-toggle"));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", CaretDropDown);
        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, -100);");
        CaretDropDown.Click();

        Thread.Sleep(1000);
        IWebElement TextButton = wait.Until(driver => driver.FindElements(By.ClassName("text-number"))[0]);
        TextButton.Click();

        Thread.Sleep(1000);
        IWebElement TextArea = wait.Until(driver => driver.FindElement(By.Id("sms_communication_text"))); 
        TextArea.SendKeys(data.Content);
    }
    public void SelectNextLead()
    {
        // currently selected lead
        IWebElement initialElement = driver.FindElement(By.CssSelector(".list-group-item.lead-item.ng-scope.lead-detail-open"));
        IWebElement nextChildElement = initialElement.FindElement(By.XPath("following-sibling::*[1]"));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", nextChildElement);
        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, -100);");
        nextChildElement.Click();
    }
    public bool CheckOptOut()
    {
    Thread.Sleep(1000);
    Console.WriteLine("OptOut");
    IWebElement tableElement = driver.FindElement(By.CssSelector(".table.table-striped.table-bordered.table-hover"));

    // Find all elements under the table
    var elements = tableElement.FindElements(By.CssSelector("*"));

    // Iterate over the found elements and check if any of them contain the word "Opt-out"
    bool containsOptOut = false;
    foreach (IWebElement element in elements)
        {
            if (element.Text.Trim().Equals("Opt-out"))
            {
                containsOptOut = true;
                break;
            }
        }


    return containsOptOut;
    }
    public void Repeater()
    {
        int i = 0;
        try {
            while (i < 100)
            {
                Console.WriteLine("Loop");
                if(CheckOptOut())
                {
                    SelectNextLead();
                    i++;
                    continue;
                }
                Thread.Sleep(1000);
                Text();
                Thread.Sleep(1000);
                SelectNextLead();
                i+=1;
            }            
        }
        catch (StaleElementReferenceException)
        {
            Repeater();
        }
        // Grab Lead count and put into function as Int128| Replace placeholder i | subtract i and count down lead count

    }
}
