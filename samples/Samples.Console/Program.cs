using OpenQA.Selenium.Chrome;
using System;
using Isotope80;
using LanguageExt;
using LanguageExt.Common;
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

            var ma = info("item").Map(_ => 1);
            var mb = info("item").Map(_ => 2);
            var mc = info("item").Map(_ => 3);

            var ms = context("items", Seq(ma, mb, mc).Sequence());

            var (nstate, nvalue) = ms.Run(settings);

            WriteLine(nstate.Log.ToString());
            
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
