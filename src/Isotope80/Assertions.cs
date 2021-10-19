using System;
using LanguageExt;
using OpenQA.Selenium;
using static Isotope80.Isotope;
using static LanguageExt.Prelude;

namespace Isotope80
{
    /// <summary>
    /// Common assertions
    /// </summary>
    public static class Assertions
    {
        /// <summary>
        /// Assert that fact is True
        /// </summary>
        /// <param name="fact">Fact to assert</param>
        /// <param name="label">Label on failure</param>
        public static Isotope<Unit> assert(Isotope<bool> fact, string label) =>
            from f in fact
            from _ in assert(f, label)
            select unit;
        
        /// <summary>
        /// Assert that fact is True
        /// </summary>
        /// <param name="fact">Fact to assert</param>
        /// <param name="label">Label on failure</param>
        public static Isotope<Unit> assert(Func<bool> fact, string label) =>
            assert(fact(), label);

        /// <summary>
        /// Assert that fact is True
        /// </summary>
        /// <param name="fact">Fact to assert</param>
        /// <param name="label">Label on failure</param>
        public static Isotope<Unit> assert(bool fact, string label) =>
            fact ? pure(unit) : fail(label);

        /// <summary>
        /// Assert that an element has text that matches `expected`
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <param name="expected">Label on failure</param>
        public static Isotope<Unit> assertElementHasText(string cssSelector, string expected) =>
            assertElementHasText(Select.byCss(cssSelector), expected);

        /// <summary>
        /// Assert that an element has text that matches `expected`
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <param name="expected">Label on failure</param>
        public static Isotope<Unit> assertElementHasText(Select selector, string expected) =>
            from el in selector.ToIsotopeHead()
            from __ in assertElementIsDisplayed(selector)
            from ht in hasText(selector, expected) 
            select unit;

        /// <summary>
        /// Assert element is displayed
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        public static Isotope<Unit> assertElementIsDisplayed(string cssSelector) =>
            assertElementIsDisplayed(Select.byCss(cssSelector));

        /// <summary>
        /// Assert element is displayed
        /// </summary>
        /// <param name="selector">Element selector</param>
        public static Isotope<Unit> assertElementIsDisplayed(Select selector) =>
            assert(displayed(selector), $@"Expected selector ""{selector}"" to be displayed.");
    }
}
