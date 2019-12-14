using OpenQA.Selenium.Chrome;
using System;

namespace Isotope79.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Meddbase.GoToPageAndOpenCareers.Run(new ChromeDriver(), IsotopeSettings.Create(loggingAction: (x,y) => Console.WriteLine(new string('\t', y) + x)));
            
            Console.Clear();
            
            Console.WriteLine("Current Vacancies:\n");

            result.error.Match(
                Some: x => Console.WriteLine($"ERROR: {x}"),
                None: () => result.value.Iter(x => Console.WriteLine(x)));

            Console.WriteLine("\n\nLogs:\n");

            Console.WriteLine(result.log.ToString());

            Console.WriteLine();
        }
    }
}
