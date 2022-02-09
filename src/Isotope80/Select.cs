using System.Collections.ObjectModel;
using static LanguageExt.Prelude;
using System.Collections.Generic;
using static Isotope80.Isotope;
using OpenQA.Selenium.Internal;
using LanguageExt.TypeClasses;
using System.Globalization;
using OpenQA.Selenium;
using System.Linq;
using LanguageExt;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using OpenQA.Selenium.Remote;

namespace Isotope80
{
    /// <summary>
    /// Select selector
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
        /// Identity select
        /// </summary>
        /// <remarks>By supporting identity and `+` Select becomes a monoid</remarks>
        public static readonly Select Identity =
            byCss("*");

        /// <summary>
        /// Create a Select from a By
        /// </summary>
        /// <param name="by">Selector arrows</param>
        /// <returns>Select</returns>
        public static Select fromBy(params By[] by) =>
            new Select(by.ToSeq().Map(SelectStepCon.ByStep).Strict());

        /// <summary>
        /// Select an element by identifier
        /// </summary>
        /// <param name="id">Identifier of the element</param>
        /// <returns>Select</returns>
        public static Select byId(string id) =>
            By.Id(id);

        /// <summary>
        /// Select elements by the text within a link
        /// </summary>
        /// <param name="linkTextToFind">Link text to find</param>
        /// <returns>Select</returns>
        public static Select byLinkText(string linkTextToFind) =>
            By.LinkText(linkTextToFind);

        /// <summary>
        /// Select elements by the text within a link
        /// </summary>
        /// <param name="linkTextToFind">Link text to find</param>
        /// <returns>Select</returns>
        public static Select byPartialLinkText(string linkTextToFind) =>
            By.PartialLinkText(linkTextToFind);

        /// <summary>
        /// Select elements by name attribute
        /// </summary>
        /// <param name="name">Name o</param>
        /// <returns>Select</returns>
        public static Select byName(string name) =>
            By.Name(name);

        /// <summary>
        /// Select elements using a CSS selector
        /// </summary>
        /// <param name="selector">CSS selector</param>
        /// <returns>Select</returns>
        public static Select byCss(string selector) =>
            By.CssSelector(selector);

        /// <summary>
        /// Select elements by tag-name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Select</returns>
        public static Select byTag(string tagName) =>
            By.TagName(tagName);

        /// <summary>
        /// Select elements by XPath
        /// </summary>
        /// <param name="xpath">XPath selector</param>
        /// <returns>Select</returns>
        public static Select byXPath(string xpath) =>
            By.XPath(xpath);

        /// <summary>
        /// Select elements by class
        /// </summary>
        /// <param name="className">Class selector</param>
        /// <returns>Select</returns>
        public static Select byClass(string className) =>
            By.ClassName(className);

        /// <summary>
        /// Select an item at a specific index
        /// </summary>
        /// <param name="ix"></param>
        public static Select atIndex(int ix) =>
            new Select(Seq1(SelectStepCon.IndexSelect(ix, $"[{ix}]")));
        
        /// <summary>
        /// Wait until element exists select
        /// </summary>
        /// <returns>Select</returns>
        public static readonly Select waitUntilExists =
            waitUntilExistsFor();
        
        /// <summary>
        /// Wait until element exists select
        /// </summary>
        /// <param name="interval">Optional interval between checks</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Select</returns>
        public static Select waitUntilExistsFor(Option<TimeSpan> wait = default, Option<TimeSpan> interval = default) =>
            waitUntil(es => es.IsEmpty
                                ? fail("No elements")
                                : pure(unit),
                      "waitUntilElementExists",
                      interval,
                      wait);
        
        /// <summary>
        /// Select must have at least one element
        /// </summary>
        /// <returns>Select</returns>
        public static Select whenAtLeastOne =
            filter(es => es.IsEmpty
                             ? fail("No elements: expected at least one element")
                             : pure(unit),
                   "head");

        /// <summary>
        /// Select the first element and only the first.  Multiple elements is failure 
        /// </summary>
        /// <returns>Select</returns>
        public static Select whenSingle =
            filter(es => es.IsEmpty
                             ? fail("No elements: expected one element")
                             : es.Tail.IsEmpty
                                 ? pure(unit)
                                 : fail("Too many elements: expected only one"),
                  "single");
        
        /// <summary>
        /// Filter with an Isotope: fail means filtered out, success means let through
        /// The error reported by the Isotope is used as the final error 
        /// </summary>
        static Select filter(Func<Seq<IWebElement>, Isotope<Unit>> f, string desc) => 
            new Select(Seq1(SelectStepCon.FilterM(f, desc)));
        
        /// <summary>
        /// Filter with an Isotope: fail means filtered out, success means let through
        /// The error reported by the Isotope is used as the final error
        ///
        /// Differs from filter in that it retries until timeout 
        /// </summary>
        static Select waitUntil(Func<Seq<IWebElement>, Isotope<Unit>> f, string desc, Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) => 
            new Select(Seq1(SelectStepCon.WaitM(f, desc, interval, wait)));
        
        /// <summary>
        /// Conversion operator 
        /// </summary>
        public static implicit operator Select(By by) =>
            new Select(Seq1(SelectStepCon.ByStep(by)));

        /// <summary>
        /// Associative select composition
        /// </summary>
        /// <remarks>Selects can be composed associatively. The left-hand-side select gets refined by the right-hand-side.</remarks>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
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
        /// <param name="name">Name o</param>
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
        /// Maps the select to a runnable Isotope computation that returns the first item.  If there's 0 or more than 1
        /// item then it fails.
        /// </summary>
        /// <returns></returns>
        internal Isotope<IWebElement> ToIsotope1() =>
            ToIsotope().Bind(es => es.Count switch
                                   {
                                       0 => fail("Element not found"),
                                       1 => pure(es.Head),
                                       _ => fail("More than one element found that matches the selector")
                                   });

        /// <summary>
        /// Maps the select to a runnable Isotope computation that returns the first item or fails
        /// </summary>
        /// <returns></returns>
        internal Isotope<IWebElement> ToIsotopeHead() =>
            ToIsotope().Bind(es => es.Count switch
                                   {
                                       0 => fail("Element not found"),
                                       _ => pure(es.Head)
                                   });

        /// <summary>
        /// Maps the select to a runnable Isotope computation that returns the first item or fails
        /// </summary>
        /// <returns></returns>
        internal Isotope<Option<IWebElement>> ToIsotopeHeadOrNone() =>
            ToIsotope().Bind(es => es.Count switch
                                   {
                                       0 => pure<Option<IWebElement>>(None),
                                       _ => pure(Some(es.Head))
                                   });

        /// <summary>
        /// Maps the select to a runnable Isotope computation
        /// </summary>
        /// <returns></returns>
        internal Isotope<Seq<IWebElement>> ToIsotope()
        {
            var ma = pure(Option<Seq<IWebElement>>.None);
            
            foreach (var arr in arrows)
            {
                if (arr is ByStep byStep)
                {
                    ma = from op in ma
                         from rs in op.Match(
                             
                             // If we haven't selected anything yet, start by selecting via the web-driver using the `by`
                             None: () => from dr in webDriver
                                         from es in iso(() => dr.FindElements(byStep.Value).ToSeq().Strict())
                                         select Some(es),
                             
                             // If we have selected something, let's use that as a filter going forward
                             Some: ws => ws.IsEmpty
                                         
                                             // Nothing selected, so short cut
                                             ? pure(Some(ws))
                                             
                                             // Apply the `by` to each selected element 
                                             : from r in ws.Fold(
                                                   pure(Seq<IWebElement>()), 
                                                   (s, w) =>
                                                        from os in s
                                                        from ns in iso(() => w.FindElements(byStep.Value).ToSeq().Strict())
                                                        select os + ns)
                                               select Some(r))
                         select rs;
                }
                else if (arr is WaitM waitM)
                {
                    ma = Isotope.waitUntil(
                        from a in ma
                        from r in a.Match(
                            None: fail("`waitUntil` must follow something that queries elements.  It can't run alone"),
                            Some: waitM.Ma)
                        select a);
                }
                else if (arr is FilterM filterM)
                {
                    ma = from a in ma
                         from r in a.Match(
                            None: fail("filtering must follow something that queries elements.  It can't run alone"),
                            Some: filterM.Ma)
                         select a;
                }
                else if (arr is IndexSelect ix)
                {
                    ma = from a in ma
                         from r in a.Match(
                             None: fail("index selection must follow something that queries elements.  It can't run alone"),
                             Some: es => ix.Index < es.Count
                                             ? pure(Seq1(es[ix.Index]))
                                             : fail($"Index is out of range of the matching elements.  Only {es.Count} elements found, element at index {ix} requested;"))
                         select Some(r);
                }
            }

            return ma.Map(op => op.IfNone(Empty));
        }

        /// <summary>
        /// Maps the select to a runnable Isotope computation
        /// </summary>
        /// <returns></returns>
        public Isotope<Seq<WebElement>> ToSeq() =>
            from es in ToIsotope()
            from ie in es.Map(e => iso(() => (e.GetAttribute("id"), e)) | pure(("", e))).Sequence()
            from pe in pure(ie.Map(e => e.Add(GetElementIdField<IWebElement>.Invoke(e.Item2))))  
            select pe.Map<(string Id, IWebElement El, string ElId), WebElement>((ix, e) => 
                       WebElement.New(
                           this, 
                           ix, 
                           e.ElId,
                           e.Id,
                           e.El.TagName, 
                           e.El.Text, 
                           e.El.Enabled, 
                           e.El.Selected, 
                           e.El.Location, 
                           e.El.Size, 
                           e.El.Displayed))
                   .ToSeq()
                   .Strict();
        
        /// <summary>
        /// Migration of the IWebElement.PrettyPrint
        /// TODO: Make this a bit more elegant
        /// </summary>
        /// <returns></returns>
        public Isotope<string> PrettyPrint() =>
            ToIsotopeHead().Map(IsotopeInternal.prettyPrint);

        /// <summary>
        /// To string
        /// </summary>
        public override string ToString() =>
            String.Join(" → ", arrows.Map(arrow => arrow.Show()));
    }

    internal static class GetElementIdField<A> where A : IWebElement
    {
        static readonly Func<A, string> InvokeA;
            
        static GetElementIdField()
        {
            var dynamic = new DynamicMethod("GetElementIdField", typeof(string), new[] { typeof(A) }, true);
            var field   = typeof(A).GetTypeInfo().DeclaredFields.Filter(f => f.Name == "elementId").HeadOrNone();
            var il      = dynamic.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            field.IfSome(f => il.Emit(OpCodes.Ldfld, f));
            il.Emit(OpCodes.Ret);
 
            InvokeA = (Func<A, string>)dynamic.CreateDelegate(typeof(Func<A, string>));            
        }

        public static string Invoke(IWebElement element) =>
            InvokeA((A)element);
    }

    /// <summary>
    /// Step in a select
    /// </summary>
    [Union]
    internal interface SelectStep
    {
        /// <summary>
        /// Selector step - filters by `value`
        /// </summary>
        SelectStep ByStep(By value);
        
        /// <summary>
        /// Like a LINQ Where, but instead of bool return it returns an Isotope, which means we can propagate errors
        /// </summary>
        SelectStep FilterM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc);
        
        /// <summary>
        /// Like a LINQ Where, but instead of bool return it returns an Isotope, which means we can propagate errors
        /// It differs from FilterM in that it waits for a period of time, retrying, until a timeout 
        /// </summary>
        SelectStep WaitM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc,  Option<TimeSpan> interval = default, Option<TimeSpan> wait = default);
        
        /// <summary>
        /// Pick an item at an index within the result set.  If out of range `fail` is thrown
        /// </summary>
        SelectStep IndexSelect(int index, string desc);
    }
    
    internal static class SelectStepExt
    {
        internal static string Show(this SelectStep step) =>
            step switch
            {
                ByStep      (var value)                   => value.ToString(),
                FilterM     (var _, var d)                => d,
                WaitM       (var _, var d, var _, var _)  => d,
                IndexSelect (var _, var d)                => d,
                _                                         => throw new NotSupportedException()
            };
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

    internal class ByElementId : By
    {
        public ByElementId(IWebDriver wd, string eid) : base(
            context => new RemoteWebElement((RemoteWebDriver)wd, eid),
            context => new ReadOnlyCollection<IWebElement>(new List<IWebElement>() { new RemoteWebElement((RemoteWebDriver)wd, eid) }))
        {
        }
    }
}