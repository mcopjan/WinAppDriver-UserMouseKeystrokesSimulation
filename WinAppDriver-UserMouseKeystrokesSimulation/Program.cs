using System.CommandLine;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;

namespace WinAppDriver.UserMouseKeystrokesSimulation
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("Open Notepad and type a random text in a loop using WinAppDriver.");

            var appIdOption = new Option<string>(
                "--appid",
                getDefaultValue: () => @"C:\Windows\System32\notepad.exe",
                description: "The Notepad path.");
            rootCommand.AddOption(appIdOption);

            var appDriverUrlOption = new Option<string>(
                "--appdriverurl",
                getDefaultValue: () => "http://127.0.0.1:4723",
                description: "The AppDriver URL.");
            rootCommand.AddOption(appDriverUrlOption);

            rootCommand.SetHandler(RunAutomationScenario, appIdOption, appDriverUrlOption);

            return await rootCommand.InvokeAsync(args);
        }

        static void RunAutomationScenario(string appId, string appDriverUrl)
        {
            WindowsDriver<WindowsElement> driver = InitiateDriver(appId, appDriverUrl);
            Console.WriteLine("Press <Enter> to exit");
            Random random = new Random();
            int i = 0;

            try
            {
                //CaptureAllElements(driver); 

                while (true)
                {
                    Console.WriteLine($"Session {i}");
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);  // Read the key (doesn't output the key)
                        Console.WriteLine("Exiting....");
                        driver?.Quit();
                        break;
                    }

                    string randomText = GenerateRandomString(50, random);
                    var editArea = driver.FindElementByAccessibilityId("ContentTextBlock");
                    editArea.Click();
                    Actions actions = new Actions(driver);
                    actions.SendKeys(randomText).Perform();
                    driver.CloseApp();
                    ClickOnDontSaveButtonIfPresented(driver);
                    Thread.Sleep(1000);
                    driver.LaunchApp();
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing driver -> {ex.Message}");
            }
            finally
            {
                driver?.Quit();
            }
        }

        private static void ClickOnDontSaveButtonIfPresented(WindowsDriver<WindowsElement> driver)
        {
            try
            {
                //CaptureAllElements(driver);
                var dontSaveButton = driver.FindElementByAccessibilityId("SecondaryButton");
                dontSaveButton.Click();
                Console.WriteLine("Closed without saving.");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("No save prompt.");
            }
        }

        private static void CloseAppAndHandlePrompt(WindowsDriver<WindowsElement> driver)
        {
            try
            {
                driver.CloseApp();
                ClickOnDontSaveButtonIfPresented(driver);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing app: {ex.Message}");
            }
        }


        static string GenerateRandomString(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void CaptureAllElements(WindowsDriver<WindowsElement> driver)
        {
            var elements = driver.FindElementsByXPath("//*");
            foreach (var elem in elements)
            {
                Console.WriteLine($"Name: {elem.Text}, AutomationId: {elem.GetAttribute("AutomationId")}");
            }
        }

        static WindowsDriver<WindowsElement> InitiateDriver(string appId, string appDriverUrl)
        {
            AppiumOptions options = new AppiumOptions();
            options.PlatformName = "Windows";
            options.AddAdditionalCapability("app", appId);

            Console.WriteLine("Initializing driver...");
            return new WindowsDriver<WindowsElement>(new Uri(appDriverUrl), options);

        }
    }
}
