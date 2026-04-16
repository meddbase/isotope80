using System;
using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class WindowAndTabTests
{
    [Fact]
    public async Task SetWindowSize_and_GetWindowSize()
    {
        var test =
            from _1 in nav("data:text/html,<html><body>resize test</body></html>")
            from _2 in setWindowSize(800, 600)
            from sz in getWindowSize
            from _3 in assert(sz.Width == 800, $"Expected width 800, got {sz.Width}")
            from _4 in assert(sz.Height == 600, $"Expected height 600, got {sz.Height}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Scroll_operations()
    {
        var test =
            from _1 in nav("https://the-internet.herokuapp.com/large")
            from _2 in scrollToBottom
            from scrollYAfterBottom in eval<double>("window.scrollY")
            from _3 in assert(scrollYAfterBottom > 0, $"Expected scrollY > 0 after scrollToBottom, got {scrollYAfterBottom}")
            from _4 in scrollToTop
            from scrollYAfterTop in eval<double>("window.scrollY")
            from _5 in assert(scrollYAfterTop == 0, $"Expected scrollY == 0 after scrollToTop, got {scrollYAfterTop}")
            from _6 in scrollBy(0, 200)
            from scrollYAfterBy in eval<double>("window.scrollY")
            from _7 in assert(scrollYAfterBy >= 150 && scrollYAfterBy <= 250, $"Expected scrollY ~200 after scrollBy(0,200), got {scrollYAfterBy}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task Tab_lifecycle()
    {
        var test =
            from _1 in nav("data:text/html,<html><body>tab test</body></html>")
            from count1 in getOpenedTabsCount
            from _2 in assert(count1 == 1, $"Expected 1 tab, got {count1}")
            from tabNum in getCurrentTabNumber
            from _3 in assert(tabNum == 0, $"Expected current tab number 0, got {tabNum}")
            from _4 in newTab
            from count2 in getOpenedTabsCount
            from _5 in assert(count2 == 2, $"Expected 2 tabs after newTab, got {count2}")
            from _6 in switchTabs(0)
            from _7 in closeTab
            from count3 in getOpenedTabsCount
            from _8 in assert(count3 == 1, $"Expected 1 tab after closeTab, got {count3}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task ScrollToElement_scrolls_target_into_view()
    {
        // Use a data URL with a very tall page so the target element is guaranteed off-screen
        var dataUrl = "data:text/html,<html><body><div style='height:5000px'>spacer</div><div id='target'>bottom</div></body></html>";

        var test =
            from _1 in nav(dataUrl)
            from scrollBefore in eval<double>("window.scrollY")
            from _2 in assert(scrollBefore == 0, $"Expected scrollY == 0 before scroll, got {scrollBefore}")
            from _3 in scrollToElement(css("#target"))
            from _4 in pause(TimeSpan.FromMilliseconds(100))
            from scrollAfter in eval<double>("window.scrollY")
            from _5 in assert(scrollAfter > 0, $"Expected scrollY > 0 after scrollToElement, got {scrollAfter}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }

    [Fact]
    public async Task NewWindow_creates_isolated_context()
    {
        var test =
            from _1 in nav("data:text/html,<html><body>original window</body></html>")
            from count1 in getOpenedTabsCount
            from _2 in assert(count1 == 1, $"Expected 1 tab in original context, got {count1}")
            from _3 in newWindow
            from count2 in getOpenedTabsCount
            from _4 in assert(count2 == 1, $"Expected 1 tab in new window context, got {count2}")
            select unit;

        await withChromium(test).RunAndThrowOnError();
    }
}
