using Isotope80;
using LanguageExt;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
using static Isotope80.Assertions;

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
        public static Isotope<Unit> GoToLoginPage =>
            context("Navigate to login page",
                from _ in nav($"{BaseUrl}/login")
                select unit);

        /// <summary>
        /// Enter credentials and submit the login form
        /// </summary>
        public static Isotope<Unit> Login(string username, string password) =>
            context("Login",
                from _1 in sendKeys(css("#username"), username)
                from _2 in sendKeys(css("#password"), password)
                from _3 in click(css("button[type='submit']"))
                select unit);

        /// <summary>
        /// Assert that login succeeded by checking the flash message
        /// </summary>
        public static Isotope<Unit> AssertLoginSuccess =>
            context("Assert login success",
                from msg in text(css("#flash"))
                from _   in assert(msg.Contains("You logged into a secure area"),
                                   $"Expected success message but got: {msg}")
                select unit);

        /// <summary>
        /// Assert that login failed
        /// </summary>
        public static Isotope<Unit> AssertLoginFailure =>
            context("Assert login failure",
                from msg in text(css("#flash"))
                from _   in assert(msg.Contains("Your username is invalid"),
                                   $"Expected failure message but got: {msg}")
                select unit);

        /// <summary>
        /// Full login flow with assertion
        /// </summary>
        public static Isotope<Unit> LoginFlow =>
            from _1 in GoToLoginPage
            from _2 in Login("tomsmith", "SuperSecretPassword!")
            from _3 in AssertLoginSuccess
            select unit;

        // ── Checkboxes ─────────────────────────────────────────────────

        /// <summary>
        /// Navigate to the checkboxes page and toggle them
        /// </summary>
        public static Isotope<Unit> CheckboxFlow =>
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
        public static Isotope<string> DropdownFlow =>
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
        public static Isotope<Seq<string>> HoverFlow =>
            context("Hover demo",
                from _1 in nav($"{BaseUrl}/hovers")
                from figures in find(css(".figure"))
                from names in figures.Map(fig =>
                    from _ in moveToElement(fig.Selector)
                    from t in text(fig.Selector + css("h5"))
                    select t).Sequence()
                select names);
    }
}
