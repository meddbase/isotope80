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
            // the editor injects its content after its own scripts run, so wait for it
            // rather than racing it
            from _2 in inFrame(css("#mce_0_ifr"), waitForFunction(css("#tinymce"), "el => el.textContent.trim().length > 0"))
            from t in inFrame(css("#mce_0_ifr"), text(css("#tinymce")))
            from _3 in assert(!string.IsNullOrWhiteSpace(t), $"Expected non-empty text from iframe, got '{t}'")
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

    [Fact]
    public async Task WaitForFunction_resolves_when_the_element_condition_becomes_true()
    {
        var dataUrl = "data:text/html,<div id='grid'><span>a</span><span>b</span></div>"
                    + "<script>setTimeout(() => document.getElementById('grid').innerHTML = '', 300)</script>";

        var test =
            from _1 in nav(dataUrl)
            from _2 in waitForFunction(css("#grid"), "el => el.children.length === 0")
            from n in elementCount(css("#grid span"))
            from _3 in assert(n == 0, $"Expected the grid to be empty, found {n} children")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task WaitForFunction_fails_when_the_condition_never_becomes_true()
    {
        var dataUrl = "data:text/html,<div id='grid'><span>a</span></div>";

        var test =
            from _1 in nav(dataUrl)
            from _2 in waitForFunction(css("#grid"), "el => el.children.length === 0", TimeSpan.FromMilliseconds(500))
            select unit;

        var (state, _) = await withChromium(test).Run();

        Assert.True(state.IsFaulted, "Expected waitForFunction to fail when the condition never holds");
    }

    [Fact]
    public async Task ConsoleMessages_are_readable_after_the_fact()
    {
        // unlike withConsoleCapture, this needs no handler registered up-front
        var dataUrl = "data:text/html,<html><body>after the fact</body>"
                    + "<script>console.log('recorded message')</script></html>";

        var test =
            from _1 in nav(dataUrl)
            from ms in consoleMessages
            from _2 in assert(ms.Exists(m => m.Message.Contains("recorded message")),
                              $"Expected 'recorded message', got: {string.Join(", ", ms.Map(m => m.Message))}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task ConsoleMessage_timestamps_are_within_a_day_of_now()
    {
        // Playwright types the timestamp as a float of epoch milliseconds, which at that
        // magnitude only resolves to a few minutes. This pins that it is the right epoch
        // and unit, not that it is precise.
        var dataUrl = "data:text/html,<html><body>ts</body><script>console.log('ts')</script></html>";

        var test =
            from _1 in nav(dataUrl)
            from ms in consoleMessages
            from m in ms.Find(x => x.Message.Contains("ts")).Match(
                          Some: pure,
                          None: () => fail<BrowserLogEntry>("Expected a console message"))
            let drift = (DateTime.UtcNow - m.Timestamp).Duration()
            from _2 in assert(drift < TimeSpan.FromDays(1), $"Timestamp {m.Timestamp:O} is {drift} from now")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task PageErrors_captures_an_uncaught_exception()
    {
        var dataUrl = "data:text/html,<html><body>boom</body>"
                    + "<script>setTimeout(() => { throw new Error('kaboom') }, 0)</script></html>";

        var test =
            from _1 in nav(dataUrl)
            from _2 in pause(TimeSpan.FromMilliseconds(300))
            from es in pageErrors
            from _3 in assert(es.Exists(e => e.Contains("kaboom")),
                              $"Expected 'kaboom', got: {string.Join(", ", es)}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task InFrame_runs_an_environment_aware_computation()
    {
        var dataUrl = "data:text/html,<iframe id='f' srcdoc=\"<input id='inner'/>\"></iframe>";

        // an environment-aware computation, as a page object method would be
        IsotopeAsync<string, Unit> fillFromEnv =
            from e in Isotope.ask<string>()
            from _ in fill(css("#inner"), e)
            select unit;

        var test =
            from _1 in nav(dataUrl)
            from _2 in inFrame(css("#f"), fillFromEnv)
            from v in inFrame(css("#f"), value(css("#inner")))
            from _3 in assert(v == "from the environment", $"Expected the env value in the iframe, got '{v}'")
            select unit;

        await withChromium(test).RunAndThrowOnError("from the environment");
    }

    [Fact]
    public async Task DropFile_delivers_a_file_to_a_drop_zone()
    {
        var file = Path.Combine(Path.GetTempPath(), $"isotope_drop_{Guid.NewGuid()}.txt");
        try
        {
            File.WriteAllText(file, "dropped content");

            var dataUrl = "data:text/html,<div id='zone' style='width:200px;height:200px'>zone</div><div id='out'></div>"
                        + "<script>const z=document.getElementById('zone');"
                        + "z.addEventListener('dragover', e => e.preventDefault());"
                        + "z.addEventListener('drop', e => { e.preventDefault();"
                        + "document.getElementById('out').textContent = [...e.dataTransfer.files].map(f => f.name).join(',') });"
                        + "</script>";

            var test =
                from _1 in nav(dataUrl)
                from _2 in dropFile(css("#zone"), new FileInfo(file))
                from t in text(css("#out"))
                from _3 in assert(t == Path.GetFileName(file), $"Expected the dropped file name, got '{t}'")
                select unit;

            await withChromium(test).RunAndThrowOnError();
        }
        finally
        {
            if (File.Exists(file)) File.Delete(file);
        }
    }

    [Fact]
    public async Task DropFiles_delivers_several_files_to_a_drop_zone()
    {
        var one = Path.Combine(Path.GetTempPath(), $"isotope_drop_a_{Guid.NewGuid()}.txt");
        var two = Path.Combine(Path.GetTempPath(), $"isotope_drop_b_{Guid.NewGuid()}.txt");
        try
        {
            File.WriteAllText(one, "one");
            File.WriteAllText(two, "two");

            var dataUrl = "data:text/html,<div id='zone' style='width:200px;height:200px'>zone</div><div id='out'></div>"
                        + "<script>const z=document.getElementById('zone');"
                        + "z.addEventListener('dragover', e => e.preventDefault());"
                        + "z.addEventListener('drop', e => { e.preventDefault();"
                        + "document.getElementById('out').textContent = e.dataTransfer.files.length });"
                        + "</script>";

            var test =
                from _1 in nav(dataUrl)
                from _2 in dropFiles(css("#zone"), new[] { new FileInfo(one), new FileInfo(two) })
                from t in text(css("#out"))
                from _3 in assert(t == "2", $"Expected 2 dropped files, got '{t}'")
                select unit;

            await withChromium(test).RunAndThrowOnError();
        }
        finally
        {
            if (File.Exists(one)) File.Delete(one);
            if (File.Exists(two)) File.Delete(two);
        }
    }

    [Fact]
    public async Task WithHar_records_the_requests_the_browser_made()
    {
        var harPath = Path.Combine(Path.GetTempPath(), $"isotope_har_{Guid.NewGuid()}.har");
        try
        {
            var test = withHar(harPath, nav("https://the-internet.herokuapp.com/login"));

            await withChromium(test).RunAndThrowOnError();

            Assert.True(File.Exists(harPath), $"Expected a HAR file at {harPath}");

            var har = File.ReadAllText(harPath);
            Assert.True(har.Length > 0, "Expected the HAR file to be non-empty");
            Assert.Contains("the-internet.herokuapp.com", har);
        }
        finally
        {
            if (File.Exists(harPath)) File.Delete(harPath);
        }
    }

    [Fact]
    public async Task WithScreencast_produces_a_video_file()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"isotope_cast_{Guid.NewGuid()}");
        try
        {
            Directory.CreateDirectory(dir);
            var videoPath = Path.Combine(dir, "run.webm");

            var inner =
                from _1 in nav("data:text/html,<html><body><h1 id='t'>Recording</h1></body></html>")
                from _2 in pause(TimeSpan.FromMilliseconds(500))
                from _3 in text(css("#t"))
                select unit;

            await withChromium(withScreencast(videoPath, inner)).RunAndThrowOnError();

            Assert.True(File.Exists(videoPath), $"Expected a video at {videoPath}");
            Assert.True(new FileInfo(videoPath).Length > 0, "Expected the video to be non-empty");
        }
        finally
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }
    }

    [Fact]
    public async Task StartTrace_with_live_still_produces_a_trace()
    {
        var tracePath = Path.Combine(Path.GetTempPath(), $"isotope_live_trace_{Guid.NewGuid()}.zip");
        try
        {
            var test =
                from _1 in startTrace("live-trace", live: true)
                from _2 in nav("data:text/html,<html><body>live</body></html>")
                from _3 in stopTrace(tracePath)
                select unit;

            await withChromium(test).RunAndThrowOnError();

            Assert.True(File.Exists(tracePath), $"Expected a trace at {tracePath}");
            Assert.True(new FileInfo(tracePath).Length > 0, "Expected the trace to be non-empty");
        }
        finally
        {
            if (File.Exists(tracePath)) File.Delete(tracePath);
        }
    }
}
