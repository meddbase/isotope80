using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class QueryTests
{
    [Fact]
    public async Task Texts_returns_multiple_element_texts()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/checkboxes")
            from ts in texts(css("input[type='checkbox']"))
            from _2 in assert(ts.Count == 2, $"Expected 2 text entries, got {ts.Count}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Attribute_and_AttributeOrNone()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            from nameAttr in attribute(css("#username"), "name")
            from _2 in assert(nameAttr == "username", $"Expected name attribute 'username', got '{nameAttr}'")
            from noneAttr in attributeOrNone(css("#username"), "nonexistent")
            from _3 in assert(noneAttr.IsNone, "Expected None for nonexistent attribute")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task GetStyle_and_GetZIndex()
    {
        var dataUrl = "data:text/html,<div id='box' style='color:rgb(255,0,0);z-index:42;position:relative'>test</div>";

        var test =
            from _1 in nav(dataUrl)
            from color in getStyle(css("#box"), "color")
            from _2 in assert(color.Contains("rgb"), $"Expected color to contain 'rgb', got '{color}'")
            from z in getZIndex(css("#box"))
            from _3 in assert(z == 42, $"Expected z-index 42, got {z}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Enabled_disabled_element()
    {
        var dataUrl = "data:text/html,<button id='btn' disabled>Click</button><input id='inp' value='hi'>";

        var test =
            from _1 in nav(dataUrl)
            from btnEnabled in enabled(css("#btn"))
            from _2 in assert(!btnEnabled, "Expected disabled button to return false for enabled")
            from inpEnabled in enabled(css("#inp"))
            from _3 in assert(inpEnabled, "Expected enabled input to return true for enabled")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Obscured_element()
    {
        var dataUrl = "data:text/html,<div style='position:relative'><div id='back' style='position:absolute;top:0;left:0;width:100px;height:100px;background:red'>back</div><div id='front' style='position:absolute;top:0;left:0;width:100px;height:100px;background:blue'>front</div></div>";

        var test =
            from _1 in nav(dataUrl)
            from backObscured in obscured(css("#back"))
            from _2 in assert(backObscured, "Expected #back to be obscured by #front")
            from frontObscured in obscured(css("#front"))
            from _3 in assert(!frontObscured, "Expected #front to not be obscured")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task PageSource_contains_html()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/")
            from src in pageSource
            from _2 in assert(src.ToLower().Contains("<html"), "Expected page source to contain '<html'")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }
}
