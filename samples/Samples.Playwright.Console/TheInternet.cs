using LanguageExt;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using Microsoft.Playwright;

namespace Isotope80.Samples.Console
{
    /// <summary>
    /// Sample automation against https://the-internet.herokuapp.com
    /// </summary>
    public static class TheInternet
    {
        static readonly string BaseUrl = "https://the-internet.herokuapp.com";

        // ── Login ──────────────────────────────────────────────────────

        /// <summary>
        /// Navigate to the login page
        /// </summary>
        public static IsotopeAsync<Unit> GoToLoginPage =>
            context("Navigate to login page",
                from _ in nav($"{BaseUrl}/login")
                select unit);

        /// <summary>
        /// Enter credentials and submit the login form
        /// </summary>
        public static IsotopeAsync<Unit> Login(string username, string password) =>
            context("Login",
                from _1 in fill(css("#username"), username)
                from _2 in fill(css("#password"), password)
                from _3 in click(css("button[type='submit']"))
                select unit);

        /// <summary>
        /// Assert that login succeeded by checking the flash message
        /// </summary>
        public static IsotopeAsync<Unit> AssertLoginSuccess =>
            context("Assert login success",
                from msg in text(css("#flash"))
                from _   in assert(msg.Contains("You logged into a secure area"),
                                   $"Expected success message but got: {msg}")
                select unit);

        /// <summary>
        /// Assert that login failed
        /// </summary>
        public static IsotopeAsync<Unit> AssertLoginFailure =>
            context("Assert login failure",
                from msg in text(css("#flash"))
                from _   in assert(msg.Contains("Your username is invalid"),
                                   $"Expected failure message but got: {msg}")
                select unit);

        /// <summary>
        /// Full login flow with assertion
        /// </summary>
        public static IsotopeAsync<Unit> LoginFlow =>
            from _1 in GoToLoginPage
            from _2 in Login("tomsmith", "SuperSecretPassword!")
            from _3 in AssertLoginSuccess
            select unit;

        // ── Checkboxes ─────────────────────────────────────────────────

        /// <summary>
        /// Navigate to the checkboxes page and toggle them
        /// </summary>
        public static IsotopeAsync<Unit> CheckboxFlow =>
            context("Checkbox demo",
                from _1 in nav($"{BaseUrl}/checkboxes")
                from _2 in setCheckbox(css("#checkboxes input:first-child"), true)
                from _3 in setCheckbox(css("#checkboxes input:last-child"), false)
                from c1 in isCheckboxChecked(css("#checkboxes input:first-child"))
                from c2 in isCheckboxChecked(css("#checkboxes input:last-child"))
                from _4 in assert(c1, "First checkbox should be checked")
                from _5 in assert(!c2, "Second checkbox should be unchecked")
                select unit);

        // ── Dropdown ───────────────────────────────────────────────────

        /// <summary>
        /// Navigate to the dropdown page and select an option
        /// </summary>
        public static IsotopeAsync<string> DropdownFlow =>
            context("Dropdown demo",
                from _1 in nav($"{BaseUrl}/dropdown")
                from _2 in selectByText(css("#dropdown"), "Option 2")
                from selected in getSelectedOptionText(css("#dropdown"))
                from _3 in assert(selected == "Option 2",
                                  $"Expected 'Option 2' but got '{selected}'")
                select selected);

        // ── Hover ──────────────────────────────────────────────────────

        /// <summary>
        /// Navigate to the hovers page and reveal hidden text
        /// </summary>
        public static IsotopeAsync<Seq<string>> HoverFlow =>
            context("Hover demo",
                from _1 in nav($"{BaseUrl}/hovers")
                from figures in find(css(".figure"))
                from names in figures.Map(fig =>
                    from _ in moveToElement(fig.Selector)
                    from t in text(fig.Selector + css("h5"))
                    select t).Sequence()
                select names);

        // ── Tracing ────────────────────────────────────────────────────

        /// <summary>
        /// Demonstrates Playwright trace recording. Runs the login flow
        /// wrapped in a trace, saving the result to a .zip file that can
        /// be viewed with `npx playwright show-trace trace.zip`.
        /// </summary>
        public static IsotopeAsync<Unit> TraceDemo(string tracePath) =>
            context("Trace demo",
                withTrace(tracePath,
                    from _1 in GoToLoginPage
                    from _2 in Login("tomsmith", "SuperSecretPassword!")
                    from _3 in AssertLoginSuccess
                    select unit));

        /// <summary>
        /// Demonstrates withTraceOnFailure — saves a trace only when something goes wrong.
        /// Runs a login with a bad password so the trace captures the failure.
        /// </summary>
        public static IsotopeAsync<Unit> TraceOnFailureDemo(string traceDir) =>
            context("Trace-on-failure demo",
                withTraceOnFailure(traceDir,
                    from _1 in GoToLoginPage
                    from _2 in Login("tomsmith", "WrongPassword")
                    from _3 in AssertLoginSuccess
                    select unit));

        // ── Console Capture ────────────────────────────────────────────

        /// <summary>
        /// Demonstrates capturing browser console output.
        /// Navigates to a page and runs a script that logs a message,
        /// then returns the captured log entries.
        /// </summary>
        public static IsotopeAsync<Seq<BrowserLogEntry>> ConsoleCaptureDemo =>
            context("Console capture demo",
                from result in withConsoleCapture(
                    from _1 in nav($"{BaseUrl}/javascript_alerts")
                    from _2 in eval("console.log('Isotope80.Playwright captured this!')")
                    from _3 in eval("console.warn('And this warning too.')")
                    select unit)
                select result.Logs);

        // ── Network Interception ───────────────────────────────────────

        /// <summary>
        /// Demonstrates intercepting and mocking a network request.
        /// Sets up a route that intercepts image requests and aborts them,
        /// then navigates to a page with images.
        /// </summary>
        public static IsotopeAsync<int> NetworkInterceptionDemo =>
            context("Network interception demo",
                from _1 in route("**/*.png", async r => await r.AbortAsync())
                from _2 in route("**/*.jpg", async r => await r.AbortAsync())
                from _3 in route("**/*.gif", async r => await r.AbortAsync())
                from _4 in nav($"{BaseUrl}/broken_images")
                from count in elementCount(css("img"))
                from _5 in unroute("**/*.png")
                from _6 in unroute("**/*.jpg")
                from _7 in unroute("**/*.gif")
                select count);

        // ── Dialog Handling ────────────────────────────────────────────

        /// <summary>
        /// Demonstrates Playwright's dialog handling via onDialog.
        /// Triggers a JS alert and captures the message.
        /// </summary>
        public static IsotopeAsync<Seq<string>> DialogDemo =>
            context("Dialog handling demo",
                from _1 in nav($"{BaseUrl}/javascript_alerts")
                from alertMsg in onDialog(click(css("button[onclick='jsAlert()']")))
                from confirmMsg in onDialog(click(css("button[onclick='jsConfirm()']")), accept: false)
                select Seq(alertMsg, confirmMsg));

        // ── Semantic Selectors ─────────────────────────────────────────

        /// <summary>
        /// Demonstrates Playwright-specific semantic selectors (byRole, byText, etc.)
        /// on the login page.
        /// </summary>
        public static IsotopeAsync<Unit> SemanticSelectorDemo =>
            context("Semantic selector demo",
                from _1 in nav($"{BaseUrl}/login")
                from _2 in fill(label("Username"), "tomsmith")
                from _3 in fill(label("Password"), "SuperSecretPassword!")
                from _4 in click(role(AriaRole.Button, " Login"))
                from msg in text(css("#flash"))
                from _5 in assert(msg.Contains("You logged into"), "Semantic selector login failed")
                select unit);

        // ── Frame Access ───────────────────────────────────────────────

        /// <summary>
        /// Demonstrates accessing content inside an iframe using inFrame.
        /// </summary>
        public static IsotopeAsync<string> FrameDemo =>
            context("Frame demo",
                from _1 in nav($"{BaseUrl}/iframe")
                from t in inFrame(css("#mce_0_ifr"),
                    from content in text(css("#tinymce"))
                    select content)
                select t);

        // ── Context Isolation ──────────────────────────────────────────

        /// <summary>
        /// Demonstrates that withNewBrowserContext creates an isolated browser context.
        /// Sets a cookie in the main context, then verifies it's absent in a new one.
        /// </summary>
        public static IsotopeAsync<(bool MainHasCookie, bool IsolatedHasCookie)> ContextIsolationDemo =>
            context("Context isolation demo",
                from _1 in nav($"{BaseUrl}/")
                from _2 in setCookie(new BrowserCookie("isotope_demo", "hello", ".herokuapp.com", "/", null, false, false, "Lax"))
                from mainCookies in getCookies
                let mainHas = mainCookies.Exists(c => c.Name == "isotope_demo")
                from isolatedCookies in withNewBrowserContext(
                    from _3 in nav($"{BaseUrl}/")
                    from c in getCookies
                    select c)
                let isolatedHas = isolatedCookies.Exists(c => c.Name == "isotope_demo")
                select (mainHas, isolatedHas));
    }
}
