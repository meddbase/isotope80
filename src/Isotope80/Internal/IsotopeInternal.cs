using LanguageExt;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;

namespace Isotope80
{
    /// <summary>
    /// Isotope operations that are working more directly with Selenium, and therefore 
    /// should never see the light of day.
    /// </summary>
    internal static class IsotopeInternal
    {
        /// <summary>
        /// Convert an IWebElement to a SelectElement
        /// </summary>  
        public static Isotope<SelectElement> toSelectElement(IWebElement element) =>
            tryf(() => new SelectElement(element), x => "Problem creating select element: " + x.Message);
        

        /// <summary>
        /// Select a &lt;select&gt; option by text
        /// </summary>        
        public static Isotope<Unit> selectByText(SelectElement select, string text) =>
            trya(() => select.SelectByText(text), x => "Unable to select" + x.Message);
 
        /// <summary>
        /// Select a &lt;select&gt; option by value
        /// </summary>        
        public static Isotope<Unit> selectByValue(SelectElement select, string value) =>
            trya(() => select.SelectByValue(value), x => "Unable to select" + x.Message);

        /// <summary>
        /// Retrieves the selected option element in a Select Element
        /// </summary>
        /// <param name="sel">Web Driver Select Element</param>
        /// <returns>The selected Option Web Element</returns>
        public static Isotope<IWebElement> getSelectedOption(SelectElement sel) =>
            tryf(() => sel.SelectedOption, x => "Unable to get selected option" + x.Message);

        /// <summary>
        /// Set checkbox value for existing element
        /// </summary>
        /// <param name="el">Web Driver Element</param>
        /// <param name="ticked">Check the box or not</param>
        /// <returns>Unit</returns>
        public static Isotope<Unit> setCheckbox(IWebElement el, bool ticked) =>
            from val in isCheckboxChecked(el)
            from _   in val == ticked
                            ? pure(unit)
                            : click(el)
            select unit;

        /// <summary>
        /// Identifies whether an existing checkbox is checked
        /// </summary>
        /// <param name="el">Web Driver Element</param>
        /// <returns>Is checked\s</returns>
        public static Isotope<bool> isCheckboxChecked(IWebElement el) =>
            pure(el.Selected);

        /// <summary>
        /// Gets the text inside an element
        /// </summary>
        /// <param name="element">Element containing txt</param>
        public static Isotope<string> text(IWebElement element) =>
            tryf(() => element.Text, $@"Error getting text from element: {prettyPrint(element)}");

        /// <summary>
        /// Gets the value attribute of an element
        /// </summary>
        /// <param name="element">Element containing value</param>
        public static Isotope<string> value(IWebElement element) =>
            tryf(() => element.GetAttribute("Value"), $@"Error getting value from element: {prettyPrint(element)}");

        /// <summary>
        /// Looks for a particular style attribute on an existing element
        /// </summary>
        /// <param name="el">Web Driver Element</param>
        /// <param name="style">Style attribute to look up</param>
        /// <returns>A string representing the style value</returns>
        public static Isotope<string> getStyle(IWebElement el, string style) =>
            tryf(() => el.GetCssValue(style), $"Could not find style {style}");

        /// <summary>
        /// Gets the Z Index style attribute value for an existing element
        /// </summary>
        /// <param name="el">Web Driver Element</param>
        /// <returns>The Z Index value</returns>
        public static Isotope<int> getZIndex(IWebElement el) =>
            from zis in getStyle(el, "zIndex")
            from zii in parseInt(zis).ToIsotope($"z-Index was not valid integer: {zis}.")
            select zii;

        /// <summary>
        /// Looks for a particular style attribute on an existing element
        /// </summary>
        /// <param name="el">Web element</param>
        /// <param name="att">Attribute to look up</param>
        /// <returns>A string representing the attribute value</returns>
        public static Isotope<string> attribute(IWebElement el, string att) =>
            tryf(() => el.GetAttribute(att), $"Attribute {att} could not be found.");

        /// <summary>
        /// Simulates keyboard by sending `keys` 
        /// </summary>
        /// <param name="element">Element to type into</param>
        /// <param name="keys">String of characters that are typed</param>
        public static Isotope<Unit> sendKeys(IWebElement element, string keys) =>
            trya(() => element.SendKeys(keys), $@"Error sending keys ""{keys}"" to element: {prettyPrint(element)}");

        /// <summary>
        /// Simulates the mouse-click
        /// </summary>
        /// <param name="element">Element to click</param>
        public static Isotope<Unit> click(IWebElement element) =>
            trya(element.Click, $@"Error clicking element: {prettyPrint(element)}");

        /// <summary>
        /// Clears the content of an element
        /// </summary>
        /// <param name="element">Web Driver Element</param>
        /// <returns>Unit</returns>
        public static Isotope<Unit> clear(IWebElement element) =>
            trya(element.Clear, $@"Error clearing element: {prettyPrint(element)}");
        
        /// <summary>
        /// Overwrites the content of an element
        /// </summary>
        /// <param name="element">Web Driver Element</param>
        /// <param name="keys">String of characters that are typed</param>
        /// <returns>Unit</returns>
        public static Isotope<Unit> overwrite(IWebElement element, string keys) =>
            from dvr in webDriver
            let actions = new Actions(dvr)
            from _1 in trya(() => actions.Click(element)
                                         .KeyDown(Keys.Control)
                                         .SendKeys("a")
                                         .KeyUp(Keys.Control)
                                         .SendKeys(Keys.Backspace)
                                         .SendKeys(keys)
                                         .Perform(), $@"Error overwriting element: {prettyPrint(element)}")
            select unit;
        
        public static string prettyPrint(IWebElement x)
        {
            var tag = x.TagName;
            var css = x.GetAttribute("class");
            var id  = x.GetAttribute("id");

            return $"<{tag} class='{css}' id='{id}'>";
        }

        /// <summary>
        /// Checks if an element is currently displayed
        /// </summary>
        /// <param name="el">Web element</param>
        /// <returns>True if the element is currently displayed</returns>
        public static Isotope<bool> displayed(IWebElement el) =>
            tryf(() => el.Displayed, $"Error getting display status of {el}");

        /// <summary>
        /// Checks if an element is currently enabled
        /// </summary>
        /// <param name="el">Web element</param>
        /// <returns>True if the element is currently enabled</returns>
        public static Isotope<bool> enabled(IWebElement el) =>
            tryf(() => el.Enabled, $"Error getting enabled status of {el}");

        /// <summary>
        /// Checks whether the centre point of an element is the foremost element at that position on the page.
        /// (Uses the JavaScript document.elementFromPoint function)
        /// </summary>
        /// <param name="element">Target element</param>
        /// <returns>true if the element is foremost</returns>
        public static Isotope<bool> obscured(IWebElement element) =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            let coords = element.Location
            let x = coords.X + (int)Math.Floor((double)(element.Size.Width >> 1))
            let y = coords.Y + (int)Math.Floor((double)(element.Size.Height >> 1))
            from _ in info($"X: {x}, Y: {y}")
            from top in pure((IWebElement)jsExec.ExecuteScript($"return document.elementFromPoint({x}, {y});"))
            from _1  in info($"Target: {prettyPrint(element)}, Top: {prettyPrint(top)}")
            select !element.Equals(top);

        /// <summary>
        /// Repeatedly runs an Isotope function and checks whether the condition is met 
        /// </summary>        
        public static Isotope<A> waitUntil<A>(
            Isotope<A> iso,
            Func<A, bool> condition,
            TimeSpan interval,
            TimeSpan wait,
            DateTime started)
        {
            var cond = (from x in iso
                        from _ in condition(x)
                                      ? pure(unit)
                                      : fail("Condition failed")
                        select (CondPassed: true, Result: x)) |
                       (from _ in pause(interval)
                        select (CondPassed: false, Result: default(A)));
            
            return new Isotope<A>(s =>
            {
                var l = cond.Invoke(s);
                while (!l.Value.CondPassed)
                {
                    l = DateTime.UtcNow - started >= wait
                            ? new IsotopeState<(bool CondPassed, A Result)>(
                                (true, default(A)),
                                s.With(Error: Seq1(fail("Timed out"))))
                            : cond.Invoke(s);
                }

                return l.Map(v => v.Result);
            });
        }

        public static Exception Aggregate(Seq<Error> errs) =>
            errs.IsEmpty
                ? null
                : errs.Count == 1
                    ? (Exception) errs.Head
                    : new AggregateException(errs.Map(e => (Exception) e));
    }
}
