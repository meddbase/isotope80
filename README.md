*<sup>80</sup>Se : the most abundant stable isotope of Selenium*

# Isotope80

Isotope80 (Isotope hereafter) is a functional C# wrapper around Selenium and WebDriver for browser automation. It provides a declarative, composable way to write browser automation scripts that handle logging, error management, and driver lifecycle behind the scenes. It builds on the functional C# base class library provided by [Language-Ext](https://github.com/louthy/language-ext).

## Motivation

Using Selenium in C# often seems trivial at first. Install the NuGet packages and you can quickly start automating a browser:

```cs
var driver = new ChromeDriver();

driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

var userName = driver.FindElement(By.Id("username"));
userName.SendKeys("tomsmith");

var pass = driver.FindElement(By.Id("password"));
pass.SendKeys("SuperSecretPassword!");

var loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));
loginButton.Click();
```

As the complexity and volume of automation code increases you start to encounter issues. You need to pass the `IWebDriver` instance around (or worse, reference it globally). To get reproducible steps you need logging. You need to pass config around. You need error handling strategies. `NullReferenceException` and `NoSuchElementException` are common when elements don't exist, and unhandled exceptions can leave browser processes running. None of these problems are insurmountable, but each one makes originally readable code harder to maintain.

## DSL

Isotope provides a domain specific language that addresses these problems. The equivalent of the login example above:

```cs
var login = from _1 in nav("https://the-internet.herokuapp.com/login")
            from _2 in sendKeys(css("#username"), "tomsmith")
            from _3 in sendKeys(css("#password"), "SuperSecretPassword!")
            from _4 in click(css("button[type='submit']"))
            select unit;

var result = withChromeDriver(login).RunAndThrowOnError();
```

Logging, error handling, and driver lifecycle are handled automatically. The `Isotope<A>` type is a monad — each step composes with the next, and if any step fails the chain short-circuits with a descriptive error rather than throwing an exception.

## Using LINQ

Isotope allows statements to be composed together using the `Bind` function:

```cs
var logic = nav("https://the-internet.herokuapp.com/login")
              .Bind(_ => sendKeys(css("#username"), "tomsmith"));
```

However the recommended approach is to use C#'s LINQ syntax:

```cs
var logic = from _1 in nav("https://the-internet.herokuapp.com/login")
            from _2 in sendKeys(css("#username"), "tomsmith")
            select unit;
```

Both perform the same action but LINQ is more readable when composing many steps. The `_1`, `_2` convention is used for values we don't need — both `nav` and `sendKeys` return `Unit`.

The Language-Ext wiki explains why this works: [What is LINQ Really?](https://github.com/louthy/language-ext/wiki/Thinking-Functionally:-What-is-LINQ-really%3F).

## Getting Started

Install via NuGet:

```bash
dotnet add package Isotope80
```

Isotope is intended to be referenced via `using static` so that the functions can be called directly:

```cs
using LanguageExt;
using OpenQA.Selenium;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
```

Isotope depends on [Selenium.WebDriver](https://www.nuget.org/packages/Selenium.WebDriver) but doesn't include browser driver implementations. You'll need the relevant driver for your browser:

- [ChromeDriver](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver)
- [GeckoDriver (Firefox)](https://www.nuget.org/packages/Selenium.WebDriver.GeckoDriver)

The examples here use Chrome and assume you have both `Isotope80.Isotope` and `LanguageExt.Prelude` as static usings.

## Composable Selectors

Isotope uses a `Select` type rather than raw `By` selectors. Selects are composable — you can chain them with `+` to refine queries, add wait conditions, and enforce cardinality:

```cs
// Wait for an element to exist, then get the first match
var selector = css(".result") + waitUntilExists + whenAtLeastOne;

// Compose selectors to find elements within elements
var childSelector = css(".parent") + css(".child");

// Use the active (focused) element
var focusedText = text(active);
```

Built-in selector combinators include `waitUntilExists`, `waitUntilNotExists`, `whenAtLeastOne`, `whenSingle`, and `atIndex`.

## A Complete Example

Here's a more realistic example that demonstrates composition, assertions, and the `context` function for structured logging:

```cs
using Isotope80;
using LanguageExt;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
using static Isotope80.Assertions;

public static class LoginTest
{
    static readonly string BaseUrl = "https://the-internet.herokuapp.com";

    public static Isotope<Unit> GoToLoginPage =>
        context("Navigate to login page",
            from _ in nav($"{BaseUrl}/login")
            select unit);

    public static Isotope<Unit> Login(string username, string password) =>
        context("Login",
            from _1 in sendKeys(css("#username"), username)
            from _2 in sendKeys(css("#password"), password)
            from _3 in click(css("button[type='submit']"))
            select unit);

    public static Isotope<Unit> AssertLoginSuccess =>
        context("Assert login success",
            from msg in text(css("#flash"))
            from _   in assert(msg.Contains("You logged into a secure area"),
                               $"Expected success message but got: {msg}")
            select unit);

    public static Isotope<Unit> LoginFlow =>
        from _1 in GoToLoginPage
        from _2 in Login("tomsmith", "SuperSecretPassword!")
        from _3 in AssertLoginSuccess
        select unit;
}
```

Running it:

```cs
var settings = IsotopeSettings.Create();
settings.LogStream.Subscribe(x => Console.WriteLine(x));

var (state, _) = withChromeDriver(LoginTest.LoginFlow).Run(settings);
```

## Running Within a Test Framework

Test frameworks like NUnit and xUnit rely on exceptions to detect failures. Since Isotope handles errors internally, use `RunAndThrowOnError` to bridge the gap — it runs the automation, cleans up the WebDriver, and throws if the result is a failure:

```cs
[Fact]
public void LoginSucceeds()
{
    var settings = IsotopeSettings.Create();
    withChromeDriver(LoginTest.LoginFlow).RunAndThrowOnError(settings: settings);
}
```

## Logging

Isotope has built-in logging. Use `info` to add log entries and `context` to create nested log scopes:

```cs
public static Isotope<Unit> GoToDesktopSite =>
    context("Go to Desktop Site",
        from _1 in info("Set window size")
        from _2 in setWindowSize(1280, 960)
        from _3 in nav("https://the-internet.herokuapp.com")
        select unit);
```

This produces structured output:

```
Go to Desktop Site
  Set window size
```

Subscribe to the log stream to see logs in real time:

```cs
var settings = IsotopeSettings.Create();
settings.LogStream.Subscribe(x => Console.WriteLine(x));
```

## Error Handling

Isotope captures errors rather than throwing exceptions. When a step fails, the chain short-circuits and the error is collected in the state. Use `context` to add meaningful scope to error messages:

```cs
var result = context("Login workflow",
    from _1 in nav("https://example.com/login")
    from _2 in sendKeys(css("#missing-element"), "text")  // This will fail
    select unit);
```

The error will include the context: `Login workflow > Element not found`.

Use `exists` to check for elements without failing:

```cs
from found in exists(css(".optional-banner"))
from _     in found
                  ? click(css(".optional-banner .dismiss"))
                  : pure(unit)
select unit;
```

## Assertions

Isotope provides assertion functions that integrate with the error model rather than throwing:

```cs
using static Isotope80.Assertions;

// Assert a condition
from _ in assert(url == expected, $"Expected {expected} but got {url}")

// Assert element has specific text
from _ in assertElementHasText(css(".heading"), "Welcome")

// Assert element is displayed
from _ in assertElementIsDisplayed(css("#content"))
```

## Multi-Browser Testing

Run the same automation across multiple browsers:

```cs
var result = withWebDrivers(myTest,
    WebDriverSelect.Chrome,
    WebDriverSelect.Firefox,
    WebDriverSelect.Edge);
```

Errors from each browser are collected separately and prefixed with the browser name.

## API Reference

Full API documentation is available in the [docs](./docs/) folder, generated from XML documentation comments.
