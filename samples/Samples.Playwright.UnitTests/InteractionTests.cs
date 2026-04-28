using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class InteractionTests
{
    [Fact]
    public async Task SendKeys_Clear_Overwrite_ClearAndType()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            // sendKeys types into the field
            from _2 in sendKeys(css("#username"), "hello")
            from v1 in value(css("#username"))
            from _3 in assert(v1 == "hello", $"Expected 'hello' after sendKeys, got '{v1}'")
            // clear empties the field
            from _4 in clear(css("#username"))
            from v2 in value(css("#username"))
            from _5 in assert(v2 == "", $"Expected '' after clear, got '{v2}'")
            // overwrite replaces the value
            from _6 in overwrite(css("#username"), "world")
            from v3 in value(css("#username"))
            from _7 in assert(v3 == "world", $"Expected 'world' after overwrite, got '{v3}'")
            // clearAndType clears then types sequentially
            from _8 in clearAndType(css("#username"), "final")
            from v4 in value(css("#username"))
            from _9 in assert(v4 == "final", $"Expected 'final' after clearAndType, got '{v4}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task DoubleClick_triggers_event()
    {
        var dataUrl = "data:text/html,<p id='target' ondblclick=\"this.textContent='double-clicked'\">click me</p>";

        var test =
            from _1 in nav(dataUrl)
            from t1 in text(css("#target"))
            from _2 in assert(t1 == "click me", $"Expected 'click me' before double-click, got '{t1}'")
            from _3 in doubleClick(css("#target"))
            from t2 in text(css("#target"))
            from _4 in assert(t2 == "double-clicked", $"Expected 'double-clicked' after double-click, got '{t2}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task RightClick_triggers_context_menu()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/context_menu")
            from msg in onDialog(rightClick(css("#hot-spot")), accept: true)
            from _2 in assert(msg.Contains("You selected a context menu"), $"Expected context menu alert, got '{msg}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Hover_reveals_hidden_text()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/hovers")
            from _2 in moveToElement(css(".figure") + atIndex(0))
            from h5 in text(css(".figure") + atIndex(0) + css("h5"))
            from _3 in assert(h5.Contains("name:"), $"Expected h5 text containing 'name:', got '{h5}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task DragAndDrop()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/drag_and_drop")
            from hBefore in text(css("#column-a header"))
            from _2 in assert(hBefore == "A", $"Expected column-a header 'A', got '{hBefore}'")
            from _3 in dragTo(css("#column-a"), css("#column-b"))
            from hAfter in text(css("#column-a header"))
            from _4 in assert(hAfter == "B", $"Expected column-a header 'B' after drag, got '{hAfter}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task SelectByText_SelectByValue_SelectByPosition_GetSelected()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/dropdown")
            // selectByText to pick "Option 1"
            from _2 in selectByText(css("#dropdown"), "Option 1")
            from st in getSelectedOptionText(css("#dropdown"))
            from _3 in assert(st == "Option 1", $"Expected selected text 'Option 1', got '{st}'")
            // selectByValue to pick value "2"
            from _4 in selectByValue(css("#dropdown"), "2")
            from sv in getSelectedOptionValue(css("#dropdown"))
            from _5 in assert(sv == "2", $"Expected selected value '2', got '{sv}'")
            // selectByPosition to pick option in position 1
            from _6 in selectByPosition(css("#dropdown"), 1)
            from si in getSelectedOptionValue(css("#dropdown"))
            from _7 in assert(si == "1", $"Expected selected value '1', got '{si}'")
            // selectByTextContaining to pick option containing "tion 1"
            from _8 in selectByTextContaining(css("#dropdown"), "tion 1")
            from sc in getSelectedOptionText(css("#dropdown"))
            from _9 in assert(sc == "Option 1", $"Expected selected text 'Option 1', got '{sc}'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task SetCheckbox_IsCheckboxChecked()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/checkboxes")
            // Set first checkbox to true
            from _2 in setCheckbox(css("input[type='checkbox']") + atIndex(0), true)
            from c1 in isCheckboxChecked(css("input[type='checkbox']") + atIndex(0))
            from _3 in assert(c1, "Expected first checkbox to be checked")
            // Set second checkbox to false
            from _4 in setCheckbox(css("input[type='checkbox']") + atIndex(1), false)
            from c2 in isCheckboxChecked(css("input[type='checkbox']") + atIndex(1))
            from _5 in assert(!c2, "Expected second checkbox to be unchecked")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task WaitUntilClickable_succeeds_on_visible_enabled_element()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            from _2 in waitUntilClickable(css("button[type='submit']"))
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task MoveToLocation_moves_mouse()
    {
        var test =
            from _1 in nav("data:text/html,<html><body>mouse test</body></html>")
            from _2 in moveToLocation(100, 200)
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }
}
