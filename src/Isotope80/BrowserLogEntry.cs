using System;

namespace Isotope80
{
    /// <summary>
    /// Isotope-owned browser log entry, decoupled from Selenium's LogEntry type
    /// </summary>
    /// <param name="Message">Log message</param>
    /// <param name="Level">Log level (e.g. "Info", "Warning", "Severe")</param>
    /// <param name="Timestamp">When the log entry was recorded</param>
    public record BrowserLogEntry(
        string Message,
        string Level,
        DateTime Timestamp)
    {
        internal static BrowserLogEntry FromSelenium(OpenQA.Selenium.LogEntry entry) =>
            new BrowserLogEntry(
                entry.Message,
                entry.Level.ToString(),
                entry.Timestamp);
    }
}
