using System;
using LanguageExt;
using Microsoft.Playwright;

namespace Isotope80
{
    public static partial class Isotope
    {
        /// <summary>
        /// Creates a CSS Selector for use with Playwright
        /// </summary>
        /// <param name="cssSelector">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select css(string cssSelector) => Select.byCss(cssSelector);

        /// <summary>
        /// Creates a XPath Selector for use with Playwright
        /// </summary>
        /// <param name="xpath">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select xPath(string xpath) => Select.byXPath(xpath);

        /// <summary>
        /// Creates a Class Name Selector for use with Playwright
        /// </summary>
        /// <param name="classname">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select className(string classname) => Select.byClass(classname);

        /// <summary>
        /// Creates an Id Selector for use with Playwright
        /// </summary>
        /// <param name="id">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select id(string id) => Select.byId(id);

        /// <summary>
        /// Creates a Tag Name Selector for use with Playwright
        /// </summary>
        /// <param name="tagname">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select tagName(string tagname) => Select.byTag(tagname);

        /// <summary>
        /// Creates a Name Selector for use with Playwright
        /// </summary>
        /// <param name="name">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select name(string name) => Select.byName(name);

        /// <summary>
        /// Creates a Link Text Selector for use with Playwright
        /// </summary>
        /// <param name="linktext">Selector</param>
        /// <returns>Web element selector</returns>
        public static Select linkText(string linktext) => Select.byLinkText(linktext);

        /// <summary>
        /// Creates a Partial Link Text Selector for use with Playwright
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
        /// Wait until no elements match the selector query
        /// </summary>
        /// <returns>Select</returns>
        public static readonly Select waitUntilNotExists = Select.waitUntilNotExists;

        /// <summary>
        /// Wait until no elements match the selector query
        /// </summary>
        /// <param name="interval">Optional interval between checks</param>
        /// <param name="wait">Optional total wait time</param>
        /// <returns>Select</returns>
        public static Select waitUntilNotExistsFor(Option<TimeSpan> interval = default, Option<TimeSpan> wait = default) =>
            Select.waitUntilNotExistsFor(interval, wait);

        /// <summary>
        /// Selects the currently focused element
        /// </summary>
        public static readonly Select active = Select.active;

        /// <summary>
        /// Select an item at a specific index
        /// </summary>
        /// <param name="ix">Index to select</param>
        public static Select atIndex(int ix) =>
            Select.atIndex(ix);

        /// <summary>
        /// Select elements by ARIA role
        /// </summary>
        /// <param name="role">ARIA role to match</param>
        /// <returns>Web element selector</returns>
        public static Select role(AriaRole role) => Select.byRole(role);

        /// <summary>
        /// Select elements by ARIA role and accessible name
        /// </summary>
        /// <param name="role">ARIA role to match</param>
        /// <param name="name">Accessible name to match</param>
        /// <returns>Web element selector</returns>
        public static Select role(AriaRole role, string name) => Select.byRole(role, name);

        /// <summary>
        /// Select elements by ARIA role and accessible name with exact matching control
        /// </summary>
        /// <param name="role">ARIA role to match</param>
        /// <param name="name">Accessible name to match</param>
        /// <param name="exact">Whether to match the name exactly</param>
        /// <returns>Web element selector</returns>
        public static Select role(AriaRole role, string name, bool exact) => Select.byRole(role, name, exact);

        /// <summary>
        /// Select elements by their associated label text
        /// </summary>
        /// <param name="text">Label text to match</param>
        /// <returns>Web element selector</returns>
        public static Select label(string text) => Select.byLabel(text);

        /// <summary>
        /// Select elements by their associated label text with exact matching control
        /// </summary>
        /// <param name="text">Label text to match</param>
        /// <param name="exact">Whether to match the text exactly</param>
        /// <returns>Web element selector</returns>
        public static Select label(string text, bool exact) => Select.byLabel(text, exact);

        /// <summary>
        /// Select elements by their text content
        /// </summary>
        /// <param name="text">Text content to match</param>
        /// <returns>Web element selector</returns>
        public static Select byText(string text) => Select.byText(text);

        /// <summary>
        /// Select elements by their text content with exact matching control
        /// </summary>
        /// <param name="text">Text content to match</param>
        /// <param name="exact">Whether to match the text exactly</param>
        /// <returns>Web element selector</returns>
        public static Select byText(string text, bool exact) => Select.byText(text, exact);

        /// <summary>
        /// Select elements by their test ID attribute (data-testid by default)
        /// </summary>
        /// <param name="testId">Test ID to match</param>
        /// <returns>Web element selector</returns>
        public static Select testId(string testId) => Select.byTestId(testId);

        /// <summary>
        /// Select elements by their placeholder text
        /// </summary>
        /// <param name="text">Placeholder text to match</param>
        /// <returns>Web element selector</returns>
        public static Select placeholder(string text) => Select.byPlaceholder(text);

        /// <summary>
        /// Select elements by their placeholder text with exact matching control
        /// </summary>
        /// <param name="text">Placeholder text to match</param>
        /// <param name="exact">Whether to match the text exactly</param>
        /// <returns>Web element selector</returns>
        public static Select placeholder(string text, bool exact) => Select.byPlaceholder(text, exact);
    }
}
