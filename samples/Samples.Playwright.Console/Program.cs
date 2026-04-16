using System;
using System.IO;
using Isotope80;
using Isotope80.Samples.Console;
using LanguageExt;
using static LanguageExt.Prelude;
using static System.Console;
using static Isotope80.Isotope;

var settings = IsotopeSettings.Create();
settings.LogStream.Subscribe(x => WriteLine(x));

// ── Login demo ──────────────────────────────────────────

WriteLine("=== Login Flow ===\n");

try
{
    await withChromium(TheInternet.LoginFlow).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine("\nLogin test passed.");
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nLogin test failed: {ex.Message}");
}

ResetColor();

// ── Checkbox demo ───────────────────────────────────────

WriteLine("\n=== Checkbox Flow ===\n");

try
{
    await withChromium(TheInternet.CheckboxFlow).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine("\nCheckbox test passed.");
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nCheckbox test failed: {ex.Message}");
}

ResetColor();

// ── Dropdown demo ───────────────────────────────────────

WriteLine("\n=== Dropdown Flow ===\n");

try
{
    var (_, selected) = await withChromium(TheInternet.DropdownFlow).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine($"\nDropdown test passed. Selected: {selected}");
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nDropdown test failed: {ex.Message}");
}

ResetColor();

// ── Hover demo ──────────────────────────────────────────

WriteLine("\n=== Hover Flow ===\n");

try
{
    var (_, names) = await withChromium(TheInternet.HoverFlow).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine("\nHover test passed. Found:");
    names.Iter(n => WriteLine($"  {n}"));
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nHover test failed: {ex.Message}");
}

ResetColor();

// ── Playwright-specific features ───────────────────────

// ── Trace demo ─────────────────────────────────────────

WriteLine("\n=== Trace Demo ===\n");

try
{
    var tracePath = Path.Combine(Path.GetTempPath(), "isotope-trace.zip");
    await withChromium(TheInternet.TraceDemo(tracePath)).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    var size = new FileInfo(tracePath).Length;
    WriteLine($"\nTrace saved to {tracePath} ({size:N0} bytes)");
    WriteLine("View with: npx playwright show-trace " + tracePath);
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nTrace demo failed: {ex.Message}");
}

ResetColor();

// ── Trace on failure demo ──────────────────────────────

WriteLine("\n=== Trace-on-Failure Demo ===\n");

{
    var traceDir = Path.Combine(Path.GetTempPath(), "isotope-traces");
    Directory.CreateDirectory(traceDir);

    try
    {
        // This will fail (wrong password) — the trace captures the failure
        await withChromium(TheInternet.TraceOnFailureDemo(traceDir)).RunAndThrowOnError(settings);
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine("\nUnexpectedly passed (no trace saved)");
    }
    catch
    {
        ForegroundColor = ConsoleColor.Green;
        var traces = Directory.GetFiles(traceDir, "trace_*.zip");
        if (traces.Length > 0)
        {
            WriteLine($"\nTest failed as expected. Trace saved to: {traces[^1]}");
            WriteLine("View with: npx playwright show-trace " + traces[^1]);
        }
        else
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("\nTest failed but no trace was saved.");
        }
    }

    ResetColor();
}

// ── Console capture demo ───────────────────────────────

WriteLine("\n=== Console Capture Demo ===\n");

try
{
    var (_, logs) = await withChromium(TheInternet.ConsoleCaptureDemo).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine($"\nCaptured {logs.Count} console messages:");
    logs.Iter(l => WriteLine($"  [{l.Level}] {l.Message}"));
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nConsole capture demo failed: {ex.Message}");
}

ResetColor();

// ── Network interception demo ──────────────────────────

WriteLine("\n=== Network Interception Demo ===\n");

try
{
    var (_, imgCount) = await withChromium(TheInternet.NetworkInterceptionDemo).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine($"\nFound {imgCount} <img> elements (images were blocked by route interception)");
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nNetwork interception demo failed: {ex.Message}");
}

ResetColor();

// ── Dialog handling demo ───────────────────────────────

WriteLine("\n=== Dialog Handling Demo ===\n");

try
{
    var (_, messages) = await withChromium(TheInternet.DialogDemo).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine("\nCaptured dialog messages:");
    messages.Iter(m => WriteLine($"  \"{m}\""));
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nDialog handling demo failed: {ex.Message}");
}

ResetColor();

// ── Semantic selector demo ─────────────────────────────

WriteLine("\n=== Semantic Selector Demo ===\n");

try
{
    await withChromium(TheInternet.SemanticSelectorDemo).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine("\nLogin via semantic selectors (byLabel, byRole) passed.");
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nSemantic selector demo failed: {ex.Message}");
}

ResetColor();

// ── Frame demo ─────────────────────────────────────────

WriteLine("\n=== Frame Demo ===\n");

try
{
    var (_, frameText) = await withChromium(TheInternet.FrameDemo).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine($"\nText inside iframe: \"{frameText.Trim()}\"");
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nFrame demo failed: {ex.Message}");
}

ResetColor();

// ── Context isolation demo ─────────────────────────────

WriteLine("\n=== Context Isolation Demo ===\n");

try
{
    var (_, result) = await withChromium(TheInternet.ContextIsolationDemo).RunAndThrowOnError(settings);
    ForegroundColor = ConsoleColor.Green;
    WriteLine($"\nMain context has 'isotope_demo' cookie: {result.MainHasCookie}");
    WriteLine($"Isolated context has 'isotope_demo' cookie: {result.IsolatedHasCookie}");
    WriteLine(!result.IsolatedHasCookie
        ? "Context isolation confirmed — custom cookie doesn't leak."
        : "WARNING: Cookie leaked between contexts!");
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"\nContext isolation demo failed: {ex.Message}");
}

ResetColor();
