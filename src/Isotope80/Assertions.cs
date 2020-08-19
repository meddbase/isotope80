using LanguageExt;
using OpenQA.Selenium;
using System;
using static Isotope80.Isotope;
using static LanguageExt.Prelude;

namespace Isotope80
{
    public static class Assertions
    {
        public static Isotope<Env, Unit> assert<Env>(Isotope<Env, bool> fact, string label) =>
            from f in fact
            from _ in assert<Env>(f, label)
            select unit;

        public static Isotope<Env, Unit> assert<Env>(Func<bool> fact, string label) =>
            assert<Env>(fact(), label);

        public static Isotope<Env, Unit> assert<Env>(bool fact, string label) =>
            fact ? pure<Env, Unit>(unit) : fail<Env, Unit>(label);

        public static Isotope<Env, Unit> assertElementHasText<Env>(string cssSelector, string expected) =>
            assertElementHasText<Env>(By.CssSelector(cssSelector), expected);

        public static Isotope<Env, Unit> assertElementHasText<Env>(By selector, string expected) =>
            from el in findElement<Env>(selector)
            from re in assertElementHasText<Env>(el, expected)
            select unit;

        public static Isotope<Env, Unit> assertElementHasText<Env>(IWebElement el, string expected) =>
            from _ in assertElementIsDisplayed<Env>(el)
            from re in assert(hasText<Env>(el, expected), $@"Expected element ""{el}"" to have text ""{expected}"" but it was ""{el.Text}""")
            select unit;

        public static Isotope<Env, Unit> assertElementIsDisplayed<Env>(string cssSelector) =>
            assertElementIsDisplayed<Env>(By.CssSelector(cssSelector));

        public static Isotope<Env, Unit> assertElementIsDisplayed<Env>(By selector) =>
            assert(displayed<Env>(selector), $@"Expected selector ""{selector}"" to be displayed.");

        public static Isotope<Env, Unit> assertElementIsDisplayed<Env>(IWebElement el) =>
            assert(displayed<Env>(el), $@"Expected element ""{el}"" to be displayed.");
    }
}
