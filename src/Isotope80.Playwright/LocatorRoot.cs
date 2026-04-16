using Microsoft.Playwright;

namespace Isotope80
{
    /// <summary>
    /// Abstraction over the root from which locators are created.
    /// Both <see cref="IPage"/> and <see cref="IFrameLocator"/> can create locators,
    /// but they do not share a common interface. This wrapper unifies them so the
    /// Select pipeline can root from either without branching.
    /// </summary>
    internal readonly struct LocatorRoot
    {
        readonly IPage _page;
        readonly IFrameLocator _frame;
        readonly bool _isFrame;

        LocatorRoot(IPage page)
        {
            _page = page;
            _frame = null;
            _isFrame = false;
        }

        LocatorRoot(IFrameLocator frame)
        {
            _page = null;
            _frame = frame;
            _isFrame = true;
        }

        /// <summary>
        /// Create a root from a page
        /// </summary>
        public static LocatorRoot FromPage(IPage page) => new LocatorRoot(page);

        /// <summary>
        /// Create a root from a frame locator
        /// </summary>
        public static LocatorRoot FromFrame(IFrameLocator frame) => new LocatorRoot(frame);

        /// <summary>
        /// Create a locator from a CSS/XPath/etc. selector string
        /// </summary>
        public ILocator Locator(string selector) =>
            _isFrame ? _frame.Locator(selector) : _page.Locator(selector);

        /// <summary>
        /// Create a locator by ARIA role
        /// </summary>
        public ILocator GetByRole(AriaRole role, PageGetByRoleOptions options = null) =>
            _isFrame
                ? _frame.GetByRole(role, options == null ? null : new FrameLocatorGetByRoleOptions
                    { Name = options.Name, NameString = options.NameString, Exact = options.Exact, NameRegex = options.NameRegex })
                : _page.GetByRole(role, options);

        /// <summary>
        /// Create a locator by label text
        /// </summary>
        public ILocator GetByLabel(string text, bool? exact = null) =>
            _isFrame
                ? _frame.GetByLabel(text, exact.HasValue ? new FrameLocatorGetByLabelOptions { Exact = exact } : null)
                : _page.GetByLabel(text, exact.HasValue ? new PageGetByLabelOptions { Exact = exact } : null);

        /// <summary>
        /// Create a locator by text content
        /// </summary>
        public ILocator GetByText(string text, bool? exact = null) =>
            _isFrame
                ? _frame.GetByText(text, exact.HasValue ? new FrameLocatorGetByTextOptions { Exact = exact } : null)
                : _page.GetByText(text, exact.HasValue ? new PageGetByTextOptions { Exact = exact } : null);

        /// <summary>
        /// Create a locator by test ID
        /// </summary>
        public ILocator GetByTestId(string testId) =>
            _isFrame ? _frame.GetByTestId(testId) : _page.GetByTestId(testId);

        /// <summary>
        /// Create a locator by placeholder text
        /// </summary>
        public ILocator GetByPlaceholder(string text, bool? exact = null) =>
            _isFrame
                ? _frame.GetByPlaceholder(text, exact.HasValue ? new FrameLocatorGetByPlaceholderOptions { Exact = exact } : null)
                : _page.GetByPlaceholder(text, exact.HasValue ? new PageGetByPlaceholderOptions { Exact = exact } : null);

        /// <summary>
        /// Create a frame locator scoped to this root
        /// </summary>
        public IFrameLocator FrameLocator(string selector) =>
            _isFrame ? _frame.FrameLocator(selector) : _page.FrameLocator(selector);
    }
}
