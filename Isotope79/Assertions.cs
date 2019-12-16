using System;
using LanguageExt;
using OpenQA.Selenium;
using static Isotope79.Isotope;
using static LanguageExt.Prelude;

namespace Isotope79
{
    public static class Assertions
    {
        public static Isotope<Unit> assert(Isotope<bool> fact, string label) =>
            from f in fact
            from _ in assert(f, label)
            select unit;
        
        public static Isotope<Unit> assert(Func<bool> fact, string label) =>
            assert(fact(), label);

        public static Isotope<Unit> assert(bool fact, string label) =>
            fact ? pure(unit) : fail<Unit>(label);

        public static Isotope<Unit> assertElementHasText(string cssSelector, string expected) =>
            assertElementHasText(By.CssSelector(cssSelector), expected);

        public static Isotope<Unit> assertElementHasText(By selector, string expected) =>
            from el in findElement(selector)
            from re in assertElementHasText(el, expected)
            select unit;

        public static Isotope<Unit> assertElementHasText(IWebElement el, string expected) =>
            from _ in assertElementIsDisplayed(el)
            from re in assert(hasText(el, expected), $@"Expected element ""{el}"" to have text ""{expected}"" but it was ""{el.Text}""")
            select unit;

        public static Isotope<Unit> assertElementIsDisplayed(string cssSelector) =>
            assertElementIsDisplayed(By.CssSelector(cssSelector));

        public static Isotope<Unit> assertElementIsDisplayed(By selector) =>
            assert(displayed(selector), $@"Expected selector ""{selector}"" to be displayed.");

        public static Isotope<Unit> assertElementIsDisplayed(IWebElement el) =>
            assert(displayed(el), $@"Expected element ""{el}"" to be displayed.");
    }
}
