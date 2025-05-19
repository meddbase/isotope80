﻿using System;
using LanguageExt;
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

        public static Isotope<Unit> MoveTo(By selector) =>
            moveToElement(selector);
        
        public static Isotope<Unit> MoveToMoreMenu =>
            context("Move to Why Meddbase",
                MoveTo(By.XPath("//li/div/span[text() = 'Why Meddbase']")));
        
        public static Isotope<Unit> ClickCareers =>
            context("Click Careers",
                WaitThenClick(By.XPath("//div[@class = 'content']/p/a/span[text() = 'Careers']")));

        public static Isotope<Seq<string>> SelectVacancyTitles =>
            from links in find(xPath("//div[h6[text() = 'Current Vacancies']]//div/a"))
            let titles = links.Map(x => x.Text)
            select titles;

        public static Isotope<Unit> GoToPageAndOpenCareers =>
            from _1 in GoToDesktopSite
            from _2 in MoveToMoreMenu
            from _3 in ClickCareers
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
