using System;
using LanguageExt;
using LanguageExt.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using Xunit.Abstractions;
using Isotope80;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
using static Isotope80.Assertions;

namespace Samples.UnitTests
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
                from _1 in info("Update Window Size")
                from _2 in setWindowSize(1280, 960)
                from _3 in nav("https://www.meddbase.com")
                select unit);

        public static Isotope<Unit> WaitThenClick(By selector) =>
            from el in waitUntilClickable(selector)
            from _2 in click(selector)
            select unit;

        public static Isotope<Unit> ClickMoreMenu =>
            context("Click More menu",
                WaitThenClick(By.XPath("//li[@id = 'menu-item-39']/a")));

        public static Isotope<Unit> ClickJobsMenu =>
            context("Click Jobs menu",
                WaitThenClick(By.XPath("//li[@id = 'menu-item-29']/a")));

        public static Isotope<Seq<string>> SelectVacancyTitles =>
            from links in find(xPath("//div[h4[strong[text() = 'Current Vacancies']]]/p/a") + whenAtLeastOne)
            let title = links.Map(x => x.Text)
            select title;

        public static Isotope<Unit> GoToPageAndOpenCareers =>
            from _1 in GoToDesktopSite
            from _2 in ClickMoreMenu
            from _3 in ClickMoreMenu // second click necessary as the first closes the menu opened by focus
            from _4 in ClickJobsMenu
            select unit;

        [Fact]
        public void CareersMenuItemLoadsCareersPage() 
        {
            var expected = "https://www.meddbase.com/jobs-at-meddbase/";

            var iso = from _1  in GoToPageAndOpenCareers 
                      from vacancies in SelectVacancyTitles
                      from _2 in vacancies.Map(vacancy => info(vacancy)).Sequence()
                      from url in url
                      from _3  in assert(url == expected, $"Expected URL to be {expected} but it was {url}")
                      select unit;
           
            var stgs = IsotopeSettings.Create();
            stgs.LogStream.Subscribe(x => output.WriteLine(x.ToString()));
            stgs.ErrorStream.Subscribe(x => output.WriteLine(x.ToString()));
 
            (var state, var value) = withChromeDriver(iso).RunAndThrowOnError(settings: stgs);
        }
    }
}
