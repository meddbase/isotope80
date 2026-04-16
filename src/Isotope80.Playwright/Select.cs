using System;
using System.Drawing;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.TypeClasses;
using Microsoft.Playwright;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;

namespace Isotope80
{
    /// <summary>
    /// Composable selector abstraction - a pipeline of steps that finds elements
    /// </summary>
    /// <remarks>
    /// Selects can be composed associatively. The left-hand-side select gets refined by the right-hand-side.
    /// Identity selects all.
    /// </remarks>
    public class Select
    {
        readonly Seq<SelectStep> arrows;

        /// <summary>
        /// Select
        /// </summary>
        Select(Seq<SelectStep> arrows) =>
            this.arrows = arrows;


        /// <summary>
        /// Identity select - selects all elements
        /// </summary>
        /// <remarks>By supporting identity and `+` Select becomes a monoid</remarks>
        public static readonly Select Identity =
            byCss("*");

        /// <summary>
        /// Selects the currently focused element
        /// </summary>
        public static readonly Select active =
            byCss(":focus");

        /// <summary>
        /// Select elements using a CSS selector
        /// </summary>
        /// <param name="selector">CSS selector</param>
        /// <returns>Select</returns>
        public static Select byCss(string selector) =>
            new Select(Seq1<SelectStep>(new LocatorStep(selector, $"css: {selector}")));

        /// <summary>
        /// Select elements by XPath
        /// </summary>
        /// <param name="xpath">XPath selector</param>
        /// <returns>Select</returns>
        public static Select byXPath(string xpath) =>
            new Select(Seq1<SelectStep>(new LocatorStep($"xpath={xpath}", $"xpath: {xpath}")));

        /// <summary>
        /// Select an element by identifier
        /// </summary>
        /// <param name="id">Identifier of the element</param>
        /// <returns>Select</returns>
        public static Select byId(string id) =>
            new Select(Seq1<SelectStep>(new LocatorStep($"#{id}", $"id: {id}")));

        /// <summary>
        /// Select elements by class
        /// </summary>
        /// <param name="className">Class selector</param>
        /// <returns>Select</returns>
        public static Select byClass(string className) =>
            new Select(Seq1<SelectStep>(new LocatorStep($".{className}", $"class: {className}")));

        /// <summary>
        /// Select elements by tag-name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Select</returns>
        public static Select byTag(string tagName) =>
            new Select(Seq1<SelectStep>(new LocatorStep(tagName, $"tag: {tagName}")));

        /// <summary>
        /// Select elements by name attribute
        /// </summary>
        /// <param name="name">Name attribute value</param>
        /// <returns>Select</returns>
        public static Select byName(string name) =>
            new Select(Seq1<SelectStep>(new LocatorStep($"[name='{name}']", $"name: {name}")));

        /// <summary>
        /// Select elements by the exact text within a link
        /// </summary>
        /// <param name="text">Link text to find</param>
        /// <returns>Select</returns>
        public static Select byLinkText(string text) =>
            new Select(Seq1<SelectStep>(new LocatorStep($"a:text-is(\"{text}\")", $"linkText: {text}")));

        /// <summary>
        /// Select elements by partial text within a link
        /// </summary>
        /// <param name="text">Partial link text to find</param>
        /// <returns>Select</returns>
        public static Select byPartialLinkText(string text) =>
            new Select(Seq1<SelectStep>(new LocatorStep($"a:has-text(\"{text}\")", $"partialLinkText: {text}")));

        /// <summary>
        /// Select an item at a specific index
        /// </summary>
        /// <param name="ix">Index to select</param>
        /// <returns>Select</returns>
        public static Select atIndex(int ix) =>
            new Select(Seq1<SelectStep>(new IndexSelect(ix, $"[{ix}]")));

        /// <summary>
        /// Select elements by ARIA role
        /// </summary>
        /// <param name="role">ARIA role to match</param>
        /// <returns>Select</returns>
        public static Select byRole(AriaRole role) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByRole(role),
                l => l.GetByRole(role),
                $"role: {role}")));

        /// <summary>
        /// Select elements by ARIA role and accessible name
        /// </summary>
        /// <param name="role">ARIA role to match</param>
        /// <param name="name">Accessible name to match</param>
        /// <returns>Select</returns>
        public static Select byRole(AriaRole role, string name) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByRole(role, new PageGetByRoleOptions { Name = name }),
                l => l.GetByRole(role, new LocatorGetByRoleOptions { Name = name }),
                $"role: {role} name: '{name}'")));

        /// <summary>
        /// Select elements by ARIA role and accessible name with exact matching control
        /// </summary>
        /// <param name="role">ARIA role to match</param>
        /// <param name="name">Accessible name to match</param>
        /// <param name="exact">Whether to match the name exactly</param>
        /// <returns>Select</returns>
        public static Select byRole(AriaRole role, string name, bool exact) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByRole(role, new PageGetByRoleOptions { Name = name, Exact = exact }),
                l => l.GetByRole(role, new LocatorGetByRoleOptions { Name = name, Exact = exact }),
                $"role: {role} name: '{name}' exact: {exact}")));

        /// <summary>
        /// Select elements by their associated label text
        /// </summary>
        /// <param name="text">Label text to match</param>
        /// <returns>Select</returns>
        public static Select byLabel(string text) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByLabel(text),
                l => l.GetByLabel(text),
                $"label: '{text}'")));

        /// <summary>
        /// Select elements by their associated label text with exact matching control
        /// </summary>
        /// <param name="text">Label text to match</param>
        /// <param name="exact">Whether to match the text exactly</param>
        /// <returns>Select</returns>
        public static Select byLabel(string text, bool exact) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByLabel(text, exact),
                l => l.GetByLabel(text, new LocatorGetByLabelOptions { Exact = exact }),
                $"label: '{text}' exact: {exact}")));

        /// <summary>
        /// Select elements by their text content
        /// </summary>
        /// <param name="text">Text content to match</param>
        /// <returns>Select</returns>
        public static Select byText(string text) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByText(text),
                l => l.GetByText(text),
                $"text: '{text}'")));

        /// <summary>
        /// Select elements by their text content with exact matching control
        /// </summary>
        /// <param name="text">Text content to match</param>
        /// <param name="exact">Whether to match the text exactly</param>
        /// <returns>Select</returns>
        public static Select byText(string text, bool exact) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByText(text, exact),
                l => l.GetByText(text, new LocatorGetByTextOptions { Exact = exact }),
                $"text: '{text}' exact: {exact}")));

        /// <summary>
        /// Select elements by their test ID attribute (data-testid by default)
        /// </summary>
        /// <param name="testId">Test ID to match</param>
        /// <returns>Select</returns>
        public static Select byTestId(string testId) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByTestId(testId),
                l => l.GetByTestId(testId),
                $"testId: '{testId}'")));

        /// <summary>
        /// Select elements by their placeholder text
        /// </summary>
        /// <param name="text">Placeholder text to match</param>
        /// <returns>Select</returns>
        public static Select byPlaceholder(string text) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByPlaceholder(text),
                l => l.GetByPlaceholder(text),
                $"placeholder: '{text}'")));

        /// <summary>
        /// Select elements by their placeholder text with exact matching control
        /// </summary>
        /// <param name="text">Placeholder text to match</param>
        /// <param name="exact">Whether to match the text exactly</param>
        /// <returns>Select</returns>
        public static Select byPlaceholder(string text, bool exact) =>
            new Select(Seq1<SelectStep>(new SemanticLocatorStep(
                r => r.GetByPlaceholder(text, exact),
                l => l.GetByPlaceholder(text, new LocatorGetByPlaceholderOptions { Exact = exact }),
                $"placeholder: '{text}' exact: {exact}")));


        /// <summary>
        /// Select must have at least one element
        /// </summary>
        /// <returns>Select</returns>
        public static Select whenAtLeastOne =
            filter(
                locator => isoAsync<Unit>(async () =>
                {
                    var count = await locator.CountAsync().ConfigureAwait(false);
                    if (count < 1) throw new Exception("No elements: expected at least one element");
                    return unit;
                }),
                "head");

        /// <summary>
        /// Select the first element and only the first.  Multiple elements is failure
        /// </summary>
        /// <returns>Select</returns>
        public static Select whenSingle =
            filter(
                locator => isoAsync<Unit>(async () =>
                {
                    var count = await locator.CountAsync().ConfigureAwait(false);
                    if (count == 0) throw new Exception("No elements: expected one element");
                    if (count > 1) throw new Exception("Too many elements: expected only one");
                    return unit;
                }),
                "single");

        /// <summary>
        /// Wait until element exists select
        /// </summary>
        /// <returns>Select</returns>
        public static readonly Select waitUntilExists =
            waitUntilExistsFor();

        /// <summary>
        /// Wait until element exists select
        /// </summary>
        /// <param name="interval">Optional interval between checks (unused - Playwright handles retry internally)</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Select</returns>
        public static Select waitUntilExistsFor(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            waitUntil(
                locator => isoAsync<Unit>(async () =>
                {
                    var timeout = wait.IfNone(TimeSpan.FromSeconds(10));
                    await locator.WaitForAsync(new LocatorWaitForOptions
                    {
                        State = WaitForSelectorState.Attached,
                        Timeout = (float)timeout.TotalMilliseconds
                    }).ConfigureAwait(false);
                    return unit;
                }),
                "waitUntilElementExists",
                interval,
                wait);

        /// <summary>
        /// Wait until no elements match the selector (or all matching elements are not displayed)
        /// </summary>
        /// <returns>Select</returns>
        public static readonly Select waitUntilNotExists =
            waitUntilNotExistsFor();

        /// <summary>
        /// Wait until no elements match the selector (or all matching elements are not displayed)
        /// </summary>
        /// <param name="interval">Optional interval between checks (unused - Playwright handles retry internally)</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Select</returns>
        public static Select waitUntilNotExistsFor(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            waitUntil(
                locator => isoAsync<Unit>(async () =>
                {
                    var timeout = wait.IfNone(TimeSpan.FromSeconds(10));
                    await locator.WaitForAsync(new LocatorWaitForOptions
                    {
                        State = WaitForSelectorState.Hidden,
                        Timeout = (float)timeout.TotalMilliseconds
                    }).ConfigureAwait(false);
                    return unit;
                }),
                "waitUntilElementNotExists",
                interval,
                wait);


        /// <summary>
        /// Filter with an IsotopeAsync: fail means filtered out, success means let through.
        /// The error reported by the IsotopeAsync is used as the final error.
        /// </summary>
        static Select filter(Func<ILocator, IsotopeAsync<Unit>> f, string desc) =>
            new Select(Seq1<SelectStep>(new FilterMStep(f, desc)));

        /// <summary>
        /// Filter with an IsotopeAsync: fail means filtered out, success means let through.
        /// The error reported by the IsotopeAsync is used as the final error.
        /// Differs from filter in that it represents a wait/retry semantic.
        /// </summary>
        static Select waitUntil(Func<ILocator, IsotopeAsync<Unit>> f, string desc, Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            new Select(Seq1<SelectStep>(new WaitMStep(f, desc)));


        /// <summary>
        /// Implicit conversion from a string CSS selector
        /// </summary>
        public static implicit operator Select(string selector) =>
            byCss(selector);


        /// <summary>
        /// Associative select composition
        /// </summary>
        /// <remarks>Selects can be composed associatively. The left-hand-side select gets refined by the right-hand-side.</remarks>
        public static Select operator +(Select lhs, Select rhs) =>
            new Select(lhs.arrows + rhs.arrows);


        /// <summary>
        /// Select an element by identifier
        /// </summary>
        /// <param name="id">Identifier of the element</param>
        /// <returns>Select</returns>
        public Select Id(string id) =>
            this + byId(id);

        /// <summary>
        /// Select elements by the text within a link
        /// </summary>
        /// <param name="linkTextToFind">Link text to find</param>
        /// <returns>Select</returns>
        public Select LinkText(string linkTextToFind) =>
            this + byLinkText(linkTextToFind);

        /// <summary>
        /// Select elements by the text within a link
        /// </summary>
        /// <param name="linkTextToFind">Link text to find</param>
        /// <returns>Select</returns>
        public Select PartialLinkText(string linkTextToFind) =>
            this + byPartialLinkText(linkTextToFind);

        /// <summary>
        /// Select elements by name attribute
        /// </summary>
        /// <param name="name">Name attribute value</param>
        /// <returns>Select</returns>
        public Select Name(string name) =>
            this + byName(name);

        /// <summary>
        /// Select elements using a CSS selector
        /// </summary>
        /// <param name="selector">CSS selector</param>
        /// <returns>Select</returns>
        public Select Css(string selector) =>
            this + byCss(selector);

        /// <summary>
        /// Select elements by tag-name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Select</returns>
        public Select Tag(string tagName) =>
            this + byTag(tagName);

        /// <summary>
        /// Select elements by XPath
        /// </summary>
        /// <param name="xpath">XPath selector</param>
        /// <returns>Select</returns>
        public Select XPath(string xpath) =>
            this + byXPath(xpath);

        /// <summary>
        /// Select elements by class
        /// </summary>
        /// <param name="className">Class selector</param>
        /// <returns>Select</returns>
        public Select Class(string className) =>
            this + byClass(className);

        /// <summary>
        /// Select elements by ARIA role
        /// </summary>
        /// <param name="role">ARIA role to match</param>
        /// <returns>Select</returns>
        public Select Role(AriaRole role) =>
            this + byRole(role);

        /// <summary>
        /// Select elements by ARIA role and accessible name
        /// </summary>
        /// <param name="role">ARIA role to match</param>
        /// <param name="name">Accessible name to match</param>
        /// <returns>Select</returns>
        public Select Role(AriaRole role, string name) =>
            this + byRole(role, name);

        /// <summary>
        /// Select elements by their associated label text
        /// </summary>
        /// <param name="text">Label text to match</param>
        /// <returns>Select</returns>
        public Select Label(string text) =>
            this + byLabel(text);

        /// <summary>
        /// Select elements by their text content
        /// </summary>
        /// <param name="text">Text content to match</param>
        /// <returns>Select</returns>
        public Select Text(string text) =>
            this + byText(text);

        /// <summary>
        /// Select elements by their test ID attribute
        /// </summary>
        /// <param name="testId">Test ID to match</param>
        /// <returns>Select</returns>
        public Select TestId(string testId) =>
            this + byTestId(testId);

        /// <summary>
        /// Select elements by their placeholder text
        /// </summary>
        /// <param name="text">Placeholder text to match</param>
        /// <returns>Select</returns>
        public Select Placeholder(string text) =>
            this + byPlaceholder(text);

        /// <summary>
        /// Select must have at least one matching element
        /// </summary>
        /// <returns>Select</returns>
        public Select AtLeastOne =>
            this + whenAtLeastOne;

        /// <summary>
        /// Select must have only one matching element
        /// </summary>
        /// <returns>Select</returns>
        public Select Single =>
            this + whenSingle;

        /// <summary>
        /// Select an item at a specific index
        /// </summary>
        /// <returns>Select</returns>
        public Select Index(int ix) =>
            this + atIndex(ix);

        /// <summary>
        /// Wait until element exists select
        /// </summary>
        /// <returns>Select</returns>
        public Select WaitUntilExists =>
            this + waitUntilExistsFor();

        /// <summary>
        /// Wait until element exists select
        /// </summary>
        /// <param name="interval">Optional interval between checks</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Select</returns>
        public Select WaitUntilExistsFor(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            this + waitUntilExistsFor(interval, wait);

        /// <summary>
        /// Wait until no elements match the selector (or all matching elements are not displayed)
        /// </summary>
        /// <returns>Select</returns>
        public Select WaitUntilNotExists =>
            this + waitUntilNotExistsFor();

        /// <summary>
        /// Wait until no elements match the selector (or all matching elements are not displayed)
        /// </summary>
        /// <param name="interval">Optional interval between checks</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Select</returns>
        public Select WaitUntilNotExistsFor(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            this + waitUntilNotExistsFor(interval, wait);


        /// <summary>
        /// Maps the select to a runnable IsotopeAsync computation that returns a Playwright ILocator.
        /// Locator construction is synchronous (locators are lazy queries) but filters and waits are async.
        /// </summary>
        internal IsotopeAsync<ILocator> ToIsotopeLocator()
        {
            IsotopeAsync<Option<ILocator>> ma = pure(Option<ILocator>.None);

            foreach (var step in arrows)
            {
                if (step is LocatorStep loc)
                {
                    ma = from op in ma
                         from rs in op.Match(
                             None: () => from root in locatorRoot
                                         select Some(root.Locator(loc.Selector)),
                             Some: parent => pure(Some(parent.Locator(loc.Selector))))
                         select rs;
                }
                else if (step is SemanticLocatorStep sem)
                {
                    ma = from op in ma
                         from rs in op.Match(
                             None: () => from root in locatorRoot
                                         select Some(sem.FromLocatorRoot(root)),
                             Some: parent => pure(Some(sem.FromLocator(parent))))
                         select rs;
                }
                else if (step is WaitMStep waitM)
                {
                    ma = from a in ma
                         from _ in a.Match(
                             None: () => fail<Unit>("`waitUntil` must follow something that queries elements.  It can't run alone"),
                             Some: locator => waitM.Ma(locator))
                         select a;
                }
                else if (step is FilterMStep filterM)
                {
                    ma = from a in ma
                         from _ in a.Match(
                             None: () => fail<Unit>("filtering must follow something that queries elements.  It can't run alone"),
                             Some: locator => filterM.Ma(locator))
                         select a;
                }
                else if (step is IndexSelect ix)
                {
                    ma = from a in ma
                         from r in a.Match(
                             None: () => fail<Option<ILocator>>("index selection must follow something that queries elements.  It can't run alone"),
                             Some: locator => pure(Some(locator.Nth(ix.Index))))
                         select r;
                }
            }

            return ma.Map(op => op.IfNone(() => throw new InvalidOperationException("Select has no steps")));
        }

        /// <summary>
        /// Maps the select to a runnable IsotopeAsync computation that returns the first/only element's locator.
        /// If there's 0 or more than 1 item then it fails.
        /// </summary>
        internal IsotopeAsync<ILocator> ToIsotope1() =>
            from loc in ToIsotopeLocator()
            from cnt in isoAsync<int>(async () => await loc.CountAsync().ConfigureAwait(false))
            from res in cnt switch
                        {
                            0 => fail<ILocator>("Element not found"),
                            1 => pure(loc),
                            _ => fail<ILocator>("More than one element found that matches the selector")
                        }
            select res;

        /// <summary>
        /// Maps the select to a runnable IsotopeAsync computation that returns the first element's locator or fails
        /// </summary>
        internal IsotopeAsync<ILocator> ToIsotopeHead() =>
            from loc in ToIsotopeLocator()
            from cnt in isoAsync<int>(async () => await loc.CountAsync().ConfigureAwait(false))
            from res in cnt switch
                        {
                            0 => fail<ILocator>("Element not found"),
                            _ => pure(loc.First)
                        }
            select res;

        /// <summary>
        /// Maps the select to a runnable IsotopeAsync computation that returns the first element's locator or None
        /// </summary>
        internal IsotopeAsync<Option<ILocator>> ToIsotopeHeadOrNone() =>
            from loc in ToIsotopeLocator()
            from cnt in isoAsync<int>(async () => await loc.CountAsync().ConfigureAwait(false))
            from res in cnt switch
                        {
                            0 => pure(Option<ILocator>.None),
                            _ => pure(Some(loc.First))
                        }
            select res;

        /// <summary>
        /// Maps the select to a runnable IsotopeAsync computation that returns WebElement snapshots
        /// </summary>
        public IsotopeAsync<Seq<WebElement>> ToSeq() =>
            from loc in ToIsotopeLocator()
            from cnt in isoAsync<int>(async () => await loc.CountAsync().ConfigureAwait(false))
            from elems in isoAsync<Seq<WebElement>>(async () =>
            {
                var result = Seq<WebElement>();
                for (var i = 0; i < cnt; i++)
                {
                    var el = loc.Nth(i);
                    var idAttr     = await el.GetAttributeAsync("id").ConfigureAwait(false) ?? "";
                    var valueAttr  = await el.GetAttributeAsync("value").ConfigureAwait(false) ?? "";
                    var titleAttr  = await el.GetAttributeAsync("title").ConfigureAwait(false) ?? "";
                    var tagName    = await el.EvaluateAsync<string>("e => e.tagName.toLowerCase()").ConfigureAwait(false) ?? "";
                    var text       = await el.InnerTextAsync().ConfigureAwait(false) ?? "";
                    var enabled    = await el.IsEnabledAsync().ConfigureAwait(false);
                    var visible    = await el.IsVisibleAsync().ConfigureAwait(false);
                    bool isChecked;
                    try { isChecked = await el.IsCheckedAsync().ConfigureAwait(false); }
                    catch { isChecked = false; }
                    var box        = await el.BoundingBoxAsync().ConfigureAwait(false);

                    result = result.Add(new WebElement(
                        this + atIndex(i),
                        i,
                        idAttr,
                        tagName,
                        text,
                        enabled,
                        isChecked,
                        box != null ? new Point((int)box.X, (int)box.Y) : Point.Empty,
                        box != null ? new Size((int)box.Width, (int)box.Height) : Size.Empty,
                        visible,
                        valueAttr,
                        titleAttr));
                }
                return result;
            })
            select elems;

        /// <summary>
        /// Pretty-print the first matching element for debugging
        /// </summary>
        public IsotopeAsync<string> PrettyPrint() =>
            from loc in ToIsotopeHead()
            from pp in isoAsync<string>(async () =>
            {
                var tag = await loc.EvaluateAsync<string>("e => e.tagName.toLowerCase()").ConfigureAwait(false) ?? "";
                var css = await loc.GetAttributeAsync("class").ConfigureAwait(false) ?? "";
                var id  = await loc.GetAttributeAsync("id").ConfigureAwait(false) ?? "";
                return $"<{tag} class='{css}' id='{id}'>";
            })
            select pp;

        /// <summary>
        /// Returns the raw Playwright selector string from the first LocatorStep,
        /// or "*" if the Select has no LocatorStep. Used internally for FrameLocator scoping.
        /// </summary>
        internal string PrimarySelector =>
            arrows.Find(a => a is LocatorStep)
                  .Map(a => ((LocatorStep)a).Selector)
                  .IfNone("*");

        /// <summary>
        /// To string
        /// </summary>
        public override string ToString() =>
            String.Join(" → ", arrows.Map(arrow => arrow.Show()));
    }


    /// <summary>
    /// Base type for steps in a select pipeline
    /// </summary>
    internal abstract class SelectStep
    {
        /// <summary>
        /// Display description of this step
        /// </summary>
        public abstract string Show();
    }

    /// <summary>
    /// Locator step - creates or refines a Playwright locator using a selector string
    /// </summary>
    internal sealed class LocatorStep : SelectStep
    {
        /// <summary>
        /// Playwright selector string (CSS, XPath, etc.)
        /// </summary>
        public readonly string Selector;

        /// <summary>
        /// Human-readable description
        /// </summary>
        public readonly string Description;

        public LocatorStep(string selector, string description)
        {
            Selector    = selector;
            Description = description;
        }

        public override string Show() => Description;
    }

    /// <summary>
    /// Filter step - validates the locator result set (e.g., whenSingle checks count == 1)
    /// </summary>
    internal sealed class FilterMStep : SelectStep
    {
        /// <summary>
        /// The validation function
        /// </summary>
        public readonly Func<ILocator, IsotopeAsync<Unit>> Ma;

        /// <summary>
        /// Human-readable description
        /// </summary>
        public readonly string Desc;

        public FilterMStep(Func<ILocator, IsotopeAsync<Unit>> ma, string desc)
        {
            Ma   = ma;
            Desc = desc;
        }

        public override string Show() => Desc;
    }

    /// <summary>
    /// Wait step - retries until condition met (e.g., waitUntilExists)
    /// </summary>
    internal sealed class WaitMStep : SelectStep
    {
        /// <summary>
        /// The wait/retry function
        /// </summary>
        public readonly Func<ILocator, IsotopeAsync<Unit>> Ma;

        /// <summary>
        /// Human-readable description
        /// </summary>
        public readonly string Desc;

        public WaitMStep(Func<ILocator, IsotopeAsync<Unit>> ma, string desc)
        {
            Ma   = ma;
            Desc = desc;
        }

        public override string Show() => Desc;
    }

    /// <summary>
    /// Index step - picks element at index within the result set
    /// </summary>
    internal sealed class IndexSelect : SelectStep
    {
        /// <summary>
        /// Index to select
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// Human-readable description
        /// </summary>
        public readonly string Desc;

        public IndexSelect(int index, string desc)
        {
            Index = index;
            Desc  = desc;
        }

        public override string Show() => Desc;
    }

    /// <summary>
    /// Semantic locator step - uses Playwright's semantic locator methods (GetByRole, GetByLabel, etc.)
    /// rather than CSS/XPath selector strings. These are methods on IPage/ILocator, not selector strings.
    /// </summary>
    internal sealed class SemanticLocatorStep : SelectStep
    {
        /// <summary>
        /// Factory function to create a locator from a locator root (page or frame)
        /// </summary>
        public readonly Func<LocatorRoot, ILocator> FromLocatorRoot;

        /// <summary>
        /// Factory function to create a locator from a parent locator (used when chained)
        /// </summary>
        public readonly Func<ILocator, ILocator> FromLocator;

        /// <summary>
        /// Human-readable description
        /// </summary>
        public readonly string Desc;

        public SemanticLocatorStep(Func<LocatorRoot, ILocator> fromLocatorRoot, Func<ILocator, ILocator> fromLocator, string desc)
        {
            FromLocatorRoot = fromLocatorRoot;
            FromLocator     = fromLocator;
            Desc            = desc;
        }

        public override string Show() => Desc;
    }


    /// <summary>
    /// Select semigroup instance
    /// </summary>
    public struct SemiSelect : Semigroup<Select>
    {
        /// <summary>
        /// Associative binary operator for select composition
        /// </summary>
        /// <param name="x">Left select</param>
        /// <param name="y">Right select</param>
        /// <returns>Composed select</returns>
        public Select Append(Select x, Select y) =>
            x + y;
    }

    /// <summary>
    /// Select monoid instance
    /// </summary>
    public struct MSelect : Monoid<Select>
    {
        /// <summary>
        /// Associative binary operator for select composition
        /// </summary>
        /// <param name="x">Left select</param>
        /// <param name="y">Right select</param>
        /// <returns>Composed select</returns>
        public Select Append(Select x, Select y) =>
            x + y;

        /// <summary>
        /// Monoidal unit.  For Select this selects all.
        /// </summary>
        /// <returns>Unit select</returns>
        public Select Empty() =>
            Select.Identity;
    }
}
