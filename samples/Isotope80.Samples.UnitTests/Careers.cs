using Isotope80;
using System;
using Xunit;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit.Abstractions;

namespace Isotope80.Samples.UnitTests
{
    public class Careers
    {
        private readonly ITestOutputHelper output;

        public Careers(ITestOutputHelper output)
        {
            this.output = output;
        }

        public static Isotope<Unit> GoToDesktopSite =>
            context("Go to Desktop Site",
                    from _1 in log("Update Window Size")
                    from _2 in setWindowSize(1280, 960)
                    from _3 in nav("https://www.meddbase.com")
                    select unit);

        public static Isotope<Unit> WaitThenClick(By selector) =>
            from el in waitUntilClickable(selector)
            from _2 in click(selector)
            select unit;

        public static Isotope<Unit> ClickMoreMenu =>
            context("Click More menu",
                    WaitThenClick(By.CssSelector("#menu-item-39 > a")));

        public static Isotope<Unit> ClickCareersMenu =>
            context("Click Careers menu",
                    WaitThenClick(By.CssSelector("#menu-item-29 > a")));

        public static Isotope<Seq<string>> SelectVacancyTitles =>
            from links in findElements(By.XPath(@"//section[@class=""careers""]//div[h2[text() = ""Current Vacancies""]]/div[@class=""item""]/a"))
            let title = links.Map(x => x.Text)
            select title;

        public static Isotope<Unit> GoToPageAndOpenCareers =>
            from _1 in GoToDesktopSite
            from _2 in ClickMoreMenu
            from _3 in ClickCareersMenu
            select unit;

        [Fact]
        public void CareersMenuItemLoadsCareersPage() 
        {
            var expected = "https://www.meddbase.com/careers/";

            var iso = from _1  in GoToPageAndOpenCareers
                      from url in url
                      from _2  in assert(url == expected, $"Expected URL to be {expected} but it was {url}")
                      select unit;
           
            Action<string, Log> xunitOutput = 
                (x,y) => output.WriteLine(y.ToString());

            var result = iso.RunAndThrowOnError(new ChromeDriver(), settings: IsotopeSettings.Create(failureAction: xunitOutput));
        }


    }
}
