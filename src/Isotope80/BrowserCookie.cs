using System;

namespace Isotope80
{
    /// <summary>
    /// Immutable browser cookie representation, decoupled from Selenium's Cookie type
    /// </summary>
    /// <param name="Name">Cookie name</param>
    /// <param name="Value">Cookie value</param>
    /// <param name="Domain">Cookie domain</param>
    /// <param name="Path">Cookie path</param>
    /// <param name="Expiry">Expiry date, or null if session cookie</param>
    /// <param name="Secure">Whether the cookie is secure</param>
    /// <param name="HttpOnly">Whether the cookie is HTTP only</param>
    /// <param name="SameSite">SameSite attribute value</param>
    public record BrowserCookie(
        string Name,
        string Value,
        string Domain,
        string Path,
        DateTime? Expiry,
        bool Secure,
        bool HttpOnly,
        string SameSite)
    {
        internal static BrowserCookie FromSelenium(OpenQA.Selenium.Cookie c) =>
            new BrowserCookie(
                c.Name,
                c.Value,
                c.Domain,
                c.Path,
                c.Expiry,
                c.Secure,
                c.IsHttpOnly,
                c.SameSite);

        internal OpenQA.Selenium.Cookie ToSelenium() =>
            new OpenQA.Selenium.Cookie(Name, Value, Domain, Path, Expiry, Secure, HttpOnly, SameSite);
    }
}
