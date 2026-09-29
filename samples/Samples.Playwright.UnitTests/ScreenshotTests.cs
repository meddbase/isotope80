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

    [Fact]
    public async Task Save_screenshot_as_png_when_the_extension_says_so()
    {
        // Playwright infers the image type from the path extension, so no format
        // parameter is needed - this test is what that claim rests on.
        var tempPath = Path.Combine(Path.GetTempPath(), $"isotope_test_{Guid.NewGuid()}.png");
        try
        {
            var test =
                from _1 in nav("data:text/html,<html><body><h1>Png</h1></body></html>")
                from _2 in saveScreenshot(tempPath)
                select unit;

            await withChromium(test).RunAndThrowOnError();

            Assert.True(File.Exists(tempPath), $"Expected screenshot at {tempPath}");

            var bytes = File.ReadAllBytes(tempPath);
            Assert.True(bytes.Length > 0, "Expected screenshot to be non-empty");
            // \x89PNG
            Assert.Equal(new byte[] { 0x89, 0x50, 0x4E, 0x47 }, bytes[0..4]);
        }
        finally
        {
            if (File.Exists(tempPath)) File.Delete(tempPath);
        }
    }

    [Fact]
    public async Task Save_screenshot_as_jpeg_from_the_file_extension()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"isotope_test_{Guid.NewGuid()}.jpeg");
        try
        {
            var test =
                from _1 in nav("data:text/html,<html><body><h1>Jpeg</h1></body></html>")
                from _2 in saveScreenshot(tempPath, 80)
                select unit;

            await withChromium(test).RunAndThrowOnError();

            var bytes = File.ReadAllBytes(tempPath);
            // SOI marker
            Assert.Equal(new byte[] { 0xFF, 0xD8 }, bytes[0..2]);
        }
        finally
        {
            if (File.Exists(tempPath)) File.Delete(tempPath);
        }
    }

    [Fact]
    public async Task Lower_quality_produces_a_smaller_file()
    {
        var high = Path.Combine(Path.GetTempPath(), $"isotope_test_{Guid.NewGuid()}.jpeg");
        var low  = Path.Combine(Path.GetTempPath(), $"isotope_test_{Guid.NewGuid()}.jpeg");
        try
        {
            var page = "data:text/html,<html><body style='background:linear-gradient(red,blue)'><h1>Quality</h1></body></html>";

            var test =
                from _1 in nav(page)
                from _2 in saveScreenshot(high, 100)
                from _3 in saveScreenshot(low, 10)
                select unit;

            await withChromium(test).RunAndThrowOnError();

            var highLength = new FileInfo(high).Length;
            var lowLength  = new FileInfo(low).Length;

            Assert.True(lowLength < highLength, $"Expected quality 10 ({lowLength}) to be smaller than quality 100 ({highLength})");
        }
        finally
        {
            if (File.Exists(high)) File.Delete(high);
            if (File.Exists(low)) File.Delete(low);
        }
    }

    [Fact]
    public async Task Save_element_screenshot_as_jpeg()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"isotope_test_{Guid.NewGuid()}.jpeg");
        try
        {
            var test =
                from _1 in nav("data:text/html,<html><body><h1 id='title'>Element</h1></body></html>")
                from _2 in saveElementScreenshot(css("#title"), tempPath, 70)
                select unit;

            await withChromium(test).RunAndThrowOnError();

            var bytes = File.ReadAllBytes(tempPath);
            // SOI marker
            Assert.Equal(new byte[] { 0xFF, 0xD8 }, bytes[0..2]);
        }
        finally
        {
            if (File.Exists(tempPath)) File.Delete(tempPath);
        }
    }
}
