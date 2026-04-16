using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class PlaywrightFeatureTests
{
    [Fact]
    public async Task OnDialog_captures_alert_message()
    {
        var dataUrl = "data:text/html,<button id='btn' onclick=\"alert('hello!')\">Click</button>";

        var test =
            from _1 in nav(dataUrl)
            from msg in onDialog(click(css("#btn")))
            from _2 in assert(msg == "hello!", $"Expected dialog message 'hello!', got '{msg}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task WithConsoleCapture_captures_console_log()
    {
        var dataUrl = "data:text/html,<html><body>console test</body><script>console.log('test message')</script></html>";

        var test =
            from result in withConsoleCapture(nav(dataUrl))
            let logs = result.Logs
            from _1 in assert(logs.Exists(l => l.Message.Contains("test message")),
                $"Expected console logs to contain 'test message', got: {string.Join(", ", logs.Map(l => l.Message))}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Route_intercepts_network_request()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/")
            from _2 in route("**/api/data", async r =>
                await r.FulfillAsync(new RouteFulfillOptions { Body = "mocked" }))
            from result in eval<string>("fetch('/api/data').then(r => r.text())")
            from _3 in assert(result == "mocked", $"Expected 'mocked', got '{result}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task WithNewContext_isolates_cookies()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/")
            from _2 in setCookie(new BrowserCookie(
                "isolation_test", "original", ".herokuapp.com", "/", null, false, false, "Lax"))
            from cookies1 in getCookies
            from _3 in assert(cookies1.Exists(c => c.Name == "isolation_test"),
                "Expected cookie to exist in original context")
            from isolatedCookies in withNewBrowserContext(getCookies)
            from _4 in assert(!isolatedCookies.Exists(c => c.Name == "isolation_test"),
                "Expected new context to have no 'isolation_test' cookie")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task InFrame_accesses_iframe_content()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/iframe")
            from t in inFrame(css("#mce_0_ifr"), text(css("#tinymce")))
            from _2 in assert(!string.IsNullOrWhiteSpace(t), $"Expected non-empty text from iframe, got '{t}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task WaitUntil_retries_until_condition()
    {
        var dataUrl = "data:text/html,<p id='counter'>0</p><script>let c=0;setInterval(()=>{c++;document.getElementById('counter').textContent=c},100)</script>";

        var test =
            from _1 in nav(dataUrl)
            from t in waitUntil(text(css("#counter")), t => int.Parse(t) >= 3)
            from _2 in assert(int.Parse(t) >= 3, $"Expected counter >= 3, got '{t}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task SetGeolocation_and_grantPermissions()
    {
        var geoTest =
            from _1 in nav("https://the-internet.herokuapp.com/")
            from lat in eval<double>("new Promise(r => navigator.geolocation.getCurrentPosition(p => r(p.coords.latitude)))")
            from _2 in assert(lat > 51.0 && lat < 52.0, $"Expected latitude ~51.5, got {lat}")
            select unit;

        var test = withNewBrowserContext(geoTest, new BrowserNewContextOptions
        {
            Geolocation = new Geolocation { Latitude = 51.5074f, Longitude = -0.1278f },
            Permissions = new[] { "geolocation" }
        });

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task OnDialogWithResponse_handles_prompt()
    {
        var dataUrl = "data:text/html,<button id='btn' onclick=\"document.title=prompt('Name?','')\">Ask</button>";

        var test =
            from _1 in nav(dataUrl)
            from msg in onDialogWithResponse(click(css("#btn")), "Alice")
            from t in title
            from _2 in assert(t == "Alice", $"Expected title 'Alice', got '{t}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task WithTrace_creates_trace_file()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"isotope_trace_{Guid.NewGuid()}.zip");
        try
        {
            var inner = nav("https://the-internet.herokuapp.com/");
            var test = withTrace(tempPath, inner);

            await withChromium(test).RunAndThrowOnError();

            Assert.True(File.Exists(tempPath), $"Expected trace file to exist at {tempPath}");
            Assert.True(new FileInfo(tempPath).Length > 0, "Expected trace file to be non-empty");
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

    [Fact]
    public async Task WithTraceOnFailure_saves_trace_only_on_failure()
    {
        var failDir = Path.Combine(Path.GetTempPath(), $"isotope_trace_fail_{Guid.NewGuid()}");
        var successDir = Path.Combine(Path.GetTempPath(), $"isotope_trace_success_{Guid.NewGuid()}");

        try
        {
            Directory.CreateDirectory(failDir);
            Directory.CreateDirectory(successDir);

            // Failure case: trace should be saved
            IsotopeAsync<Unit> failIso = fail<Unit>("intentional");
            var failTest = withTraceOnFailure(failDir, failIso);
            var (failState, _) = await withChromium(failTest).Run();
            Assert.True(failState.IsFaulted, "Expected failure test to be faulted");

            var failFiles = Directory.GetFiles(failDir, "*.zip");
            Assert.True(failFiles.Length > 0, $"Expected trace file in {failDir} on failure");
            Assert.True(new FileInfo(failFiles[0]).Length > 0, "Expected trace file to be non-empty");

            // Success case: no trace should be saved
            var successTest = withTraceOnFailure(successDir, nav("https://the-internet.herokuapp.com/"));
            await withChromium(successTest).RunAndThrowOnError();

            var successFiles = Directory.GetFiles(successDir, "*.zip");
            Assert.True(successFiles.Length == 0, $"Expected no trace file in {successDir} on success, but found {successFiles.Length}");
        }
        finally
        {
            if (Directory.Exists(failDir))
                Directory.Delete(failDir, true);
            if (Directory.Exists(successDir))
                Directory.Delete(successDir, true);
        }
    }

    [Fact]
    public async Task WaitForResponse_captures_response()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/")
            from resp in waitForResponse("**/login", nav("https://the-internet.herokuapp.com/login"))
            from _2 in assert(resp != null, "Expected response to not be null")
            from _3 in assert(resp.Status >= 200 && resp.Status < 400, $"Expected success status code, got {resp.Status}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }
}
