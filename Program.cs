using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ynot_texter
{
    class Program 
    {
        static void Main(string[] args)
        {
        EdgeOptions options = new EdgeOptions();
        options.AddArgument("ignore-certificate-errors"); 
        options.AddArgument("--inprivate");
        EdgeDriver driver = new EdgeDriver(options);

        driver.Manage().Window.Size = new System.Drawing.Size(1200, 1200);

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(84000));
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        string json = File.ReadAllText("config.json");
        Data? data = JsonConvert.DeserializeObject<Data>(json);

        Controller c = new Controller(options, driver, wait, data);
        c.Login();
        // c.Filter();
        c.Repeater();
        }
    }
}