using OpenQA.Selenium.Chrome;
using System;
using static System.Console;

namespace Isotope80.Samples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string, int> consoleLogger =
                (x, y) => WriteLine(new string('\t', y) + x);


            var result = Meddbase.GoToPageAndOpenCareers.Run(
                new ChromeDriver(), 
                    IsotopeSettings.Create(
                        loggingAction: consoleLogger));
            
            Clear();
            
            WriteLine("Current Vacancies:\n");

            result.error.Match(
                Some: x => WriteLine($"ERROR: {x}"),
                None: () => result.value.Iter(x => WriteLine(x)));

            WriteLine("\n\nLogs:\n");

            WriteLine(result.log.ToString());

            WriteLine();
        }
    }
}
