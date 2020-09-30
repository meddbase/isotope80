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
        /// Wait until element exists query
        /// </summary>
        /// <param name="interval">Optional interval between checks</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Query</returns>
        internal static Query waitUntilElementExists(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            waitUntil(es => es.IsEmpty
                                ? fail("No elements")
                                : pure(unit),
                      "waitUntilElementExists",
                      interval,
                      wait);

        /// <summary>
        /// Query the first element
        /// </summary>
        /// <returns>Query</returns>
        public static Query byHead =
            filter(es => es.IsEmpty
                             ? fail("No elements: expected a head element")
                             : pure(unit),
                   "head");

        /// <summary>
        /// Query the first element and only the first.  Multiple elements is failure 
        /// </summary>
        /// <returns>Query</returns>
        public static Query bySingle =
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
        public Query WhereId(string id) =>
            this + byId(id);

        /// <summary>
        /// Query elements by the text within a link
        /// </summary>
        /// <param name="linkTextToFind">Link text to find</param>
        /// <returns>Query</returns>
        public Query WhereLinkText(string linkTextToFind) =>
            this + byLinkText(linkTextToFind);

        /// <summary>
        /// Query elements by name attribute
        /// </summary>
        /// <param name="name">Name o</param>
        /// <returns>Query</returns>
        public Query WhereName(string name) =>
            this + byName(name);

        /// <summary>
        /// Query elements using a CSS selector
        /// </summary>
        /// <param name="selector">CSS selector</param>
        /// <returns>Query</returns>
        public Query WhereCss(string selector) =>
            this + byCss(selector);

        /// <summary>
        /// Query elements by tag-name
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Query</returns>
        public Query WhereTag(string tagName) =>
            this + byTag(tagName);

        /// <summary>
        /// Query elements by XPath
        /// </summary>
        /// <param name="xpath">XPath selector</param>
        /// <returns>Query</returns>
        public Query WhereXPath(string xpath) =>
            this + byXPath(xpath);

        /// <summary>
        /// Query the first element
        /// </summary>
        /// <returns>Query</returns>
        public Query Head =>
            this + byHead;

        /// <summary>
        /// Query the first element
        /// </summary>
        /// <returns>Query</returns>
        public Query Single =>
            this + bySingle;

        /// <summary>
        /// Wait until element exists query
        /// </summary>
        /// <param name="interval">Optional interval between checks</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Query</returns>
        public Query WaitUntilElementExists(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            this + waitUntilElementExists(interval, wait);

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
                                         from es in iso(() => dr.FindElements(byStep.Value).ToSeq().Strict()) | fail("No elements found")
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
                                                        from ns in iso(() => w.FindElements(byStep.Value).ToSeq().Strict()) | fail("No elements found")
                                                        select os + ns)
                                               select Some(r))
                         select rs;
                }
                else if (arr is WaitM waitM)
                {
                    ma = Isotope.waitUntil(
                        from a in ma
                        from r in a.Match(
                            None: fail("WaitM until must follow a something that queries elements.  It can't run alone"),
                            Some: waitM.Ma)
                        select a);
                }
                else if (arr is FilterM filterM)
                {
                    ma = from a in ma
                         from r in a.Match(
                            None: fail("FilterM until must follow a something that queries elements.  It can't run alone"),
                            Some: filterM.Ma)
                         select a;
                }
            }

            return ma.Map(op => op.IfNone(Empty));
        }

        /// <summary>
        /// Maps the query to a runnable Isotope computation
        /// </summary>
        /// <returns></returns>
        public Isotope<Seq<WebElement>> ToSeq() =>
            ToIsotope().Map(es => es.Map<IWebElement, WebElement>((ix, e) => WebElement.New(this, ix, e.TagName, e.Text, e.Enabled, e.Selected, e.Location, e.Size, e.Displayed))
                                    .ToSeq()
                                    .Strict());

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
            String.Join(" → ", arrows);
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
    }
    
    internal static class QueryStepExt
    {
        internal static string Show(QueryStep step) =>
            step switch
            {
                ByStep  (var value)                   => value.ToString(),
                FilterM (var _, var d)                => d,
                WaitM   (var _, var d, var _, var _)  => d,
                _                                     => throw new NotSupportedException()
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
}