using System;
using System.IO;

namespace Isotope80
{
    /// <summary>
    /// Isotope-owned screenshot representation, decoupled from Selenium's Screenshot type
    /// </summary>
    /// <param name="Data">Raw screenshot bytes (PNG)</param>
    public record BrowserScreenshot(byte[] Data)
    {
        /// <summary>
        /// Returns the screenshot as a Base64-encoded string
        /// </summary>
        public string AsBase64String => Convert.ToBase64String(Data);

        /// <summary>
        /// Saves the screenshot to a file
        /// </summary>
        /// <param name="path">File path to write</param>
        public void SaveToFile(string path) => File.WriteAllBytes(path, Data);

        internal static BrowserScreenshot FromSelenium(OpenQA.Selenium.Screenshot screenshot) =>
            new BrowserScreenshot(screenshot.AsByteArray);
    }
}
