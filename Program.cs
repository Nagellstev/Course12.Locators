using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using System.Threading;
using System;
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;

namespace MultiThreading
{
    class BrowserStackOptions : DriverOptions
    {
        public BrowserStackOptions(String browser_name, String browser_version)
        {
            this.BrowserName = browser_name;
            this.BrowserVersion = browser_version;
        }
        [Obsolete]
        public /*override*/ void AddAdditionalCapability(string capabilityName, object capabilityValue)
        {
            this.AddAdditionalOption(capabilityName, capabilityValue);
        }
        public override ICapabilities ToCapabilities()
        {
            IWritableCapabilities capabilities = this.GenerateDesiredCapabilities(true);
            return capabilities.AsReadOnly();
        }
    }
    class MultiThreading
    {
        static void Main(string[] args)
        {
            Dictionary<string, object> cap1 = new Dictionary<string, object>();
            cap1.Add("browserName", "Chrome");
            cap1.Add("browserVersion", "103.0");
            cap1.Add("os", "Windows");
            cap1.Add("osVersion", "11");
            Dictionary<string, object> cap2 = new Dictionary<string, object>();
            cap2.Add("browserName", "Firefox");
            cap2.Add("browserVersion", "102.0");
            cap2.Add("os", "Windows");
            cap2.Add("osVersion", "10");
            Dictionary<string, object> cap3 = new Dictionary<string, object>();
            cap3.Add("browserName", "Safari");
            cap3.Add("browserVersion", "14.1");
            cap3.Add("os", "OS X");
            cap3.Add("osVersion", "Big Sur");
            //Creating Threads and defining the browser and OS combinations where the test will run
            Thread t1 = new Thread(obj => sampleTestCase(cap1));
            Thread t2 = new Thread(obj => sampleTestCase(cap2));
            Thread t3 = new Thread(obj => sampleTestCase(cap3));
            //Executing the methods
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();
        }
        static void sampleTestCase(Dictionary<string, object> cap)
        {
            // Update your credentials
            String BROWSERSTACK_USERNAME = "paveld_qt6d1B";
            String BROWSERSTACK_ACCESS_KEY = "69acyaHRpo5HJzjNKpKp";
            String BUILD_NAME = "BbcSportExplore";
            cap.Add("userName", BROWSERSTACK_USERNAME);
            cap.Add("accessKey", BROWSERSTACK_ACCESS_KEY);
            cap.Add("buildName", BUILD_NAME);
            String browserVersion = cap.ContainsKey("browserVersion") == true ? (string)cap["browserVersion"] : "";
            var browserstackOptions = new BrowserStackOptions((string)cap["browserName"], browserVersion);
            browserstackOptions.AddAdditionalOption("bstack:options", cap);
            //FootballLinkClickTest(browserstackOptions);
            //SearchTest(browserstackOptions);
            IconTest(browserstackOptions);
        }

        //executeTestWithCaps function takes capabilities from 'sampleTestCase' function and executes the test
        static void executeTestWithCaps(DriverOptions capability)
        {
            IWebDriver driver = new RemoteWebDriver(new Uri("https://hub.browserstack.com/wd/hub/"), capability);
            try
            {
                driver.Navigate().GoToUrl("https://www.duckduckgo.com");
                IWebElement query = driver.FindElement(By.Name("q"));
                query.SendKeys("BrowserStack");
                query.Submit();
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(localDriver => localDriver.Title.ToLower().Contains("browserstack"));
                // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Title matched!\"}}");
            }
            catch (WebDriverTimeoutException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" Title not matched \"}}");
            }
            Console.WriteLine(driver.Title);
            driver.Quit();
        }

        //FootballLinkClickTest function takes capabilities from 'sampleTestCase' function and executes the test
        static void FootballLinkClickTest(DriverOptions capability)
        {
            IWebDriver driver = new RemoteWebDriver(new Uri("https://hub.browserstack.com/wd/hub/"), capability);

            try
            {
                driver.Navigate().GoToUrl("https://www.bbc.com/sport");

                IWebElement query = driver.FindElement(By.LinkText("Football"));

                new Actions(driver).MoveToElement(query).Click().Perform();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(localDriver => localDriver.Title.ToLower().Contains("football"));
                // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Title matched!\"}}");
            }
            catch (WebDriverTimeoutException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" Title not matched \"}}");
            }

            Console.WriteLine(driver.Title);
            driver.Quit();
        }

        //SearchTest function takes capabilities from 'sampleTestCase' function and executes the test
        static void SearchTest(DriverOptions capability)
        {
            IWebDriver driver = new RemoteWebDriver(new Uri("https://hub.browserstack.com/wd/hub/"), capability);

            try
            {
                driver.Navigate().GoToUrl("https://www.bbc.com/sport");

                IWebElement query = driver.FindElement(By.PartialLinkText("Search"));

                new Actions(driver).MoveToElement(query).Click().Perform();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                wait.Until(localDriver => localDriver.Title.ToLower().Contains("search"));

                query = driver.FindElement(By.Name("q"));
                query.SendKeys("shit");
                query.Submit();
                wait.Until(localDriver => localDriver.Title.ToLower().Contains("shit"));
                // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Title matched!\"}}");

                // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Title matched!\"}}");
            }
            catch (WebDriverTimeoutException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" Title not matched \"}}");
            }

            Console.WriteLine(driver.Title);
            driver.Quit();
        }

        //IconTest function takes capabilities from 'sampleTestCase' function and executes the test
        static void IconTest(DriverOptions capability)
        {
            IWebDriver driver = new RemoteWebDriver(new Uri("https://hub.browserstack.com/wd/hub/"), capability);

            try
            {
                driver.Navigate().GoToUrl("https://www.bbc.com/sport");
                IWebElement query = driver.FindElement(By.Id("homepage-link"));

                new Actions(driver).MoveToElement(query).Click().Perform();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                wait.Until(localDriver => localDriver.Title.ToLower().Contains("bbc"));

                query = driver.FindElement(By.Name("q"));
                query.SendKeys("shit");
                query.Submit();
                wait.Until(localDriver => localDriver.Title.ToLower().Contains("shit"));
                // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Title matched!\"}}");

                // Setting the status of test as 'passed' or 'failed' based on the condition; if title of the web page starts with 'BrowserStack'
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \" Title matched!\"}}");
            }
            catch (WebDriverTimeoutException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" Title not matched \"}}");
            }

            Console.WriteLine(driver.Title);
            driver.Quit();
        }
    }
}