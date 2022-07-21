using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;

namespace Isotope80
{
    public static partial class Isotope
    {
        /// <summary>
        /// Creates a CSS Selector for use with WebDriver. Equivalent of `By.CssSelector`
        /// </summary>
        /// <param name="cssSelector">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select css(string cssSelector) => Select.byCss(cssSelector);
        
        /// <summary>
        /// Creates a XPath Selector for use with WebDriver. Equivalent of `By.XPath`
        /// </summary>
        /// <param name="xpath">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select xPath(string xpath) => Select.byXPath(xpath);
        
        /// <summary>
        /// Creates a Class Name Selector for use with WebDriver. Equivalent of `By.ClassName`
        /// </summary>
        /// <param name="classname">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select className(string classname) => Select.byClass(classname);
        
        /// <summary>
        /// Creates an Id Selector for use with WebDriver. Equivalent of `By.Id`
        /// </summary>
        /// <param name="id">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select id(string id) => Select.byId(id);
        
        /// <summary>
        /// Creates a Tag Name Selector for use with WebDriver. Equivalent of `By.TagName`
        /// </summary>
        /// <param name="tagname">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select tagName(string tagname) => Select.byTag(tagname);
        
        /// <summary>
        /// Creates a Name Selector for use with WebDriver. Equivalent of `By.Name`
        /// </summary>
        /// <param name="name">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select name(string name) => Select.byName(name);
        
        /// <summary>
        /// Creates a Link Text Selector for use with WebDriver. Equivalent of `By.LinkText`
        /// </summary>
        /// <param name="linktext">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select linkText(string linktext) => Select.byLinkText(linktext);
        
        /// <summary>
        /// Creates a Partial Link Text Selector for use with WebDriver. Equivalent of `By.PartialLinkText`
        /// </summary>
        /// <param name="linktext">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select partialLinkText(string linktext) => Select.byPartialLinkText(linktext);
        
        /// <summary>
        /// When composed with another query, it enforces at least one result
        /// </summary>
        public static readonly Select whenAtLeastOne = Select.whenAtLeastOne;
        
        /// <summary>
        /// When composed with another query, it enforces only one result or fails
        /// </summary>
        public static readonly Select whenSingle = Select.whenSingle;
        
        /// <summary>
        /// Wait until element exists query
        /// </summary>
        /// <returns>Select</returns>
        public static readonly Select waitUntilExists = Select.waitUntilExists;

        /// <summary>
        /// Wait until element exists query
        /// </summary>
        /// <param name="interval">Optional interval between checks</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Select</returns>
        public static Select waitUntilExistsFor(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            Select.waitUntilExistsFor(interval, wait);

        /// <summary>
        /// Select an item at a specific index
        /// </summary>
        /// <param name="ix">Index to select</param>
        public static Select atIndex(int ix) =>
            Select.atIndex(ix);
    }
}
