using LanguageExt;
using OpenQA.Selenium;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using LanguageExt.Common;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
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
        /// Run the isotope provided with the web-driver context.
        /// When <paramref name="keepAlive"/> is true the driver is not disposed after the isotope completes,
        /// allowing the same driver to be reused across multiple runs (e.g. REPL / persistent sessions).
        /// </summary>
        public static Isotope<A> withWebDriver<A>(IWebDriver driver, Isotope<A> ma, bool keepAlive) =>
            keepAlive
                ? from st in get
                  from _1 in setWebDriver(driver)
                  from rs in ma
                  from _2 in st.Driver.Match(Some: setWebDriver, None: clearWebDriver)
                  select rs
                : withWebDriver(driver, ma);

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
        /// Run the isotope provided with the web-driver context.
        /// When <paramref name="keepAlive"/> is true the driver is not disposed after the isotope completes,
        /// allowing the same driver to be reused across multiple runs (e.g. REPL / persistent sessions).
        /// </summary>
        public static Isotope<Env, A> withWebDriver<Env, A>(IWebDriver driver, Isotope<Env, A> ma, bool keepAlive) =>
            keepAlive
                ? from st in get
                  from _1 in setWebDriver(driver)
                  from rs in ma
                  from _2 in st.Driver.Match(Some: setWebDriver, None: clearWebDriver)
                  select rs
                : withWebDriver(driver, ma);

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
            withChromeDriver(ma, new ChromeOptions());

        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static Isotope<A> withChromeDriver<A>(Isotope<A> ma, ChromeOptions options) =>
            context("Chrome", withWebDriver(new ChromeDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static Isotope<A> withChromeDriver<A>(Isotope<A> ma, ChromeDriverService service) =>
            context("Chrome", withWebDriver(new ChromeDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static Isotope<A> withChromeDriver<A>(Isotope<A> ma, ChromeDriverService service, ChromeOptions options) =>
            context("Chrome", withWebDriver(new ChromeDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<A> withEdgeDriver<A>(Isotope<A> ma) =>
            withEdgeDriver(ma, new EdgeOptions());

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<A> withEdgeDriver<A>(Isotope<A> ma, EdgeOptions options) =>
            context("Edge", withWebDriver(new EdgeDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<A> withEdgeDriver<A>(Isotope<A> ma, EdgeDriverService service) =>
            context("Edge", withWebDriver(new EdgeDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<A> withEdgeDriver<A>(Isotope<A> ma, EdgeDriverService service, EdgeOptions options) =>
            context("Edge", withWebDriver(new EdgeDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<A> withFirefoxDriver<A>(Isotope<A> ma) =>
            withFirefoxDriver(ma, new FirefoxOptions());

        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<A> withFirefoxDriver<A>(Isotope<A> ma, FirefoxOptions options) =>
            context("Firefox", withWebDriver(new FirefoxDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<A> withFirefoxDriver<A>(Isotope<A> ma, FirefoxDriverService service) =>
            context("Firefox", withWebDriver(new FirefoxDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<A> withFirefoxDriver<A>(Isotope<A> ma, FirefoxDriverService service, FirefoxOptions options) =>
            context("Firefox", withWebDriver(new FirefoxDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<A> withSafariDriver<A>(Isotope<A> ma) =>
            withSafariDriver(ma, new SafariOptions());

        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<A> withSafariDriver<A>(Isotope<A> ma, SafariOptions options) =>
            context("Safari", withWebDriver(new SafariDriver(options), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<A> withSafariDriver<A>(Isotope<A> ma, SafariDriverService service) =>
            context("Safari", withWebDriver(new SafariDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<A> withSafariDriver<A>(Isotope<A> ma, SafariDriverService service, SafariOptions options) =>
            context("Safari", withWebDriver(new SafariDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static Isotope<Env, A> withChromeDriver<Env, A>(Isotope<Env, A> ma) =>
            withChromeDriver(ma, new ChromeOptions());
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static Isotope<Env, A> withChromeDriver<Env, A>(Isotope<Env, A> ma, ChromeOptions options) =>
            context("Chrome", withWebDriver(new ChromeDriver(options), ma));
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static Isotope<Env, A> withChromeDriver<Env, A>(Isotope<Env, A> ma, ChromeDriverService service) =>
            context("Chrome", withWebDriver(new ChromeDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static Isotope<Env, A> withChromeDriver<Env, A>(Isotope<Env, A> ma, ChromeDriverService service, ChromeOptions options) =>
            context("Chrome", withWebDriver(new ChromeDriver(service, options), ma));

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<Env, A> withEdgeDriver<Env, A>(Isotope<Env, A> ma) =>
            withEdgeDriver(ma, new EdgeOptions());
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<Env, A> withEdgeDriver<Env, A>(Isotope<Env, A> ma, EdgeOptions options) =>
            context("Edge", withWebDriver(new EdgeDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<Env, A> withEdgeDriver<Env, A>(Isotope<Env, A> ma, EdgeDriverService service) =>
            context("Edge", withWebDriver(new EdgeDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static Isotope<Env, A> withEdgeDriver<Env, A>(Isotope<Env, A> ma, EdgeDriverService service, EdgeOptions options) =>
            context("Edge", withWebDriver(new EdgeDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<Env, A> withFirefoxDriver<Env, A>(Isotope<Env, A> ma) =>
            withFirefoxDriver(ma, new FirefoxOptions());
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<Env, A> withFirefoxDriver<Env, A>(Isotope<Env, A> ma, FirefoxOptions options) =>
            context("Firefox", withWebDriver(new FirefoxDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<Env, A> withFirefoxDriver<Env, A>(Isotope<Env, A> ma, FirefoxDriverService service) =>
            context("Firefox", withWebDriver(new FirefoxDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static Isotope<Env, A> withFirefoxDriver<Env, A>(Isotope<Env, A> ma, FirefoxDriverService service, FirefoxOptions options) =>
            context("Firefox", withWebDriver(new FirefoxDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<Env, A> withSafariDriver<Env, A>(Isotope<Env, A> ma) =>
            withSafariDriver(ma, new SafariOptions());
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<Env, A> withSafariDriver<Env, A>(Isotope<Env, A> ma, SafariOptions options) =>
            context("Safari", withWebDriver(new SafariDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<Env, A> withSafariDriver<Env, A>(Isotope<Env, A> ma, SafariDriverService service) =>
            context("Safari", withWebDriver(new SafariDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static Isotope<Env, A> withSafariDriver<Env, A>(Isotope<Env, A> ma, SafariDriverService service, SafariOptions options) =>
            context("Safari", withWebDriver(new SafariDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<A> withChromeDriver<A>(IsotopeAsync<A> ma) =>
            withChromeDriver(ma, new ChromeOptions());
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<A> withChromeDriver<A>(IsotopeAsync<A> ma, ChromeOptions options) =>
            context("Chrome", withWebDriver(new ChromeDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<A> withChromeDriver<A>(IsotopeAsync<A> ma, ChromeDriverService service) =>
            context("Chrome", withWebDriver(new ChromeDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<A> withChromeDriver<A>(IsotopeAsync<A> ma, ChromeDriverService service, ChromeOptions options) =>
            context("Chrome", withWebDriver(new ChromeDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<A> withEdgeDriver<A>(IsotopeAsync<A> ma) =>
            withEdgeDriver(ma, new EdgeOptions());
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<A> withEdgeDriver<A>(IsotopeAsync<A> ma, EdgeOptions options) =>
            context("Edge", withWebDriver(new EdgeDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<A> withEdgeDriver<A>(IsotopeAsync<A> ma, EdgeDriverService service) =>
            context("Edge", withWebDriver(new EdgeDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<A> withEdgeDriver<A>(IsotopeAsync<A> ma, EdgeDriverService service, EdgeOptions options) =>
            context("Edge", withWebDriver(new EdgeDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<A> withFirefoxDriver<A>(IsotopeAsync<A> ma) =>
            withFirefoxDriver(ma, new FirefoxOptions());
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<A> withFirefoxDriver<A>(IsotopeAsync<A> ma, FirefoxOptions options) =>
            context("Firefox", withWebDriver(new FirefoxDriver(options), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<A> withFirefoxDriver<A>(IsotopeAsync<A> ma, FirefoxDriverService service) =>
            context("Firefox", withWebDriver(new FirefoxDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<A> withFirefoxDriver<A>(IsotopeAsync<A> ma, FirefoxDriverService service, FirefoxOptions options) =>
            context("Firefox", withWebDriver(new FirefoxDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<A> withSafariDriver<A>(IsotopeAsync<A> ma) =>
            withSafariDriver(ma, new SafariOptions());
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<A> withSafariDriver<A>(IsotopeAsync<A> ma, SafariOptions options) =>
            context("Safari", withWebDriver(new SafariDriver(options), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<A> withSafariDriver<A>(IsotopeAsync<A> ma, SafariDriverService service) =>
            context("Safari", withWebDriver(new SafariDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<A> withSafariDriver<A>(IsotopeAsync<A> ma, SafariDriverService service, SafariOptions options) =>
            context("Safari", withWebDriver(new SafariDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withChromeDriver<Env, A>(IsotopeAsync<Env, A> ma) =>
            withChromeDriver(ma, new ChromeOptions());
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withChromeDriver<Env, A>(IsotopeAsync<Env, A> ma, ChromeOptions options) =>
            context("Chrome", withWebDriver(new ChromeDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withChromeDriver<Env, A>(IsotopeAsync<Env, A> ma, ChromeDriverService service) =>
            context("Chrome", withWebDriver(new ChromeDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Chrome web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withChromeDriver<Env, A>(IsotopeAsync<Env, A> ma, ChromeDriverService service, ChromeOptions options) =>
            context("Chrome", withWebDriver(new ChromeDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withEdgeDriver<Env, A>(IsotopeAsync<Env, A> ma) =>
            withEdgeDriver(ma, new EdgeOptions());
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withEdgeDriver<Env, A>(IsotopeAsync<Env, A> ma, EdgeOptions options) =>
            context("Edge", withWebDriver(new EdgeDriver(options), ma));

        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withEdgeDriver<Env, A>(IsotopeAsync<Env, A> ma, EdgeDriverService service) =>
            context("Edge", withWebDriver(new EdgeDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Edge web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withEdgeDriver<Env, A>(IsotopeAsync<Env, A> ma, EdgeDriverService service, EdgeOptions options) =>
            context("Edge", withWebDriver(new EdgeDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withFirefoxDriver<Env, A>(IsotopeAsync<Env, A> ma) =>
            withFirefoxDriver(ma, new FirefoxOptions());
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withFirefoxDriver<Env, A>(IsotopeAsync<Env, A> ma, FirefoxOptions options) =>
            context("Firefox", withWebDriver(new FirefoxDriver(options), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withFirefoxDriver<Env, A>(IsotopeAsync<Env, A> ma, FirefoxDriverService service) =>
            context("Firefox", withWebDriver(new FirefoxDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Firefox web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withFirefoxDriver<Env, A>(IsotopeAsync<Env, A> ma, FirefoxDriverService service, FirefoxOptions options) =>
            context("Firefox", withWebDriver(new FirefoxDriver(service, options), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withSafariDriver<Env, A>(IsotopeAsync<Env, A> ma) =>
            withSafariDriver(ma, new SafariOptions());
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withSafariDriver<Env, A>(IsotopeAsync<Env, A> ma, SafariOptions options) =>
            context("Safari", withWebDriver(new SafariDriver(options), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withSafariDriver<Env, A>(IsotopeAsync<Env, A> ma, SafariDriverService service) =>
            context("Safari", withWebDriver(new SafariDriver(service), ma));
        
        /// <summary>
        /// Run the isotope provided with Safari web-driver
        /// </summary>
        public static IsotopeAsync<Env, A> withSafariDriver<Env, A>(IsotopeAsync<Env, A> ma, SafariDriverService service, SafariOptions options) =>
            context("Safari", withWebDriver(new SafariDriver(service, options), ma));
        
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
        /// Get browser logs
        /// </summary>
        public static Isotope<Seq<BrowserLogEntry>> getBrowserLogs =>
            from d in webDriver
            from logs in tryf(() => d.Manage().Logs.GetLog(OpenQA.Selenium.LogType.Browser).ToSeq().Map(BrowserLogEntry.FromSelenium), "Failed to get browser logs")
            select logs;

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
        /// Get count of currently opened tabs
        /// </summary>
        public static Isotope<int> getOpenedTabsCount =>
            from d in webDriver
            select d.WindowHandles.Count;
        
        /// <summary>
        /// Get currently opened tab position (zero-based)
        /// </summary>
        public static Isotope<int> getCurrentTabNumber =>
            from d in webDriver
            select d.WindowHandles.IndexOf(d.CurrentWindowHandle);
        
        /// <summary>
        /// Opens and switches to new window
        /// </summary>
        public static Isotope<Unit> newWindow =>
            from d in webDriver
            from _ in trya(() => d.SwitchTo().NewWindow(WindowType.Window), "Failed to open new window")
            select unit;
                
        /// <summary>
        /// Switch to frame
        /// </summary>
        /// <param name="selector">Frame selector</param>
        /// <returns></returns>
        public static Isotope<Unit> switchToFrame(Select selector) =>
            from el in selector.ToIsotopeHead()
            from d in webDriver
            from _ in trya(() => d.SwitchTo().Frame(el), $"Failed to switch to frame with selector: {selector}")
            select unit;
        
        /// <summary>
        /// Switch to parent frame
        /// </summary>
        public static Isotope<Unit> switchToParentFrame =>
            from d in webDriver
            from _ in trya(() => d.SwitchTo().ParentFrame(), $"Failed to switch to parent frame")
            select unit;

        /// <summary>
        /// Switch back to the top-level document (exit all frames)
        /// </summary>
        public static Isotope<Unit> switchToDefaultContent =>
            from d in webDriver
            from _ in trya(() => d.SwitchTo().DefaultContent(), "Failed to switch to default content")
            select unit;

        /// <summary>
        /// Accept allert
        /// </summary>
        public static Isotope<Unit> acceptAlert =>
            from d in webDriver
            from _ in trya(() => d.SwitchTo().Alert().Accept(), $"Failed to accept alert")
            select unit;
        
        /// <summary>
        /// Dismiss allert
        /// </summary>
        public static Isotope<Unit> dismissAlert =>
            from d in webDriver
            from _ in trya(() => d.SwitchTo().Alert().Dismiss(), $"Failed to dismiss alert")
            select unit;
        
        /// <summary>
        /// Get text from alert message
        /// </summary>
        public static Isotope<string> getAlertText =>
            from d in webDriver
            from t in tryf(() => d.SwitchTo().Alert().Text, $"Failed to get alert text")
            select t;
        
        /// <summary>
        /// Send keys to alert
        /// </summary>
        public static Isotope<Unit> sendKeysToAlert(string keys) =>
            from d in webDriver
            from _ in trya(() => d.SwitchTo().Alert().SendKeys(keys), $"Failed to send keys {keys} to alert")
            select unit;

        /// <summary>
        /// Identifies whether alert is present
        /// </summary>
        public static Isotope<bool> isAlertPresent =>
            from d in webDriver
            from a in tryf(() => SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent().Invoke(d), $"Failed to get alert presence")
            select a != null;
        
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
        /// Gets the page source currently displayed by the browser
        /// </summary>
        public static Isotope<string> pageSource =>
            from d in webDriver
            select d.PageSource;
        
        /// <summary>
        /// Gets the page title currently displayed by the browser
        /// </summary>
        public static Isotope<string> title =>
            from d in webDriver
            select d.Title;
        
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
        /// Looks for a particular attribute on an existing element, returning None if not found
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="att">Attribute to look up</param>
        /// <returns>Some(value) if the attribute exists, None otherwise</returns>
        public static Isotope<Option<string>> attributeOrNone(Select selector, string att) =>
            from el in selector.ToIsotopeHead()
            from rs in tryf(() => el.GetAttribute(att), $"Error reading attribute {att} from element: {prettyPrint(el)}")
            select Optional(rs);

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
        /// Double-clicks on the element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static Isotope<Unit> doubleClick(Select selector) =>
            selector.ToIsotopeHead()
                    .Bind(IsotopeInternal.doubleClick);

        /// <summary>
        /// Right-clicks (context menu) on the element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static Isotope<Unit> rightClick(Select selector) =>
            selector.ToIsotopeHead()
                    .Bind(IsotopeInternal.rightClick);

        /// <summary>
        /// Drags the source element and drops it onto the target element
        /// </summary>
        /// <param name="source">Source element selector</param>
        /// <param name="target">Target element selector</param>
        public static Isotope<Unit> dragTo(Select source, Select target) =>
            from s in source.ToIsotopeHead()
            from t in target.ToIsotopeHead()
            from _ in IsotopeInternal.dragTo(s, t)
            select unit;

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
        /// Moves the mouse to the specified element
        /// </summary>
        /// <param name="element">The element to which to move the mouse</param>
        public static Isotope<Unit> moveToElement(Select selector) =>
            selector.ToIsotopeHead()
                    .Bind(el => IsotopeInternal.moveToElement(el));

        /// <summary>
        /// Moves the mouse to the specified offset of the last known mouse coordinates.
        /// </summary>
        /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
        public static Isotope<Unit> moveByOffset(int offsetX, int offsetY) =>
            IsotopeInternal.moveByOffset(offsetX, offsetY);
        
        /// <summary>
        /// Moves the mouse from the upper left corner of the current viewport by the provided offset
        /// </summary>
        /// <param name="offsetX">The horizontal offset to which to move the mouse</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse</param>
        public static Isotope<Unit> moveToLocation(int offsetX, int offsetY) =>
            IsotopeInternal.moveToLocation(offsetX, offsetY);

        /// <summary>
        /// Scrolls the page until the element is in the viewport
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static Isotope<Unit> scrollToElement(Select selector) =>
            from el in selector.ToIsotopeHead()
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from _ in trya(() => jsExec.ExecuteScript("arguments[0].scrollIntoView({block:'center'})", el), $"Error scrolling to element: {IsotopeInternal.prettyPrint(el)}")
            select unit;

        /// <summary>
        /// Scrolls the viewport by a relative pixel offset
        /// </summary>
        /// <param name="x">Horizontal pixel offset</param>
        /// <param name="y">Vertical pixel offset</param>
        public static Isotope<Unit> scrollBy(int x, int y) =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from _ in trya(() => jsExec.ExecuteScript($"window.scrollBy({x},{y})"), $"Error scrolling by offset x: {x} y: {y}")
            select unit;

        /// <summary>
        /// Scrolls to the top of the page
        /// </summary>
        public static Isotope<Unit> scrollToTop =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from _ in trya(() => jsExec.ExecuteScript("window.scrollTo(0,0)"), "Error scrolling to top")
            select unit;

        /// <summary>
        /// Scrolls to the bottom of the page
        /// </summary>
        public static Isotope<Unit> scrollToBottom =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from _ in trya(() => jsExec.ExecuteScript("window.scrollTo(0,document.body.scrollHeight)"), "Error scrolling to bottom")
            select unit;

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
        /// ONLY USE AS A LAST RESORT
        /// Pauses the processing for an interval to brute force waiting for actions to complete
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds to pause</param>
        public static Isotope<Unit> pause(int milliseconds) =>
            pause(TimeSpan.FromMilliseconds(milliseconds));

        /// <summary>
        /// Gets the text inside an element
        /// </summary>
        /// <param name="selector">Element containing txt</param>
        public static Isotope<string> text(Select selector) =>
            from el in selector.ToIsotopeHead()
            from rs in tryf(() => el.Text, $@"Error getting text from element: {prettyPrint(el)}")
            select rs;

        /// <summary>
        /// Gets the visible text of all elements matching the selector
        /// </summary>
        /// <param name="selector">Element selector</param>
        /// <returns>Text of all matching elements. Returns an empty Seq if no elements match.</returns>
        public static Isotope<Seq<string>> texts(Select selector) =>
            from es in selector.ToIsotope()
            from ts in es.Map(el => tryf(() => el.Text, $@"Error getting text from element: {prettyPrint(el)}")).Sequence()
            select ts;

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
        /// Returns the count of elements matching the selector
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static Isotope<int> elementCount(Select selector) =>
            from es in selector.ToIsotope()
            select es.Count;

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
        public static Isotope<Option<BrowserScreenshot>> getScreenshot =>
            from dvr in webDriver
            let ts = dvr as ITakesScreenshot
            select ts == null ? None : Some(BrowserScreenshot.FromSelenium(ts.GetScreenshot()));

        /// <summary>
        /// Takes a screenshot of a specific element
        /// </summary>
        /// <param name="selector">Web element selector</param>
        public static Isotope<Option<BrowserScreenshot>> getElementScreenshot(Select selector) =>
            from el in selector.ToIsotopeHead()
            let ts = el as ITakesScreenshot
            select ts == null ? Option<BrowserScreenshot>.None : Some(BrowserScreenshot.FromSelenium(ts.GetScreenshot()));

        static void saveScreenshotToFile(BrowserScreenshot screenshot, string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            screenshot.SaveToFile(filePath);
        }

        /// <summary>
        /// Captures a screenshot and saves it to the given file path.
        /// Creates parent directories if they don't exist.
        /// </summary>
        /// <param name="filePath">File path to save the screenshot to</param>
        public static Isotope<Unit> saveScreenshot(string filePath) =>
            from s in getScreenshot
            from _ in s.Match(
                Some: ss => trya(() => saveScreenshotToFile(ss, filePath), $"Failed to save screenshot to: {filePath}"),
                None: fail("WebDriver does not support screenshots"))
            select unit;

        /// <summary>
        /// Captures a screenshot of a specific element and saves it to the given file path.
        /// Creates parent directories if they don't exist.
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="filePath">File path to save the screenshot to</param>
        public static Isotope<Unit> saveElementScreenshot(Select selector, string filePath) =>
            from s in getElementScreenshot(selector)
            from _ in s.Match(
                Some: ss => trya(() => saveScreenshotToFile(ss, filePath), $"Failed to save element screenshot to: {filePath}"),
                None: fail("Element does not support screenshots"))
            select unit;

        /// <summary>
        /// Runs the javascript and returns a value
        /// </summary>
        public static Isotope<T> eval<T>(string javascript) =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from res in pure((T)jsExec.ExecuteScript(javascript))
            select res;

        /// <summary>
        /// Executes JavaScript in the browser context with no return value
        /// </summary>
        /// <param name="javascript">JavaScript to execute</param>
        public static Isotope<Unit> eval(string javascript) =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            let _ = jsExec.ExecuteScript(javascript)
            select unit;

        /// <summary>
        /// Executes JavaScript against a specific element.
        /// The element is available as arguments[0] in the script.
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="javascript">JavaScript to execute</param>
        public static Isotope<T> eval<T>(Select selector, string javascript) =>
            from el in selector.ToIsotopeHead()
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from res in pure((T)jsExec.ExecuteScript(javascript, el))
            select res;

        /// <summary>
        /// Runs the javascript and returns a value.
        /// Errors are caught and converted to Isotope failures.
        /// </summary>
        /// <param name="javascript">JavaScript to execute</param>
        public static Isotope<T> evalSafe<T>(string javascript) =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from res in tryf(() => (T)jsExec.ExecuteScript(javascript), "Error executing JavaScript")
            select res;

        /// <summary>
        /// Executes JavaScript in the browser context with no return value.
        /// Errors are caught and converted to Isotope failures.
        /// </summary>
        /// <param name="javascript">JavaScript to execute</param>
        public static Isotope<Unit> evalSafe(string javascript) =>
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from _ in trya(() => jsExec.ExecuteScript(javascript), "Error executing JavaScript")
            select unit;

        /// <summary>
        /// Executes JavaScript against a specific element.
        /// The element is available as arguments[0] in the script.
        /// Errors are caught and converted to Isotope failures.
        /// </summary>
        /// <param name="selector">Web element selector</param>
        /// <param name="javascript">JavaScript to execute</param>
        public static Isotope<T> evalSafe<T>(Select selector, string javascript) =>
            from el in selector.ToIsotopeHead()
            from dvr in webDriver
            let jsExec = (IJavaScriptExecutor)dvr
            from res in tryf(() => (T)jsExec.ExecuteScript(javascript, el), "Error executing JavaScript on element")
            select res;

        /// <summary>
        /// Returns all cookies for the current domain
        /// </summary>
        public static Isotope<Seq<BrowserCookie>> getCookies =>
            from d in webDriver
            from cs in tryf(() => d.Manage().Cookies.AllCookies.ToSeq().Map(BrowserCookie.FromSelenium).Strict(), "Failed to get cookies")
            select cs;

        /// <summary>
        /// Sets a cookie
        /// </summary>
        /// <param name="cookie">Cookie to set</param>
        public static Isotope<Unit> setCookie(BrowserCookie cookie) =>
            from d in webDriver
            from _ in trya(() => d.Manage().Cookies.AddCookie(cookie.ToSelenium()), $"Failed to set cookie: {cookie.Name}")
            select unit;

        /// <summary>
        /// Deletes a cookie by name
        /// </summary>
        /// <param name="name">Name of the cookie to delete</param>
        public static Isotope<Unit> deleteCookie(string name) =>
            from d in webDriver
            from _ in trya(() => d.Manage().Cookies.DeleteCookieNamed(name), $"Failed to delete cookie: {name}")
            select unit;

        /// <summary>
        /// Deletes all cookies for the current domain
        /// </summary>
        public static Isotope<Unit> deleteAllCookies =>
            from d in webDriver
            from _ in trya(() => d.Manage().Cookies.DeleteAllCookies(), "Failed to delete all cookies")
            select unit;

        /// <summary>
        /// Gets the current window size
        /// </summary>
        public static Isotope<Size> getWindowSize =>
            from d in webDriver
            from sz in tryf(() => d.Manage().Window.Size, "Failed to get window size")
            select sz;
    }
}
