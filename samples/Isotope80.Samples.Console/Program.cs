using OpenQA.Selenium.Chrome;
using System;
using LanguageExt;
using LanguageExt.Common;
using static System.Console;

namespace Isotope80.Samples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string, int> consoleLogger =
                (x, y) => WriteLine(new string('\t', y) + x);

            (var state, var value) = Meddbase.GoToPageAndOpenCareers.Run(
                new ChromeDriver(), 
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
