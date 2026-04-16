namespace Isotope80
{
    public partial record BrowserScreenshot
    {
        internal static BrowserScreenshot FromSelenium(OpenQA.Selenium.Screenshot screenshot) =>
            new BrowserScreenshot(screenshot.AsByteArray);
    }
}
