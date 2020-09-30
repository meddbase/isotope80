using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Isotope80
{
    public static partial class Isotope
    {
        /// <summary>
        /// Creates a CSS Selector for use with WebDriver. Equivalent of `By.CssSelector`
        /// </summary>
        /// <param name="cssSelector">Selector</param>
        /// <returns>Web element selector</returns>
        public static Query css(string cssSelector) => Query.byCss(cssSelector);
        
        /// <summary>
        /// Creates a XPath Selector for use with WebDriver. Equivalent of `By.XPath`
        /// </summary>
        /// <param name="xpath">Selector</param>
        /// <returns>Web element selector</returns>
        public static Query xPath(string xpath) => Query.byXPath(xpath);
        
        /// <summary>
        /// Creates a Class Name Selector for use with WebDriver. Equivalent of `By.ClassName`
        /// </summary>
        /// <param name="classname">Selector</param>
        /// <returns>Web element selector</returns>
        public static Query className(string classname) => Query.byClass(classname);
        
        /// <summary>
        /// Creates an Id Selector for use with WebDriver. Equivalent of `By.Id`
        /// </summary>
        /// <param name="id">Selector</param>
        /// <returns>Web element selector</returns>
        public static Query id(string id) => Query.byId(id);
        
        /// <summary>
        /// Creates a Tag Name Selector for use with WebDriver. Equivalent of `By.TagName`
        /// </summary>
        /// <param name="tagname">Selector</param>
        /// <returns>Web element selector</returns>
        public static Query tagName(string tagname) => Query.byTag(tagname);
        
        /// <summary>
        /// Creates a Name Selector for use with WebDriver. Equivalent of `By.Name`
        /// </summary>
        /// <param name="name">Selector</param>
        /// <returns>Web element selector</returns>
        public static Query name(string name) => Query.byName(name);
        
        /// <summary>
        /// Creates a Link Text Selector for use with WebDriver. Equivalent of `By.LinkText`
        /// </summary>
        /// <param name="linktext">Selector</param>
        /// <returns>Web element selector</returns>
        public static Query linkText(string linktext) => Query.byLinkText(linktext);
        
        /// <summary>
        /// Creates a Partial Link Text Selector for use with WebDriver. Equivalent of `By.PartialLinkText`
        /// </summary>
        /// <param name="linktext">Selector</param>
        /// <returns>Web element selector</returns>
        public static Query partialLinkText(string linktext) => Query.byPartialLinkText(linktext);
        
        /// <summary>
        /// When composed with another query, it enforces at least one result
        /// </summary>
        public static readonly Query whenAtLeastOne = Query.whenAtLeastOne;
        
        /// <summary>
        /// When composed with another query, it enforces only one result or fails
        /// </summary>
        public static readonly Query whenSingle = Query.whenSingle;
    }
}
