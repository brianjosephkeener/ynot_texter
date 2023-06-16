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
        driver.Navigate().GoToUrl("https://www.ynotlms.com/leads?&format=adrep#");
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
        IWebElement CaretDropDown = null;
        while (true)
        {
            try {
                CaretDropDown = driver.FindElement(By.CssSelector(".btn.btn-xs.btn-inverse.dropdown-toggle"));
                break;
            }
            catch (NoSuchElementException) 
            {
                Thread.Sleep(1000);
            }
            catch (ElementClickInterceptedException)
            {
                Thread.Sleep(1000);
            }
        }

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", CaretDropDown);
        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, -100);");
        CaretDropDown.Click();

        // click the text button
        Thread.Sleep(1000);
        IWebElement TextButton;
        try {
        TextButton = wait.Until(driver => driver.FindElements(By.ClassName("text-number"))[0]);
        TextButton.Click();
        }
        catch (ElementNotInteractableException ex)
        {
            Console.WriteLine(ex);
            return;
        }

        // type in the text area
        Thread.Sleep(1000);
        IWebElement TextArea = wait.Until(driver => driver.FindElement(By.Id("sms_communication_text"))); 
        TextArea.SendKeys(data.Content);

        // grab the correct "Send Text Message" Button 
        Thread.Sleep(1000);
        var sendTextButtonCollection = driver.FindElements(By.CssSelector(".btn.btn-success.btn-sm.pull-right"));
        
        IWebElement sendTextButton = null;
        foreach (var element in sendTextButtonCollection)
        {
            if (element.Text == "Send Text Message")
            {
                sendTextButton = element;
                break;
            }
        }
        Console.WriteLine("Sent text");
        sendTextButton.Click();

        // Click OK pop-up 
        Thread.Sleep(3000);
        var okButtonCollection = driver.FindElements(By.CssSelector(".btn.btn-primary"));
                
                IWebElement okButton = null;
                foreach (var element in okButtonCollection)
                {
                    if (element.Text == "OK")
                    {
                        okButton = element;
                        break;
                    }
                }
                while (true)
                {
                    try {
                        okButton.Click(); // fix this try catch
                        break;
                    }
                    catch (Exception){
                        okButtonCollection = driver.FindElements(By.CssSelector(".btn.btn-primary"));
                        foreach (var element in okButtonCollection)
                        {
                            if (element.Text == "OK")
                            {
                                okButton = element;
                                break;
                            }
                        }
                        Thread.Sleep(1000);
                          }
                }
    }
    public void SelectNextLead()
    {
        // currently selected lead
        IWebElement initialElement;
        IWebElement nextChildElement;
        // Sometimes the Lead detail open (with green highlight) does not go off properly
        while (true)
        {
            try {
                    initialElement = driver.FindElement(By.CssSelector(".list-group-item.lead-item.ng-scope.lead-detail-open"));
                    nextChildElement = initialElement.FindElement(By.XPath("following-sibling::*[1]"));
                    driver.ExecuteScript($"arguments[0].setAttribute('class', '{nextChildElement.GetAttribute("class")}');", initialElement);
                    driver.ExecuteScript("arguments[0].setAttribute('class', 'list-group-item lead-item ng-scope lead-detail-open');", nextChildElement);
                    break;
                }
                catch (NoSuchElementException){

                Thread.Sleep(2000);
                }
        }
        
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", nextChildElement);
        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, -100);");
        nextChildElement.Click();
    }
public bool CheckOptOut()
{
    bool containsOptOut = false;
    bool containsDncFederal = false;

    try
    {
        var elements = driver.FindElements(By.CssSelector("span.label.label-default.ng-binding.ng-scope"));

        foreach (IWebElement element in elements)
        {
            string text = element.Text.Trim();
            if ("Opt-Out" == text)
            {
                containsOptOut = true;
                break;
            }
            else if ("DNC Federal" == text)
            {
                containsDncFederal = true;
                break;
            }
        }
    }
    catch (NoSuchElementException)
    {
        Console.WriteLine("Elements not found.");
    }

    // Print the result
    // Console.WriteLine("Opt-Out: " + containsOptOut);
    // Console.WriteLine("DNC Federal: " + containsDncFederal);

    return containsOptOut || containsDncFederal;
    }    
    public void Repeater()
    {
        int i = 0;
        try {
            while (i < 100000)
            {
                if(CheckOptOut())
                {
                    SelectNextLead();
                    i++;
                    continue;
                }
                Text();
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