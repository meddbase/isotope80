using LanguageExt;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt.Common;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Remote;
using static LanguageExt.Prelude;
using static Isotope80.IsotopeInternal;

namespace Isotope80
{
    /// <summary>
    /// Isotope extensions
    /// </summary>
    public static partial class Isotope
    {
        /// <summary>
        /// Simple configuration setup
        /// </summary>
        /// <param name="config">Map of config items</param>
        public static Isotope<Unit> initConfig(params (string, string)[] config) =>
            initConfig(toHashMap(config));

        /// <summary>
        /// Simple configuration setup
        /// </summary>
        /// <param name="config">Map of config items</param>
        public static Isotope<Unit> initConfig(Map<string, string> config) =>
            initConfig(toHashMap(config));

        /// <summary>
        /// Simple configuration setup
        /// </summary>
        /// <param name="config">Map of config items</param>
        public static Isotope<Unit> initConfig(HashMap<string, string> config) =>
            from s in get
            from _ in put(s.With(Configuration: config))
            select unit;

        /// <summary>
        /// Get a config key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Isotope<string> config(string key) =>
            from s in get
            from r in s.Configuration.Find(key).ToIsotope($"Configuration key not found: {key}")
            select r;

        /// <summary>
        /// Update the settings within the Isotope computation
        /// </summary>
        public static Isotope<Unit> initSettings(IsotopeSettings settings) =>
            from s in get
            from _ in put(s.With(Settings: settings))
            select unit;

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static Isotope<B> use<A, B>(A resource, Func<A, Isotope<B>> f) where A : IDisposable =>
            new Isotope<B>(s => 
            {
                try
                {
                    return f(resource).Invoke(s);
                }
                finally
                {
                    resource?.Dispose();
                }
            });

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="Env">Environment type</typeparam>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static Isotope<Env, B> use<Env, A, B>(A resource, Func<A, Isotope<Env, B>> f) where A : IDisposable =>
            new Isotope<Env, B>((e, s) => 
            {
                try
                {
                    return f(resource).Invoke(e, s);
                }
                finally
                {
                    resource?.Dispose();
                }
            });


        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static IsotopeAsync<B> use<A, B>(A resource, Func<A, IsotopeAsync<B>> f) where A : IDisposable =>
            new IsotopeAsync<B>(async s => 
            {
                try
                {
                    return await f(resource).Invoke(s).ConfigureAwait(false);
                }
                finally
                {
                    resource?.Dispose();
                }
            });

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="Env">Environment type</typeparam>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static IsotopeAsync<Env, B> use<Env, A, B>(A resource, Func<A, IsotopeAsync<Env, B>> f) where A : IDisposable =>
            new IsotopeAsync<Env, B>(async (e, s) => 
            {
                try
                {
                    return await f(resource).Invoke(e, s).ConfigureAwait(false);
                }
                finally
                {
                    resource?.Dispose();
                }
            });

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="dispose">Function to clean up the resource on completion</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static Isotope<B> use<A, B>(A resource, Func<A, Unit> dispose, Func<A, Isotope<B>> f) =>
            new Isotope<B>(s => 
            {
                try
                {
                    return f(resource).Invoke(s);
                }
                finally
                {
                    dispose(resource);
                }
            });

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="dispose">Function to clean up the resource on completion</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="Env">Environment type</typeparam>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static Isotope<Env, B> use<Env, A, B>(A resource, Func<A, Unit> dispose, Func<A, Isotope<Env, B>> f) =>
            new Isotope<Env, B>((e, s) => 
            {
                try
                {
                    return f(resource).Invoke(e, s);
                }
                finally
                {
                    dispose(resource);
                }
            });


        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="dispose">Function to clean up the resource on completion</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static IsotopeAsync<B> use<A, B>(A resource, Func<A, Unit> dispose, Func<A, IsotopeAsync<B>> f) =>
            new IsotopeAsync<B>(async s => 
            {
                try
                {
                    return await f(resource).Invoke(s).ConfigureAwait(false);
                }
                finally
                {
                    dispose(resource);
                }
            });

        /// <summary>
        /// Use a disposable resource, and clean it up afterwards
        /// </summary>
        /// <param name="resource">Disposable resource</param>
        /// <param name="dispose">Function to clean up the resource on completion</param>
        /// <param name="f">Function to receive the resource and return an isotope run in that context</param>
        /// <typeparam name="Env">Environment type</typeparam>
        /// <typeparam name="A">Disposable resource type</typeparam>
        /// <typeparam name="B">Resulting bound value type</typeparam>
        public static IsotopeAsync<Env, B> use<Env, A, B>(A resource, Func<A, Unit> dispose, Func<A, IsotopeAsync<Env, B>> f) =>
            new IsotopeAsync<Env, B>(async (e, s) => 
            {
                try
                {
                    return await f(resource).Invoke(e, s).ConfigureAwait(false);
                }
                finally
                {
                    dispose(resource);
                }
            });

        /// <summary>
        /// Clean up function for web-drivers
        /// </summary>
        static Unit disposeWebDriver(IWebDriver d)
        {
            d.Quit();
            d.Dispose();
            return unit;
        }

        /// <summary>
        /// Run the isotope provided with the web-driver context
        /// </summary>
        public static Isotope<A> withWebDriver<A>(IWebDriver driver, Isotope<A> ma) =>
            use(driver, disposeWebDriver, d => from st in get
                                               from _1 in setWebDriver(driver)  
                                               from rs in ma
                                               from _2 in st.Driver.Match(Some: setWebDriver, None: clearWebDriver) 
                                               select rs);

        /// <summary>
        /// Run the isotope provided with the web-driver context
        /// </summary>
        public static Isotope<Env, A> withWebDriver<Env, A>(IWebDriver driver, Isotope<Env, A> ma) =>
            use(driver, disposeWebDriver, d => from st in get
                                               from _1 in setWebDriver(driver)  
                                               from rs in ma
                                               from _2 in st.Driver.Match(Some: setWebDriver, None: clearWebDriver) 
                                               select rs);

        /// <summary>
        /// Run the isotope provided with the web-driver context
        /// </summary>
        public static IsotopeAsync<A> withWebDriver<A>(IWebDriver driver, IsotopeAsync<A> ma) =>
            use(driver, disposeWebDriver, d => from st in get
                                               from _1 in setWebDriver(driver)  
                                               from rs in ma
                                               from _2 in st.Driver.Match(Some: setWebDriver, None: clearWebDriver) 
                                               select rs);

        /// <summary>
        /// Run the isotope provided with the web-driver context
        /// </summary>
        public static IsotopeAsync<Env, A> withWebDriver<Env, A>(IWebDriver driver, IsotopeAsync<Env, A> ma) =>
            use(driver, disposeWebDriver, d => from st in get
                                               from _1 in setWebDriver(driver)  
                                               from rs in ma
                                               from _2 in st.Driver.Match(Some: setWebDriver, None: clearWebDriver) 
                                               select rs);

        /// <summary>
        /// Map a local environment
        /// </summary>
        public static Isotope<EnvA, A> local<EnvA, EnvB, A>(Func<EnvA, EnvB> f, Isotope<EnvB, A> ma) =>
            new Isotope<EnvA, A>((ea, s) => ma.Invoke(f(ea), s));

        /// <summary>
        /// Map a local environment
        /// </summary>
        public static IsotopeAsync<EnvA, A> local<EnvA, EnvB, A>(Func<EnvA, EnvB> f, IsotopeAsync<EnvB, A> ma) =>
            new IsotopeAsync<EnvA, A>((ea, s) => ma.Invoke(f(ea), s));

        /// <summary>
        /// Run the isotope provided with the web-driver context
        /// </summary>
        public static Isotope<Unit> withWebDrivers<A>(Isotope<A> ma, params WebDriverSelect[] webDrivers) =>
            new Isotope<Unit>(s => {
                           
                Seq<Error> errors = Empty;
                
                foreach (var webDriver in webDrivers)
                {
                    var (d, nm) = webDriver switch
                                  {
                                      WebDriverSelect.Chrome           => (new ChromeDriver() as IWebDriver, "Chrome"),
                                      WebDriverSelect.Firefox          => (new FirefoxDriver(), "Firefox"),
                                      WebDriverSelect.Edge             => (new EdgeDriver(), "Edge"),
                                      WebDriverSelect.InternetExplorer => (new InternetExplorerDriver(), "IE"),
                                      WebDriverSelect.Safari           => (new SafariDriver(), "Safari"),
                                      _                                => throw new NotSupportedException($"Web-driver not supported: {webDriver}")
                                  };

                    // Run with the web-driver
                    var r = context(nm, withWebDriver(d, ma)).Invoke(s);

                    // Collect the errors, prefix them with the name of the browser
                    errors = errors + r.State.Error;
                }
                return new IsotopeState<Unit>(default, s.With(Error: errors));
            });

        /// <summary>
        /// Run the isotope provided with the web-driver context
        /// </summary>
        public static Isotope<Env, Unit> withWebDrivers<Env, A>(Isotope<Env, A> ma, params WebDriverSelect[] webDrivers) =>
            new Isotope<Env, Unit>((e, s) => {
                           
                Seq<Error> errors = Empty;
                
                foreach (var webDriver in webDrivers)
                {
                    var (d, nm) = webDriver switch
                                  {
                                      WebDriverSelect.Chrome           => (new ChromeDriver() as IWebDriver, "Chrome"),
                                      WebDriverSelect.Firefox          => (new FirefoxDriver(), "Firefox"),
                                      WebDriverSelect.Edge             => (new EdgeDriver(), "Edge"),
                                      WebDriverSelect.InternetExplorer => (new InternetExplorerDriver(), "IE"),
                                      WebDriverSelect.Safari           => (new SafariDriver(), "Safari"),
                                      _                                => throw new NotSupportedException($"Web-driver not supported: {webDriver}")
                                  };

                    // Run with the web-driver
                    var r = context(nm, withWebDriver(d, ma)).Invoke(e, s);

                    // Collect the errors, prefix them with the name of the browser
                    errors = errors + r.State.Error;
                }
                return new IsotopeState<Unit>(default, s.With(Error: errors));
            });

        /// <summary>
        /// Run the isotope provided with the web-driver context
        /// </summary>
        public static IsotopeAsync<Unit> withWebDrivers<A>(IsotopeAsync<A> ma, params WebDriverSelect[] webDrivers) =>
            new IsotopeAsync<Unit>(async s => {
                           
                Seq<Error> errors = Empty;
                
                foreach (var webDriver in webDrivers)
                {
                    var (d, nm) = webDriver switch
                                  {
                                      WebDriverSelect.Chrome           => (new ChromeDriver() as IWebDriver, "Chrome"),
                                      WebDriverSelect.Firefox          => (new FirefoxDriver(), "Firefox"),
                                      WebDriverSelect.Edge             => (new EdgeDriver(), "Edge"),
                                      WebDriverSelect.InternetExplorer => (new InternetExplorerDriver(), "IE"),
                                      WebDriverSelect.Safari           => (new SafariDriver(), "Safari"),
                                      _                                => throw new NotSupportedException($"Web-driver not supported: {webDriver}")
                                  };

                    // Run with the web-driver
                    var r = await context(nm, withWebDriver(d, ma)).Invoke(s).ConfigureAwait(false);

                    // Collect the errors, prefix them with the name of the browser
                    errors = errors + r.State.Error;
                }
                return new IsotopeState<Unit>(default, s.With(Error: errors));
            });

        /// <summary>
        /// Run the isotope provided with the web-driver context
        /// </summary>
        public static IsotopeAsync<Env, Unit> withWebDrivers<Env, A>(IsotopeAsync<Env, A> ma, params WebDriverSelect[] webDrivers) =>
            new IsotopeAsync<Env, Unit>(async (e, s) => {
                           
                Seq<Error> errors = Empty;
                
                foreach (var webDriver in webDrivers)
                {
                    var (d, nm) = webDriver switch
                                  {
                                      WebDriverSelect.Chrome           => (new ChromeDriver() as IWebDriver, "Chrome"),
                                      WebDriverSelect.Firefox          => (new FirefoxDriver(), "Firefox"),
                                      WebDriverSelect.Edge             => (new EdgeDriver(), "Edge"),
                                      WebDriverSelect.InternetExplorer => (new InternetExplorerDriver(), "IE"),
                                      WebDriverSelect.Safari           => (new SafariDriver(), "Safari"),
                                      _                                => throw new NotSupportedException($"Web-driver not supported: {webDriver}")
                                  };

                    // Run with the web-driver
                    var r = await context(nm, withWebDriver(d, ma)).Invoke(e, s);

                    // Collect the errors, prefix them with the name of the browser
                    errors = errors + r.State.Error;
                }
                return new IsotopeState<Unit>(default, s.With(Error: errors));
            });
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static Isotope<A> withChromeDriver<A>(Isotope<A> ma) =>
            context("Chrome", withWebDriver(new ChromeDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<A> withEdgeDriver<A>(Isotope<A> ma) =>
            context("Edge", withWebDriver(new EdgeDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<A> withFirefoxDriver<A>(Isotope<A> ma) =>
            context("Firefox", withWebDriver(new FirefoxDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Internet Explorer web-driver
        /// </summary>
        public static Isotope<A> withInternetExplorerDriver<A>(Isotope<A> ma) =>
            context("IE", withWebDriver(new InternetExplorerDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<A> withSafariDriver<A>(Isotope<A> ma) =>
            context("Safari", withWebDriver(new SafariDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static Isotope<Env, A> withChromeDriver<Env, A>(Isotope<Env, A> ma) =>
            context("Chrome", withWebDriver(new ChromeDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<Env, A> withEdgeDriver<Env, A>(Isotope<Env, A> ma) =>
            context("Edge", withWebDriver(new EdgeDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<Env, A> withFirefoxDriver<Env, A>(Isotope<Env, A> ma) =>
            context("Firefox", withWebDriver(new FirefoxDriver(), ma));
        
        /// <summary>
        /// Run the isotope provided with Internet Explorer web-driver
        /// </summary>
        public static Isotope<Env, A> withInternetExplorerDriver<Env, A>(Isotope<Env, A> ma) =>
            context("IE", withWebDriver(new InternetExplorerDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<Env, A> withSafariDriver<Env, A>(Isotope<Env, A> ma) =>
            context("Safari", withWebDriver(new SafariDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<A> withChromeDriver<A>(IsotopeAsync<A> ma) =>
            context("Chrome", withWebDriver(new ChromeDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<A> withEdgeDriver<A>(IsotopeAsync<A> ma) =>
            context("Edge", withWebDriver(new EdgeDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<A> withFirefoxDriver<A>(IsotopeAsync<A> ma) =>
            context("Firefox", withWebDriver(new FirefoxDriver(), ma));
                
        /// <summary>
        /// Run the isotope provided with Internet Explorer web-driver
        /// </summary>
        public static IsotopeAsync<A> withInternetExplorerDriver<A>(IsotopeAsync<A> ma) =>
            context("IE", withWebDriver(new InternetExplorerDriver(), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<A> withSafariDriver<A>(IsotopeAsync<A> ma) =>
            context("Safari", withWebDriver(new SafariDriver(), ma));
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withChromeDriver<Env, A>(IsotopeAsync<Env, A> ma) =>
            context("Chrome", withWebDriver(new ChromeDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withEdgeDriver<Env, A>(IsotopeAsync<Env, A> ma) =>
            context("Edge", withWebDriver(new EdgeDriver(), ma));

        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withFirefoxDriver<Env, A>(IsotopeAsync<Env, A> ma) =>
            context("Firefox", withWebDriver(new FirefoxDriver(), ma));
                
        /// <summary>
        /// Run the isotope provided with Internet Explorer web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withInternetExplorerDriver<Env, A>(IsotopeAsync<Env, A> ma) =>
            context("IE", withWebDriver(new InternetExplorerDriver(), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withSafariDriver<Env, A>(IsotopeAsync<Env, A> ma) =>
            context("Safari", withWebDriver(new SafariDriver(), ma));
        
        /// <summary>
        /// Set the window size of the browser
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        public static Isotope<Unit> setWindowSize(int width, int height) =>
            setWindowSize(new Size(width, height));

        /// <summary>
        /// Set the window size of the browser
        /// </summary>
        public static Isotope<Unit> setWindowSize(Size size) =>
            from d in webDriver
            from _ in trya(() => d.Manage().Window.Size = size, $"Failed to change browser window size to {size.Width} by {size.Height}")
            select unit;

        /// <summary>
        /// Set the window position of the browser
        /// </summary>
        /// <param name="x">Horizontal offset coordinate from left screen bound</param>
        /// <param name="y">Vertical offset coordinate from upper screen bount</param>
        public static Isotope<Unit> setWindowPosition(int x, int y) =>
            setWindowPosition(new Point(x, y));

        /// <summary>
        /// Set the window position of the browser
        /// </summary>
        public static Isotope<Unit> setWindowPosition(Point point) =>
            from d in webDriver
            from _ in trya(() => d.Manage().Window.Position = point, $"Failed to move browser window to {point.X}, {point.Y}")
            select unit;

        /// <summary>
        /// Maximise browser window
        /// </summary>
        public static Isotope<Unit> maximiseWindow =>
            from d in webDriver
            from _ in trya(() => d.Manage().Window.Maximize(), "Failed to maximise browser window")
            select unit;
        
        /// <summary>
        /// Minimise browser window
        /// </summary>
        public static Isotope<Unit> minimiseWindow =>
            from d in webDriver
            from _ in trya(() => d.Manage().Window.Minimize(), "Failed to minimise browser window")
            select unit;
        
        /// <summary>
        /// Set browser window to full screen
        /// </summary>
        public static Isotope<Unit> fullscreenWindow =>
            from d in webDriver
            from _ in trya(() => d.Manage().Window.FullScreen(), "Failed to change browser to fullscreen")
            select unit;

        /// <summary>
        /// Navigate back using the browser's back button
        /// </summary>
        public static Isotope<Unit> back =>
            from d in webDriver
            from _ in trya(() => d.Navigate().Back(), "Failed to go back in browser")
            select unit;

        /// <summary>
        /// Navigate forward using the browser's forward button
        /// </summary>
        public static Isotope<Unit> forward =>
            from d in webDriver
            from _ in trya(() => d.Navigate().Forward(), "Failed to go forward in browser")
            select unit;

        /// <summary>
        /// Refresh current page
        /// </summary>
        public static Isotope<Unit> refresh =>
            from d in webDriver
            from _ in trya(() => d.Navigate().Refresh(), "Failed to refresh current page")
            select unit;

        /// <summary>
        /// Opens and switches to new tab
        /// </summary>
        public static Isotope<Unit> newTab =>
            from d in webDriver
            from _ in trya(() => d.SwitchTo().NewWindow(WindowType.Tab), "Failed to open new tab")
            select unit;

        /// <summary>
        /// Change browser tab by position, determined by the order opened <para/>
        /// Tabs in separate window also switchable to in the same order
        /// </summary>
        /// <param name="position">Zero-based position of tab</param>
        /// <returns></returns>
        public static Isotope<Unit> switchTabs(int position) =>
            from d in webDriver
            let tabs = d.WindowHandles
            from _ in trya(() => d.SwitchTo().Window(tabs[position]), $"Failed to switch to tab {position}")
            select unit;

        /// <summary>
        /// Close current tab
        /// </summary>
        public static Isotope<Unit> closeTab =>
            from d in webDriver
            let currentTab = d.WindowHandles.IndexOf(d.CurrentWindowHandle)
            from _c in trya(() => d.Close(), "Failed to close current tab")
            from _s in currentTab > 0                   
                           ? switchTabs(currentTab - 1) // focus has been lost when tab closed
                           : pure(unit)
            select unit;
        
        /// <summary>
        /// Opens and switches to new window
        /// </summary>
        public static Isotope<Unit> newWindow =>
            from d in webDriver
            from _ in trya(() => d.SwitchTo().NewWindow(WindowType.Window), "Failed to open new window")
            select unit;

        /// <summary>
        /// Navigate to a URL
        /// </summary>
        /// <param name="url">URL to navigate to</param>
        public static Isotope<Unit> nav(string url) =>
            from d in webDriver
            from _ in trya(() => d.Navigate().GoToUrl(url), $"Failed to navigate to: {url}")
            select unit;

        /// <summary>
        /// Gets the URL currently displayed by the browser
        /// </summary>
        public static Isotope<string> url =>
            from d in webDriver
            select d.Url;

        /// <summary>
        /// Find an HTML element
        /// </summary>
        /// <param name="selector">Element selector</param>
        public static Isotope<WebElement> find1(Select selector) =>
            find(selector + whenAtLeastOne).Map(es => es.Head);

        /// <summary>
        /// Find an HTML element within another
        /// </summary>
        /// <param name="element">Element to search</param>
        /// <param name="selector">Child element selector</param>
        public static Isotope<WebElement> find1(WebElement element, Select selector) =>
            from dr in webDriver
            from rs in find1(Select.byId(element.Id) + selector) |
                       find1(element.Selector + atIndex(element.SelectionIndex) + selector)
            select rs; 
        
        /// <summary>
        /// Find HTML elements
        /// </summary>
        /// <param name="selector">Element selector</param>
        public static Isotope<Seq<WebElement>> find(Select selector) =>
            selector.ToSeq();

        /// <summary>
        /// Find HTML elements within another
        /// </summary>
        /// <param name="element">Element to search</param>
        /// <param name="selector">Element selector</param>
        public static Isotope<Seq<WebElement>> find(WebElement element, Select selector) =>
            from dr in webDriver
            from rs in find(Select.byId(element.Id) + selector) |
                       find(element.Selector + atIndex(element.SelectionIndex) + selector)
            select rs; 

        /// <summary>
        /// Select a &lt;select&gt; option by text
        /// </summary>     
        public static Isotope<Unit> selectByText(Select selector, string text) =>
            from el in selector.ToIsotopeHead()
            from se in IsotopeInternal.toSelectElement(el)
            from _  in IsotopeInternal.selectByText(se, text)
            select unit;

        /// <summary>
        /// Select a &lt;select&gt; option by value
        /// </summary>     
        public static Isotope<Unit> selectByValue(Select selector, string value) =>
            from el in selector.ToIsotopeHead()
            from se in IsotopeInternal.toSelectElement(el)
            from _  in IsotopeInternal.selectByValue(se, value)
            select unit;

        /// <summary>
        /// Retrieves the text for the selected option element in a Select Element
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <returns>The selected Option text</returns>
        public static Isotope<string> getSelectedOptionText(Select selector) =>
            from ele in selector.ToIsotopeHead()
            from sel in IsotopeInternal.toSelectElement(ele)
            from opt in IsotopeInternal.getSelectedOption(sel)
            from txt in IsotopeInternal.text(opt)
            select txt;

        /// <summary>
        /// Retrieves the value for the selected option element in a Select Element
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <returns>The selected Option value</returns>
        public static Isotope<string> getSelectedOptionValue(Select selector) =>
            from ele in selector.ToIsotopeHead()
            from sel in IsotopeInternal.toSelectElement(ele)
            from opt in IsotopeInternal.getSelectedOption(sel)
            from val in IsotopeInternal.value(opt)
            select val;

        /// <summary>
        /// Finds a checkbox element by selector and identifies whether it is checked
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>Is checked\s</returns>
        public static Isotope<bool> isCheckboxChecked(Select selector) =>
            from ele in selector.ToIsotopeHead()
            from res in IsotopeInternal.isCheckboxChecked(ele)
            select res;

        /// <summary>
        /// Set checkbox value for existing element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="ticked">Check the box or not</param>
        public static Isotope<Unit> setCheckbox(Select selector, bool ticked) =>
            from ele in selector.ToIsotopeHead()
            from val in IsotopeInternal.isCheckboxChecked(ele)
            from _   in val == ticked
                        ? pure(unit)
                        : IsotopeInternal.click(ele)
            select unit;

        /// <summary>
        /// Looks for a particular style attribute on an existing element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="style">Style attribute to look up</param>
        /// <returns>A string representing the style value</returns>
        public static Isotope<string> getStyle(Select selector, string style) =>
            selector.ToIsotopeHead()
                    .Bind(el => IsotopeInternal.getStyle(el, style));

        /// <summary>
        /// Gets the Z Index style attribute value for an existing element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>The Z Index value</returns>
        public static Isotope<int> getZIndex(Select selector) =>
            from ele in selector.ToIsotopeHead()
            from zis in IsotopeInternal.getStyle(ele, "zIndex")
            from zii in parseInt(zis).ToIsotope($"z-Index was not valid integer: {zis}.")
            select zii;

        /// <summary>
        /// Looks for a particular style attribute on an existing element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="att">Attribute to look up</param>
        /// <returns>A string representing the attribute value</returns>
        public static Isotope<string> attribute(Select selector, string att) =>
            selector.ToIsotopeHead()
                    .Bind(el => IsotopeInternal.attribute(el, att));

        /// <summary>
        /// Simulates keyboard by sending `keys` 
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="keys">String of characters that are typed</param>
        public static Isotope<Unit> sendKeys(Select selector, string keys) =>
            selector.ToIsotopeHead()
                    .Bind(el => IsotopeInternal.sendKeys(el, keys));

        /// <summary>
        /// Simulates the mouse-click
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static Isotope<Unit> click(Select selector) =>
            selector.ToIsotopeHead()
                    .Bind(IsotopeInternal.click);
        
        /// <summary>
        /// Clears the content of an element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static Isotope<Unit> clear(Select selector) =>
            selector.ToIsotopeHead()
                    .Bind(IsotopeInternal.clear);

        /// <summary>
        /// Simulates keyboard by sending `keys` and overwriting current content
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="keys">String of characters that are typed</param>
        /// <remarks>
        /// Series of actions where element is clicked, pressed keys CTRL+A, Backspace then typed in new keys.
        /// It is an alternative to clear (without triggering event (change, blur or focus)) and sendKeys
        /// 
        /// https://stackoverflow.com/questions/19833728/webelement-clear-fires-javascript-change-event-alternatives
        /// </remarks>
        public static Isotope<Unit> overwrite(Select selector, string keys) =>
            selector.ToIsotopeHead()
                    .Bind(el => IsotopeInternal.overwrite(el, keys));
        
        /// <summary>
        /// ONLY USE AS A LAST RESORT
        /// Pauses the processing for an interval to brute force waiting for actions to complete
        /// </summary>
        public static Isotope<Unit> pause(TimeSpan interval)
        {
            Thread.Sleep((int)interval.TotalMilliseconds);
            return pure(unit);
        }

        /// <summary>
        /// Gets the text inside an element
        /// </summary>
        /// <param name="selector">Element containing txt</param>
        public static Isotope<string> text(Select selector) =>
            from el in selector.ToIsotopeHead()
            from rs in tryf(() => el.Text, $@"Error getting text from element: {prettyPrint(el)}")
            select rs;

        /// <summary>
        /// Gets the value attribute of an element
        /// </summary>
        /// <param name="selector">Element containing value</param>
        public static Isotope<string> value(Select selector) =>
            from el in selector.ToIsotopeHead()
            from rs in tryf(() => el.GetAttribute("Value"), $@"Error getting value from element: {prettyPrint(el)}")
            select rs;

        /// <summary>
        /// Web driver accessor
        /// </summary>
        public static Isotope<IWebDriver> webDriver =>
            from s in get
            from r in s.Driver.ToIsotope("web-driver hasn't been selected yet")
            select r;

        /// <summary>
        /// Web driver setter
        /// </summary>
        static Isotope<Unit> setWebDriver(IWebDriver d) =>
            from s in get
            from _ in put(s.With(Driver: Some(d)))
            select unit;

        /// <summary>
        /// Web driver clear
        /// </summary>
        static Isotope<Unit> clearWebDriver =>
            from s in get
            from _ in put(s.With(Driver: None))
            select unit;

        /// <summary>
        /// Default wait accessor
        /// </summary>
        public static Isotope<TimeSpan> defaultWait =>
            from s in get
            select s.Settings.Wait;

        /// <summary>
        /// Default wait accessor
        /// </summary>
        public static Isotope<TimeSpan> defaultInterval =>
            from s in get
            select s.Settings.Interval;

        /// <summary>
        /// Identity - lifts a value of `A` into the Isotope monad
        /// 
        /// * Always succeeds *
        /// 
        /// </summary>
        public static Isotope<A> pure<A>(A value) =>
            Isotope<A>.Pure(value);
        
        /// <summary>
        /// Useful for starting a linq expression if you need lets first
        /// i.e.
        ///         from _ in unitM
        ///         let foo = "123"
        ///         let bar = "456"
        ///         from x in ....
        /// </summary>
        public static Isotope<Unit> unitM =>
            pure(unit);

        static string showContext(Stck<string> ctx) =>
            String.Join(" → ", ctx.Rev());

        /// <summary>
        /// Failure
        /// </summary>
        /// <param name="err">Error</param>
        public static Error fail(Error err) =>
            err;

        /// <summary>
        /// Failure
        /// </summary>
        /// <param name="err">Error</param>
        public static Error fail(string err) =>
            Error.New(err);

        /// <summary>
        /// Failure
        /// </summary>
        /// <param name="msg">Error message</param>
        /// <param name="ex">Exception</param>
        public static Error fail(string msg, Exception ex) =>
            Error.New(msg, ex);

        /// <summary>
        /// Failure
        /// </summary>
        /// <param name="err">Error</param>
        public static Error fail(Exception err) =>
            Error.New(err);


        /// <summary>
        /// Failure - creates an Isotope monad that always fails
        /// </summary>
        /// <param name="err">Error</param>
        public static Isotope<A> fail<A>(Error err) =>
            from s in get
            from _ in error(err.ToString())
            from r in Isotope<A>.Fail(Error.New($"{err.Message} ({showContext(s.Context)})", err.Exception.IsSome ? (Exception)err : null))
            select r;
        
        /// <summary>
        /// Failure - creates an Isotope monad that always fails
        /// </summary>
        /// <param name="message">Error message</param>
        public static Isotope<A> fail<A>(string message) =>
            from s in get
            from _ in error(message)
            from r in Isotope<A>.Fail(Error.New($"{message} ({showContext(s.Context)})"))
            select r;

        /// <summary>
        /// Failure - creates an Isotope monad that always fails
        /// </summary>
        /// <param name="ex">Exception</param>
        public static Isotope<A> fail<A>(Exception ex) =>
            from s in get
            from _ in error(ex.Message)
            from r in Isotope<A>.Fail(Error.New($"{ex.Message} ({showContext(s.Context)})", ex))
            select r;

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<A> iso<A>(Func<A> f) =>
            new Isotope<A>(s => new IsotopeState<A>(f(), s)); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<A> iso<A>(Func<Fin<A>> f) =>
            new Isotope<A>(s => f().Match(Succ: a => new IsotopeState<A>(a, s),
                                          Fail: e => new IsotopeState<A>(default, s.AddError(e)))); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<A> iso<A>(Func<IsotopeState, A> f) =>
            new Isotope<A>(s => new IsotopeState<A>(f(s), s)); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<A> iso<A>(Func<IsotopeState, Fin<A>> f) =>
            new Isotope<A>(s => f(s).Match(Succ: a => new IsotopeState<A>(a, s),
                                           Fail: e => new IsotopeState<A>(default, s.AddError(e)))); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<Env, A> iso<Env, A>(Func<Env, IsotopeState, A> f) =>
            new Isotope<Env, A>((e, s) => new IsotopeState<A>(f(e, s), s)); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static Isotope<Env, A> iso<Env, A>(Func<Env, IsotopeState, Fin<A>> f) =>
            new Isotope<Env, A>((e, s) => f(e, s).Match(Succ: a => new IsotopeState<A>(a, s),
                                                       Fail: e => new IsotopeState<A>(default, s.AddError(e)))); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<ValueTask<A>> f) =>
            new IsotopeAsync<A>(async s => new IsotopeState<A>(await f().ConfigureAwait(false), s)); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<ValueTask<Fin<A>>> f) =>
            new IsotopeAsync<A>(async s => (await f().ConfigureAwait(false)).Match(Succ: a => new IsotopeState<A>(a, s),
                                                                                   Fail: e => new IsotopeState<A>(default, s.AddError(e)))); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<IsotopeState, ValueTask<A>> f) =>
            new IsotopeAsync<A>(async s => new IsotopeState<A>(await f(s).ConfigureAwait(false), s)); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<A> isoAsync<A>(Func<IsotopeState, ValueTask<Fin<A>>> f) =>
            new IsotopeAsync<A>(async s => (await f(s).ConfigureAwait(false)).Match(Succ: a => new IsotopeState<A>(a, s),
                                                                                    Fail: e => new IsotopeState<A>(default, s.AddError(e)))); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<Env, A> isoAsync<Env, A>(Func<Env, IsotopeState, ValueTask<A>> f) =>
            new IsotopeAsync<Env, A>(async (e, s) => new IsotopeState<A>(await f(e, s).ConfigureAwait(false), s)); 

        /// <summary>
        /// Lift the function into the isotope monadic space
        /// </summary>
        public static IsotopeAsync<Env, A> isoAsync<Env, A>(Func<Env, IsotopeState, ValueTask<Fin<A>>> f) =>
            new IsotopeAsync<Env, A>(async (e, s) => (await f(e, s).ConfigureAwait(false)).Match(Succ: a => new IsotopeState<A>(a, s),
                                                                                                 Fail: e => new IsotopeState<A>(default, s.AddError(e))));

        /// <summary>
        /// Gets the environment from the Isotope monad
        /// </summary>
        /// <typeparam name="Env">Environment</typeparam>
        public static Isotope<Env, Env> ask<Env>() =>
            iso((Env env, IsotopeState _) => env);

        /// <summary>
        /// Gets a function of the current environment
        /// </summary>
        public static Isotope<Env, R> asks<Env, R>(Func<Env, R> f) =>
            ask<Env>().Map(f);

        /// <summary>
        /// Gets the state from the Isotope monad
        /// </summary>
        public static Isotope<IsotopeState> get =
            iso(identity);

        /// <summary>
        /// Puts the state back into the Isotope monad
        /// </summary>
        public static Isotope<Unit> put(IsotopeState state) =>
            new Isotope<Unit>(_ => new IsotopeState<Unit>(default, state));

        /// <summary>
        /// Modify the state from the Isotope monad
        /// </summary>
        public static Isotope<Unit> modify(Func<IsotopeState, IsotopeState> f) =>
            get.Bind(s => put(f(s)));

        /// <summary>
        /// Try and action
        /// </summary>
        /// <param name="action">Action to try</param>
        /// <param name="label">Error string if exception is thrown</param>
        /// <returns></returns>
        public static Isotope<Unit> trya(Action action, string label) =>
            iso(fun(action))
               .MapFail(e => Error.New(label, Aggregate(e)));

        /// <summary>
        /// Try and action
        /// </summary>
        /// <param name="action">Action to try</param>
        /// <param name="makeError">Convert errors to string</param>
        /// <returns></returns>
        public static Isotope<Unit> trya(Action action, Func<Error, string> makeError) =>
            iso(fun(action))
               .MapFail(e => Error.New(makeError(e.Last), Aggregate(e)));        

        /// <summary>
        /// Try a function
        /// </summary>
        /// <typeparam name="A">Return type of the function</typeparam>
        /// <param name="func">Function to try</param>
        /// <param name="label">Error string if exception is thrown</param>
        /// <returns></returns>
        public static Isotope<A> tryf<A>(Func<A> func, string label) =>
            iso(func)
               .MapFail(e => Error.New(label, Aggregate(e)));

        /// <summary>
        /// Try a function
        /// </summary>
        /// <typeparam name="A">Return type of the function</typeparam>
        /// <param name="func">Function to try</param>
        /// <param name="makeError">Convert errors to string</param>
        /// <returns></returns>
        public static Isotope<A> tryf<A>(Func<A> func, Func<Error, string> makeError) =>
            iso(func)
               .MapFail(e => Error.New(makeError(e.Last), Aggregate(e)));

        /// <summary>
        /// Run a void returning action
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <returns>Unit</returns>
        public static Isotope<Unit> voida(Action action) => new Isotope<Unit>(state =>
        {
            action();
            return new IsotopeState<Unit>(unit, state);
        });

        /// <summary>
        /// Log some output
        /// </summary>
        [Obsolete("Use `info | warn | error` instead")]
        public static Isotope<Unit> log(
            string message,
            [CallerMemberName] string callerMemberName = "", 
            [CallerFilePath] string callerFilePath = "", 
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = s.Log.Add(Log.Info(message, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber))
            from x in put(s.With(Log: p.Log))
            from y in writeToLogStream(p.Added)
            select unit;

        /// <summary>
        /// Log some output as info
        /// </summary>
        public static Isotope<Unit> info(
            string message,
            [CallerMemberName] string callerMemberName = "", 
            [CallerFilePath] string callerFilePath = "", 
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = s.Log.Add(Log.Info(message, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber))
            from x in put(s.With(Log: p.Log))
            from y in writeToLogStream(p.Added)
            select unit;

        /// <summary>
        /// Log some output as a warning
        /// </summary>
        public static Isotope<Unit> warn(
            string message,
            [CallerMemberName] string callerMemberName = "", 
            [CallerFilePath] string callerFilePath = "", 
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = s.Log.Add(Log.Warning(message, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber))
            from x in put(s.With(Log: p.Log))
            from y in writeToLogStream(p.Added)
            select unit;

        /// <summary>
        /// Log some output as an error
        /// </summary>
        /// <remarks>Note: This only logs the error, it doesn't stop the computation.  Use `fail` for computation
        /// termination.  `fail` also logs to the output using this function.</remarks>
        public static Isotope<Unit> error(
            string message,
            [CallerMemberName] string callerMemberName = "", 
            [CallerFilePath] string callerFilePath = "", 
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = s.Log.Add(Log.Error(message, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber))
            from x in put(s.With(Log: p.Log))
            from y in writeToLogStream(p.Added)
            select unit;
        
        /// <summary>
        /// Create a logging context
        /// </summary>
        public static Isotope<A> context<A>(
            string context,
            Isotope<A> iso,
            [CallerMemberName] string callerMemberName = "", 
            [CallerFilePath] string callerFilePath = "", 
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = Log.Context(context, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber).Rebase(s.Log.Indent)
            from y in writeToLogStream(p)
            from x in put(s.With(Log: p, Context: s.Context.Push(context)))
            from r in iso
            from _ in modify(s2 => s2.With(Context: s.Context, 
                                           Log: s.Log.Add(s2.Log).Log))   
            select r;
        
        /// <summary>
        /// Create a logging context
        /// </summary>
        public static Isotope<Env, A> context<Env, A>(
            string context,
            Isotope<Env, A> iso,
            [CallerMemberName] string callerMemberName = "", 
            [CallerFilePath] string callerFilePath = "", 
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = Log.Context(context, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber).Rebase(s.Log.Indent)
            from y in writeToLogStream(p)
            from x in put(s.With(Log: p, Context: s.Context.Push(context)))
            from r in iso
            from _ in modify(s2 => s2.With(Context: s.Context, 
                                           Log: s.Log.Add(s2.Log).Log))   
            select r;
        
        /// <summary>
        /// Create a logging context
        /// </summary>
        public static IsotopeAsync<A> context<A>(
            string context,
            IsotopeAsync<A> iso,
            [CallerMemberName] string callerMemberName = "", 
            [CallerFilePath] string callerFilePath = "", 
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = Log.Context(context, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber).Rebase(s.Log.Indent)
            from y in writeToLogStream(p)
            from x in put(s.With(Log: p, Context: s.Context.Push(context)))
            from r in iso
            from _ in modify(s2 => s2.With(Context: s.Context, 
                                           Log: s.Log.Add(s2.Log).Log))   
            select r;
        
        /// <summary>
        /// Create a logging context
        /// </summary>
        public static IsotopeAsync<Env, A> context<Env, A>(
            string context, 
            IsotopeAsync<Env, A> iso,
            [CallerMemberName] string callerMemberName = "", 
            [CallerFilePath] string callerFilePath = "", 
            [CallerLineNumber] int callerLineNumber = 0) =>
            from s in get
            let p = Log.Context(context, DateTime.UtcNow, callerMemberName, callerFilePath, callerLineNumber).Rebase(s.Log.Indent)
            from y in writeToLogStream(p)
            from x in put(s.With(Log: p, Context: s.Context.Push(context)))
            from r in iso
            from _ in modify(s2 => s2.With(Context: s.Context, 
                                           Log: s.Log.Add(s2.Log).Log))   
            select r;

        /// <summary>
        /// Mute log
        /// </summary>
        public static Isotope<A> mute<A>(Isotope<A> iso) =>
            from s in get
            from x in put(s.With(Mute: true))
            from r in iso
            from _ in modify(s2 => s2.With(Mute: s.Mute))
            select r;

        /// <summary>
        /// Mute log
        /// </summary>
        public static Isotope<Env, A> mute<Env, A>(Isotope<Env, A> iso) =>
            from s in get
            from x in put(s.With(Mute: true))
            from r in iso
            from _ in modify(s2 => s2.With(Mute: s.Mute))
            select r;

        /// <summary>
        /// Mute log
        /// </summary>
        public static IsotopeAsync<A> mute<A>(IsotopeAsync<A> iso) =>
            from s in get
            from x in put(s.With(Mute: true))
            from r in iso
            from _ in modify(s2 => s2.With(Mute: s.Mute))
            select r;

        /// <summary>
        /// Mute log
        /// </summary>
        public static IsotopeAsync<Env, A> mute<Env, A>(IsotopeAsync<Env, A> iso) =>
            from s in get
            from x in put(s.With(Mute: true))
            from r in iso
            from _ in modify(s2 => s2.With(Mute: s.Mute))
            select r;

        /// <summary>
        /// Measure the time interval of an isotope
        /// </summary>
        public static Isotope<(A Result, TimeSpan Time)> stopwatch<A>(Isotope<A> iso) =>
            from _1 in info("Start stopwatch")
            from t in pure(Stopwatch.StartNew())
            from r in iso
            from _2 in trya(() => t.Stop(), "Unable to stop stopwatch")
            from e in tryf(() => t.Elapsed, "Unable to get elapsed time")
            from _3 in info($"Stop stopwatch, elapsed time: {e.ToString(@"m\:ss\.fff")}")
            select (r, e);

        /// <summary>
        /// Measure the time interval of an isotope
        /// </summary>
        public static Isotope<Env, (A Result, TimeSpan Time)> stopwatch<Env, A>(Isotope<Env, A> iso) =>
            from _1 in info("Start stopwatch")
            from t in pure(Stopwatch.StartNew())
            from r in iso
            from _2 in trya(() => t.Stop(), "Unable to stop stopwatch")
            from e in tryf(() => t.Elapsed, "Unable to get elapsed time")
            from _3 in info($"Stop stopwatch, elapsed time: {e.ToString(@"m\:ss\.fff")}")
            select (r, e);

        /// <summary>
        /// Measure the time interval of an isotope
        /// </summary>
        public static IsotopeAsync<(A Result, TimeSpan Time)> stopwatch<A>(IsotopeAsync<A> iso) =>
            from _1 in info("Start stopwatch")
            from t in pure(Stopwatch.StartNew())
            from r in iso
            from _2 in trya(() => t.Stop(), "Unable to stop stopwatch")
            from e in tryf(() => t.Elapsed, "Unable to get elapsed time")
            from _3 in info($"Stop stopwatch, elapsed time: {e.ToString(@"m\:ss\.fff")}")
            select (r, e);

        /// <summary>
        /// Measure the time interval of an isotope
        /// </summary>
        public static IsotopeAsync<Env, (A Result, TimeSpan Time)> stopwatch<Env, A>(IsotopeAsync<Env, A> iso) =>
            from _1 in info("Start stopwatch")
            from t in pure(Stopwatch.StartNew())
            from r in iso
            from _2 in trya(() => t.Stop(), "Unable to stop stopwatch")
            from e in tryf(() => t.Elapsed, "Unable to get elapsed time")
            from _3 in info($"Stop stopwatch, elapsed time: {e.ToString(@"m\:ss\.fff")}")
            select (r, e);
        
        static Isotope<Unit> writeToLogStream(Log entry) =>
            new Isotope<Unit>(s => {
                  if (!String.IsNullOrWhiteSpace(entry.Message) && !s.Mute)
                  {
                      s.Settings.LogStream.OnNext(new LogOutput(entry.Message, entry.Type, s.Context.Count, entry.Time, entry.CallerMemberName, entry.CallerFilePath, entry.CallerLineNumber));
                  }
                  return new IsotopeState<Unit>(default, s);
              });

        /// <summary>
        /// Wait for an element to be rendered and clickable, fail if exceeds default timeout
        /// </summary>
        public static Isotope<Unit> waitUntilClickable(Select selector) =>
            from w  in defaultWait
            from el in waitUntilClickable(selector, w)
            select unit;

        /// <summary>
        /// Wait for an element to be rendered and clickable, fail if exceeds timeout specified
        /// </summary>
        public static Isotope<Unit> waitUntilClickable(Select selector, TimeSpan timeout) =>
            from _1 in info($"Waiting until clickable: {selector}")
            from _ in Isotope.waitUntil(
                from el in selector.ToIsotopeHead()
                from _1a in info($"Checking clickability " + prettyPrint(el))
                from d in IsotopeInternal.displayed(el)
                from e in IsotopeInternal.enabled(el)
                from o in IsotopeInternal.obscured(el)
                from _2a in info($"Displayed: {d}, Enabled: {e}, Obscured: {o}")
                select d && e && (!o),
                identity, wait: timeout)
            select unit;
        
        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.   
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way, then the computation ends immediately.  Therefore the items in the
        /// sequence after that point are not evaluated.
        ///
        /// The resulting state will contain the log of all items evaluated or the first error encountered.
        ///
        /// Each item runs in an indexed `context`. i.e. 
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static Isotope<Seq<A>> Sequence<A>(this Seq<Isotope<A>> mas) =>
            new Isotope<Seq<A>>(
                state => {
                    var vals  = new A[mas.Count];
                    var log   = state.Log;
                    int index = 0;

                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = context($"[{index}]", ma).Invoke(nstate);
                        if (rstate.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(
                                vals.ToSeq(),
                                state.With(Error: rstate.State.Error, Log: rstate.State.Log));
                        }

                        nstate      = rstate.State;
                        vals[index] = rstate.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.   
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way, then the computation ends immediately.  Therefore the items in the
        /// sequence after that point are not evaluated.
        ///
        /// The resulting state will contain the log of all items evaluated or the first error encountered.
        ///
        /// Each item runs in an indexed `context`. i.e. 
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static Isotope<Env, Seq<A>> Sequence<Env, A>(this Seq<Isotope<Env, A>> mas) =>
            new Isotope<Env, Seq<A>>(
                (env, state) => {
                    var vals  = new A[mas.Count];
                    var log   = state.Log;
                    int index = 0;

                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = context($"[{index}]", ma).Invoke(env, nstate);
                        if (rstate.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(
                                vals.ToSeq(),
                                state.With(Error: rstate.State.Error, Log: rstate.State.Log));
                        }

                        nstate      = rstate.State;
                        vals[index] = rstate.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.   
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way, then the computation ends immediately.  Therefore the items in the
        /// sequence after that point are not evaluated.
        ///
        /// The resulting state will contain the log of all items evaluated or the first error encountered.
        ///
        /// Each item runs in an indexed `context`. i.e. 
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static IsotopeAsync<Seq<A>> Sequence<A>(this Seq<IsotopeAsync<A>> mas) =>
            new IsotopeAsync<Seq<A>>(
                async state => {
                    var vals  = new A[mas.Count];
                    var log   = state.Log;
                    int index = 0;

                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = await context($"[{index}]", ma).Invoke(nstate).ConfigureAwait(false);
                        if (rstate.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(
                                vals.ToSeq(),
                                state.With(Error: rstate.State.Error, Log: rstate.State.Log));
                        }

                        nstate      = rstate.State;
                        vals[index] = rstate.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.   
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way, then the computation ends immediately.  Therefore the items in the
        /// sequence after that point are not evaluated.
        ///
        /// The resulting state will contain the log of all items evaluated or the first error encountered.
        ///
        /// Each item runs in an indexed `context`. i.e. 
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static IsotopeAsync<Env, Seq<A>> Sequence<Env, A>(this Seq<IsotopeAsync<Env, A>> mas) =>
            new IsotopeAsync<Env, Seq<A>>(
                async (env, state) => {
                    var vals  = new A[mas.Count];
                    var log   = state.Log;
                    int index = 0;

                    var nstate = state;

                    foreach (var ma in mas)
                    {
                        var rstate = await context($"[{index}]", ma).Invoke(env, nstate).ConfigureAwait(false);
                        if (rstate.IsFaulted)
                        {
                            return new IsotopeState<Seq<A>>(
                                vals.ToSeq(),
                                state.With(Error: rstate.State.Error, Log: rstate.State.Log));
                        }

                        nstate      = rstate.State;
                        vals[index] = rstate.Value;
                        index++;
                    }

                    return new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.   
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way then it is collected.  The process then continues to evaluate the
        /// subsequent items.  The resulting resulting state will contain the log of all items evaluated.  If there was
        /// an error, the resulting state will have an aggregated list of errors.
        ///
        /// Each item runs in an indexed `context`. i.e. 
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static Isotope<Seq<A>> Collect<A>(this Seq<Isotope<A>> mas) =>
            new Isotope<Seq<A>>(
                state => {
                    var vals   = new A[mas.Count];
                    var errs   = new Seq<Error>[mas.Count];
                    int index  = 0;
                    var nstate = state;
                    
                    foreach (var ma in mas)
                    {
                        var rstate = context($"[{index}]", ma).Invoke(nstate);

                        vals[index] = rstate.Value;
                        errs[index] = rstate.State.Error;
                        nstate = state.With(Error: Empty, Log: rstate.State.Log);                        

                        index++;
                    }

                    // Aggregate all the errors
                    var nerr = errs.Fold(Seq<Error>(), (s, e) => s + e);
                    
                    return nerr.IsEmpty
                               ? new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log))
                               : new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Error: nerr, Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.   
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way then it is collected.  The process then continues to evaluate the
        /// subsequent items.  The resulting resulting state will contain the log of all items evaluated.  If there was
        /// an error, the resulting state will have an aggregated list of errors.
        ///
        /// Each item runs in an indexed `context`. i.e. 
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static Isotope<Env,  Seq<A>> Collect<Env, A>(this Seq<Isotope<Env,  A>> mas) =>
            new Isotope<Env,  Seq<A>>(
                (env, state) => {
                    var vals   = new A[mas.Count];
                    var errs   = new Seq<Error>[mas.Count];
                    int index  = 0;
                    var nstate = state;
                    
                    foreach (var ma in mas)
                    {
                        var rstate = context($"[{index}]", ma).Invoke(env, nstate);

                        vals[index] = rstate.Value;
                        errs[index] = rstate.State.Error;
                        nstate      = state.With(Error: Empty, Log: rstate.State.Log);                        

                        index++;
                    }

                    // Aggregate all the errors
                    var nerr = errs.Fold(Seq<Error>(), (s, e) => s + e);
                    
                    return nerr.IsEmpty
                               ? new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log))
                               : new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Error: nerr, Log: nstate.Log));
                });        

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.   
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way then it is collected.  The process then continues to evaluate the
        /// subsequent items.  The resulting resulting state will contain the log of all items evaluated.  If there was
        /// an error, the resulting state will have an aggregated list of errors.
        ///
        /// Each item runs in an indexed `context`. i.e. 
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static IsotopeAsync<Seq<A>> Collect<A>(this Seq<IsotopeAsync<A>> mas) =>
            new IsotopeAsync<Seq<A>>(
                async state => {
                    var vals   = new A[mas.Count];
                    var errs   = new Seq<Error>[mas.Count];
                    int index  = 0;
                    var nstate = state;
                    
                    foreach (var ma in mas)
                    {
                        var rstate = await context($"[{index}]", ma).Invoke(nstate).ConfigureAwait(false);

                        vals[index] = rstate.Value;
                        errs[index] = rstate.State.Error;
                        nstate      = state.With(Error: Empty, Log: rstate.State.Log);                        

                        index++;
                    }

                    // Aggregate all the errors
                    var nerr = errs.Fold(Seq<Error>(), (s, e) => s + e);
                    
                    return nerr.IsEmpty
                               ? new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log))
                               : new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Error: nerr, Log: nstate.Log));
                });

        /// <summary>
        /// Flips the sequence of Isotopes to an Isotope of Sequences.  It does this by running each Isotope within
        /// the Seq and collects the results into a single Seq and then re-wraps within an Isotope.   
        /// </summary>
        /// <remarks>
        /// If an error is encountered along the way then it is collected.  The process then continues to evaluate the
        /// subsequent items.  The resulting resulting state will contain the log of all items evaluated.  If there was
        /// an error, the resulting state will have an aggregated list of errors.
        ///
        /// Each item runs in an indexed `context`. i.e. 
        ///
        ///     [0], [1], [2] ...
        ///
        /// Which makes it easier to know which index a log entry is for.
        /// </remarks>
        public static IsotopeAsync<Env,  Seq<A>> Collect<Env, A>(this Seq<IsotopeAsync<Env,  A>> mas) =>
            new IsotopeAsync<Env,  Seq<A>>(
                async (env, state) => {
                    var vals   = new A[mas.Count];
                    var errs   = new Seq<Error>[mas.Count];
                    int index  = 0;
                    var nstate = state;
                    
                    foreach (var ma in mas)
                    {
                        var rstate = await context($"[{index}]", ma).Invoke(env, nstate).ConfigureAwait(false);

                        vals[index] = rstate.Value;
                        errs[index] = rstate.State.Error;
                        nstate      = state.With(Error: Empty, Log: rstate.State.Log);                        

                        index++;
                    }

                    // Aggregate all the errors
                    var nerr = errs.Fold(Seq<Error>(), (s, e) => s + e);
                    
                    return nerr.IsEmpty
                               ? new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Log: nstate.Log))
                               : new IsotopeState<Seq<A>>(vals.ToSeq(), state.With(Error: nerr, Log: nstate.Log));
                });              
        
        /// <summary>
        /// Convert an option to a pure isotope
        /// </summary>
        /// <param name="maybe">Optional value</param>
        /// <param name="label">Failure value to use if None</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Pure isotope</returns>
        public static Isotope<A> ToIsotope<A>(this Option<A> maybe, string label) =>
            maybe.Match(Some: pure, None: fail(label));
        
        /// <summary>
        /// Convert an option to a pure isotope
        /// </summary>
        /// <param name="maybe">Optional value</param>
        /// <param name="alternative">Alternative to use if None</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Pure isotope</returns>
        public static Isotope<A> ToIsotope<A>(this Option<A> maybe, Isotope<A> alternative) =>
            maybe.ToIsotope("") | alternative;

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried) =>
            tried.Match(
                Succ: pure,
                Fail: x => fail(Error.New(x)));

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <param name="label">Failure value to use if Fail</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried, string label) =>
            tried.ToIsotope().MapFail(e => Error.New(label, Aggregate(e)));

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <param name="makeError">Failure value to use if Fail</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried, Func<Error, string> makeError) =>
            tried.ToIsotope().MapFail(e => Error.New(makeError(e.Last), Aggregate(e)));

        /// <summary>
        /// Convert a try to an isotope computation
        /// </summary>
        /// <param name="tried">Try value</param>
        /// <param name="alternative">Alternative to use if Fail</param>
        /// <typeparam name="A">Bound value type</typeparam>
        /// <returns>Try computation wrapped in an isotope computation</returns>
        public static Isotope<A> ToIsotope<A>(this Try<A> tried, Isotope<A> alternative) =>
            tried.ToIsotope() | alternative;

        /// <summary>
        /// Convert an Either to a pure isotope
        /// </summary>
        /// <param name="either">Either to convert</param>
        /// <typeparam name="R">Right param</typeparam>
        /// <returns>Pure isotope</returns>
        public static Isotope<R> ToIsotope<R>(this Either<Error, R> either) =>
            either.Match(Right: pure, Left: fail<R>);

        /// <summary>
        /// Convert an Either to a pure isotope
        /// </summary>
        /// <param name="either">Either to convert</param>
        /// <param name="label">Label for the failure</param>
        /// <returns>Pure isotope</returns>
        public static Isotope<B> ToIsotope<A, B>(this Either<A, B> either, string label) =>
            either.Match(
                Left: _ => fail(Error.New(label)),
                Right: pure);

        /// <summary>
        /// Convert an Either to a pure isotope
        /// </summary>
        /// <param name="either">Either to convert</param>
        /// <param name="makeError">Label for the failure</param>
        /// <returns>Pure isotope</returns>
        public static Isotope<B> ToIsotope<A, B>(this Either<A, B> either, Func<A, string> makeError) =>
            either.Match(
                Left: e => fail(Error.New(makeError(e))),
                Right: pure);

        /// <summary>
        /// Finds an element by a selector and checks if it is currently displayed
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>True if the element is currently displayed</returns>
        public static Isotope<bool> displayed(Select selector) =>
            selector.ToIsotopeHead()
                    .Bind(IsotopeInternal.displayed);

        /// <summary>
        /// Finds an element by a selector and checks if it is currently enabled
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>True if the element is currently enabled</returns>
        public static Isotope<bool> enabled(Select selector) =>
             selector.ToIsotopeHead()
                     .Bind(IsotopeInternal.enabled);

        /// <summary>
        /// Checks if an element exists that matches the selector
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>True if a matching element exists</returns>
        public static Isotope<bool> exists(Select selector) =>
            from es in selector.ToIsotope()
            select !es.IsEmpty;

        /// <summary>
        /// Checks whether the centre point of an element is the foremost element at that position on the page.
        /// (Uses the JavaScript document.elementFromPoint function)
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <returns>true if the element is foremost</returns>
        public static Isotope<bool> obscured(Select selector) =>
            selector.ToIsotopeHead()
                    .Bind(IsotopeInternal.obscured);

        /// <summary>
        /// Compares the text of an element with a string
        /// </summary>
        /// <param name="element">Element to compare</param>
        /// <param name="comparison">String to match</param>
        /// <returns>Fails if no match, with a contextual error</returns>
        /// <remarks>
        ///
        ///     hasText doesn't return a bool Isotope because it's expected you do this:
        ///
        ///         var ma = hasText(selector, txt) | ...
        ///
        ///     Where `...` can be what to do if the text doesn't match.  That could be
        ///     reporting a different error to the default, or providing an alternative
        ///     success operation.
        /// 
        /// </remarks>
        public static Isotope<Unit> hasText(Select element, string comparison) =>
            from t in text(element)
            from r in t == comparison
                          ? unitM
                          : fail("Element text doesn't match.  \"{t}\" <> \"{comparison}\"")
            select r;
                
        /// <summary>
        /// Wait until the `condition` is `true`, or it times-out
        /// </summary>        
        public static Isotope<A> waitUntil<A>(
            Isotope<A> iso,
            Func<A, bool> condition,
            Option<TimeSpan> interval = default,
            Option<TimeSpan> wait = default) =>
            from w in wait.Match(Some: pure, None: defaultWait)
            from i in interval.Match(Some: pure, None: defaultInterval)
            from r in IsotopeInternal.waitUntil(iso, condition, i, w, DateTime.UtcNow)
            select r;

        /// <summary>
        /// Wait until `iso` succeeds, or it times-out
        /// </summary>        
        public static Isotope<A> waitUntil<A>(
            Isotope<A> iso,
            Option<TimeSpan> interval = default,
            Option<TimeSpan> wait = default) =>
            waitUntil<A>(iso, _ => true, interval, wait);
        
        /// <summary>
        /// Do while the `condition` is `true`, or it times-out
        /// </summary>        
        public static Isotope<A> doWhile<A>(
            Isotope<A> iso,
            Func<A, bool> condition,
            int maxRepeats = 100) =>
            maxRepeats <= 0
                ? pure(default(A))
                : from x in iso
                  from y in condition(x)
                                ? doWhile(iso, condition, maxRepeats - 1)
                                : pure(x)
                  select y;

        /// <summary>
        /// Run `iso` while the `condition` is `true`.
        ///
        ///     * If it turns `false` or the result of `iso` is returned
        ///     * If the max-attempts are reached, then `fail`.
        /// 
        /// </summary>        
        public static Isotope<A> doWhileOrFail<A>(
            Isotope<A> iso,
            Func<A, bool> condition,
            int maxAttempts = 100) =>
            maxAttempts <= 0
                ? fail("do while reached the max-attempts")
                : from x in iso
                  from y in condition(x)
                                ? doWhileOrFail(iso, condition, maxAttempts - 1)
                                : pure(x)
                  select y;

        /// <summary>
        /// Run `iso`  while the `condition` is `true`.  
        ///
        ///     * If it turns `false` or the result of `iso` is returned
        ///     * If the max-attempts are reached, then `fail`.
        ///     * `interval` specifies the delay between attempts
        /// 
        /// </summary>        
        public static Isotope<A> doWhileOrFail<A>(
            Isotope<A> iso,
            Func<A, bool> continueCondition,
            TimeSpan interval,
            int maxAttempts = 1000) =>
            maxAttempts <= 0
                ? fail("do while reached the max-attempts")
                : from x in iso
                  from y in continueCondition(x)
                                ? from _ in pause(interval)
                                  from z in doWhileOrFail(iso, continueCondition, interval, maxAttempts - 1)
                                  select z
                                : pure(x)
                  select y;

        /// <summary>
        /// Takes a screenshot if the current WebDriver supports that functionality
        /// </summary>
        public static Isotope<Option<Screenshot>> getScreenshot =>
            from dvr in webDriver
            let ts = dvr as ITakesScreenshot
            select ts == null ? None : Some(ts.GetScreenshot());

        /// <summary>
        /// Runs the javascript and returns a value
        /// </summary>
        public static Isotope<T> eval<T>(string javascript) =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from res in pure((T)jsExec.ExecuteScript(javascript))
            select res;
    }
}
