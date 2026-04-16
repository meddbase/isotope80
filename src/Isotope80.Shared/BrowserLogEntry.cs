using System;

namespace Isotope80
{
    /// <summary>
    /// Immutable browser log entry representation
    /// </summary>
    /// <param name="Message">Log message</param>
    /// <param name="Level">Log level (e.g. "Info", "Warning", "Severe")</param>
    /// <param name="Timestamp">When the log entry was recorded</param>
    public partial record BrowserLogEntry(
        string Message,
        string Level,
        DateTime Timestamp);
}
