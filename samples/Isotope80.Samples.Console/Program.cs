using OpenQA.Selenium.Chrome;
using System;
using static System.Console;
using static LanguageExt.Prelude;

namespace Isotope80.Samples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string, int> consoleLogger =
                (x, y) => WriteLine(new string('\t', y) + x);

            (var state, var value) = Meddbase.GoToPageAndOpenCareers.Run(
                unit,
                new ChromeDriver(), 
                    IsotopeSettings.Create(
                        loggingAction: consoleLogger));
            
            Clear();
            
            WriteLine("Current Vacancies:\n");

            state.Error.Match(
                Some: x => WriteLine($"ERROR: {x}"),
                None: () => value.Iter(x => WriteLine(x)));

            WriteLine("\n\nLogs:\n");

            WriteLine(state.Log.ToString());

            WriteLine();
        }
    }
}
