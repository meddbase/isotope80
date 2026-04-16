using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class BasicTests
{
    [Fact]
    public async Task Login_flow_succeeds()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            from _2 in fill(css("#username"), "tomsmith")
            from _3 in fill(css("#password"), "SuperSecretPassword!")
            from _4 in click(css("button[type='submit']"))
            from msg in text(css("#flash"))
            from _5 in assert(msg.Contains("You logged into"), "Expected success message")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Login_failure_shows_error_message()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            from _2 in fill(css("#username"), "tomsmith")
            from _3 in fill(css("#password"), "WrongPassword")
            from _4 in click(css("button[type='submit']"))
            from msg in text(css("#flash"))
            from _5 in assert(msg.Contains("Your password is invalid"), "Expected error message")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Checkbox_toggle()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/checkboxes")
            from before in exists(css("input[type='checkbox']:checked"))
            from _2 in click(css("input[type='checkbox']") + atIndex(0))
            from _3 in click(css("input[type='checkbox']") + atIndex(1))
            from after in exists(css("input[type='checkbox']:checked"))
            from _4 in assert(before || after, "At least one checkbox state should exist")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Dropdown_select()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/dropdown")
            from p  in page
            from _2 in isoAsync<Unit>(async () =>
            {
                await p.SelectOptionAsync("#dropdown", "2").ConfigureAwait(false);
                return unit;
            })
            from val in value(css("#dropdown"))
            from _3 in assert(val == "2", $"Expected dropdown value '2', got '{val}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Element_counting()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/checkboxes")
            from count in elementCount(css("input[type='checkbox']"))
            from _2 in assert(count == 2, $"Expected 2 checkboxes, got {count}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Fill_combinator()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            from _2 in fill(css("#username"), "tomsmith")
            from val in value(css("#username"))
            from _3 in assert(val == "tomsmith", $"Expected 'tomsmith', got '{val}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Navigation_back_forward_title_url()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/")
            from t1 in title
            from _2 in assert(t1.Contains("The Internet"), $"Expected title containing 'The Internet', got '{t1}'")
            from u1 in url
            from _3 in assert(u1.Contains("the-internet"), $"Expected url containing 'the-internet', got '{u1}'")
            from _4 in nav("https://the-internet.herokuapp.com/login")
            from t2 in title
            from _5 in assert(t2.Contains("The Internet"), $"Expected title containing 'The Internet', got '{t2}'")
            from _6 in back
            from u2 in url
            from _7 in assert(!u2.Contains("/login"), $"Expected url without '/login' after back, got '{u2}'")
            from _8 in forward
            from u3 in url
            from _9 in assert(u3.Contains("/login"), $"Expected url with '/login' after forward, got '{u3}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Element_displayed_and_exists()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            from ex in exists(css("#username"))
            from _2 in assert(ex, "Expected #username to exist")
            from vis in displayed(css("#username"))
            from _3 in assert(vis, "Expected #username to be displayed")
            from noExist in exists(css("#nonexistent-element"))
            from _4 in assert(!noExist, "Expected #nonexistent-element to not exist")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Refresh_reloads_page()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/")
            from t1 in title
            from _2 in assert(t1.Contains("The Internet"), $"Expected title containing 'The Internet' before refresh, got '{t1}'")
            from _3 in refresh
            from t2 in title
            from _4 in assert(t2.Contains("The Internet"), $"Expected title containing 'The Internet' after refresh, got '{t2}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }
}
