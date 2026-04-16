namespace Isotope80
{
    public partial record BrowserCookie
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
