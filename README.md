*<sup>80</sup>Se : the most abundant stable isotope of Selenium*

# Isotope80
Isotope80 (Isotope hereafter) is a functional C# wrapper around Selenium and WebDriver for browser automation. It aims to provide a frictionless way to write declarative, composable statements that can then be run using a WebDriver instance. 

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
Isotope provides a domain specific language which addresses the problems previously outlined and results in composable code that wraps up the complexity of logging, error handling, driver instance management and passing config behind the scenes allowing you to focus on automation code.

```cs
//Create the driver for Chrome
var driver = new ChromeDriver();

var result = from _1 in nav("https://twitter.com/login")
             from _2 in sendKeys(By.ClassName("js-username-field"), "Your Email Address")
             from _3 in sendKeys(By.ClassName("js-password-field"), "Your Password")
             from _4 in sendKeys(By.CssSelector("button.submit"), Keys.Enter)
             select unit;
```
