using OpenQA.Selenium.Chrome;
using System;
using LanguageExt;
using LanguageExt.Common;
using static System.Console;
using static Isotope80.Isotope;

namespace Isotope80.Samples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string, int> consoleLogger =
                (x, y) => WriteLine(new string('\t', y) + x);

            (var state, var value) = withChromeDriver(Meddbase.GoToPageAndOpenCareers).Run(
                    IsotopeSettings.Create(
                        loggingAction: consoleLogger));
            
            Clear();
            
            WriteLine("Current Vacancies:\n");

            if (state.Error.IsEmpty)
            {
                value.Iter(x => WriteLine(x));
            }
            else
            {
                WriteLine($"ERROR: {state.Error.Head}");
            }
            
            WriteLine("\n\nLogs:\n");
            WriteLine(state.Log.ToString());
            WriteLine();
        }
      
    }
}
