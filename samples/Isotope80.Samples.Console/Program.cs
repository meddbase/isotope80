using OpenQA.Selenium.Chrome;
using static System.Console;

namespace Isotope80.Samples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Meddbase.GoToPageAndOpenCareers.Run(new ChromeDriver(), IsotopeSettings.Create(loggingAction: (x,y) => WriteLine(new string('\t', y) + x)));
            
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
