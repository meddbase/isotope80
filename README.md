*<sup>80</sup>Se : the most abundant stable isotope of Selenium*

# Isotope80
Isotope80 (Isotope hereafter) is a functional C# wrapper around Selenium and WebDriver for browser automation. It aims to provide a frictionless way to write declarative, composable statements that can then be run using a WebDriver instance. It builds on the functional C# base class library provided by [Language-Ext](https://github.com/louthy/language-ext).

## Motivation
Using Selenium in C# often seems trivial at first glance. The Nuget packages can easily be installed and then you can quickly start to automate a browser instance. Here's a quick example of logging into Twitter:

```cs
//Create the driver for Chrome
var driver = new ChromeDriver();

//Navigate to the Twitter's Login page
driver.Navigate().GoToUrl("https://twitter.com/login");

//Find the Email box and type the address
var userName = driver.FindElement(By.ClassName("js-username-field"));
userName.SendKeys("Your Email Address");

//Find the Password box and type it
var pass = driver.FindElement(By.ClassName("js-password-field"));
pass.SendKeys("Your Password");

//Find Login Button and press Enter on it
var loginButton = driver.FindElement(By.CssSelector("button.submit"));
loginButton.SendKeys(Keys.Enter);
```

When the complexity and volume of automation code increases you start to encounter issues. As you break parts of your code into reusable steps you start to need to pass the WebDriver instance around (or worse reference it globally), to get reproducible steps you may need to add logging code, you might need to pass config around and you start to need error handling strategies. Null reference exceptions are common when you attempt to access elements within a page that do not exist and if not handled then you can leave browsers running beyond the lifetime of your program. None of the problems are insurmountable but each one makes the originally trivial and readable code more complex and much harder to maintain.

## DSL
Isotope provides a domain specific language which addresses the problems previously outlined and results in composable code that wraps up the complexity of logging, error handling, driver instance management and passing config behind the scenes allowing you to focus on automation code. The equivalent of the Twitter example in Isotope would be:

```cs
var logic =  from _1 in nav("https://twitter.com/login")
             from _2 in sendKeys(className("js-username-field"), "Your Email Address")
             from _3 in sendKeys(className("js-password-field"), "Your Password")
             from _4 in sendKeys(cssSelector("button.submit"), Keys.Enter)
             select unit;
             
var result = chromeDriver(logic).Run();
```
## Using Linq
Isotope allows statements to be composed together, you can do this using the Bind function, for example:

```cs
var logic =  nav("https://twitter.com/login")
              .Bind(x => sendKeys(className("js-username-field"), "Your Email Address"));
```

However the recommended method is to use C#'s LINQ syntax:

```cs
var logic =  from _1 in nav("https://twitter.com/login")
             from _2 in sendKeys(className("js-username-field"), "Your Email Address")
             select unit;
```

Both of these examples perform the same action however the LINQ syntax allows for more flexibility and less clutter.

The Language-Ext wiki contains an article detailing why this works: [What is LINQ Really?](https://github.com/louthy/language-ext/wiki/Thinking-Functionally:-What-is-LINQ-really%3F).

In the above example you can see the use of `_1` and `_2` this is a convention we use for values that we are going to ignore, in this instance both `nav(...)` and `sendKeys(...)` return `Unit` so there is no need to use them.

## Getting Started
Isotope can be installed via [Nuget](https://www.nuget.org/packages/Isotope80/0.0.0-beta)

```bash
PM> Install-Package Isotope80 -Version 0.0.0-beta
```

Isotope is intended to be referenced via a `using static` statement so that the functions are able to be accessed in a very terse way.

```cs
using LanguageExt;
using OpenQA.Selenium;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
```

Isotope depends on the [Selenium WebDriver](https://www.nuget.org/packages/Selenium.WebDriver) Nuget package but it doesn't include any implementations of `IWebDriver`. To run against a browser you will need the relevant WebDriver package, for example:

- [ChromeDriver](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver)
- [IEDriver](https://www.nuget.org/packages/Selenium.WebDriver.IEDriver/)

The examples presented here will use ChromeDriver and assume that you have both `Isotope80.Isotope` and `LanguageExt.Prelude` as static usings.

## Running Within a Test Framework
A common method for running web automation tests it to utilise a unit testing framework such as NUnit or XUnit. These frameworks rely on catching exceptions to determine if a test has failed. Since Isotope handles exceptions for you we provide a special function to use for this purpose: `RunAndThrowOnError`. This function runs the automation and if the end result is a failure it handles cleaning up your WebDriver instance before throwing the exception for the test framework to deal with.

## Logging
Isotope provides a built in logging mechanism to ensure that detailed output of browser automations can be gathered. Anywhere within an Isotope declaration can use `log` to add to the logs.

```cs
from _1 in log("Update Window Size")
from _2 in setWindowSize(1280, 960)
from _3 in nav("https://www.meddbase.com")
select unit
```

This simply adds a log entry prior to doing some work. The system also includes the ability to nest logs to make them more readable. To do this you call the context function with the top level string and the Isotope<T> that you want to occur within that context.

```cs
public static Isotope<Unit> GoToDesktopSite =>
  context("Go to Desktop Site",
    from _1 in log("Update Window Size")
    from _2 in setWindowSize(1280, 960)
    from _3 in nav("https://www.meddbase.com")
    select unit);
```

This would log:

```
Go to Desktop Site
  Update Window Size
```

It is also possible via the settings to provide an additional action to the logging mechanism to be performed on each log entry, the following writes all logs to the console as they occur with the relevant level of indentation:

```cs
var settings = IsotopeSettings.Create();
settings.LogStream.Subscribe(x => WriteLine(x));

var result = chromeDriver(Meddbase.GoToPageAndOpenCareers).Run(settings);
```
