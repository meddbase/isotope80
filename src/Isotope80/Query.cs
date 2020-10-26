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
    /// Query selector
    /// </summary>
    /// <remarks>
    /// Queries can be composed associatively. The left-hand-side query gets refined by the right-hand-side.
    /// Identity selects all.
    /// </remarks>
    public class Query
    {
        readonly Seq<QueryStep> arrows;

        /// <summary>
        /// Query
        /// </summary>
        Query(Seq<QueryStep> arrows) =>
            this.arrows = arrows;

        /// <summary>
        /// Identity query
        /// </summary>
        /// <remarks>By supporting identity and `+` Query becomes a monoid</remarks>
        public static readonly Query Identity =
            byCss("*");

        /// <summary>
        /// Create a Query from a By
        /// </summary>
        /// <param name="by">Selector arrows</param>
        /// <returns>Query</returns>
        public static Query fromBy(params By[] by) =>
            new Query(by.ToSeq().Map(QueryStepCon.ByStep).Strict());

        /// <summary>
        /// Query an element by identifier
        /// </summary>
        /// <param name="id">Identifier of the element</param>
        /// <returns>Query</returns>
        public static Query byId(string id) =>
            By.Id(id);

        /// <summary>
        /// Query elements by the text within a link
        /// </summary>
        /// <param name="linkTextToFind">Link text to find</param>
        /// <returns>Query</returns>
        public static Query byLinkText(string linkTextToFind) =>
            By.LinkText(linkTextToFind);

        /// <summary>
        /// Query elements by the text within a link
        /// </summary>
        /// <param name="linkTextToFind">Link text to find</param>
        /// <returns>Query</returns>
        public static Query byPartialLinkText(string linkTextToFind) =>
            By.PartialLinkText(linkTextToFind);

        /// <summary>
        /// Query elements by name attribute
        /// </summary>
        /// <param name="name">Name o</param>
        /// <returns>Query</returns>
        public static Query byName(string name) =>
            By.Name(name);

        /// <summary>
        /// Query elements using a CSS selector
        /// </summary>
        /// <param name="selector">CSS selector</param>
        /// <returns>Query</returns>
        public static Query byCss(string selector) =>
            By.CssSelector(selector);

        /// <summary>
        /// Query elements by tag-name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Query</returns>
        public static Query byTag(string tagName) =>
            By.TagName(tagName);

        /// <summary>
        /// Query elements by XPath
        /// </summary>
        /// <param name="xpath">XPath selector</param>
        /// <returns>Query</returns>
        public static Query byXPath(string xpath) =>
            By.XPath(xpath);

        /// <summary>
        /// Query elements by class
        /// </summary>
        /// <param name="className">Class selector</param>
        /// <returns>Query</returns>
        public static Query byClass(string className) =>
            By.ClassName(className);

        /// <summary>
        /// Select an item at a specific index
        /// </summary>
        /// <param name="ix"></param>
        public static Query atIndex(int ix) =>
            new Query(Seq1(QueryStepCon.IndexSelect(ix, $"[{ix}]")));
        
        /// <summary>
        /// Wait until element exists query
        /// </summary>
        /// <returns>Query</returns>
        public static readonly Query waitUntilExists =
            waitUntilExistsFor();
        
        /// <summary>
        /// Wait until element exists query
        /// </summary>
        /// <param name="interval">Optional interval between checks</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Query</returns>
        public static Query waitUntilExistsFor(Option<TimeSpan> wait = default, Option<TimeSpan> interval = default) =>
            waitUntil(es => es.IsEmpty
                                ? fail("No elements")
                                : pure(unit),
                      "waitUntilElementExists",
                      interval,
                      wait);
        
        /// <summary>
        /// Query must have at least one element
        /// </summary>
        /// <returns>Query</returns>
        public static Query whenAtLeastOne =
            filter(es => es.IsEmpty
                             ? fail("No elements: expected at least one element")
                             : pure(unit),
                   "head");

        /// <summary>
        /// Query the first element and only the first.  Multiple elements is failure 
        /// </summary>
        /// <returns>Query</returns>
        public static Query whenSingle =
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
        static Query filter(Func<Seq<IWebElement>, Isotope<Unit>> f, string desc) => 
            new Query(Seq1(QueryStepCon.FilterM(f, desc)));
        
        /// <summary>
        /// Filter with an Isotope: fail means filtered out, success means let through
        /// The error reported by the Isotope is used as the final error
        ///
        /// Differs from filter in that it retries until timeout 
        /// </summary>
        static Query waitUntil(Func<Seq<IWebElement>, Isotope<Unit>> f, string desc, Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) => 
            new Query(Seq1(QueryStepCon.WaitM(f, desc, interval, wait)));
        
        /// <summary>
        /// Conversion operator 
        /// </summary>
        public static implicit operator Query(By by) =>
            new Query(Seq1(QueryStepCon.ByStep(by)));

        /// <summary>
        /// Associative query composition
        /// </summary>
        /// <remarks>Queries can be composed associatively. The left-hand-side query gets refined by the right-hand-side.</remarks>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Query operator +(Query lhs, Query rhs) =>
            new Query(lhs.arrows + rhs.arrows);

        /// <summary>
        /// Query an element by identifier
        /// </summary>
        /// <param name="id">Identifier of the element</param>
        /// <returns>Query</returns>
        public Query Id(string id) =>
            this + byId(id);

        /// <summary>
        /// Query elements by the text within a link
        /// </summary>
        /// <param name="linkTextToFind">Link text to find</param>
        /// <returns>Query</returns>
        public Query LinkText(string linkTextToFind) =>
            this + byLinkText(linkTextToFind);
        
        /// <summary>
        /// Query elements by the text within a link
        /// </summary>
        /// <param name="linkTextToFind">Link text to find</param>
        /// <returns>Query</returns>
        public Query PartialLinkText(string linkTextToFind) =>
            this + byPartialLinkText(linkTextToFind);

        /// <summary>
        /// Query elements by name attribute
        /// </summary>
        /// <param name="name">Name o</param>
        /// <returns>Query</returns>
        public Query Name(string name) =>
            this + byName(name);

        /// <summary>
        /// Query elements using a CSS selector
        /// </summary>
        /// <param name="selector">CSS selector</param>
        /// <returns>Query</returns>
        public Query Css(string selector) =>
            this + byCss(selector);

        /// <summary>
        /// Query elements by tag-name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Query</returns>
        public Query Tag(string tagName) =>
            this + byTag(tagName);

        /// <summary>
        /// Query elements by XPath
        /// </summary>
        /// <param name="xpath">XPath selector</param>
        /// <returns>Query</returns>
        public Query XPath(string xpath) =>
            this + byXPath(xpath);

        /// <summary>
        /// Query elements by class
        /// </summary>
        /// <param name="className">Class selector</param>
        /// <returns>Query</returns>
        public Query Class(string className) =>
            this + byClass(className);

        /// <summary>
        /// Query must have at least one matching element
        /// </summary>
        /// <returns>Query</returns>
        public Query AtLeastOne =>
            this + whenAtLeastOne;

        /// <summary>
        /// Query must have only one matching element
        /// </summary>
        /// <returns>Query</returns>
        public Query Single =>
            this + whenSingle;

        /// <summary>
        /// Select an item at a specific index
        /// </summary>
        /// <returns>Query</returns>
        public Query Index(int ix) =>
            this + atIndex(ix);
        
        /// <summary>
        /// Wait until element exists query
        /// </summary>
        /// <returns>Query</returns>
        public Query WaitUntilExists =>
            this + waitUntilExistsFor();

        /// <summary>
        /// Wait until element exists query
        /// </summary>
        /// <param name="interval">Optional interval between checks</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Query</returns>
        public Query WaitUntilExistsFor(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            this + waitUntilExistsFor(interval, wait);

        /// <summary>
        /// Maps the query to a runnable Isotope computation that returns the first item.  If there's 0 or more than 1
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
        /// Maps the query to a runnable Isotope computation that returns the first item or fails
        /// </summary>
        /// <returns></returns>
        internal Isotope<IWebElement> ToIsotopeHead() =>
            ToIsotope().Bind(es => es.Count switch
                                   {
                                       0 => fail("Element not found"),
                                       _ => pure(es.Head)
                                   });

        /// <summary>
        /// Maps the query to a runnable Isotope computation that returns the first item or fails
        /// </summary>
        /// <returns></returns>
        internal Isotope<Option<IWebElement>> ToIsotopeHeadOrNone() =>
            ToIsotope().Bind(es => es.Count switch
                                   {
                                       0 => pure<Option<IWebElement>>(None),
                                       _ => pure(Some(es.Head))
                                   });

        /// <summary>
        /// Maps the query to a runnable Isotope computation
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
        /// Maps the query to a runnable Isotope computation
        /// </summary>
        /// <returns></returns>
        public Isotope<Seq<WebElement>> ToSeq() =>
            from es in ToIsotope()
            from ie in es.Map(e => iso(() => (e.GetAttribute("id"), e)) | pure(("", e))).Sequence()
            from pe in pure(ie.Map(e => e.Add(GetElementIdField<RemoteWebElement>.Invoke(e.Item2))))  
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
        readonly static Func<A, string> InvokeA;
            
        static GetElementIdField()
        {
            var dynamic = new DynamicMethod("GetElementIdField", typeof(string), new[] { typeof(A) }, true);
            var field   = typeof(A).GetTypeInfo().DeclaredFields.Filter(f => f.Name == "elementId").Head();
            var il      = dynamic.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field);
            il.Emit(OpCodes.Ret);
 
            InvokeA = (Func<A, string>)dynamic.CreateDelegate(typeof(Func<A, string>));            
        }

        public static string Invoke(IWebElement element) =>
            InvokeA((A)element);
    }

    /// <summary>
    /// Step in a query
    /// </summary>
    [Union]
    internal interface QueryStep
    {
        /// <summary>
        /// Selector step - filters by `value`
        /// </summary>
        QueryStep ByStep(By value);
        
        /// <summary>
        /// Like a LINQ Where, but instead of bool return it returns an Isotope, which means we can propagate errors
        /// </summary>
        QueryStep FilterM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc);
        
        /// <summary>
        /// Like a LINQ Where, but instead of bool return it returns an Isotope, which means we can propagate errors
        /// It differs from FilterM in that it waits for a period of time, retrying, until a timeout 
        /// </summary>
        QueryStep WaitM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc,  Option<TimeSpan> interval = default, Option<TimeSpan> wait = default);
        
        /// <summary>
        /// Pick an item at an index within the result set.  If out of range `fail` is thrown
        /// </summary>
        QueryStep IndexSelect(int index, string desc);
    }
    
    internal static class QueryStepExt
    {
        internal static string Show(this QueryStep step) =>
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
    /// Query semigroup instance
    /// </summary>
    public struct SemiQuery : Semigroup<Query>
    {
        /// <summary>
        /// Associative binary operator for query composition
        /// </summary>
        /// <param name="x">Left query</param>
        /// <param name="y">Right query</param>
        /// <returns>Composed query</returns>
        public Query Append(Query x, Query y) =>
            x + y;
    }

    /// <summary>
    /// Query monoid instance
    /// </summary>
    public struct MQuery : Monoid<Query>
    {
        /// <summary>
        /// Associative binary operator for query composition
        /// </summary>
        /// <param name="x">Left query</param>
        /// <param name="y">Right query</param>
        /// <returns>Composed query</returns>
        public Query Append(Query x, Query y) =>
            x + y;

        /// <summary>
        /// Monoidal unit.  For Query this selects all.
        /// </summary>
        /// <returns>Unit query</returns>
        public Query Empty() =>
            Query.Identity;
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