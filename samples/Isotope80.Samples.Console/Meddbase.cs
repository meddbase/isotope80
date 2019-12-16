using LanguageExt;
using OpenQA.Selenium;
using static LanguageExt.Prelude;
using static Isotope79.Isotope;

namespace Isotope79.Samples.Console
{
    public static class Meddbase
    {
        public static Isotope<Unit> GoToDesktopSite =>
            context("Go to Desktop Site",
                    from _1 in log("Update Window Size")
                    from _2 in setWindowSize(1280,960)
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

        public static Isotope<Seq<string>> GoToPageAndOpenCareers =>
            from _1 in GoToDesktopSite
            from _2 in ClickMoreMenu
            from _3 in ClickCareersMenu
            from titles in SelectVacancyTitles
            select titles;
    }
}
