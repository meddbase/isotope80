using Isotope80;
using LanguageExt;
using OpenQA.Selenium;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;

namespace Samples.Console
{
    public static class Meddbase
    {
        public static Isotope<Unit> GoToDesktopSite =>
            context("Go to Desktop Site",
                from _1 in info("Update Window Size")
                from _2 in setWindowSize(1280,960)
                from _3 in nav("https://www.meddbase.com")
                select unit);

        public static Isotope<Unit> WaitThenClick(Select selector) =>
            from el in waitUntilClickable(selector)
            from _2 in click(selector)
            select unit;

        public static Isotope<Unit> ClickMoreMenu =>
            context("Click More menu",
                WaitThenClick(css("#menu-item-39 > a")));

        public static Isotope<Unit> ClickCareersMenu =>
            context("Click Careers menu",
                WaitThenClick(css("#menu-item-29 > a")));

        public static Isotope<Seq<string>> SelectVacancyTitles =>
            from links in find(xPath(@"//section[@class=""careers""]//div[h2[text() = ""Current Vacancies""]]/div[@class=""item""]/a") + whenAtLeastOne)
            let title = links.Map(x => x.Text)
            select title;

        public static Isotope<Seq<string>> GoToPageAndOpenCareers =>
            from _1 in GoToDesktopSite
            from _2 in ClickMoreMenu
            from _3 in ClickCareersMenu
            from titles in SelectVacancyTitles
            select titles;
    }
}
