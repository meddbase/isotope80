using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class LifecycleTests
{
    [Fact]
    public async Task WithPage_runs_with_external_page()
    {
        var pw = await Microsoft.Playwright.Playwright.CreateAsync();
        IBrowser br = null;
        IPage pg = null;
        try
        {
            br = await pw.Chromium.LaunchAsync();
            var ctx = await br.NewContextAsync();
            pg = await ctx.NewPageAsync();

            var test =
                from _1 in nav("https://the-internet.herokuapp.com/")
                from t in title
                from _2 in assert(t.Contains("The Internet"), $"Expected title containing 'The Internet', got '{t}'")
                select t;

            var (state, value) = await withPage(pg, test, keepAlive: true).RunAndThrowOnError();
            Assert.Contains("The Internet", value);
        }
        finally
        {
            if (pg != null) await pg.CloseAsync();
            if (br != null) await br.CloseAsync();
            pw?.Dispose();
        }
    }
}
