namespace Isotope80
{
    public partial record BrowserLogEntry
    {
        internal static BrowserLogEntry FromSelenium(OpenQA.Selenium.LogEntry entry) =>
            new BrowserLogEntry(
                entry.Message,
                entry.Level.ToString(),
                entry.Timestamp);
    }
}
