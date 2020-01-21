using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Isotope80
{
    public static partial class Isotope
    {
        public static By css(string cssSelector) => By.CssSelector(cssSelector);
        public static By xPath(string xpath) => By.XPath(xpath);
        public static By className(string classname) => By.ClassName(classname);
        public static By id(string id) => By.Id(id);
        public static By tagName(string tagname) => By.TagName(tagname);
        public static By name(string name) => By.Name(name);
        public static By linkText(string linktext) => By.LinkText(linktext);
        public static By partialLinkText(string linktext) => By.PartialLinkText(linktext);
    }
}
