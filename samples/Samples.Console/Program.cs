using System;
using Isotope80;
using Isotope80.Samples.Console;
using LanguageExt;
using static LanguageExt.Prelude;
using static System.Console;
using static Isotope80.Isotope;

namespace Samples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = IsotopeSettings.Create();
            settings.LogStream.Subscribe(x => WriteLine(x));

            // ── Login demo ──────────────────────────────────────────

            WriteLine("=== Login Flow ===\n");

            var (loginState, _) = withChromeDriver(TheInternet.LoginFlow).Run(settings);

            if (loginState.Error.IsEmpty)
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine("\nLogin test passed.");
            }
            else
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"\nLogin test failed: {loginState.Error.Head}");
            }

            ResetColor();

            // ── Dropdown demo ───────────────────────────────────────

            WriteLine("\n=== Dropdown Flow ===\n");

            var (dropdownState, selected) = withChromeDriver(TheInternet.DropdownFlow).Run(settings);

            if (dropdownState.Error.IsEmpty)
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine($"\nDropdown test passed. Selected: {selected}");
            }
            else
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"\nDropdown test failed: {dropdownState.Error.Head}");
            }

            ResetColor();

            // ── Hover demo ──────────────────────────────────────────

            WriteLine("\n=== Hover Flow ===\n");

            var (hoverState, names) = withChromeDriver(TheInternet.HoverFlow).Run(settings);

            if (hoverState.Error.IsEmpty)
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine("\nHover test passed. Found:");
                names.Iter(n => WriteLine($"  {n}"));
            }
            else
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"\nHover test failed: {hoverState.Error.Head}");
            }

            ResetColor();
        }
    }
}
