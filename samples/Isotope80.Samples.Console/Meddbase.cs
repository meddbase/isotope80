using LanguageExt;
using OpenQA.Selenium;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
using System;

namespace Isotope80.Samples.Console
{
    public static class Meddbase
    {
        public static Isotope<Unit, Unit> GoToDesktopSite =>
            context("Go to Desktop Site",
                    from _1 in log<Unit>("Update Window Size")
                    from _2 in setWindowSize<Unit>(1280,960)
                    from _3 in nav<Unit>("https://www.meddbase.com")
                    select unit);

        public static Isotope<Unit, Unit> WaitThenClick(By selector) =>
            from el in waitUntilClickable<Unit>(selector)
            from _2 in click<Unit>(el)
            select unit;

        public static Isotope<Unit, Unit> ClickMoreMenu =>
            context("Click More menu",
                    WaitThenClick(css("#menu-item-39 > a")));

        public static Isotope<Unit, Unit> ClickCareersMenu =>
            context("Click Careers menu",
                    WaitThenClick(css("#menu-item-29 > a")));

        public static Isotope<Unit, Seq<string>> SelectVacancyTitles =>
            from links in findElements<Unit>(xPath(@"//section[@class=""careers""]//div[h2[text() = ""Current Vacancies""]]/div[@class=""item""]/a"))
            let title = links.Map(x => x.Text)
            select title;

        public static Isotope<Unit, Seq<string>> GoToPageAndOpenCareers =>
            from _1 in GoToDesktopSite
            from _2 in ClickMoreMenu
            from _3 in ClickCareersMenu
            from titles in SelectVacancyTitles
            select titles;
    }
}
