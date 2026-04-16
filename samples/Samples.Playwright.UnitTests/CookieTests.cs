using System;
using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class CookieTests
{
    [Fact]
    public async Task Cookie_set_get_delete_cycle()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/")
            // Set a cookie
            from _2 in setCookie(new BrowserCookie(
                "testcookie", "hello", ".herokuapp.com", "/", null, false, false, "Lax"))
            // Verify the cookie exists
            from cookies1 in getCookies
            let found1 = cookies1.Find(c => c.Name == "testcookie")
            from _3 in assert(found1.IsSome, "Expected to find 'testcookie' in cookies")
            from _4 in assert(found1.Map(c => c.Value == "hello").IfNone(false), "Expected cookie value 'hello'")
            // Delete the cookie
            from _5 in deleteCookie("testcookie")
            from cookies2 in getCookies
            let found2 = cookies2.Find(c => c.Name == "testcookie")
            from _6 in assert(found2.IsNone, "Expected 'testcookie' to be deleted")
            // Set two cookies then delete all
            from _7 in setCookie(new BrowserCookie(
                "cookie_a", "val_a", ".herokuapp.com", "/", null, false, false, "Lax"))
            from _8 in setCookie(new BrowserCookie(
                "cookie_b", "val_b", ".herokuapp.com", "/", null, false, false, "Lax"))
            from cookies3 in getCookies
            let countBefore = cookies3.Filter(c => c.Name == "cookie_a" || c.Name == "cookie_b").Count
            from _9 in assert(countBefore == 2, $"Expected 2 custom cookies, got {countBefore}")
            from _10 in deleteAllCookies
            from cookies4 in getCookies
            from _11 in assert(cookies4.IsEmpty, $"Expected no cookies after deleteAll, got {cookies4.Count}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }
}
