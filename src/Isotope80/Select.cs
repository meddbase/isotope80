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
        public static Select waitUntilExistsFor(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
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
                        select a, waitM.Interval, waitM.Wait);
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
            select ie.Map<(string Id, IWebElement El), WebElement>((ix, e) => 
                       new WebElement(
                           this, 
                           ix,
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

    /// <summary>
    /// Step in a select
    /// </summary>
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

    [System.Serializable]
    internal sealed class ByStep : SelectStep, System.IEquatable<ByStep>, System.IComparable<ByStep>, System.IComparable
    {
        public readonly By Value;
        public ByStep(By Value)
        {
            this.Value = Value;
        }

        public static ByStep New(By Value) => new ByStep(Value);
        public void Deconstruct(out By Value)
        {
            Value = this.Value;
        }

        private ByStep(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            this.Value = (By)info.GetValue("value", typeof(By));
        }

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("value", this.Value);
        }

        public static bool operator ==(ByStep x, ByStep y) => ReferenceEquals(x, y) || (x?.Equals(y) ?? false);
        public static bool operator !=(ByStep x, ByStep y) => !(x == y);
        public static bool operator >(ByStep x, ByStep y) => !ReferenceEquals(x, y) && !ReferenceEquals(x, null) && x.CompareTo(y) > 0;
        public static bool operator <(ByStep x, ByStep y) => !ReferenceEquals(x, y) && (ReferenceEquals(x, null) && !ReferenceEquals(y, null) || x.CompareTo(y) < 0);
        public static bool operator >=(ByStep x, ByStep y) => ReferenceEquals(x, y) || (!ReferenceEquals(x, null) && x.CompareTo(y) >= 0);
        public static bool operator <=(ByStep x, ByStep y) => ReferenceEquals(x, y) || (ReferenceEquals(x, null) && !ReferenceEquals(y, null) || x.CompareTo(y) <= 0);
        public bool Equals(ByStep other)
        {
            if (LanguageExt.Prelude.isnull(other))
                return false;
            if (!default(LanguageExt.ClassInstances.EqDefault<By>).Equals(this.Value, other.Value))
                return false;
            return true;
        }

        public override bool Equals(object obj) => obj is ByStep tobj && Equals(tobj);
        public int CompareTo(object obj) => obj is ByStep p ? CompareTo(p) : 1;
        public int CompareTo(ByStep other)
        {
            if (LanguageExt.Prelude.isnull(other))
                return 1;
            int cmp = 0;
            cmp = default(LanguageExt.ClassInstances.OrdDefault<By>).Compare(this.Value, other.Value);
            if (cmp != 0)
                return cmp;
            return 0;
        }

        public override int GetHashCode()
        {
            const int fnvOffsetBasis = -2128831035;
            const int fnvPrime = 16777619;
            int state = fnvOffsetBasis;
            unchecked
            {
                state = (default(LanguageExt.ClassInstances.HashableDefault<By>).GetHashCode(this.Value) ^ state) * fnvPrime;
            }

            return state;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("ByStep(");
            sb.Append(LanguageExt.Prelude.isnull(Value) ? $"Value: [null]" : $"Value: {Value}");
            sb.Append(")");
            return sb.ToString();
        }

        SelectStep SelectStep.ByStep(By value) => throw new System.NotSupportedException();
        SelectStep SelectStep.FilterM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc) => throw new System.NotSupportedException();
        SelectStep SelectStep.WaitM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc, Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) => throw new System.NotSupportedException();
        SelectStep SelectStep.IndexSelect(int index, string desc) => throw new System.NotSupportedException();
        public ByStep With(By Value = null) => new ByStep(Value ?? this.Value);
        public static Lens<ByStep, By> value => __LensFields.value;
        static class __LensFields
        {
            public static readonly Lens<ByStep, By> value = Lens<ByStep, By>.New(_x => _x.Value, _x => _y => _y.With(Value: _x));
        }
    }

    [System.Serializable]
    internal sealed class FilterM : SelectStep, System.IEquatable<FilterM>, System.IComparable<FilterM>, System.IComparable
    {
        public readonly Func<Seq<IWebElement>, Isotope<Unit>> Ma;
        public readonly string Desc;
        public FilterM(Func<Seq<IWebElement>, Isotope<Unit>> Ma, string Desc)
        {
            this.Ma = Ma;
            this.Desc = Desc;
        }

        public static FilterM New(Func<Seq<IWebElement>, Isotope<Unit>> Ma, string Desc) => new FilterM(Ma, Desc);
        public void Deconstruct(out Func<Seq<IWebElement>, Isotope<Unit>> Ma, out string Desc)
        {
            Ma = this.Ma;
            Desc = this.Desc;
        }

        private FilterM(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            this.Ma = (Func<Seq<IWebElement>, Isotope<Unit>>)info.GetValue("ma", typeof(Func<Seq<IWebElement>, Isotope<Unit>>));
            this.Desc = (string)info.GetValue("desc", typeof(string));
        }

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("ma", this.Ma);
            info.AddValue("desc", this.Desc);
        }

        public static bool operator ==(FilterM x, FilterM y) => ReferenceEquals(x, y) || (x?.Equals(y) ?? false);
        public static bool operator !=(FilterM x, FilterM y) => !(x == y);
        public static bool operator >(FilterM x, FilterM y) => !ReferenceEquals(x, y) && !ReferenceEquals(x, null) && x.CompareTo(y) > 0;
        public static bool operator <(FilterM x, FilterM y) => !ReferenceEquals(x, y) && (ReferenceEquals(x, null) && !ReferenceEquals(y, null) || x.CompareTo(y) < 0);
        public static bool operator >=(FilterM x, FilterM y) => ReferenceEquals(x, y) || (!ReferenceEquals(x, null) && x.CompareTo(y) >= 0);
        public static bool operator <=(FilterM x, FilterM y) => ReferenceEquals(x, y) || (ReferenceEquals(x, null) && !ReferenceEquals(y, null) || x.CompareTo(y) <= 0);
        public bool Equals(FilterM other)
        {
            if (LanguageExt.Prelude.isnull(other))
                return false;
            if (!default(LanguageExt.ClassInstances.EqDefault<Func<Seq<IWebElement>, Isotope<Unit>>>).Equals(this.Ma, other.Ma))
                return false;
            if (!default(LanguageExt.ClassInstances.EqDefault<string>).Equals(this.Desc, other.Desc))
                return false;
            return true;
        }

        public override bool Equals(object obj) => obj is FilterM tobj && Equals(tobj);
        public int CompareTo(object obj) => obj is FilterM p ? CompareTo(p) : 1;
        public int CompareTo(FilterM other)
        {
            if (LanguageExt.Prelude.isnull(other))
                return 1;
            int cmp = 0;
            cmp = default(LanguageExt.ClassInstances.OrdDefault<Func<Seq<IWebElement>, Isotope<Unit>>>).Compare(this.Ma, other.Ma);
            if (cmp != 0)
                return cmp;
            cmp = default(LanguageExt.ClassInstances.OrdDefault<string>).Compare(this.Desc, other.Desc);
            if (cmp != 0)
                return cmp;
            return 0;
        }

        public override int GetHashCode()
        {
            const int fnvOffsetBasis = -2128831035;
            const int fnvPrime = 16777619;
            int state = fnvOffsetBasis;
            unchecked
            {
                state = (default(LanguageExt.ClassInstances.HashableDefault<Func<Seq<IWebElement>, Isotope<Unit>>>).GetHashCode(this.Ma) ^ state) * fnvPrime;
                state = (default(LanguageExt.ClassInstances.HashableDefault<string>).GetHashCode(this.Desc) ^ state) * fnvPrime;
            }

            return state;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("FilterM(");
            sb.Append(LanguageExt.Prelude.isnull(Ma) ? $"Ma: [null]" : $"Ma: {Ma}");
            sb.Append($", ");
            sb.Append(LanguageExt.Prelude.isnull(Desc) ? $"Desc: [null]" : $"Desc: {Desc}");
            sb.Append(")");
            return sb.ToString();
        }

        SelectStep SelectStep.ByStep(By value) => throw new System.NotSupportedException();
        SelectStep SelectStep.FilterM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc) => throw new System.NotSupportedException();
        SelectStep SelectStep.WaitM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc, Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) => throw new System.NotSupportedException();
        SelectStep SelectStep.IndexSelect(int index, string desc) => throw new System.NotSupportedException();
        public FilterM With(Func<Seq<IWebElement>, Isotope<Unit>> Ma = null, string Desc = null) => new FilterM(Ma ?? this.Ma, Desc ?? this.Desc);
        public static Lens<FilterM, Func<Seq<IWebElement>, Isotope<Unit>>> ma => __LensFields.ma;
        public static Lens<FilterM, string> desc => __LensFields.desc;
        static class __LensFields
        {
            public static readonly Lens<FilterM, Func<Seq<IWebElement>, Isotope<Unit>>> ma = Lens<FilterM, Func<Seq<IWebElement>, Isotope<Unit>>>.New(_x => _x.Ma, _x => _y => _y.With(Ma: _x));
            public static readonly Lens<FilterM, string> desc = Lens<FilterM, string>.New(_x => _x.Desc, _x => _y => _y.With(Desc: _x));
        }
    }

    [System.Serializable]
    internal sealed class WaitM : SelectStep, System.IEquatable<WaitM>, System.IComparable<WaitM>, System.IComparable
    {
        public readonly Func<Seq<IWebElement>, Isotope<Unit>> Ma;
        public readonly string Desc;
        public readonly Option<TimeSpan> Interval;
        public readonly Option<TimeSpan> Wait;
        public WaitM(Func<Seq<IWebElement>, Isotope<Unit>> Ma, string Desc, Option<TimeSpan> Interval, Option<TimeSpan> Wait)
        {
            this.Ma = Ma;
            this.Desc = Desc;
            this.Interval = Interval;
            this.Wait = Wait;
        }

        public static WaitM New(Func<Seq<IWebElement>, Isotope<Unit>> Ma, string Desc, Option<TimeSpan> Interval, Option<TimeSpan> Wait) => new WaitM(Ma, Desc, Interval, Wait);
        public void Deconstruct(out Func<Seq<IWebElement>, Isotope<Unit>> Ma, out string Desc, out Option<TimeSpan> Interval, out Option<TimeSpan> Wait)
        {
            Ma = this.Ma;
            Desc = this.Desc;
            Interval = this.Interval;
            Wait = this.Wait;
        }

        private WaitM(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            this.Ma = (Func<Seq<IWebElement>, Isotope<Unit>>)info.GetValue("ma", typeof(Func<Seq<IWebElement>, Isotope<Unit>>));
            this.Desc = (string)info.GetValue("desc", typeof(string));
            this.Interval = (Option<TimeSpan>)info.GetValue("interval", typeof(Option<TimeSpan>));
            this.Wait = (Option<TimeSpan>)info.GetValue("wait", typeof(Option<TimeSpan>));
        }

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("ma", this.Ma);
            info.AddValue("desc", this.Desc);
            info.AddValue("interval", this.Interval);
            info.AddValue("wait", this.Wait);
        }

        public static bool operator ==(WaitM x, WaitM y) => ReferenceEquals(x, y) || (x?.Equals(y) ?? false);
        public static bool operator !=(WaitM x, WaitM y) => !(x == y);
        public static bool operator >(WaitM x, WaitM y) => !ReferenceEquals(x, y) && !ReferenceEquals(x, null) && x.CompareTo(y) > 0;
        public static bool operator <(WaitM x, WaitM y) => !ReferenceEquals(x, y) && (ReferenceEquals(x, null) && !ReferenceEquals(y, null) || x.CompareTo(y) < 0);
        public static bool operator >=(WaitM x, WaitM y) => ReferenceEquals(x, y) || (!ReferenceEquals(x, null) && x.CompareTo(y) >= 0);
        public static bool operator <=(WaitM x, WaitM y) => ReferenceEquals(x, y) || (ReferenceEquals(x, null) && !ReferenceEquals(y, null) || x.CompareTo(y) <= 0);
        public bool Equals(WaitM other)
        {
            if (LanguageExt.Prelude.isnull(other))
                return false;
            if (!default(LanguageExt.ClassInstances.EqDefault<Func<Seq<IWebElement>, Isotope<Unit>>>).Equals(this.Ma, other.Ma))
                return false;
            if (!default(LanguageExt.ClassInstances.EqDefault<string>).Equals(this.Desc, other.Desc))
                return false;
            if (!default(LanguageExt.ClassInstances.EqDefault<Option<TimeSpan>>).Equals(this.Interval, other.Interval))
                return false;
            if (!default(LanguageExt.ClassInstances.EqDefault<Option<TimeSpan>>).Equals(this.Wait, other.Wait))
                return false;
            return true;
        }

        public override bool Equals(object obj) => obj is WaitM tobj && Equals(tobj);
        public int CompareTo(object obj) => obj is WaitM p ? CompareTo(p) : 1;
        public int CompareTo(WaitM other)
        {
            if (LanguageExt.Prelude.isnull(other))
                return 1;
            int cmp = 0;
            cmp = default(LanguageExt.ClassInstances.OrdDefault<Func<Seq<IWebElement>, Isotope<Unit>>>).Compare(this.Ma, other.Ma);
            if (cmp != 0)
                return cmp;
            cmp = default(LanguageExt.ClassInstances.OrdDefault<string>).Compare(this.Desc, other.Desc);
            if (cmp != 0)
                return cmp;
            cmp = default(LanguageExt.ClassInstances.OrdDefault<Option<TimeSpan>>).Compare(this.Interval, other.Interval);
            if (cmp != 0)
                return cmp;
            cmp = default(LanguageExt.ClassInstances.OrdDefault<Option<TimeSpan>>).Compare(this.Wait, other.Wait);
            if (cmp != 0)
                return cmp;
            return 0;
        }

        public override int GetHashCode()
        {
            const int fnvOffsetBasis = -2128831035;
            const int fnvPrime = 16777619;
            int state = fnvOffsetBasis;
            unchecked
            {
                state = (default(LanguageExt.ClassInstances.HashableDefault<Func<Seq<IWebElement>, Isotope<Unit>>>).GetHashCode(this.Ma) ^ state) * fnvPrime;
                state = (default(LanguageExt.ClassInstances.HashableDefault<string>).GetHashCode(this.Desc) ^ state) * fnvPrime;
                state = (default(LanguageExt.ClassInstances.HashableDefault<Option<TimeSpan>>).GetHashCode(this.Interval) ^ state) * fnvPrime;
                state = (default(LanguageExt.ClassInstances.HashableDefault<Option<TimeSpan>>).GetHashCode(this.Wait) ^ state) * fnvPrime;
            }

            return state;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("WaitM(");
            sb.Append(LanguageExt.Prelude.isnull(Ma) ? $"Ma: [null]" : $"Ma: {Ma}");
            sb.Append($", ");
            sb.Append(LanguageExt.Prelude.isnull(Desc) ? $"Desc: [null]" : $"Desc: {Desc}");
            sb.Append($", ");
            sb.Append(LanguageExt.Prelude.isnull(Interval) ? $"Interval: [null]" : $"Interval: {Interval}");
            sb.Append($", ");
            sb.Append(LanguageExt.Prelude.isnull(Wait) ? $"Wait: [null]" : $"Wait: {Wait}");
            sb.Append(")");
            return sb.ToString();
        }

        SelectStep SelectStep.ByStep(By value) => throw new System.NotSupportedException();
        SelectStep SelectStep.FilterM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc) => throw new System.NotSupportedException();
        SelectStep SelectStep.WaitM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc, Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) => throw new System.NotSupportedException();
        SelectStep SelectStep.IndexSelect(int index, string desc) => throw new System.NotSupportedException();
        public WaitM With(Func<Seq<IWebElement>, Isotope<Unit>> Ma = null, string Desc = null, Option<TimeSpan>? Interval = null, Option<TimeSpan>? Wait = null) => new WaitM(Ma ?? this.Ma, Desc ?? this.Desc, Interval ?? this.Interval, Wait ?? this.Wait);
        public static Lens<WaitM, Func<Seq<IWebElement>, Isotope<Unit>>> ma => __LensFields.ma;
        public static Lens<WaitM, string> desc => __LensFields.desc;
        public static Lens<WaitM, Option<TimeSpan>> interval => __LensFields.interval;
        public static Lens<WaitM, Option<TimeSpan>> wait => __LensFields.wait;
        static class __LensFields
        {
            public static readonly Lens<WaitM, Func<Seq<IWebElement>, Isotope<Unit>>> ma = Lens<WaitM, Func<Seq<IWebElement>, Isotope<Unit>>>.New(_x => _x.Ma, _x => _y => _y.With(Ma: _x));
            public static readonly Lens<WaitM, string> desc = Lens<WaitM, string>.New(_x => _x.Desc, _x => _y => _y.With(Desc: _x));
            public static readonly Lens<WaitM, Option<TimeSpan>> interval = Lens<WaitM, Option<TimeSpan>>.New(_x => _x.Interval, _x => _y => _y.With(Interval: _x));
            public static readonly Lens<WaitM, Option<TimeSpan>> wait = Lens<WaitM, Option<TimeSpan>>.New(_x => _x.Wait, _x => _y => _y.With(Wait: _x));
        }
    }

    [System.Serializable]
    internal sealed class IndexSelect : SelectStep, System.IEquatable<IndexSelect>, System.IComparable<IndexSelect>, System.IComparable
    {
        public readonly int Index;
        public readonly string Desc;
        public IndexSelect(int Index, string Desc)
        {
            this.Index = Index;
            this.Desc = Desc;
        }

        public static IndexSelect New(int Index, string Desc) => new IndexSelect(Index, Desc);
        public void Deconstruct(out int Index, out string Desc)
        {
            Index = this.Index;
            Desc = this.Desc;
        }

        private IndexSelect(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            this.Index = (int)info.GetValue("index", typeof(int));
            this.Desc = (string)info.GetValue("desc", typeof(string));
        }

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("index", this.Index);
            info.AddValue("desc", this.Desc);
        }

        public static bool operator ==(IndexSelect x, IndexSelect y) => ReferenceEquals(x, y) || (x?.Equals(y) ?? false);
        public static bool operator !=(IndexSelect x, IndexSelect y) => !(x == y);
        public static bool operator >(IndexSelect x, IndexSelect y) => !ReferenceEquals(x, y) && !ReferenceEquals(x, null) && x.CompareTo(y) > 0;
        public static bool operator <(IndexSelect x, IndexSelect y) => !ReferenceEquals(x, y) && (ReferenceEquals(x, null) && !ReferenceEquals(y, null) || x.CompareTo(y) < 0);
        public static bool operator >=(IndexSelect x, IndexSelect y) => ReferenceEquals(x, y) || (!ReferenceEquals(x, null) && x.CompareTo(y) >= 0);
        public static bool operator <=(IndexSelect x, IndexSelect y) => ReferenceEquals(x, y) || (ReferenceEquals(x, null) && !ReferenceEquals(y, null) || x.CompareTo(y) <= 0);
        public bool Equals(IndexSelect other)
        {
            if (LanguageExt.Prelude.isnull(other))
                return false;
            if (!default(LanguageExt.ClassInstances.EqDefault<int>).Equals(this.Index, other.Index))
                return false;
            if (!default(LanguageExt.ClassInstances.EqDefault<string>).Equals(this.Desc, other.Desc))
                return false;
            return true;
        }

        public override bool Equals(object obj) => obj is IndexSelect tobj && Equals(tobj);
        public int CompareTo(object obj) => obj is IndexSelect p ? CompareTo(p) : 1;
        public int CompareTo(IndexSelect other)
        {
            if (LanguageExt.Prelude.isnull(other))
                return 1;
            int cmp = 0;
            cmp = default(LanguageExt.ClassInstances.OrdDefault<int>).Compare(this.Index, other.Index);
            if (cmp != 0)
                return cmp;
            cmp = default(LanguageExt.ClassInstances.OrdDefault<string>).Compare(this.Desc, other.Desc);
            if (cmp != 0)
                return cmp;
            return 0;
        }

        public override int GetHashCode()
        {
            const int fnvOffsetBasis = -2128831035;
            const int fnvPrime = 16777619;
            int state = fnvOffsetBasis;
            unchecked
            {
                state = (default(LanguageExt.ClassInstances.HashableDefault<int>).GetHashCode(this.Index) ^ state) * fnvPrime;
                state = (default(LanguageExt.ClassInstances.HashableDefault<string>).GetHashCode(this.Desc) ^ state) * fnvPrime;
            }

            return state;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("IndexSelect(");
            sb.Append(LanguageExt.Prelude.isnull(Index) ? $"Index: [null]" : $"Index: {Index}");
            sb.Append($", ");
            sb.Append(LanguageExt.Prelude.isnull(Desc) ? $"Desc: [null]" : $"Desc: {Desc}");
            sb.Append(")");
            return sb.ToString();
        }

        SelectStep SelectStep.ByStep(By value) => throw new System.NotSupportedException();
        SelectStep SelectStep.FilterM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc) => throw new System.NotSupportedException();
        SelectStep SelectStep.WaitM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc, Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) => throw new System.NotSupportedException();
        SelectStep SelectStep.IndexSelect(int index, string desc) => throw new System.NotSupportedException();
        public IndexSelect With(int? Index = null, string Desc = null) => new IndexSelect(Index ?? this.Index, Desc ?? this.Desc);
        public static Lens<IndexSelect, int> index => __LensFields.index;
        public static Lens<IndexSelect, string> desc => __LensFields.desc;
        static class __LensFields
        {
            public static readonly Lens<IndexSelect, int> index = Lens<IndexSelect, int>.New(_x => _x.Index, _x => _y => _y.With(Index: _x));
            public static readonly Lens<IndexSelect, string> desc = Lens<IndexSelect, string>.New(_x => _x.Desc, _x => _y => _y.With(Desc: _x));
        }
    }

    internal static partial class SelectStepCon
    {
        public static SelectStep ByStep(By value) => new ByStep(value);
        public static SelectStep FilterM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc) => new FilterM(ma, desc);
        public static SelectStep WaitM(Func<Seq<IWebElement>, Isotope<Unit>> ma, string desc, Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) => new WaitM(ma, desc, interval, wait);
        public static SelectStep IndexSelect(int index, string desc) => new IndexSelect(index, desc);
        public static RETURN Match<RETURN>(this SelectStep _ma, System.Func<ByStep, RETURN> ByStep, System.Func<FilterM, RETURN> FilterM, System.Func<WaitM, RETURN> WaitM, System.Func<IndexSelect, RETURN> IndexSelect) => _ma switch
        {
            ByStep value => ByStep(value),
            FilterM value => FilterM(value),
            WaitM value => WaitM(value),
            IndexSelect value => IndexSelect(value),
            _ => throw new LanguageExt.ValueIsNullException()
        }

        ;
    }
}