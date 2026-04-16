using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class SelectorTests
{
    [Fact]
    public async Task ById_ByXPath_ByClass_ByTag_ByName()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/login")
            from e1 in exists(id("username"))
            from _2 in assert(e1, "Expected byId('username') to find element")
            from e2 in exists(xPath("//input[@id='username']"))
            from _3 in assert(e2, "Expected byXPath to find element")
            from e3 in exists(className("radius"))
            from _4 in assert(e3, "Expected byClass('radius') to find element")
            from e4 in exists(tagName("form"))
            from _5 in assert(e4, "Expected byTag('form') to find element")
            from e5 in exists(name("username"))
            from _6 in assert(e5, "Expected byName('username') to find element")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task ByLinkText_ByPartialLinkText()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/")
            from e1 in exists(linkText("Form Authentication"))
            from _2 in assert(e1, "Expected byLinkText('Form Authentication') to find element")
            from e2 in exists(partialLinkText("Form Auth"))
            from _3 in assert(e2, "Expected byPartialLinkText('Form Auth') to find element")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Semantic_selectors_byRole_byLabel_byText_byPlaceholder()
    {
        var dataUrl = "data:text/html,<form><label for='email'>Email</label><input id='email' placeholder='Enter email' data-testid='email-field'><button>Submit</button></form>";

        var test =
            from _1 in nav(dataUrl)
            from e1 in exists(role(AriaRole.Button))
            from _2 in assert(e1, "Expected byRole(Button) to find element")
            from e3 in exists(label("Email"))
            from _4 in assert(e3, "Expected byLabel('Email') to find element")
            from e4 in exists(byText("Submit"))
            from _5 in assert(e4, "Expected byText('Submit') to find element")
            from e5 in exists(placeholder("Enter email"))
            from _6 in assert(e5, "Expected byPlaceholder('Enter email') to find element")
            from e6 in exists(testId("email-field"))
            from _7 in assert(e6, "Expected byTestId('email-field') to find element")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Select_composition_and_WhenSingle()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/checkboxes")
            // css("form") + css("input") should find 2 checkboxes
            from cnt in elementCount(css("form") + css("input"))
            from _2 in assert(cnt == 2, $"Expected 2 inputs from composed selector, got {cnt}")
            // whenSingle should fail because there are 2 inputs
            from failState in (css("#checkboxes") + css("input") + whenSingle)
                                  .ToSeq()
                                  .Map(_ => false)
                              | pure(true)
            from _3 in assert(failState, "Expected whenSingle to fail when 2 elements match")
            // whenAtLeastOne should succeed
            from okCount in elementCount(css("#checkboxes") + css("input") + whenAtLeastOne)
            from _4 in assert(okCount >= 1, $"Expected whenAtLeastOne to succeed, got count {okCount}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }
}
