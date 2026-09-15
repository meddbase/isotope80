using System;
using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class NavigationTimeoutTests
{
    const string SlowUrl = "https://isotope80.test/slow";

    /// <summary>
    /// Intercepts the slow URL and only responds after the given delay, so the navigation
    /// takes a known amount of time regardless of network conditions.
    /// </summary>
    static IsotopeAsync<Unit> slowRoute(TimeSpan responseDelay) =>
        route("**/slow", async r =>
        {
            await Task.Delay(responseDelay);
            await r.FulfillAsync(new RouteFulfillOptions
                                 {
                                     ContentType = "text/html",
                                     Body = "<html><body><h1 id='slow'>slow page</h1></body></html>"
                                 });
        });

    [Fact]
    public async Task Nav_with_timeout_navigates_to_page()
    {
        var test =
            from _1 in nav("data:text/html,<html><body><h1>timeout nav</h1></body></html>", TimeSpan.FromSeconds(10))
            from src in pageSource
            from _2 in assert(src.Contains("timeout nav"), $"Expected page source to contain 'timeout nav', got '{src}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Nav_with_timeout_fails_when_page_loads_slower_than_timeout()
    {
        var test =
            from _1 in slowRoute(TimeSpan.FromSeconds(5))
            from _2 in nav(SlowUrl, TimeSpan.FromMilliseconds(500))
            select unit;

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => withChromium(test).RunAndThrowOnError().AsTask());
        Assert.Contains("Timeout 500ms exceeded", ex.Message);
    }

    [Fact]
    public async Task Nav_with_timeout_allows_navigation_longer_than_default_wait()
    {
        var settings = IsotopeSettings.Create(wait: TimeSpan.FromSeconds(1));

        var test =
            from _1 in slowRoute(TimeSpan.FromSeconds(2))
            from _2 in nav(SlowUrl, TimeSpan.FromSeconds(10))
            from heading in text(css("#slow"))
            from _3 in assert(heading == "slow page", $"Expected heading 'slow page', got '{heading}'")
            select unit;

        await withChromium(test).RunAndThrowOnError(settings);
    }

    [Fact]
    public async Task Nav_without_timeout_uses_default_wait()
    {
        var settings = IsotopeSettings.Create(wait: TimeSpan.FromSeconds(1));

        var test =
            from _1 in slowRoute(TimeSpan.FromSeconds(5))
            from _2 in nav(SlowUrl)
            select unit;

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => withChromium(test).RunAndThrowOnError(settings).AsTask());
        Assert.Contains("Timeout 1000ms exceeded", ex.Message);
    }
}
