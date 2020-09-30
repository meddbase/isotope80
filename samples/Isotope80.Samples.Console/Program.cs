using OpenQA.Selenium.Chrome;
using System;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using static System.Console;
using static Isotope80.Isotope;

namespace Isotope80.Samples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ForegroundColor = ConsoleColor.Yellow;
            
            var stgs = IsotopeSettings.Create();
            stgs.LogStream.Subscribe(x => WriteLine(x));
            (var state, var value) = withChromeDriver(Meddbase.GoToPageAndOpenCareers).Run(stgs);
            
            Clear();
            
            if (state.Error.IsEmpty)
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine("Current Vacancies:\n");
                value.Iter(x => WriteLine(x));
            }
            else
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"ERROR: {state.Error.Head}");
            }
            
            ForegroundColor = ConsoleColor.White;
        }
    }
}
