using System.Drawing;
using LanguageExt;

namespace Isotope80
{
    /// <summary>
    /// Immutable web-element that doesn't suffer the problems of the lazy IWebElement (namely if the IWebDriver is
    /// disposed then IWebElement fails, which is a problem for lazy processes).
    /// </summary>
    /// <param name="Selector">SelectQuery selector that found this element</param>
    /// <param name="SelectionIndex">This structure was made from a @select.  This is the index into the @select results</param>
    /// <param name="Id">Element id attribute</param>
    /// <param name="TagName">Element tag name</param>
    /// <param name="Text">string Text</param>
    /// <param name="Enabled">Element enabled flag</param>
    /// <param name="Selected">Indicates whether or not this element is selected.</param>
    /// <param name="Location">Gets a <see cref="T:System.Drawing.Point" /> object containing the coordinates of the upper-left corner of this element relative to the upper-left corner of the page.</param>
    /// <param name="Size">Gets a <see cref="P:OpenQA.Selenium.IWebElement.Size" /> object containing the height and width of this element.</param>
    /// <param name="Displayed">Indicates whether or not this element is displayed.</param>
    public record WebElement(
        Select Selector,
        int SelectionIndex,
        string Id,
        string TagName,
        string Text,
        bool Enabled,
        bool Selected,
        Point Location,
        Size Size,
        bool Displayed)
    {
        public static Select operator +(WebElement element, Select @select) =>
            element.Selector + @select;
    }
}