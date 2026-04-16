using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class JavaScriptTests
{
    [Fact]
    public async Task Eval_returns_value()
    {
        var test =
            from _1 in nav("data:text/html,<html><head><title>TestPage</title></head><body></body></html>")
            from sum in eval<int>("1 + 2")
            from _2 in assert(sum == 3, $"Expected 3, got {sum}")
            from t in eval<string>("document.title")
            from _3 in assert(!string.IsNullOrEmpty(t), "Expected non-empty document title")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Eval_on_element()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            from tag in eval<string>(css("#username"), "el => el.tagName.toLowerCase()")
            from _2 in assert(tag == "input", $"Expected 'input', got '{tag}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task EvalSafe_catches_error()
    {
        var test =
            from _1 in nav("data:text/html,<html><body></body></html>")
            from r in evalSafe<int>("throw new Error('test')")
                      | pure(0)
            from _2 in assert(r == 0, $"Expected fallback value 0, got {r}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Eval_void_runs_script()
    {
        var dataUrl = "data:text/html,<p id='t'>before</p>";

        var test =
            from _1 in nav(dataUrl)
            from _2 in eval("document.getElementById('t').textContent = 'after'")
            from t in text(css("#t"))
            from _3 in assert(t == "after", $"Expected 'after', got '{t}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }
}
