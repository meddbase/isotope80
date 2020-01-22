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
        /// <returns>WebDriver Selector</returns>
        public static By css(string cssSelector) => By.CssSelector(cssSelector);
        /// <summary>
        /// Creates a XPath Selector for use with WebDriver. Equivalent of `By.XPath`
        /// </summary>
        /// <param name="xpath">Selector</param>
        /// <returns>WebDriver Selector</returns>
        public static By xPath(string xpath) => By.XPath(xpath);
        /// <summary>
        /// Creates a Class Name Selector for use with WebDriver. Equivalent of `By.ClassName`
        /// </summary>
        /// <param name="classname">Selector</param>
        /// <returns>WebDriver Selector</returns>
        public static By className(string classname) => By.ClassName(classname);
        /// <summary>
        /// Creates an Id Selector for use with WebDriver. Equivalent of `By.Id`
        /// </summary>
        /// <param name="id">Selector</param>
        /// <returns>WebDriver Selector</returns>
        public static By id(string id) => By.Id(id);
        /// <summary>
        /// Creates a Tag Name Selector for use with WebDriver. Equivalent of `By.TagName`
        /// </summary>
        /// <param name="tagname">Selector</param>
        /// <returns>WebDriver Selector</returns>
        public static By tagName(string tagname) => By.TagName(tagname);
        /// <summary>
        /// Creates a Name Selector for use with WebDriver. Equivalent of `By.Name`
        /// </summary>
        /// <param name="name">Selector</param>
        /// <returns>WebDriver Selector</returns>
        public static By name(string name) => By.Name(name);
        /// <summary>
        /// Creates a Link Text Selector for use with WebDriver. Equivalent of `By.LinkText`
        /// </summary>
        /// <param name="linktext">Selector</param>
        /// <returns>WebDriver Selector</returns>
        public static By linkText(string linktext) => By.LinkText(linktext);                
        /// <summary>
        /// Creates a Partial Link Text Selector for use with WebDriver. Equivalent of `By.PartialLinkText`
        /// </summary>
        /// <param name="linktext">Selector</param>
        /// <returns>WebDriver Selector</returns>
        public static By partialLinkText(string linktext) => By.PartialLinkText(linktext);
    }
}
