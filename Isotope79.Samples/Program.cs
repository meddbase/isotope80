using OpenQA.Selenium.Chrome;
using System;

namespace Isotope79.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var result = Meddbase.GoToPageAndOpenCareers.Run(new ChromeDriver());

            Console.WriteLine();
        }
    }
}
