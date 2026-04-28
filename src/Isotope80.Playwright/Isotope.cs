using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Playwright;
using static LanguageExt.Prelude;

namespace Isotope80
{
    /// <summary>
    /// Core isotope combinators
    /// </summary>
    public static partial class Isotope
    {

        /// <summary>
        /// Page accessor - returns IsotopeAsync because it accesses Playwright state
        /// </summary>
        public static IsotopeAsync<IPage> page =>
            new IsotopeAsync<IPage>(state =>
                new ValueTask<IsotopeState<IPage>>(
                    state.Page.Match(
                        Some: p => new IsotopeState<IPage>(p, state),
                        None: () => new IsotopeState<IPage>(default, state.AddError(Error.New("No page available. Call withChromium, withFirefox, withWebkit, or withPage first."))))));

        /// <summary>
        /// Returns the current locator root — either the page or the innermost frame locator
        /// if <see cref="inFrame{A}"/> is active. Used internally by the Select pipeline
        /// to root locator construction.
        /// </summary>
        internal static IsotopeAsync<LocatorRoot> locatorRoot =>
            from p in page
            from s in get
            select s.FrameScope.IsEmpty
                ? LocatorRoot.FromPage(p)
                : LocatorRoot.FromFrame(s.FrameScope.Peek());

        /// <summary>
        /// Browser context accessor
        /// </summary>
        public static IsotopeAsync<IBrowserContext> browserContext =>
            new IsotopeAsync<IBrowserContext>(state =>
                new ValueTask<IsotopeState<IBrowserContext>>(
                    state.BrowserContext.Match(
                        Some: ctx => new IsotopeState<IBrowserContext>(ctx, state),
                        None: () => new IsotopeState<IBrowserContext>(default, state.AddError(Error.New("No browser context available. Call withChromium, withFirefox, withWebkit, or withContext first."))))));

        /// <summary>
        /// Browser accessor
        /// </summary>
        public static IsotopeAsync<IBrowser> browser =>
            new IsotopeAsync<IBrowser>(state =>
                new ValueTask<IsotopeState<IBrowser>>(
                    state.Browser.Match(
                        Some: b => new IsotopeState<IBrowser>(b, state),
                        None: () => new IsotopeState<IBrowser>(default, state.AddError(Error.New("No browser available. Call withChromium, withFirefox, or withWebkit first."))))));


        /// <summary>
        /// Set the page in state
        /// </summary>
        internal static Isotope<Unit> setPage(IPage p) => modify(s => s.With(Page: Some(p)));

        /// <summary>
        /// Clear the page from state
        /// </summary>
        internal static Isotope<Unit> clearPage() => modify(s => s.With(Page: Option<IPage>.None));

        /// <summary>
        /// Set the browser context in state
        /// </summary>
        internal static Isotope<Unit> setBrowserContext(IBrowserContext ctx) => modify(s => s.With(BrowserContext: Some(ctx)));

        /// <summary>
        /// Set the browser in state
        /// </summary>
        internal static Isotope<Unit> setBrowser(IBrowser b) => modify(s => s.With(Browser: Some(b)));

        /// <summary>
        /// Set the Playwright instance in state
        /// </summary>
        internal static Isotope<Unit> setPlaywright(IPlaywright pw) => modify(s => s.With(Playwright: Some(pw)));


        /// <summary>
        /// Pauses the processing for an interval
        /// </summary>
        public static IsotopeAsync<Unit> pause(TimeSpan interval) =>
            new IsotopeAsync<Unit>(async s =>
            {
                await Task.Delay(interval).ConfigureAwait(false);
                return new IsotopeState<Unit>(default, s);
            });

        /// <summary>
        /// Wait until the condition is true, or it times out
        /// </summary>
        public static IsotopeAsync<A> waitUntil<A>(
            IsotopeAsync<A> iso,
            Func<A, bool> condition,
            Option<TimeSpan> interval = default,
            Option<TimeSpan> wait = default) =>
            from w in wait.Match(Some: pure, None: defaultWait)
            from i in interval.Match(Some: pure, None: defaultInterval)
            from r in waitUntilCore(iso, condition, i, w, DateTime.UtcNow)
            select r;

        /// <summary>
        /// Wait until iso succeeds, or it times out
        /// </summary>
        public static IsotopeAsync<A> waitUntil<A>(
            IsotopeAsync<A> iso,
            Option<TimeSpan> interval = default,
            Option<TimeSpan> wait = default) =>
            waitUntil(iso, _ => true, interval, wait);

        /// <summary>
        /// Core polling loop for waitUntil
        /// </summary>
        static IsotopeAsync<A> waitUntilCore<A>(
            IsotopeAsync<A> iso,
            Func<A, bool> condition,
            TimeSpan interval,
            TimeSpan wait,
            DateTime started) =>
            new IsotopeAsync<A>(async s =>
            {
                while (true)
                {
                    var l = await iso.Invoke(s).ConfigureAwait(false);
                    if (!l.IsFaulted && condition(l.Value))
                    {
                        return l;
                    }

                    if (DateTime.UtcNow - started >= wait)
                    {
                        return l.IsFaulted
                            ? l
                            : new IsotopeState<A>(default, s.AddError(Error.New("Timed out")));
                    }

                    await Task.Delay(interval).ConfigureAwait(false);
                }
            });


        /// <summary>
        /// Navigate to a URL
        /// </summary>
        /// <param name="url">URL to navigate to</param>
        public static IsotopeAsync<Unit> nav(string url) =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.GotoAsync(url).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Gets the URL currently displayed by the browser
        /// </summary>
        public static IsotopeAsync<string> url =>
            from p in page
            select p.Url;

        /// <summary>
        /// Gets the page title currently displayed by the browser
        /// </summary>
        public static IsotopeAsync<string> title =>
            from p in page
            from t in isoAsync<string>(async () => await p.TitleAsync().ConfigureAwait(false))
            select t;

        /// <summary>
        /// Navigate back using the browser's back button
        /// </summary>
        public static IsotopeAsync<Unit> back =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.GoBackAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Navigate forward using the browser's forward button
        /// </summary>
        public static IsotopeAsync<Unit> forward =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.GoForwardAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Refresh current page
        /// </summary>
        public static IsotopeAsync<Unit> refresh =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.ReloadAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;


        /// <summary>
        /// Find an HTML element
        /// </summary>
        /// <param name="selector">Element selector</param>
        public static IsotopeAsync<WebElement> find1(Select selector) =>
            find(selector + whenAtLeastOne).Map(es => es.Head);

        /// <summary>
        /// Find HTML elements
        /// </summary>
        /// <param name="selector">Element selector</param>
        public static IsotopeAsync<Seq<WebElement>> find(Select selector) =>
            selector.ToSeq();

        /// <summary>
        /// Checks if an element exists that matches the selector
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>True if a matching element exists</returns>
        public static IsotopeAsync<bool> exists(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from cnt in isoAsync<int>(async () => await loc.CountAsync().ConfigureAwait(false))
            select cnt > 0;

        /// <summary>
        /// Returns the count of elements matching the selector
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static IsotopeAsync<int> elementCount(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from cnt in isoAsync<int>(async () => await loc.CountAsync().ConfigureAwait(false))
            select cnt;


        /// <summary>
        /// Simulates the mouse-click on an element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static IsotopeAsync<Unit> click(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.ClickAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Simulates keyboard by sending keys one at a time
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="keys">String of characters that are typed</param>
        public static IsotopeAsync<Unit> sendKeys(Select selector, string keys) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.PressSequentiallyAsync(keys).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Presses a single key or key combination on an element.
        /// Supports special keys: "Tab", "Enter", "End", "Home", "Escape", "Backspace", "Delete",
        /// "ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", and modifiers like "Shift+Tab".
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="key">Key to press (e.g. "Tab", "Enter", "End")</param>
        public static IsotopeAsync<Unit> pressKey(Select selector, string key) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.PressAsync(key).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Presses a single key or key combination on the page (not targeting a specific element).
        /// Supports special keys: "Tab", "Enter", "End", "Home", "Escape", "Backspace", "Delete",
        /// "ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", and modifiers like "Shift+Tab".
        /// </summary>
        /// <param name="key">Key to press (e.g. "Tab", "Enter", "End")</param>
        public static IsotopeAsync<Unit> pressKey(string key) =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.Keyboard.PressAsync(key).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Fill an input element with a value (clears first, then sets)
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="value">Value to fill</param>
        public static IsotopeAsync<Unit> fill(Select selector, string value) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.FillAsync(value).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Clears the content of an element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static IsotopeAsync<Unit> clear(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.ClearAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Simulates keyboard by sending keys and overwriting current content
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="keys">String of characters that are typed</param>
        /// <remarks>
        /// Uses Playwright's FillAsync which clears the field then sets the value,
        /// matching the overwrite semantics of the original Isotope80 API.
        /// </remarks>
        public static IsotopeAsync<Unit> overwrite(Select selector, string keys) => fill(selector, keys);

        /// <summary>
        /// Simulates a double-click on an element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static IsotopeAsync<Unit> doubleClick(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.DblClickAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Simulates a right-click (context menu click) on an element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static IsotopeAsync<Unit> rightClick(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.ClickAsync(new LocatorClickOptions { Button = MouseButton.Right }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Drags the source element to the target element
        /// </summary>
        /// <param name="source">Source element selector</param>
        /// <param name="target">Target element selector</param>
        public static IsotopeAsync<Unit> dragTo(Select source, Select target) =>
            from srcLoc in source.ToIsotopeLocator()
            from tgtLoc in target.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await srcLoc.DragToAsync(tgtLoc).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Moves the mouse to hover over an element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static IsotopeAsync<Unit> moveToElement(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.HoverAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Moves the mouse to the specified absolute coordinates on the page.
        /// </summary>
        /// <remarks>
        /// Alias for <see cref="moveToLocation"/> in the Playwright variant since Playwright
        /// uses absolute page coordinates.
        /// </remarks>
        /// <param name="x">Absolute X coordinate</param>
        /// <param name="y">Absolute Y coordinate</param>
        public static IsotopeAsync<Unit> moveByOffset(int x, int y) =>
            moveToLocation(x, y);

        /// <summary>
        /// Moves the mouse to the specified absolute coordinates on the page
        /// </summary>
        /// <param name="x">Absolute X coordinate</param>
        /// <param name="y">Absolute Y coordinate</param>
        public static IsotopeAsync<Unit> moveToLocation(int x, int y) =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.Mouse.MoveAsync(x, y).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Selects an option from a select element by its visible text
        /// </summary>
        /// <param name="selector">Select element selector</param>
        /// <param name="optionText">Visible text of the option to select</param>
        public static IsotopeAsync<Unit> selectByText(Select selector, string optionText) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.SelectOptionAsync(new SelectOptionValue { Label = optionText }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Selects an option from a select element by its value attribute
        /// </summary>
        /// <param name="selector">Select element selector</param>
        /// <param name="optionValue">Value attribute of the option to select</param>
        public static IsotopeAsync<Unit> selectByValue(Select selector, string optionValue) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.SelectOptionAsync(new SelectOptionValue { Value = optionValue }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Selects an option from a select element by its zero-based index
        /// </summary>
        /// <param name="selector">Select element selector</param>
        /// <param name="index">Zero-based index of the option to select</param>
        public static IsotopeAsync<Unit> selectByPosition(Select selector, int index) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.SelectOptionAsync(new SelectOptionValue { Index = index }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Selects an option from a select element where the option text contains the specified substring
        /// </summary>
        /// <param name="selector">Select element selector</param>
        /// <param name="partialText">Substring to match against option text</param>
        public static IsotopeAsync<Unit> selectByTextContaining(Select selector, string partialText) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                var option = loc.Locator("option", new LocatorLocatorOptions { HasTextString = partialText });
                var text = await option.First.TextContentAsync().ConfigureAwait(false);
                await loc.SelectOptionAsync(new SelectOptionValue { Label = text }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Sets a checkbox or radio button to the desired checked state
        /// </summary>
        /// <param name="selector">Checkbox or radio button selector</param>
        /// <param name="ticked">True to check, false to uncheck</param>
        public static IsotopeAsync<Unit> setCheckbox(Select selector, bool ticked) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.SetCheckedAsync(ticked).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Clears the element and then types the keys one at a time using PressSequentiallyAsync.
        /// This is useful when FillAsync does not trigger the required input events.
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="keys">String of characters to type</param>
        public static IsotopeAsync<Unit> clearAndType(Select selector, string keys) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.ClearAsync().ConfigureAwait(false);
                await loc.PressSequentiallyAsync(keys).ConfigureAwait(false);
                return unit;
            })
            select unit;


        /// <summary>
        /// Gets the text inside an element
        /// </summary>
        /// <param name="selector">Element containing text</param>
        public static IsotopeAsync<string> text(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from t in isoAsync<string>(async () => await loc.InnerTextAsync().ConfigureAwait(false))
            select t;

        /// <summary>
        /// Gets the visible text of all elements matching the selector
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <returns>Text of all matching elements</returns>
        public static IsotopeAsync<Seq<string>> texts(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from ts in isoAsync<Seq<string>>(async () =>
            {
                var all = await loc.AllInnerTextsAsync().ConfigureAwait(false);
                return all.ToSeq();
            })
            select ts;

        /// <summary>
        /// Gets the value attribute of an input element
        /// </summary>
        /// <param name="selector">Element containing value</param>
        public static IsotopeAsync<string> value(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<string>(async () => await loc.InputValueAsync().ConfigureAwait(false))
            select v;

        /// <summary>
        /// Looks for a particular attribute on an existing element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="att">Attribute to look up</param>
        /// <returns>A string representing the attribute value</returns>
        public static IsotopeAsync<string> attribute(Select selector, string att) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<string>(async () => await loc.GetAttributeAsync(att).ConfigureAwait(false))
            from r in v != null
                          ? pure(v)
                          : fail<string>($"Attribute '{att}' not found on element: {selector}")
            select r;

        /// <summary>
        /// Looks for a particular attribute on an existing element, returning None if not found
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="att">Attribute to look up</param>
        /// <returns>Some(value) if the attribute exists, None otherwise</returns>
        public static IsotopeAsync<Option<string>> attributeOrNone(Select selector, string att) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<Option<string>>(async () => Optional(await loc.GetAttributeAsync(att).ConfigureAwait(false)))
            select v;

        /// <summary>
        /// Finds an element by a selector and checks if it is currently displayed
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>True if the element is currently displayed</returns>
        public static IsotopeAsync<bool> displayed(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<bool>(async () => await loc.IsVisibleAsync().ConfigureAwait(false))
            select v;

        /// <summary>
        /// Finds an element by a selector and checks if it is currently enabled
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>True if the element is currently enabled</returns>
        public static IsotopeAsync<bool> enabled(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<bool>(async () => await loc.IsEnabledAsync().ConfigureAwait(false))
            select v;

        /// <summary>
        /// Gets the computed value of a CSS style property for an element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="style">CSS property name (e.g. "color", "zIndex", "display")</param>
        /// <returns>The computed style value as a string</returns>
        public static IsotopeAsync<string> getStyle(Select selector, string style) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<string>(async () =>
                await loc.EvaluateAsync<string>("(el, s) => getComputedStyle(el)[s]", style).ConfigureAwait(false))
            select v;

        /// <summary>
        /// Gets the computed z-index of an element as an integer
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>The z-index value, or 0 if "auto"</returns>
        public static IsotopeAsync<int> getZIndex(Select selector) =>
            from s in getStyle(selector, "zIndex")
            select int.TryParse(s, out var z) ? z : 0;

        /// <summary>
        /// Checks whether the element is obscured by another element at its center point
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>True if the element is obscured by another element</returns>
        public static IsotopeAsync<bool> obscured(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<bool>(async () =>
                await loc.EvaluateAsync<bool>(@"el => {
                    const rect = el.getBoundingClientRect();
                    const cx = rect.left + rect.width / 2;
                    const cy = rect.top + rect.height / 2;
                    const top = document.elementFromPoint(cx, cy);
                    return top !== el && !el.contains(top);
                }").ConfigureAwait(false))
            select v;

        /// <summary>
        /// Checks whether a checkbox or radio button element is checked
        /// </summary>
        /// <param name="selector">Checkbox or radio button selector</param>
        /// <returns>True if the element is checked</returns>
        public static IsotopeAsync<bool> isCheckboxChecked(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<bool>(async () => await loc.IsCheckedAsync().ConfigureAwait(false))
            select v;

        /// <summary>
        /// Gets the visible text of the currently selected option in a select element
        /// </summary>
        /// <param name="selector">Select element selector</param>
        /// <returns>The text of the selected option</returns>
        public static IsotopeAsync<string> getSelectedOptionText(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<string>(async () =>
                await loc.EvaluateAsync<string>("el => el.options[el.selectedIndex].text").ConfigureAwait(false))
            select v;

        /// <summary>
        /// Gets the value of the currently selected option in a select or input element
        /// </summary>
        /// <param name="selector">Select or input element selector</param>
        /// <returns>The value of the selected option or input</returns>
        public static IsotopeAsync<string> getSelectedOptionValue(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<string>(async () => await loc.InputValueAsync().ConfigureAwait(false))
            select v;

        /// <summary>
        /// Gets the full HTML source of the current page
        /// </summary>
        public static IsotopeAsync<string> pageSource =>
            from p in page
            from v in isoAsync<string>(async () => await p.ContentAsync().ConfigureAwait(false))
            select v;



        /// <summary>
        /// Evaluates a JavaScript expression in the page context and returns the result
        /// </summary>
        /// <typeparam name="T">Expected return type</typeparam>
        /// <param name="js">JavaScript expression to evaluate</param>
        /// <returns>The result of the JavaScript evaluation</returns>
        public static IsotopeAsync<T> eval<T>(string js) =>
            from p in page
            from v in isoAsync<T>(async () => await p.EvaluateAsync<T>(js).ConfigureAwait(false))
            select v;

        /// <summary>
        /// Evaluates a JavaScript expression in the page context, discarding the result
        /// </summary>
        /// <param name="js">JavaScript expression to evaluate</param>
        public static IsotopeAsync<Unit> eval(string js) =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.EvaluateAsync(js).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Evaluates a JavaScript expression with an element as the first argument and returns the result.
        /// The element is passed as the first argument to the JavaScript function.
        /// </summary>
        /// <typeparam name="T">Expected return type</typeparam>
        /// <param name="selector">Web element selector</param>
        /// <param name="js">JavaScript expression to evaluate (receives the element as first arg)</param>
        /// <returns>The result of the JavaScript evaluation</returns>
        public static IsotopeAsync<T> eval<T>(Select selector, string js) =>
            from loc in selector.ToIsotopeLocator()
            from v in isoAsync<T>(async () => await loc.EvaluateAsync<T>(js).ConfigureAwait(false))
            select v;

        /// <summary>
        /// Evaluates a JavaScript expression in the page context and returns the result.
        /// Catches any exception and converts it to an Isotope error.
        /// </summary>
        /// <typeparam name="T">Expected return type</typeparam>
        /// <param name="js">JavaScript expression to evaluate</param>
        /// <returns>The result of the JavaScript evaluation</returns>
        public static IsotopeAsync<T> evalSafe<T>(string js) =>
            new IsotopeAsync<T>(async state =>
            {
                var pState = await page.Invoke(state).ConfigureAwait(false);
                if (pState.IsFaulted) return pState.CastError<T>();
                try
                {
                    var result = await pState.Value.EvaluateAsync<T>(js).ConfigureAwait(false);
                    return new IsotopeState<T>(result, pState.State);
                }
                catch (Exception ex)
                {
                    return new IsotopeState<T>(default, pState.State.AddError(Error.New($"JavaScript evaluation failed: {ex.Message}", ex)));
                }
            });

        /// <summary>
        /// Evaluates a JavaScript expression in the page context, discarding the result.
        /// Catches any exception and converts it to an Isotope error.
        /// </summary>
        /// <param name="js">JavaScript expression to evaluate</param>
        public static IsotopeAsync<Unit> evalSafe(string js) =>
            new IsotopeAsync<Unit>(async state =>
            {
                var pState = await page.Invoke(state).ConfigureAwait(false);
                if (pState.IsFaulted) return pState.CastError<Unit>();
                try
                {
                    await pState.Value.EvaluateAsync(js).ConfigureAwait(false);
                    return new IsotopeState<Unit>(unit, pState.State);
                }
                catch (Exception ex)
                {
                    return new IsotopeState<Unit>(default, pState.State.AddError(Error.New($"JavaScript evaluation failed: {ex.Message}", ex)));
                }
            });

        /// <summary>
        /// Evaluates a JavaScript expression with an element as the first argument and returns the result.
        /// Catches any exception and converts it to an Isotope error.
        /// </summary>
        /// <typeparam name="T">Expected return type</typeparam>
        /// <param name="selector">Web element selector</param>
        /// <param name="js">JavaScript expression to evaluate (receives the element as first arg)</param>
        /// <returns>The result of the JavaScript evaluation</returns>
        public static IsotopeAsync<T> evalSafe<T>(Select selector, string js) =>
            new IsotopeAsync<T>(async state =>
            {
                var locState = await selector.ToIsotopeLocator().Invoke(state).ConfigureAwait(false);
                if (locState.IsFaulted) return locState.CastError<T>();
                try
                {
                    var result = await locState.Value.EvaluateAsync<T>(js).ConfigureAwait(false);
                    return new IsotopeState<T>(result, locState.State);
                }
                catch (Exception ex)
                {
                    return new IsotopeState<T>(default, locState.State.AddError(Error.New($"JavaScript evaluation failed: {ex.Message}", ex)));
                }
            });


        /// <summary>
        /// Sets the browser viewport size
        /// </summary>
        /// <param name="w">Width in pixels</param>
        /// <param name="h">Height in pixels</param>
        public static IsotopeAsync<Unit> setWindowSize(int w, int h) =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.SetViewportSizeAsync(w, h).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Sets the browser viewport size
        /// </summary>
        /// <param name="size">Size in pixels</param>
        public static IsotopeAsync<Unit> setWindowSize(Size size) =>
            setWindowSize(size.Width, size.Height);

        /// <summary>
        /// Gets the current browser viewport size
        /// </summary>
        /// <returns>The viewport size as a System.Drawing.Size</returns>
        public static IsotopeAsync<Size> getWindowSize =>
            from p in page
            select p.ViewportSize != null
                       ? new Size(p.ViewportSize.Width, p.ViewportSize.Height)
                       : Size.Empty;

        /// <summary>
        /// Scrolls an element into view if it is not already visible
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static IsotopeAsync<Unit> scrollToElement(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.ScrollIntoViewIfNeededAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Scrolls the page by the specified number of pixels
        /// </summary>
        /// <param name="x">Horizontal pixels to scroll</param>
        /// <param name="y">Vertical pixels to scroll</param>
        public static IsotopeAsync<Unit> scrollBy(int x, int y) =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.EvaluateAsync($"window.scrollBy({x}, {y})").ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Scrolls the page to the top
        /// </summary>
        public static IsotopeAsync<Unit> scrollToTop =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.EvaluateAsync("window.scrollTo(0, 0)").ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Scrolls the page to the bottom
        /// </summary>
        public static IsotopeAsync<Unit> scrollToBottom =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)").ConfigureAwait(false);
                return unit;
            })
            select unit;


        /// <summary>
        /// Opens a new tab (page) in the current browser context and sets it as the active page
        /// </summary>
        public static IsotopeAsync<Unit> newTab =>
            from ctx in browserContext
            from pg in isoAsync<IPage>(async () => await ctx.NewPageAsync().ConfigureAwait(false))
            from _ in setPage(pg)
            select unit;

        /// <summary>
        /// Switches to the tab (page) at the specified index in the current browser context
        /// </summary>
        /// <param name="position">Zero-based index of the tab to switch to</param>
        public static IsotopeAsync<Unit> switchTabs(int position) =>
            from ctx in browserContext
            from _ in ctx.Pages.Count > position && position >= 0
                          ? setPage(ctx.Pages[position])
                          : fail<Unit>($"Tab index {position} is out of range. There are {ctx.Pages.Count} tabs open.")
            select unit;

        /// <summary>
        /// Closes the current tab (page) and switches to the last remaining page in the context
        /// </summary>
        public static IsotopeAsync<Unit> closeTab =>
            from p in page
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await p.CloseAsync().ConfigureAwait(false);
                return unit;
            })
            from __ in ctx.Pages.Count > 0
                           ? setPage(ctx.Pages[ctx.Pages.Count - 1])
                           : clearPage()
            select unit;

        /// <summary>
        /// Gets the number of open tabs (pages) in the current browser context
        /// </summary>
        public static IsotopeAsync<int> getOpenedTabsCount =>
            from ctx in browserContext
            select ctx.Pages.Count;

        /// <summary>
        /// Gets the zero-based index of the current tab (page) in the browser context.
        /// Returns -1 if the page is not found.
        /// </summary>
        public static IsotopeAsync<int> getCurrentTabNumber =>
            from p in page
            from ctx in browserContext
            let pages = ctx.Pages
            let idx = Enumerable.Range(0, pages.Count).Where(i => ReferenceEquals(pages[i], p)).HeadOrNone()
            select idx.IfNone(-1);

        /// <summary>
        /// Opens a new browser window (new context + page) and sets it as active.
        /// This creates a completely isolated browsing session (separate cookies, storage, etc.).
        /// </summary>
        public static IsotopeAsync<Unit> newWindow =>
            from bro in browser
            from ctx in isoAsync<IBrowserContext>(async () => await bro.NewContextAsync().ConfigureAwait(false))
            from pg in isoAsync<IPage>(async () => await ctx.NewPageAsync().ConfigureAwait(false))
            from _1 in setBrowserContext(ctx)
            from _2 in setPage(pg)
            select unit;


        /// <summary>
        /// Run a computation scoped to an iframe. All element operations within <paramref name="ma"/>
        /// are rooted in the frame matched by <paramref name="frameSelector"/> rather than the page.
        /// Nesting <c>inFrame</c> calls targets nested iframes.
        /// </summary>
        /// <remarks>
        /// Playwright uses FrameLocator for scoped frame access rather than a context-switching model.
        /// This combinator adapts that model to work transparently with the existing Select / Isotope
        /// combinator API.
        /// </remarks>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <param name="frameSelector">Selector for the iframe element (e.g. <c>css("#my-iframe")</c>)</param>
        /// <param name="ma">Computation to run within the frame scope</param>
        /// <returns>Result of the inner computation</returns>
        public static IsotopeAsync<A> inFrame<A>(Select frameSelector, IsotopeAsync<A> ma) =>
            new IsotopeAsync<A>(async state =>
            {
                var pageOpt = state.Page;
                if (pageOpt.IsNone)
                    return new IsotopeState<A>(default, state.AddError(Error.New("No page available for frame selection")));

                var p = pageOpt.IfNone(() => throw new InvalidOperationException());
                var sel = frameSelector.PrimarySelector;

                // Build the frame locator from the current root
                var frame = state.FrameScope.IsEmpty
                    ? p.FrameLocator(sel)
                    : state.FrameScope.Peek().FrameLocator(sel);

                // Push frame onto scope stack and run the inner computation
                var scopedState = state.With(FrameScope: state.FrameScope.Push(frame));
                var result = await ma.Invoke(scopedState).ConfigureAwait(false);

                // Restore original frame scope
                return new IsotopeState<A>(result.Value, result.State.With(FrameScope: state.FrameScope));
            });


        /// <summary>
        /// Gets all cookies from the current browser context
        /// </summary>
        public static IsotopeAsync<Seq<BrowserCookie>> getCookies =>
            from ctx in browserContext
            from cookies in isoAsync<Seq<BrowserCookie>>(async () =>
            {
                var raw = await ctx.CookiesAsync().ConfigureAwait(false);
                return raw.ToSeq().Map(c => new BrowserCookie(
                    c.Name,
                    c.Value,
                    c.Domain,
                    c.Path,
                    c.Expires > 0 ? DateTimeOffset.FromUnixTimeSeconds((long)c.Expires).UtcDateTime : (DateTime?)null,
                    c.Secure,
                    c.HttpOnly,
                    c.SameSite.ToString()));
            })
            select cookies;

        /// <summary>
        /// Sets a cookie in the current browser context
        /// </summary>
        /// <param name="cookie">Cookie to set</param>
        public static IsotopeAsync<Unit> setCookie(BrowserCookie cookie) =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.AddCookiesAsync(new[]
                {
                    new Cookie
                    {
                        Name     = cookie.Name,
                        Value    = cookie.Value,
                        Domain   = cookie.Domain,
                        Path     = cookie.Path,
                        Expires  = cookie.Expiry.HasValue
                                       ? new DateTimeOffset(cookie.Expiry.Value).ToUnixTimeSeconds()
                                       : -1,
                        Secure   = cookie.Secure,
                        HttpOnly = cookie.HttpOnly,
                        SameSite = Enum.TryParse<SameSiteAttribute>(cookie.SameSite, true, out var ss)
                                       ? ss
                                       : SameSiteAttribute.None
                    }
                }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Deletes a cookie by name from the current browser context
        /// </summary>
        /// <param name="cookieName">Name of the cookie to delete</param>
        public static IsotopeAsync<Unit> deleteCookie(string cookieName) =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.ClearCookiesAsync(new BrowserContextClearCookiesOptions { Name = cookieName }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Deletes all cookies from the current browser context
        /// </summary>
        public static IsotopeAsync<Unit> deleteAllCookies =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.ClearCookiesAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;


        /// <summary>
        /// Takes a screenshot of the current page
        /// </summary>
        /// <returns>Some(BrowserScreenshot) on success, None on failure</returns>
        public static IsotopeAsync<Option<BrowserScreenshot>> getScreenshot =>
            from p in page
            from data in isoAsync<byte[]>(async () => await p.ScreenshotAsync().ConfigureAwait(false))
            select data != null && data.Length > 0
                       ? Some(new BrowserScreenshot(data))
                       : Option<BrowserScreenshot>.None;

        /// <summary>
        /// Takes a screenshot of a specific element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>Some(BrowserScreenshot) on success, None on failure</returns>
        public static IsotopeAsync<Option<BrowserScreenshot>> getElementScreenshot(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from data in isoAsync<byte[]>(async () => await loc.ScreenshotAsync().ConfigureAwait(false))
            select data != null && data.Length > 0
                       ? Some(new BrowserScreenshot(data))
                       : Option<BrowserScreenshot>.None;

        /// <summary>
        /// Takes a screenshot of the current page and saves it to a file
        /// </summary>
        /// <param name="path">File path to save the screenshot</param>
        public static IsotopeAsync<Unit> saveScreenshot(string path) =>
            from p in page
            from _ in isoAsync<Unit>(async () =>
            {
                await p.ScreenshotAsync(new PageScreenshotOptions { Path = path }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Takes a screenshot of a specific element and saves it to a file
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="path">File path to save the screenshot</param>
        public static IsotopeAsync<Unit> saveElementScreenshot(Select selector, string path) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.ScreenshotAsync(new LocatorScreenshotOptions { Path = path }).ConfigureAwait(false);
                return unit;
            })
            select unit;


        /// <summary>
        /// Waits until an element is visible and enabled (clickable).
        /// </summary>
        /// <remarks>
        /// In Playwright, most actions auto-wait for actionability, making this largely redundant.
        /// Provided for compatibility with the Isotope80 API.
        /// </remarks>
        /// <param name="selector">Web element selector</param>
        public static IsotopeAsync<Unit> waitUntilClickable(Select selector) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible }).ConfigureAwait(false);
                var isEnabled = await loc.IsEnabledAsync().ConfigureAwait(false);
                if (!isEnabled) throw new Exception("Element is visible but not enabled");
                return unit;
            })
            select unit;

        /// <summary>
        /// Waits until an element is visible and enabled (clickable) with a specified timeout.
        /// </summary>
        /// <remarks>
        /// In Playwright, most actions auto-wait for actionability, making this largely redundant.
        /// Provided for compatibility with the Isotope80 API.
        /// </remarks>
        /// <param name="selector">Web element selector</param>
        /// <param name="timeout">Maximum time to wait</param>
        public static IsotopeAsync<Unit> waitUntilClickable(Select selector, TimeSpan timeout) =>
            from loc in selector.ToIsotopeLocator()
            from _ in isoAsync<Unit>(async () =>
            {
                await loc.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = (float)timeout.TotalMilliseconds
                }).ConfigureAwait(false);
                var isEnabled = await loc.IsEnabledAsync().ConfigureAwait(false);
                if (!isEnabled) throw new Exception("Element is visible but not enabled");
                return unit;
            })
            select unit;


        /// <summary>
        /// Compares the text of an element with a string
        /// </summary>
        /// <param name="element">Element to compare</param>
        /// <param name="comparison">String to match</param>
        /// <returns>Fails if no match, with a contextual error</returns>
        public static IsotopeAsync<Unit> hasText(Select element, string comparison) =>
            from t in text(element)
            from r in t == comparison
                          ? unitM
                          : fail($"Element text doesn't match.  \"{t}\" <> \"{comparison}\"")
            select r;


        /// <summary>
        /// Run the isotope provided with Chromium browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<A> withChromium<A>(IsotopeAsync<A> ma) =>
            withChromium(ma, null, null);

        /// <summary>
        /// Run the isotope provided with Chromium browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<A> withChromium<A>(IsotopeAsync<A> ma, BrowserTypeLaunchOptions options) =>
            withChromium(ma, options, null);

        /// <summary>
        /// Run the isotope provided with Chromium browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<A> withChromium<A>(IsotopeAsync<A> ma, BrowserTypeLaunchOptions launchOptions, BrowserNewContextOptions contextOptions) =>
            context("Chromium", withBrowser(ma, pw => pw.Chromium, launchOptions, contextOptions));

        /// <summary>
        /// Run the isotope provided with Chromium browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<Env, A> withChromium<Env, A>(IsotopeAsync<Env, A> ma) =>
            withChromium(ma, null, null);

        /// <summary>
        /// Run the isotope provided with Chromium browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<Env, A> withChromium<Env, A>(IsotopeAsync<Env, A> ma, BrowserTypeLaunchOptions options) =>
            withChromium(ma, options, null);

        /// <summary>
        /// Run the isotope provided with Chromium browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<Env, A> withChromium<Env, A>(IsotopeAsync<Env, A> ma, BrowserTypeLaunchOptions launchOptions, BrowserNewContextOptions contextOptions) =>
            context("Chromium", withBrowser(ma, pw => pw.Chromium, launchOptions, contextOptions));

        /// <summary>
        /// Run the isotope provided with Firefox browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<A> withFirefox<A>(IsotopeAsync<A> ma) =>
            withFirefox(ma, null, null);

        /// <summary>
        /// Run the isotope provided with Firefox browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<A> withFirefox<A>(IsotopeAsync<A> ma, BrowserTypeLaunchOptions options) =>
            withFirefox(ma, options, null);

        /// <summary>
        /// Run the isotope provided with Firefox browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<A> withFirefox<A>(IsotopeAsync<A> ma, BrowserTypeLaunchOptions launchOptions, BrowserNewContextOptions contextOptions) =>
            context("Firefox", withBrowser(ma, pw => pw.Firefox, launchOptions, contextOptions));

        /// <summary>
        /// Run the isotope provided with Firefox browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<Env, A> withFirefox<Env, A>(IsotopeAsync<Env, A> ma) =>
            withFirefox(ma, null, null);

        /// <summary>
        /// Run the isotope provided with Firefox browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<Env, A> withFirefox<Env, A>(IsotopeAsync<Env, A> ma, BrowserTypeLaunchOptions options) =>
            withFirefox(ma, options, null);

        /// <summary>
        /// Run the isotope provided with Firefox browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<Env, A> withFirefox<Env, A>(IsotopeAsync<Env, A> ma, BrowserTypeLaunchOptions launchOptions, BrowserNewContextOptions contextOptions) =>
            context("Firefox", withBrowser(ma, pw => pw.Firefox, launchOptions, contextOptions));

        /// <summary>
        /// Run the isotope provided with WebKit browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<A> withWebkit<A>(IsotopeAsync<A> ma) =>
            withWebkit(ma, null, null);

        /// <summary>
        /// Run the isotope provided with WebKit browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<A> withWebkit<A>(IsotopeAsync<A> ma, BrowserTypeLaunchOptions options) =>
            withWebkit(ma, options, null);

        /// <summary>
        /// Run the isotope provided with WebKit browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<A> withWebkit<A>(IsotopeAsync<A> ma, BrowserTypeLaunchOptions launchOptions, BrowserNewContextOptions contextOptions) =>
            context("WebKit", withBrowser(ma, pw => pw.Webkit, launchOptions, contextOptions));

        /// <summary>
        /// Run the isotope provided with WebKit browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<Env, A> withWebkit<Env, A>(IsotopeAsync<Env, A> ma) =>
            withWebkit(ma, null, null);

        /// <summary>
        /// Run the isotope provided with WebKit browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<Env, A> withWebkit<Env, A>(IsotopeAsync<Env, A> ma, BrowserTypeLaunchOptions options) =>
            withWebkit(ma, options, null);

        /// <summary>
        /// Run the isotope provided with WebKit browser (full lifecycle)
        /// </summary>
        public static IsotopeAsync<Env, A> withWebkit<Env, A>(IsotopeAsync<Env, A> ma, BrowserTypeLaunchOptions launchOptions, BrowserNewContextOptions contextOptions) =>
            context("WebKit", withBrowser(ma, pw => pw.Webkit, launchOptions, contextOptions));

        /// <summary>
        /// Internal helper that manages the full Playwright lifecycle
        /// </summary>
        static IsotopeAsync<A> withBrowser<A>(
            IsotopeAsync<A> ma,
            Func<IPlaywright, IBrowserType> browserTypeSelector,
            BrowserTypeLaunchOptions options,
            BrowserNewContextOptions contextOptions) =>
            new IsotopeAsync<A>(async state =>
            {
                var prevPage = state.Page;
                var prevCtx  = state.BrowserContext;
                var prevBro  = state.Browser;
                var prevPw   = state.Playwright;

                var pw = await Microsoft.Playwright.Playwright.CreateAsync().ConfigureAwait(false);
                try
                {
                    var bro = await browserTypeSelector(pw).LaunchAsync(options).ConfigureAwait(false);
                    try
                    {
                        var ctx = await bro.NewContextAsync(contextOptions).ConfigureAwait(false);
                        try
                        {
                            var pg = await ctx.NewPageAsync().ConfigureAwait(false);
                            var newState = state.With(Page: Some(pg), BrowserContext: Some(ctx), Browser: Some(bro), Playwright: Some(pw));
                            var result = await ma.Invoke(newState).ConfigureAwait(false);
                            var finalState = result.State.With(Page: prevPage, BrowserContext: prevCtx, Browser: prevBro, Playwright: prevPw);
                            return new IsotopeState<A>(result.Value, finalState);
                        }
                        finally { await ctx.CloseAsync().ConfigureAwait(false); }
                    }
                    finally { await bro.CloseAsync().ConfigureAwait(false); }
                }
                finally { pw.Dispose(); }
            });

        /// <summary>
        /// Internal helper that manages the full Playwright lifecycle (with environment)
        /// </summary>
        static IsotopeAsync<Env, A> withBrowser<Env, A>(
            IsotopeAsync<Env, A> ma,
            Func<IPlaywright, IBrowserType> browserTypeSelector,
            BrowserTypeLaunchOptions options,
            BrowserNewContextOptions contextOptions) =>
            new IsotopeAsync<Env, A>(async (env, state) =>
            {
                var prevPage = state.Page;
                var prevCtx  = state.BrowserContext;
                var prevBro  = state.Browser;
                var prevPw   = state.Playwright;

                var pw = await Microsoft.Playwright.Playwright.CreateAsync().ConfigureAwait(false);
                try
                {
                    var bro = await browserTypeSelector(pw).LaunchAsync(options).ConfigureAwait(false);
                    try
                    {
                        var ctx = await bro.NewContextAsync(contextOptions).ConfigureAwait(false);
                        try
                        {
                            var pg = await ctx.NewPageAsync().ConfigureAwait(false);
                            var newState = state.With(Page: Some(pg), BrowserContext: Some(ctx), Browser: Some(bro), Playwright: Some(pw));
                            var result = await ma.Invoke(env, newState).ConfigureAwait(false);
                            var finalState = result.State.With(Page: prevPage, BrowserContext: prevCtx, Browser: prevBro, Playwright: prevPw);
                            return new IsotopeState<A>(result.Value, finalState);
                        }
                        finally { await ctx.CloseAsync().ConfigureAwait(false); }
                    }
                    finally { await bro.CloseAsync().ConfigureAwait(false); }
                }
                finally { pw.Dispose(); }
            });

        /// <summary>
        /// Run the isotope provided with the page context
        /// </summary>
        /// <param name="pg">Page to use</param>
        /// <param name="ma">Computation to run</param>
        public static IsotopeAsync<A> withPage<A>(IPage pg, IsotopeAsync<A> ma) =>
            withPage(pg, ma, false);

        /// <summary>
        /// Run the isotope provided with the page context.
        /// When <paramref name="keepAlive"/> is true the page is not closed after the isotope completes,
        /// allowing the same page to be reused across multiple runs.
        /// </summary>
        /// <param name="pg">Page to use</param>
        /// <param name="ma">Computation to run</param>
        /// <param name="keepAlive">When true, the page is not closed after the isotope completes</param>
        public static IsotopeAsync<A> withPage<A>(IPage pg, IsotopeAsync<A> ma, bool keepAlive) =>
            new IsotopeAsync<A>(async state =>
            {
                var prevPage = state.Page;
                var newState = state.With(Page: Some(pg));
                try
                {
                    var result = await ma.Invoke(newState).ConfigureAwait(false);
                    // Restore previous page in state
                    return new IsotopeState<A>(result.Value, result.State.With(Page: prevPage));
                }
                finally
                {
                    if (!keepAlive)
                    {
                        await pg.CloseAsync().ConfigureAwait(false);
                    }
                }
            });

        /// <summary>
        /// Run the isotope provided with the browser context
        /// </summary>
        /// <param name="ctx">Browser context to use</param>
        /// <param name="ma">Computation to run</param>
        public static IsotopeAsync<A> withContext<A>(IBrowserContext ctx, IsotopeAsync<A> ma) =>
            new IsotopeAsync<A>(async state =>
            {
                var prevCtx = state.BrowserContext;
                var newState = state.With(BrowserContext: Some(ctx));
                try
                {
                    var result = await ma.Invoke(newState).ConfigureAwait(false);
                    return new IsotopeState<A>(result.Value, result.State.With(BrowserContext: prevCtx));
                }
                finally
                {
                    await ctx.CloseAsync().ConfigureAwait(false);
                }
            });

        /// <summary>
        /// Run the isotope computation against multiple browser types, collecting errors from all.
        /// </summary>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <param name="ma">Computation to run against each browser</param>
        /// <param name="browsers">Browser types to run against</param>
        /// <returns>Unit on success, or collected errors from all browsers</returns>
        public static IsotopeAsync<Unit> withBrowsers<A>(IsotopeAsync<A> ma, params BrowserSelect[] browsers) =>
            new IsotopeAsync<Unit>(async state =>
            {
                Seq<Error> errors = Empty;

                foreach (var bs in browsers)
                {
                    var wrapped = bs switch
                    {
                        BrowserSelect.Chromium => withChromium(ma),
                        BrowserSelect.Firefox  => withFirefox(ma),
                        BrowserSelect.Webkit   => withWebkit(ma),
                        _                      => throw new ArgumentOutOfRangeException(nameof(browsers), bs, "Unknown browser type")
                    };

                    var r = await wrapped.Invoke(state).ConfigureAwait(false);
                    errors = errors + r.State.Error;
                }

                return new IsotopeState<Unit>(unit, state.With(Error: errors));
            });

        /// <summary>
        /// Run the isotope computation against multiple browser types, collecting errors from all.
        /// </summary>
        public static IsotopeAsync<Env, Unit> withBrowsers<Env, A>(IsotopeAsync<Env, A> ma, params BrowserSelect[] browsers) =>
            new IsotopeAsync<Env, Unit>(async (env, state) =>
            {
                Seq<Error> errors = Empty;

                foreach (var bs in browsers)
                {
                    var wrapped = bs switch
                    {
                        BrowserSelect.Chromium => withChromium(ma),
                        BrowserSelect.Firefox  => withFirefox(ma),
                        BrowserSelect.Webkit   => withWebkit(ma),
                        _                      => throw new ArgumentOutOfRangeException(nameof(browsers), bs, "Unknown browser type")
                    };

                    var r = await wrapped.Invoke(env, state).ConfigureAwait(false);
                    errors = errors + r.State.Error;
                }

                return new IsotopeState<Unit>(unit, state.With(Error: errors));
            });


        /// <summary>
        /// Do while the condition is true, or it reaches max repeats (async variant)
        /// </summary>
        public static IsotopeAsync<A> doWhile<A>(
            IsotopeAsync<A> iso,
            Func<A, bool> condition,
            int maxRepeats = 100) =>
            new IsotopeAsync<A>(async state =>
            {
                var s = state;
                A value = default;
                for (var i = maxRepeats; i > 0; i--)
                {
                    var r = await iso.Invoke(s).ConfigureAwait(false);
                    s = r.State;
                    value = r.Value;

                    if (r.IsFaulted || !condition(r.Value))
                        return r;
                }
                return new IsotopeState<A>(value, s);
            });

        /// <summary>
        /// Run iso while the condition is true.  Fails if max-attempts are reached (async variant).
        /// </summary>
        public static IsotopeAsync<A> doWhileOrFail<A>(
            IsotopeAsync<A> iso,
            Func<A, bool> condition,
            int maxAttempts = 100) =>
            new IsotopeAsync<A>(async state =>
            {
                var s = state;
                for (var i = maxAttempts; i > 0; i--)
                {
                    var r = await iso.Invoke(s).ConfigureAwait(false);
                    s = r.State;

                    if (r.IsFaulted || !condition(r.Value))
                        return r;
                }
                return new IsotopeState<A>(default, s.AddError(Error.New("do while reached the max-attempts")));
            });

        /// <summary>
        /// Run an action that triggers a dialog, handle it (accept/dismiss), and return the dialog message.
        /// The dialog handler is registered before the action runs and removed after it completes.
        /// </summary>
        /// <typeparam name="A">Result type of the triggering action</typeparam>
        /// <param name="action">Action that triggers the dialog</param>
        /// <param name="accept">True to accept the dialog, false to dismiss it</param>
        /// <returns>The dialog message text</returns>
        public static IsotopeAsync<string> onDialog<A>(IsotopeAsync<A> action, bool accept = true) =>
            new IsotopeAsync<string>(async state =>
            {
                var pState = await page.Invoke(state).ConfigureAwait(false);
                if (pState.IsFaulted) return pState.CastError<string>();

                var p = pState.Value;
                var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

                async void handler(object sender, IDialog dialog)
                {
                    tcs.TrySetResult(dialog.Message);
                    if (accept)
                        await dialog.AcceptAsync().ConfigureAwait(false);
                    else
                        await dialog.DismissAsync().ConfigureAwait(false);
                }

                p.Dialog += handler;
                try
                {
                    var result = await action.Invoke(pState.State).ConfigureAwait(false);
                    // Give a short window for the dialog event to fire if it hasn't yet
                    var msg = tcs.Task.IsCompleted
                        ? tcs.Task.Result
                        : await Task.WhenAny(tcs.Task, Task.Delay(5000)).ConfigureAwait(false) == tcs.Task
                            ? tcs.Task.Result
                            : "";
                    return new IsotopeState<string>(msg, result.State);
                }
                finally
                {
                    p.Dialog -= handler;
                }
            });

        /// <summary>
        /// Run an action that triggers a prompt dialog, respond with the given text, and return the dialog message.
        /// </summary>
        /// <typeparam name="A">Result type of the triggering action</typeparam>
        /// <param name="action">Action that triggers the prompt dialog</param>
        /// <param name="promptResponse">Text to enter in the prompt dialog</param>
        /// <returns>The dialog message text</returns>
        public static IsotopeAsync<string> onDialogWithResponse<A>(IsotopeAsync<A> action, string promptResponse) =>
            new IsotopeAsync<string>(async state =>
            {
                var pState = await page.Invoke(state).ConfigureAwait(false);
                if (pState.IsFaulted) return pState.CastError<string>();

                var p = pState.Value;
                var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

                async void handler(object sender, IDialog dialog)
                {
                    tcs.TrySetResult(dialog.Message);
                    await dialog.AcceptAsync(promptResponse).ConfigureAwait(false);
                }

                p.Dialog += handler;
                try
                {
                    var result = await action.Invoke(pState.State).ConfigureAwait(false);
                    var msg = tcs.Task.IsCompleted
                        ? tcs.Task.Result
                        : await Task.WhenAny(tcs.Task, Task.Delay(5000)).ConfigureAwait(false) == tcs.Task
                            ? tcs.Task.Result
                            : "";
                    return new IsotopeState<string>(msg, result.State);
                }
                finally
                {
                    p.Dialog -= handler;
                }
            });

        /// <summary>
        /// Register a route handler for the given URL pattern.
        /// The handler receives each matching request and can fulfill, abort, or continue it.
        /// </summary>
        /// <param name="urlPattern">URL pattern to intercept (glob, regex, or predicate)</param>
        /// <param name="handler">Async handler that processes the route</param>
        public static IsotopeAsync<Unit> route(string urlPattern, Func<IRoute, Task> handler) =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.RouteAsync(urlPattern, handler).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Unregister a previously registered route handler for the given URL pattern
        /// </summary>
        /// <param name="urlPattern">URL pattern to unregister</param>
        public static IsotopeAsync<Unit> unroute(string urlPattern) =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.UnrouteAsync(urlPattern).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Run an action and wait for a matching network response.
        /// Returns the response that matches the URL pattern.
        /// </summary>
        /// <typeparam name="A">Result type of the triggering action</typeparam>
        /// <param name="urlPattern">URL pattern to wait for (substring match)</param>
        /// <param name="action">Action that triggers the network request</param>
        /// <returns>The matching response</returns>
        public static IsotopeAsync<IResponse> waitForResponse<A>(string urlPattern, IsotopeAsync<A> action) =>
            new IsotopeAsync<IResponse>(async state =>
            {
                var pState = await page.Invoke(state).ConfigureAwait(false);
                if (pState.IsFaulted) return pState.CastError<IResponse>();

                var p = pState.Value;
                var responseTask = p.WaitForResponseAsync(urlPattern);
                var actionResult = await action.Invoke(pState.State).ConfigureAwait(false);
                var response = await responseTask.ConfigureAwait(false);

                return new IsotopeState<IResponse>(response, actionResult.State);
            });

        /// <summary>
        /// Run an action and wait for a navigation to complete.
        /// Uses Playwright's WaitForURLAsync to detect when navigation finishes.
        /// </summary>
        /// <typeparam name="A">Result type of the triggering action</typeparam>
        /// <param name="action">Action that triggers the navigation</param>
        /// <param name="urlPattern">Optional URL pattern to wait for. If not specified, waits for any navigation.</param>
        /// <returns>Unit</returns>
        public static IsotopeAsync<Unit> waitForNavigation<A>(IsotopeAsync<A> action, string urlPattern = "**/*") =>
            new IsotopeAsync<Unit>(async state =>
            {
                var pState = await page.Invoke(state).ConfigureAwait(false);
                if (pState.IsFaulted) return pState.CastError<Unit>();

                var p = pState.Value;
                var waitTask = p.WaitForURLAsync(urlPattern);
                var actionResult = await action.Invoke(pState.State).ConfigureAwait(false);
                await waitTask.ConfigureAwait(false);

                return new IsotopeState<Unit>(unit, actionResult.State);
            });

        /// <summary>
        /// Start a Playwright trace for the current browser context.
        /// Traces capture screenshots, snapshots, and network activity for debugging.
        /// </summary>
        /// <param name="name">Name of the trace</param>
        /// <param name="screenshots">Whether to capture screenshots during tracing</param>
        /// <param name="snapshots">Whether to capture DOM snapshots during tracing</param>
        public static IsotopeAsync<Unit> startTrace(string name, bool screenshots = true, bool snapshots = true) =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.Tracing.StartAsync(new TracingStartOptions
                {
                    Name = name,
                    Screenshots = screenshots,
                    Snapshots = snapshots
                }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Stop the current trace and save it to a file
        /// </summary>
        /// <param name="outputPath">Path to save the trace file (.zip)</param>
        public static IsotopeAsync<Unit> stopTrace(string outputPath) =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.Tracing.StopAsync(new TracingStopOptions { Path = outputPath }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Stop the current trace without saving
        /// </summary>
        static IsotopeAsync<Unit> stopTraceNoSave =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.Tracing.StopAsync().ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Run a computation with tracing enabled. The trace is saved to the specified path on completion.
        /// </summary>
        /// <typeparam name="A">Result type</typeparam>
        /// <param name="tracePath">Path to save the trace file (.zip)</param>
        /// <param name="ma">Computation to trace</param>
        /// <returns>The result of the computation</returns>
        public static IsotopeAsync<A> withTrace<A>(string tracePath, IsotopeAsync<A> ma) =>
            from _1 in startTrace("trace", screenshots: true, snapshots: true)
            from r  in ma
            from _2 in stopTrace(tracePath)
            select r;

        /// <summary>
        /// Run a computation with tracing enabled. The trace is only saved if the computation fails.
        /// On success, the trace is discarded.
        /// </summary>
        /// <typeparam name="A">Result type</typeparam>
        /// <param name="traceDir">Directory to save the trace file on failure</param>
        /// <param name="ma">Computation to trace</param>
        /// <returns>The result of the computation</returns>
        public static IsotopeAsync<A> withTraceOnFailure<A>(string traceDir, IsotopeAsync<A> ma) =>
            new IsotopeAsync<A>(async state =>
            {
                var ctxState = await browserContext.Invoke(state).ConfigureAwait(false);
                if (ctxState.IsFaulted) return ctxState.CastError<A>();

                var ctx = ctxState.Value;
                await ctx.Tracing.StartAsync(new TracingStartOptions
                {
                    Name = "traceOnFailure",
                    Screenshots = true,
                    Snapshots = true
                }).ConfigureAwait(false);

                var result = await ma.Invoke(ctxState.State).ConfigureAwait(false);

                if (result.IsFaulted)
                {
                    var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff");
                    var tracePath = System.IO.Path.Combine(traceDir, $"trace_{timestamp}.zip");
                    await ctx.Tracing.StopAsync(new TracingStopOptions { Path = tracePath }).ConfigureAwait(false);
                }
                else
                {
                    await ctx.Tracing.StopAsync().ConfigureAwait(false);
                }

                return result;
            });

        /// <summary>
        /// Run a computation in a new, isolated browser context.
        /// Creates a fresh context with separate cookies, storage, etc.
        /// The context and its page are closed after the computation completes.
        /// </summary>
        /// <typeparam name="A">Result type</typeparam>
        /// <param name="ma">Computation to run in the new context</param>
        /// <returns>The result of the computation</returns>
        public static IsotopeAsync<A> withNewBrowserContext<A>(IsotopeAsync<A> ma) =>
            new IsotopeAsync<A>(async state =>
            {
                var broState = await browser.Invoke(state).ConfigureAwait(false);
                if (broState.IsFaulted) return broState.CastError<A>();

                var bro = broState.Value;
                var prevPage = state.Page;
                var prevCtx  = state.BrowserContext;

                var ctx = await bro.NewContextAsync().ConfigureAwait(false);
                try
                {
                    var pg = await ctx.NewPageAsync().ConfigureAwait(false);
                    var newState = broState.State.With(Page: Some(pg), BrowserContext: Some(ctx));
                    var result = await ma.Invoke(newState).ConfigureAwait(false);
                    return new IsotopeState<A>(result.Value, result.State.With(Page: prevPage, BrowserContext: prevCtx));
                }
                finally
                {
                    await ctx.CloseAsync().ConfigureAwait(false);
                }
            });

        /// <summary>
        /// Run a computation in a new, isolated browser context with custom options.
        /// Creates a fresh context with the specified options (viewport, user agent, locale, etc.).
        /// The context and its page are closed after the computation completes.
        /// </summary>
        /// <typeparam name="A">Result type</typeparam>
        /// <param name="ma">Computation to run in the new context</param>
        /// <param name="options">Browser context creation options</param>
        /// <returns>The result of the computation</returns>
        public static IsotopeAsync<A> withNewBrowserContext<A>(IsotopeAsync<A> ma, BrowserNewContextOptions options) =>
            new IsotopeAsync<A>(async state =>
            {
                var broState = await browser.Invoke(state).ConfigureAwait(false);
                if (broState.IsFaulted) return broState.CastError<A>();

                var bro = broState.Value;
                var prevPage = state.Page;
                var prevCtx  = state.BrowserContext;

                var ctx = await bro.NewContextAsync(options).ConfigureAwait(false);
                try
                {
                    var pg = await ctx.NewPageAsync().ConfigureAwait(false);
                    var newState = broState.State.With(Page: Some(pg), BrowserContext: Some(ctx));
                    var result = await ma.Invoke(newState).ConfigureAwait(false);
                    return new IsotopeState<A>(result.Value, result.State.With(Page: prevPage, BrowserContext: prevCtx));
                }
                finally
                {
                    await ctx.CloseAsync().ConfigureAwait(false);
                }
            });

        /// <summary>
        /// Run a computation while capturing browser console messages.
        /// Returns the computation result alongside all console messages captured during execution.
        /// </summary>
        /// <typeparam name="A">Result type</typeparam>
        /// <param name="ma">Computation to run</param>
        /// <returns>A tuple of the computation result and captured console log entries</returns>
        public static IsotopeAsync<(A Result, Seq<BrowserLogEntry> Logs)> withConsoleCapture<A>(IsotopeAsync<A> ma) =>
            new IsotopeAsync<(A, Seq<BrowserLogEntry>)>(async state =>
            {
                var pState = await page.Invoke(state).ConfigureAwait(false);
                if (pState.IsFaulted) return pState.CastError<(A, Seq<BrowserLogEntry>)>();

                var p = pState.Value;
                var logs = new ConcurrentBag<BrowserLogEntry>();

                void handler(object sender, IConsoleMessage msg)
                {
                    logs.Add(new BrowserLogEntry(msg.Text, msg.Type, DateTime.UtcNow));
                }

                p.Console += handler;
                try
                {
                    var result = await ma.Invoke(pState.State).ConfigureAwait(false);
                    var capturedLogs = logs.ToSeq();
                    return new IsotopeState<(A, Seq<BrowserLogEntry>)>(
                        (result.Value, capturedLogs),
                        result.State);
                }
                finally
                {
                    p.Console -= handler;
                }
            });

        /// <summary>
        /// Run an action that triggers a popup (new page) and return the new page.
        /// Registers a popup handler before running the action.
        /// </summary>
        /// <typeparam name="A">Result type of the triggering action</typeparam>
        /// <param name="triggerAction">Action that triggers the popup</param>
        /// <returns>The new popup page</returns>
        public static IsotopeAsync<IPage> waitForPopup<A>(IsotopeAsync<A> triggerAction) =>
            new IsotopeAsync<IPage>(async state =>
            {
                var pState = await page.Invoke(state).ConfigureAwait(false);
                if (pState.IsFaulted) return pState.CastError<IPage>();

                var p = pState.Value;
                var popupTask = p.WaitForPopupAsync();
                var actionResult = await triggerAction.Invoke(pState.State).ConfigureAwait(false);
                var popupPage = await popupTask.ConfigureAwait(false);

                return new IsotopeState<IPage>(popupPage, actionResult.State);
            });

        /// <summary>
        /// Switch to a page in the current browser context that matches the predicate.
        /// The matching page becomes the active page in state.
        /// </summary>
        /// <param name="predicate">Predicate to match against pages (e.g., by URL or title)</param>
        public static IsotopeAsync<Unit> switchToPage(Func<IPage, bool> predicate) =>
            from ctx in browserContext
            from pg in ctx.Pages.ToSeq().Find(predicate)
                          .Map(p => pure(p))
                          .IfNone(fail<IPage>("No page matched the predicate"))
            from _  in setPage(pg)
            select unit;

        /// <summary>
        /// Set the geolocation for the current browser context.
        /// Requires the "geolocation" permission to be granted.
        /// </summary>
        /// <param name="latitude">Latitude in degrees (-90 to 90)</param>
        /// <param name="longitude">Longitude in degrees (-180 to 180)</param>
        public static IsotopeAsync<Unit> setGeolocation(float latitude, float longitude) =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.SetGeolocationAsync(new Geolocation
                {
                    Latitude = latitude,
                    Longitude = longitude
                }).ConfigureAwait(false);
                return unit;
            })
            select unit;

        /// <summary>
        /// Grant permissions to the current browser context (e.g., "geolocation", "notifications")
        /// </summary>
        /// <param name="permissions">Permissions to grant</param>
        public static IsotopeAsync<Unit> grantPermissions(params string[] permissions) =>
            from ctx in browserContext
            from _ in isoAsync<Unit>(async () =>
            {
                await ctx.GrantPermissionsAsync(permissions).ConfigureAwait(false);
                return unit;
            })
            select unit;


    }
}
