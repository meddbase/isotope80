using System;
using System.IO;
using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class ScreenshotTests
{
    [Fact]
    public async Task Screenshot_returns_bytes()
    {
        var test =
            from _1 in nav("data:text/html,<html><body><h1>Screenshot</h1></body></html>")
            from screenshot in getScreenshot
            from _2 in assert(screenshot.IsSome, "Expected Some screenshot")
            from _3 in assert(screenshot.Map(s => s.Data.Length > 0).IfNone(false), "Expected screenshot data length > 0")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Element_screenshot_returns_bytes()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            from screenshot in getElementScreenshot(css("#username"))
            from _2 in assert(screenshot.IsSome, "Expected Some element screenshot")
            from _3 in assert(screenshot.Map(s => s.Data.Length > 0).IfNone(false), "Expected element screenshot data length > 0")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Save_screenshot_to_file()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"isotope_test_{Guid.NewGuid()}.png");
        try
        {
            var test =
                from _1 in nav("data:text/html,<html><body><h1>Save Test</h1></body></html>")
                from _2 in saveScreenshot(tempPath)
                select unit;

            await withChromium(test).RunAndThrowOnError();

            Assert.True(File.Exists(tempPath), $"Expected screenshot file to exist at {tempPath}");
            Assert.True(new FileInfo(tempPath).Length > 0, "Expected screenshot file to be non-empty");
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

    [Fact]
    public async Task SaveElementScreenshot_creates_file()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"isotope_elem_screenshot_{Guid.NewGuid()}.png");
        try
        {
            var test =
                from _1 in nav("https://the-internet.herokuapp.com/login")
                from _2 in saveElementScreenshot(css("#username"), tempPath)
                select unit;

            await withChromium(test).RunAndThrowOnError();

            Assert.True(File.Exists(tempPath), $"Expected element screenshot file to exist at {tempPath}");
            Assert.True(new FileInfo(tempPath).Length > 0, "Expected element screenshot file to be non-empty");
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }
}
