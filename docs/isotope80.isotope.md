[`< Back`](./)

---

# Isotope

Namespace: Isotope80

Isotope extensions

```csharp
public static class Isotope
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Isotope](./isotope80.isotope.md)<br>
Attributes [ExtensionAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.extensionattribute)

## Fields

### **get**

Gets the state from the Isotope monad

```csharp
public static Isotope<IsotopeState> get;
```

### **whenAtLeastOne**

When composed with another query, it enforces at least one result

```csharp
public static Select whenAtLeastOne;
```

### **whenSingle**

When composed with another query, it enforces only one result or fails

```csharp
public static Select whenSingle;
```

### **waitUntilExists**

Wait until element exists query

```csharp
public static Select waitUntilExists;
```

### **waitUntilNotExists**

Wait until no elements match the selector query

```csharp
public static Select waitUntilNotExists;
```

### **active**

Selects the currently focused element

```csharp
public static Select active;
```

## Properties

### **maximiseWindow**

Maximise browser window

```csharp
public static Isotope<Unit> maximiseWindow { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **minimiseWindow**

Minimise browser window

```csharp
public static Isotope<Unit> minimiseWindow { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **fullscreenWindow**

Set browser window to full screen

```csharp
public static Isotope<Unit> fullscreenWindow { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **getBrowserLogs**

Get browser logs

```csharp
public static Isotope<Seq<LogEntry>> getBrowserLogs { get; }
```

#### Property Value

[Isotope&lt;Seq&lt;LogEntry&gt;&gt;](./isotope80.isotope-1.md)<br>

### **back**

Navigate back using the browser's back button

```csharp
public static Isotope<Unit> back { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **forward**

Navigate forward using the browser's forward button

```csharp
public static Isotope<Unit> forward { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **refresh**

Refresh current page

```csharp
public static Isotope<Unit> refresh { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **newTab**

Opens and switches to new tab

```csharp
public static Isotope<Unit> newTab { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **closeTab**

Close current tab

```csharp
public static Isotope<Unit> closeTab { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **getOpenedTabsCount**

Get count of currently opened tabs

```csharp
public static Isotope<int> getOpenedTabsCount { get; }
```

#### Property Value

[Isotope&lt;Int32&gt;](./isotope80.isotope-1.md)<br>

### **getCurrentTabNumber**

Get currently opened tab position (zero-based)

```csharp
public static Isotope<int> getCurrentTabNumber { get; }
```

#### Property Value

[Isotope&lt;Int32&gt;](./isotope80.isotope-1.md)<br>

### **newWindow**

Opens and switches to new window

```csharp
public static Isotope<Unit> newWindow { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **switchToParentFrame**

Switch to parent frame

```csharp
public static Isotope<Unit> switchToParentFrame { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **switchToDefaultContent**

Switch back to the top-level document (exit all frames)

```csharp
public static Isotope<Unit> switchToDefaultContent { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **acceptAlert**

Accept allert

```csharp
public static Isotope<Unit> acceptAlert { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **dismissAlert**

Dismiss allert

```csharp
public static Isotope<Unit> dismissAlert { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **getAlertText**

Get text from alert message

```csharp
public static Isotope<string> getAlertText { get; }
```

#### Property Value

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>

### **isAlertPresent**

Identifies whether alert is present

```csharp
public static Isotope<bool> isAlertPresent { get; }
```

#### Property Value

[Isotope&lt;Boolean&gt;](./isotope80.isotope-1.md)<br>

### **url**

Gets the URL currently displayed by the browser

```csharp
public static Isotope<string> url { get; }
```

#### Property Value

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>

### **pageSource**

Gets the page source currently displayed by the browser

```csharp
public static Isotope<string> pageSource { get; }
```

#### Property Value

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>

### **title**

Gets the page title currently displayed by the browser

```csharp
public static Isotope<string> title { get; }
```

#### Property Value

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>

### **scrollToTop**

Scrolls to the top of the page

```csharp
public static Isotope<Unit> scrollToTop { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **scrollToBottom**

Scrolls to the bottom of the page

```csharp
public static Isotope<Unit> scrollToBottom { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **webDriver**

Web driver accessor

```csharp
public static Isotope<IWebDriver> webDriver { get; }
```

#### Property Value

[Isotope&lt;IWebDriver&gt;](./isotope80.isotope-1.md)<br>

### **defaultWait**

Default wait accessor

```csharp
public static Isotope<TimeSpan> defaultWait { get; }
```

#### Property Value

[Isotope&lt;TimeSpan&gt;](./isotope80.isotope-1.md)<br>

### **defaultInterval**

Default wait accessor

```csharp
public static Isotope<TimeSpan> defaultInterval { get; }
```

#### Property Value

[Isotope&lt;TimeSpan&gt;](./isotope80.isotope-1.md)<br>

### **unitM**

Useful for starting a linq expression if you need lets first
 i.e.
 from _ in unitM
 let foo = "123"
 let bar = "456"
 from x in ....

```csharp
public static Isotope<Unit> unitM { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **getScreenshot**

Takes a screenshot if the current WebDriver supports that functionality

```csharp
public static Isotope<Option<Screenshot>> getScreenshot { get; }
```

#### Property Value

[Isotope&lt;Option&lt;Screenshot&gt;&gt;](./isotope80.isotope-1.md)<br>

### **getCookies**

Returns all cookies for the current domain

```csharp
public static Isotope<Seq<BrowserCookie>> getCookies { get; }
```

#### Property Value

[Isotope&lt;Seq&lt;BrowserCookie&gt;&gt;](./isotope80.isotope-1.md)<br>

### **deleteAllCookies**

Deletes all cookies for the current domain

```csharp
public static Isotope<Unit> deleteAllCookies { get; }
```

#### Property Value

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **getWindowSize**

Gets the current window size

```csharp
public static Isotope<Size> getWindowSize { get; }
```

#### Property Value

[Isotope&lt;Size&gt;](./isotope80.isotope-1.md)<br>

## Methods

### **initConfig(ValueTuple`2[])**

Simple configuration setup

```csharp
public static Isotope<Unit> initConfig(ValueTuple`2[] config)
```

#### Parameters

`config` [ValueTuple`2[]](https://docs.microsoft.com/en-us/dotnet/api/system.valuetuple-2)<br>
Map of config items

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **initConfig(Map&lt;String, String&gt;)**

Simple configuration setup

```csharp
public static Isotope<Unit> initConfig(Map<string, string> config)
```

#### Parameters

`config` Map&lt;String, String&gt;<br>
Map of config items

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **initConfig(HashMap&lt;String, String&gt;)**

Simple configuration setup

```csharp
public static Isotope<Unit> initConfig(HashMap<string, string> config)
```

#### Parameters

`config` HashMap&lt;String, String&gt;<br>
Map of config items

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **config(String)**

Get a config key

```csharp
public static Isotope<string> config(string key)
```

#### Parameters

`key` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>

### **initSettings(IsotopeSettings)**

Update the settings within the Isotope computation

```csharp
public static Isotope<Unit> initSettings(IsotopeSettings settings)
```

#### Parameters

`settings` [IsotopeSettings](./isotope80.isotopesettings.md)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **use&lt;A, B&gt;(A, Func&lt;A, Isotope&lt;B&gt;&gt;)**

Use a disposable resource, and clean it up afterwards

```csharp
public static Isotope<B> use<A, B>(A resource, Func<A, Isotope<B>> f)
```

#### Type Parameters

`A`<br>
Disposable resource type

`B`<br>
Resulting bound value type

#### Parameters

`resource` A<br>
Disposable resource

`f` Func&lt;A, Isotope&lt;B&gt;&gt;<br>
Function to receive the resource and return an isotope run in that context

#### Returns

Isotope&lt;B&gt;<br>

### **use&lt;Env, A, B&gt;(A, Func&lt;A, Isotope&lt;Env, B&gt;&gt;)**

Use a disposable resource, and clean it up afterwards

```csharp
public static Isotope<Env, B> use<Env, A, B>(A resource, Func<A, Isotope<Env, B>> f)
```

#### Type Parameters

`Env`<br>
Environment type

`A`<br>
Disposable resource type

`B`<br>
Resulting bound value type

#### Parameters

`resource` A<br>
Disposable resource

`f` Func&lt;A, Isotope&lt;Env, B&gt;&gt;<br>
Function to receive the resource and return an isotope run in that context

#### Returns

Isotope&lt;Env, B&gt;<br>

### **use&lt;A, B&gt;(A, Func&lt;A, IsotopeAsync&lt;B&gt;&gt;)**

Use a disposable resource, and clean it up afterwards

```csharp
public static IsotopeAsync<B> use<A, B>(A resource, Func<A, IsotopeAsync<B>> f)
```

#### Type Parameters

`A`<br>
Disposable resource type

`B`<br>
Resulting bound value type

#### Parameters

`resource` A<br>
Disposable resource

`f` Func&lt;A, IsotopeAsync&lt;B&gt;&gt;<br>
Function to receive the resource and return an isotope run in that context

#### Returns

IsotopeAsync&lt;B&gt;<br>

### **use&lt;Env, A, B&gt;(A, Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;)**

Use a disposable resource, and clean it up afterwards

```csharp
public static IsotopeAsync<Env, B> use<Env, A, B>(A resource, Func<A, IsotopeAsync<Env, B>> f)
```

#### Type Parameters

`Env`<br>
Environment type

`A`<br>
Disposable resource type

`B`<br>
Resulting bound value type

#### Parameters

`resource` A<br>
Disposable resource

`f` Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;<br>
Function to receive the resource and return an isotope run in that context

#### Returns

IsotopeAsync&lt;Env, B&gt;<br>

### **use&lt;A, B&gt;(A, Func&lt;A, Unit&gt;, Func&lt;A, Isotope&lt;B&gt;&gt;)**

Use a disposable resource, and clean it up afterwards

```csharp
public static Isotope<B> use<A, B>(A resource, Func<A, Unit> dispose, Func<A, Isotope<B>> f)
```

#### Type Parameters

`A`<br>
Disposable resource type

`B`<br>
Resulting bound value type

#### Parameters

`resource` A<br>
Disposable resource

`dispose` Func&lt;A, Unit&gt;<br>
Function to clean up the resource on completion

`f` Func&lt;A, Isotope&lt;B&gt;&gt;<br>
Function to receive the resource and return an isotope run in that context

#### Returns

Isotope&lt;B&gt;<br>

### **use&lt;Env, A, B&gt;(A, Func&lt;A, Unit&gt;, Func&lt;A, Isotope&lt;Env, B&gt;&gt;)**

Use a disposable resource, and clean it up afterwards

```csharp
public static Isotope<Env, B> use<Env, A, B>(A resource, Func<A, Unit> dispose, Func<A, Isotope<Env, B>> f)
```

#### Type Parameters

`Env`<br>
Environment type

`A`<br>
Disposable resource type

`B`<br>
Resulting bound value type

#### Parameters

`resource` A<br>
Disposable resource

`dispose` Func&lt;A, Unit&gt;<br>
Function to clean up the resource on completion

`f` Func&lt;A, Isotope&lt;Env, B&gt;&gt;<br>
Function to receive the resource and return an isotope run in that context

#### Returns

Isotope&lt;Env, B&gt;<br>

### **use&lt;A, B&gt;(A, Func&lt;A, Unit&gt;, Func&lt;A, IsotopeAsync&lt;B&gt;&gt;)**

Use a disposable resource, and clean it up afterwards

```csharp
public static IsotopeAsync<B> use<A, B>(A resource, Func<A, Unit> dispose, Func<A, IsotopeAsync<B>> f)
```

#### Type Parameters

`A`<br>
Disposable resource type

`B`<br>
Resulting bound value type

#### Parameters

`resource` A<br>
Disposable resource

`dispose` Func&lt;A, Unit&gt;<br>
Function to clean up the resource on completion

`f` Func&lt;A, IsotopeAsync&lt;B&gt;&gt;<br>
Function to receive the resource and return an isotope run in that context

#### Returns

IsotopeAsync&lt;B&gt;<br>

### **use&lt;Env, A, B&gt;(A, Func&lt;A, Unit&gt;, Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;)**

Use a disposable resource, and clean it up afterwards

```csharp
public static IsotopeAsync<Env, B> use<Env, A, B>(A resource, Func<A, Unit> dispose, Func<A, IsotopeAsync<Env, B>> f)
```

#### Type Parameters

`Env`<br>
Environment type

`A`<br>
Disposable resource type

`B`<br>
Resulting bound value type

#### Parameters

`resource` A<br>
Disposable resource

`dispose` Func&lt;A, Unit&gt;<br>
Function to clean up the resource on completion

`f` Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;<br>
Function to receive the resource and return an isotope run in that context

#### Returns

IsotopeAsync&lt;Env, B&gt;<br>

### **withWebDriver&lt;A&gt;(IWebDriver, Isotope&lt;A&gt;)**

Run the isotope provided with the web-driver context

```csharp
public static Isotope<A> withWebDriver<A>(IWebDriver driver, Isotope<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`driver` IWebDriver<br>

`ma` Isotope&lt;A&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withWebDriver&lt;Env, A&gt;(IWebDriver, Isotope&lt;Env, A&gt;)**

Run the isotope provided with the web-driver context

```csharp
public static Isotope<Env, A> withWebDriver<Env, A>(IWebDriver driver, Isotope<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`driver` IWebDriver<br>

`ma` Isotope&lt;Env, A&gt;<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withWebDriver&lt;A&gt;(IWebDriver, IsotopeAsync&lt;A&gt;)**

Run the isotope provided with the web-driver context

```csharp
public static IsotopeAsync<A> withWebDriver<A>(IWebDriver driver, IsotopeAsync<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`driver` IWebDriver<br>

`ma` IsotopeAsync&lt;A&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withWebDriver&lt;Env, A&gt;(IWebDriver, IsotopeAsync&lt;Env, A&gt;)**

Run the isotope provided with the web-driver context

```csharp
public static IsotopeAsync<Env, A> withWebDriver<Env, A>(IWebDriver driver, IsotopeAsync<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`driver` IWebDriver<br>

`ma` IsotopeAsync&lt;Env, A&gt;<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **local&lt;EnvA, EnvB, A&gt;(Func&lt;EnvA, EnvB&gt;, Isotope&lt;EnvB, A&gt;)**

Map a local environment

```csharp
public static Isotope<EnvA, A> local<EnvA, EnvB, A>(Func<EnvA, EnvB> f, Isotope<EnvB, A> ma)
```

#### Type Parameters

`EnvA`<br>

`EnvB`<br>

`A`<br>

#### Parameters

`f` Func&lt;EnvA, EnvB&gt;<br>

`ma` Isotope&lt;EnvB, A&gt;<br>

#### Returns

Isotope&lt;EnvA, A&gt;<br>

### **local&lt;EnvA, EnvB, A&gt;(Func&lt;EnvA, EnvB&gt;, IsotopeAsync&lt;EnvB, A&gt;)**

Map a local environment

```csharp
public static IsotopeAsync<EnvA, A> local<EnvA, EnvB, A>(Func<EnvA, EnvB> f, IsotopeAsync<EnvB, A> ma)
```

#### Type Parameters

`EnvA`<br>

`EnvB`<br>

`A`<br>

#### Parameters

`f` Func&lt;EnvA, EnvB&gt;<br>

`ma` IsotopeAsync&lt;EnvB, A&gt;<br>

#### Returns

IsotopeAsync&lt;EnvA, A&gt;<br>

### **withWebDrivers&lt;A&gt;(Isotope&lt;A&gt;, WebDriverSelect[])**

Run the isotope provided with the web-driver context

```csharp
public static Isotope<Unit> withWebDrivers<A>(Isotope<A> ma, WebDriverSelect[] webDrivers)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`webDrivers` [WebDriverSelect[]](./isotope80.webdriverselect.md)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **withWebDrivers&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, WebDriverSelect[])**

Run the isotope provided with the web-driver context

```csharp
public static Isotope<Env, Unit> withWebDrivers<Env, A>(Isotope<Env, A> ma, WebDriverSelect[] webDrivers)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`webDrivers` [WebDriverSelect[]](./isotope80.webdriverselect.md)<br>

#### Returns

Isotope&lt;Env, Unit&gt;<br>

### **withWebDrivers&lt;A&gt;(IsotopeAsync&lt;A&gt;, WebDriverSelect[])**

Run the isotope provided with the web-driver context

```csharp
public static IsotopeAsync<Unit> withWebDrivers<A>(IsotopeAsync<A> ma, WebDriverSelect[] webDrivers)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`webDrivers` [WebDriverSelect[]](./isotope80.webdriverselect.md)<br>

#### Returns

[IsotopeAsync&lt;Unit&gt;](./isotope80.isotopeasync-1.md)<br>

### **withWebDrivers&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, WebDriverSelect[])**

Run the isotope provided with the web-driver context

```csharp
public static IsotopeAsync<Env, Unit> withWebDrivers<Env, A>(IsotopeAsync<Env, A> ma, WebDriverSelect[] webDrivers)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`webDrivers` [WebDriverSelect[]](./isotope80.webdriverselect.md)<br>

#### Returns

IsotopeAsync&lt;Env, Unit&gt;<br>

### **withChromeDriver&lt;A&gt;(Isotope&lt;A&gt;)**

Run the isotope provided with Chrome web-driver

```csharp
public static Isotope<A> withChromeDriver<A>(Isotope<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withChromeDriver&lt;A&gt;(Isotope&lt;A&gt;, ChromeOptions)**

Run the isotope provided with Chrome web-driver

```csharp
public static Isotope<A> withChromeDriver<A>(Isotope<A> ma, ChromeOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`options` ChromeOptions<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withChromeDriver&lt;A&gt;(Isotope&lt;A&gt;, ChromeDriverService)**

Run the isotope provided with Chrome web-driver

```csharp
public static Isotope<A> withChromeDriver<A>(Isotope<A> ma, ChromeDriverService service)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`service` ChromeDriverService<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withChromeDriver&lt;A&gt;(Isotope&lt;A&gt;, ChromeDriverService, ChromeOptions)**

Run the isotope provided with Chrome web-driver

```csharp
public static Isotope<A> withChromeDriver<A>(Isotope<A> ma, ChromeDriverService service, ChromeOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`service` ChromeDriverService<br>

`options` ChromeOptions<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withEdgeDriver&lt;A&gt;(Isotope&lt;A&gt;)**

Run the isotope provided with Edge web-driver

```csharp
public static Isotope<A> withEdgeDriver<A>(Isotope<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withEdgeDriver&lt;A&gt;(Isotope&lt;A&gt;, EdgeOptions)**

Run the isotope provided with Edge web-driver

```csharp
public static Isotope<A> withEdgeDriver<A>(Isotope<A> ma, EdgeOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`options` EdgeOptions<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withEdgeDriver&lt;A&gt;(Isotope&lt;A&gt;, EdgeDriverService)**

Run the isotope provided with Edge web-driver

```csharp
public static Isotope<A> withEdgeDriver<A>(Isotope<A> ma, EdgeDriverService service)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`service` EdgeDriverService<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withEdgeDriver&lt;A&gt;(Isotope&lt;A&gt;, EdgeDriverService, EdgeOptions)**

Run the isotope provided with Edge web-driver

```csharp
public static Isotope<A> withEdgeDriver<A>(Isotope<A> ma, EdgeDriverService service, EdgeOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`service` EdgeDriverService<br>

`options` EdgeOptions<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withFirefoxDriver&lt;A&gt;(Isotope&lt;A&gt;)**

Run the isotope provided with Firefox web-driver

```csharp
public static Isotope<A> withFirefoxDriver<A>(Isotope<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withFirefoxDriver&lt;A&gt;(Isotope&lt;A&gt;, FirefoxOptions)**

Run the isotope provided with Firefox web-driver

```csharp
public static Isotope<A> withFirefoxDriver<A>(Isotope<A> ma, FirefoxOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`options` FirefoxOptions<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withFirefoxDriver&lt;A&gt;(Isotope&lt;A&gt;, FirefoxDriverService)**

Run the isotope provided with Firefox web-driver

```csharp
public static Isotope<A> withFirefoxDriver<A>(Isotope<A> ma, FirefoxDriverService service)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`service` FirefoxDriverService<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withFirefoxDriver&lt;A&gt;(Isotope&lt;A&gt;, FirefoxDriverService, FirefoxOptions)**

Run the isotope provided with Firefox web-driver

```csharp
public static Isotope<A> withFirefoxDriver<A>(Isotope<A> ma, FirefoxDriverService service, FirefoxOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`service` FirefoxDriverService<br>

`options` FirefoxOptions<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withSafariDriver&lt;A&gt;(Isotope&lt;A&gt;)**

Run the isotope provided with Safari web-driver

```csharp
public static Isotope<A> withSafariDriver<A>(Isotope<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withSafariDriver&lt;A&gt;(Isotope&lt;A&gt;, SafariOptions)**

Run the isotope provided with Safari web-driver

```csharp
public static Isotope<A> withSafariDriver<A>(Isotope<A> ma, SafariOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`options` SafariOptions<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withSafariDriver&lt;A&gt;(Isotope&lt;A&gt;, SafariDriverService)**

Run the isotope provided with Safari web-driver

```csharp
public static Isotope<A> withSafariDriver<A>(Isotope<A> ma, SafariDriverService service)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`service` SafariDriverService<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withSafariDriver&lt;A&gt;(Isotope&lt;A&gt;, SafariDriverService, SafariOptions)**

Run the isotope provided with Safari web-driver

```csharp
public static Isotope<A> withSafariDriver<A>(Isotope<A> ma, SafariDriverService service, SafariOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` Isotope&lt;A&gt;<br>

`service` SafariDriverService<br>

`options` SafariOptions<br>

#### Returns

Isotope&lt;A&gt;<br>

### **withChromeDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;)**

Run the isotope provided with Chrome web-driver

```csharp
public static Isotope<Env, A> withChromeDriver<Env, A>(Isotope<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withChromeDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, ChromeOptions)**

Run the isotope provided with Chrome web-driver

```csharp
public static Isotope<Env, A> withChromeDriver<Env, A>(Isotope<Env, A> ma, ChromeOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`options` ChromeOptions<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withChromeDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, ChromeDriverService)**

Run the isotope provided with Chrome web-driver

```csharp
public static Isotope<Env, A> withChromeDriver<Env, A>(Isotope<Env, A> ma, ChromeDriverService service)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`service` ChromeDriverService<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withChromeDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, ChromeDriverService, ChromeOptions)**

Run the isotope provided with Chrome web-driver

```csharp
public static Isotope<Env, A> withChromeDriver<Env, A>(Isotope<Env, A> ma, ChromeDriverService service, ChromeOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`service` ChromeDriverService<br>

`options` ChromeOptions<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withEdgeDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;)**

Run the isotope provided with Edge web-driver

```csharp
public static Isotope<Env, A> withEdgeDriver<Env, A>(Isotope<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withEdgeDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, EdgeOptions)**

Run the isotope provided with Edge web-driver

```csharp
public static Isotope<Env, A> withEdgeDriver<Env, A>(Isotope<Env, A> ma, EdgeOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`options` EdgeOptions<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withEdgeDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, EdgeDriverService)**

Run the isotope provided with Edge web-driver

```csharp
public static Isotope<Env, A> withEdgeDriver<Env, A>(Isotope<Env, A> ma, EdgeDriverService service)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`service` EdgeDriverService<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withEdgeDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, EdgeDriverService, EdgeOptions)**

Run the isotope provided with Edge web-driver

```csharp
public static Isotope<Env, A> withEdgeDriver<Env, A>(Isotope<Env, A> ma, EdgeDriverService service, EdgeOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`service` EdgeDriverService<br>

`options` EdgeOptions<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withFirefoxDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;)**

Run the isotope provided with Firefox web-driver

```csharp
public static Isotope<Env, A> withFirefoxDriver<Env, A>(Isotope<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withFirefoxDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, FirefoxOptions)**

Run the isotope provided with Firefox web-driver

```csharp
public static Isotope<Env, A> withFirefoxDriver<Env, A>(Isotope<Env, A> ma, FirefoxOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`options` FirefoxOptions<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withFirefoxDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, FirefoxDriverService)**

Run the isotope provided with Firefox web-driver

```csharp
public static Isotope<Env, A> withFirefoxDriver<Env, A>(Isotope<Env, A> ma, FirefoxDriverService service)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`service` FirefoxDriverService<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withFirefoxDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, FirefoxDriverService, FirefoxOptions)**

Run the isotope provided with Firefox web-driver

```csharp
public static Isotope<Env, A> withFirefoxDriver<Env, A>(Isotope<Env, A> ma, FirefoxDriverService service, FirefoxOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`service` FirefoxDriverService<br>

`options` FirefoxOptions<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withSafariDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;)**

Run the isotope provided with Safari web-driver

```csharp
public static Isotope<Env, A> withSafariDriver<Env, A>(Isotope<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withSafariDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, SafariOptions)**

Run the isotope provided with Safari web-driver

```csharp
public static Isotope<Env, A> withSafariDriver<Env, A>(Isotope<Env, A> ma, SafariOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`options` SafariOptions<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withSafariDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, SafariDriverService)**

Run the isotope provided with Safari web-driver

```csharp
public static Isotope<Env, A> withSafariDriver<Env, A>(Isotope<Env, A> ma, SafariDriverService service)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`service` SafariDriverService<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withSafariDriver&lt;Env, A&gt;(Isotope&lt;Env, A&gt;, SafariDriverService, SafariOptions)**

Run the isotope provided with Safari web-driver

```csharp
public static Isotope<Env, A> withSafariDriver<Env, A>(Isotope<Env, A> ma, SafariDriverService service, SafariOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` Isotope&lt;Env, A&gt;<br>

`service` SafariDriverService<br>

`options` SafariOptions<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **withChromeDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;)**

Run the isotope provided with Chrome web-driver

```csharp
public static IsotopeAsync<A> withChromeDriver<A>(IsotopeAsync<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withChromeDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, ChromeOptions)**

Run the isotope provided with Chrome web-driver

```csharp
public static IsotopeAsync<A> withChromeDriver<A>(IsotopeAsync<A> ma, ChromeOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`options` ChromeOptions<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withChromeDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, ChromeDriverService)**

Run the isotope provided with Chrome web-driver

```csharp
public static IsotopeAsync<A> withChromeDriver<A>(IsotopeAsync<A> ma, ChromeDriverService service)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`service` ChromeDriverService<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withChromeDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, ChromeDriverService, ChromeOptions)**

Run the isotope provided with Chrome web-driver

```csharp
public static IsotopeAsync<A> withChromeDriver<A>(IsotopeAsync<A> ma, ChromeDriverService service, ChromeOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`service` ChromeDriverService<br>

`options` ChromeOptions<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withEdgeDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;)**

Run the isotope provided with Edge web-driver

```csharp
public static IsotopeAsync<A> withEdgeDriver<A>(IsotopeAsync<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withEdgeDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, EdgeOptions)**

Run the isotope provided with Edge web-driver

```csharp
public static IsotopeAsync<A> withEdgeDriver<A>(IsotopeAsync<A> ma, EdgeOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`options` EdgeOptions<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withEdgeDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, EdgeDriverService)**

Run the isotope provided with Edge web-driver

```csharp
public static IsotopeAsync<A> withEdgeDriver<A>(IsotopeAsync<A> ma, EdgeDriverService service)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`service` EdgeDriverService<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withEdgeDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, EdgeDriverService, EdgeOptions)**

Run the isotope provided with Edge web-driver

```csharp
public static IsotopeAsync<A> withEdgeDriver<A>(IsotopeAsync<A> ma, EdgeDriverService service, EdgeOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`service` EdgeDriverService<br>

`options` EdgeOptions<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withFirefoxDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;)**

Run the isotope provided with Firefox web-driver

```csharp
public static IsotopeAsync<A> withFirefoxDriver<A>(IsotopeAsync<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withFirefoxDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, FirefoxOptions)**

Run the isotope provided with Firefox web-driver

```csharp
public static IsotopeAsync<A> withFirefoxDriver<A>(IsotopeAsync<A> ma, FirefoxOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`options` FirefoxOptions<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withFirefoxDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, FirefoxDriverService)**

Run the isotope provided with Firefox web-driver

```csharp
public static IsotopeAsync<A> withFirefoxDriver<A>(IsotopeAsync<A> ma, FirefoxDriverService service)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`service` FirefoxDriverService<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withFirefoxDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, FirefoxDriverService, FirefoxOptions)**

Run the isotope provided with Firefox web-driver

```csharp
public static IsotopeAsync<A> withFirefoxDriver<A>(IsotopeAsync<A> ma, FirefoxDriverService service, FirefoxOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`service` FirefoxDriverService<br>

`options` FirefoxOptions<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withSafariDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;)**

Run the isotope provided with Safari web-driver

```csharp
public static IsotopeAsync<A> withSafariDriver<A>(IsotopeAsync<A> ma)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withSafariDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, SafariOptions)**

Run the isotope provided with Safari web-driver

```csharp
public static IsotopeAsync<A> withSafariDriver<A>(IsotopeAsync<A> ma, SafariOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`options` SafariOptions<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withSafariDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, SafariDriverService)**

Run the isotope provided with Safari web-driver

```csharp
public static IsotopeAsync<A> withSafariDriver<A>(IsotopeAsync<A> ma, SafariDriverService service)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`service` SafariDriverService<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withSafariDriver&lt;A&gt;(IsotopeAsync&lt;A&gt;, SafariDriverService, SafariOptions)**

Run the isotope provided with Safari web-driver

```csharp
public static IsotopeAsync<A> withSafariDriver<A>(IsotopeAsync<A> ma, SafariDriverService service, SafariOptions options)
```

#### Type Parameters

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;A&gt;<br>

`service` SafariDriverService<br>

`options` SafariOptions<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **withChromeDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;)**

Run the isotope provided with Chrome web-driver

```csharp
public static IsotopeAsync<Env, A> withChromeDriver<Env, A>(IsotopeAsync<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withChromeDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, ChromeOptions)**

Run the isotope provided with Chrome web-driver

```csharp
public static IsotopeAsync<Env, A> withChromeDriver<Env, A>(IsotopeAsync<Env, A> ma, ChromeOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`options` ChromeOptions<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withChromeDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, ChromeDriverService)**

Run the isotope provided with Chrome web-driver

```csharp
public static IsotopeAsync<Env, A> withChromeDriver<Env, A>(IsotopeAsync<Env, A> ma, ChromeDriverService service)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`service` ChromeDriverService<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withChromeDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, ChromeDriverService, ChromeOptions)**

Run the isotope provided with Chrome web-driver

```csharp
public static IsotopeAsync<Env, A> withChromeDriver<Env, A>(IsotopeAsync<Env, A> ma, ChromeDriverService service, ChromeOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`service` ChromeDriverService<br>

`options` ChromeOptions<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withEdgeDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;)**

Run the isotope provided with Edge web-driver

```csharp
public static IsotopeAsync<Env, A> withEdgeDriver<Env, A>(IsotopeAsync<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withEdgeDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, EdgeOptions)**

Run the isotope provided with Edge web-driver

```csharp
public static IsotopeAsync<Env, A> withEdgeDriver<Env, A>(IsotopeAsync<Env, A> ma, EdgeOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`options` EdgeOptions<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withEdgeDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, EdgeDriverService)**

Run the isotope provided with Edge web-driver

```csharp
public static IsotopeAsync<Env, A> withEdgeDriver<Env, A>(IsotopeAsync<Env, A> ma, EdgeDriverService service)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`service` EdgeDriverService<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withEdgeDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, EdgeDriverService, EdgeOptions)**

Run the isotope provided with Edge web-driver

```csharp
public static IsotopeAsync<Env, A> withEdgeDriver<Env, A>(IsotopeAsync<Env, A> ma, EdgeDriverService service, EdgeOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`service` EdgeDriverService<br>

`options` EdgeOptions<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withFirefoxDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;)**

Run the isotope provided with Firefox web-driver

```csharp
public static IsotopeAsync<Env, A> withFirefoxDriver<Env, A>(IsotopeAsync<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withFirefoxDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, FirefoxOptions)**

Run the isotope provided with Firefox web-driver

```csharp
public static IsotopeAsync<Env, A> withFirefoxDriver<Env, A>(IsotopeAsync<Env, A> ma, FirefoxOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`options` FirefoxOptions<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withFirefoxDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, FirefoxDriverService)**

Run the isotope provided with Firefox web-driver

```csharp
public static IsotopeAsync<Env, A> withFirefoxDriver<Env, A>(IsotopeAsync<Env, A> ma, FirefoxDriverService service)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`service` FirefoxDriverService<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withFirefoxDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, FirefoxDriverService, FirefoxOptions)**

Run the isotope provided with Firefox web-driver

```csharp
public static IsotopeAsync<Env, A> withFirefoxDriver<Env, A>(IsotopeAsync<Env, A> ma, FirefoxDriverService service, FirefoxOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`service` FirefoxDriverService<br>

`options` FirefoxOptions<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withSafariDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;)**

Run the isotope provided with Safari web-driver

```csharp
public static IsotopeAsync<Env, A> withSafariDriver<Env, A>(IsotopeAsync<Env, A> ma)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withSafariDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, SafariOptions)**

Run the isotope provided with Safari web-driver

```csharp
public static IsotopeAsync<Env, A> withSafariDriver<Env, A>(IsotopeAsync<Env, A> ma, SafariOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`options` SafariOptions<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withSafariDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, SafariDriverService)**

Run the isotope provided with Safari web-driver

```csharp
public static IsotopeAsync<Env, A> withSafariDriver<Env, A>(IsotopeAsync<Env, A> ma, SafariDriverService service)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`service` SafariDriverService<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **withSafariDriver&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;, SafariDriverService, SafariOptions)**

Run the isotope provided with Safari web-driver

```csharp
public static IsotopeAsync<Env, A> withSafariDriver<Env, A>(IsotopeAsync<Env, A> ma, SafariDriverService service, SafariOptions options)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`ma` IsotopeAsync&lt;Env, A&gt;<br>

`service` SafariDriverService<br>

`options` SafariOptions<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **setWindowSize(Int32, Int32)**

Set the window size of the browser

```csharp
public static Isotope<Unit> setWindowSize(int width, int height)
```

#### Parameters

`width` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Width in pixels

`height` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Height in pixels

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **setWindowSize(Size)**

Set the window size of the browser

```csharp
public static Isotope<Unit> setWindowSize(Size size)
```

#### Parameters

`size` Size<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **setWindowPosition(Int32, Int32)**

Set the window position of the browser

```csharp
public static Isotope<Unit> setWindowPosition(int x, int y)
```

#### Parameters

`x` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Horizontal offset coordinate from left screen bound

`y` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Vertical offset coordinate from upper screen bount

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **setWindowPosition(Point)**

Set the window position of the browser

```csharp
public static Isotope<Unit> setWindowPosition(Point point)
```

#### Parameters

`point` Point<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **switchTabs(Int32)**

Change browser tab by position, determined by the order opened



Tabs in separate window also switchable to in the same order

```csharp
public static Isotope<Unit> switchTabs(int position)
```

#### Parameters

`position` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Zero-based position of tab

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **switchToFrame(Select)**

Switch to frame

```csharp
public static Isotope<Unit> switchToFrame(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Frame selector

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **sendKeysToAlert(String)**

Send keys to alert

```csharp
public static Isotope<Unit> sendKeysToAlert(string keys)
```

#### Parameters

`keys` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **nav(String)**

Navigate to a URL

```csharp
public static Isotope<Unit> nav(string url)
```

#### Parameters

`url` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
URL to navigate to

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **find1(Select)**

Find an HTML element

```csharp
public static Isotope<WebElement> find1(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Element selector

#### Returns

[Isotope&lt;WebElement&gt;](./isotope80.isotope-1.md)<br>

### **find1(WebElement, Select)**

Find an HTML element within another

```csharp
public static Isotope<WebElement> find1(WebElement element, Select selector)
```

#### Parameters

`element` [WebElement](./isotope80.webelement.md)<br>
Element to search

`selector` [Select](./isotope80.select.md)<br>
Child element selector

#### Returns

[Isotope&lt;WebElement&gt;](./isotope80.isotope-1.md)<br>

### **find(Select)**

Find HTML elements

```csharp
public static Isotope<Seq<WebElement>> find(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Element selector

#### Returns

[Isotope&lt;Seq&lt;WebElement&gt;&gt;](./isotope80.isotope-1.md)<br>

### **find(WebElement, Select)**

Find HTML elements within another

```csharp
public static Isotope<Seq<WebElement>> find(WebElement element, Select selector)
```

#### Parameters

`element` [WebElement](./isotope80.webelement.md)<br>
Element to search

`selector` [Select](./isotope80.select.md)<br>
Element selector

#### Returns

[Isotope&lt;Seq&lt;WebElement&gt;&gt;](./isotope80.isotope-1.md)<br>

### **selectByText(Select, String)**

Select a &lt;select&gt; option by text

```csharp
public static Isotope<Unit> selectByText(Select selector, string text)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>

`text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **selectByValue(Select, String)**

Select a &lt;select&gt; option by value

```csharp
public static Isotope<Unit> selectByValue(Select selector, string value)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **getSelectedOptionText(Select)**

Retrieves the text for the selected option element in a Select Element

```csharp
public static Isotope<string> getSelectedOptionText(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Element selector

#### Returns

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>
The selected Option text

### **getSelectedOptionValue(Select)**

Retrieves the value for the selected option element in a Select Element

```csharp
public static Isotope<string> getSelectedOptionValue(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Element selector

#### Returns

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>
The selected Option value

### **isCheckboxChecked(Select)**

Finds a checkbox element by selector and identifies whether it is checked

```csharp
public static Isotope<bool> isCheckboxChecked(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Boolean&gt;](./isotope80.isotope-1.md)<br>
Is checked\s

### **setCheckbox(Select, Boolean)**

Set checkbox value for existing element

```csharp
public static Isotope<Unit> setCheckbox(Select selector, bool ticked)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

`ticked` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Check the box or not

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **getStyle(Select, String)**

Looks for a particular style attribute on an existing element

```csharp
public static Isotope<string> getStyle(Select selector, string style)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

`style` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Style attribute to look up

#### Returns

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>
A string representing the style value

### **getZIndex(Select)**

Gets the Z Index style attribute value for an existing element

```csharp
public static Isotope<int> getZIndex(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Int32&gt;](./isotope80.isotope-1.md)<br>
The Z Index value

### **attribute(Select, String)**

Looks for a particular style attribute on an existing element

```csharp
public static Isotope<string> attribute(Select selector, string att)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

`att` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Attribute to look up

#### Returns

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>
A string representing the attribute value

### **attributeOrNone(Select, String)**

Looks for a particular attribute on an existing element, returning None if not found

```csharp
public static Isotope<Option<string>> attributeOrNone(Select selector, string att)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

`att` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Attribute to look up

#### Returns

[Isotope&lt;Option&lt;String&gt;&gt;](./isotope80.isotope-1.md)<br>
Some(value) if the attribute exists, None otherwise

### **sendKeys(Select, String)**

Simulates keyboard by sending `keys`

```csharp
public static Isotope<Unit> sendKeys(Select selector, string keys)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

`keys` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
String of characters that are typed

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **click(Select)**

Simulates the mouse-click

```csharp
public static Isotope<Unit> click(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **doubleClick(Select)**

Double-clicks on the element

```csharp
public static Isotope<Unit> doubleClick(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **rightClick(Select)**

Right-clicks (context menu) on the element

```csharp
public static Isotope<Unit> rightClick(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **dragTo(Select, Select)**

Drags the source element and drops it onto the target element

```csharp
public static Isotope<Unit> dragTo(Select source, Select target)
```

#### Parameters

`source` [Select](./isotope80.select.md)<br>
Source element selector

`target` [Select](./isotope80.select.md)<br>
Target element selector

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **clear(Select)**

Clears the content of an element

```csharp
public static Isotope<Unit> clear(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **overwrite(Select, String)**

Simulates keyboard by sending `keys` and overwriting current content

```csharp
public static Isotope<Unit> overwrite(Select selector, string keys)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

`keys` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
String of characters that are typed

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

**Remarks:**

Series of actions where element is clicked, pressed keys CTRL+A, Backspace then typed in new keys.
 It is an alternative to clear (without triggering event (change, blur or focus)) and sendKeys
 
 https://stackoverflow.com/questions/19833728/webelement-clear-fires-javascript-change-event-alternatives

### **moveToElement(Select)**

Moves the mouse to the specified element

```csharp
public static Isotope<Unit> moveToElement(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **moveByOffset(Int32, Int32)**

Moves the mouse to the specified offset of the last known mouse coordinates.

```csharp
public static Isotope<Unit> moveByOffset(int offsetX, int offsetY)
```

#### Parameters

`offsetX` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The horizontal offset to which to move the mouse.

`offsetY` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The vertical offset to which to move the mouse.

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **moveToLocation(Int32, Int32)**

Moves the mouse from the upper left corner of the current viewport by the provided offset

```csharp
public static Isotope<Unit> moveToLocation(int offsetX, int offsetY)
```

#### Parameters

`offsetX` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The horizontal offset to which to move the mouse

`offsetY` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The vertical offset to which to move the mouse

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **scrollToElement(Select)**

Scrolls the page until the element is in the viewport

```csharp
public static Isotope<Unit> scrollToElement(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **scrollBy(Int32, Int32)**

Scrolls the viewport by a relative pixel offset

```csharp
public static Isotope<Unit> scrollBy(int x, int y)
```

#### Parameters

`x` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Horizontal pixel offset

`y` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Vertical pixel offset

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **pause(TimeSpan)**

ONLY USE AS A LAST RESORT
 Pauses the processing for an interval to brute force waiting for actions to complete

```csharp
public static Isotope<Unit> pause(TimeSpan interval)
```

#### Parameters

`interval` [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **pause(Int32)**

ONLY USE AS A LAST RESORT
 Pauses the processing for an interval to brute force waiting for actions to complete

```csharp
public static Isotope<Unit> pause(int milliseconds)
```

#### Parameters

`milliseconds` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of milliseconds to pause

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **text(Select)**

Gets the text inside an element

```csharp
public static Isotope<string> text(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Element containing txt

#### Returns

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>

### **texts(Select)**

Gets the visible text of all elements matching the selector

```csharp
public static Isotope<Seq<string>> texts(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Element selector

#### Returns

[Isotope&lt;Seq&lt;String&gt;&gt;](./isotope80.isotope-1.md)<br>
Text of all matching elements. Returns an empty Seq if no elements match.

### **value(Select)**

Gets the value attribute of an element

```csharp
public static Isotope<string> value(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Element containing value

#### Returns

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>

### **pure&lt;A&gt;(A)**

Identity - lifts a value of `A` into the Isotope monad
 
 * Always succeeds *

```csharp
public static Isotope<A> pure<A>(A value)
```

#### Type Parameters

`A`<br>

#### Parameters

`value` A<br>

#### Returns

Isotope&lt;A&gt;<br>

### **fail(Error)**

Failure

```csharp
public static Error fail(Error err)
```

#### Parameters

`err` Error<br>
Error

#### Returns

Error<br>

### **fail(String)**

Failure

```csharp
public static Error fail(string err)
```

#### Parameters

`err` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Error

#### Returns

Error<br>

### **fail(String, Exception)**

Failure

```csharp
public static Error fail(string msg, Exception ex)
```

#### Parameters

`msg` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Error message

`ex` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Exception

#### Returns

Error<br>

### **fail(Exception)**

Failure

```csharp
public static Error fail(Exception err)
```

#### Parameters

`err` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Error

#### Returns

Error<br>

### **fail&lt;A&gt;(Error)**

Failure - creates an Isotope monad that always fails

```csharp
public static Isotope<A> fail<A>(Error err)
```

#### Type Parameters

`A`<br>

#### Parameters

`err` Error<br>
Error

#### Returns

Isotope&lt;A&gt;<br>

### **fail&lt;A&gt;(String)**

Failure - creates an Isotope monad that always fails

```csharp
public static Isotope<A> fail<A>(string message)
```

#### Type Parameters

`A`<br>

#### Parameters

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Error message

#### Returns

Isotope&lt;A&gt;<br>

### **fail&lt;A&gt;(Exception)**

Failure - creates an Isotope monad that always fails

```csharp
public static Isotope<A> fail<A>(Exception ex)
```

#### Type Parameters

`A`<br>

#### Parameters

`ex` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Exception

#### Returns

Isotope&lt;A&gt;<br>

### **iso&lt;A&gt;(Func&lt;A&gt;)**

Lift the function into the isotope monadic space

```csharp
public static Isotope<A> iso<A>(Func<A> f)
```

#### Type Parameters

`A`<br>

#### Parameters

`f` Func&lt;A&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **iso&lt;A&gt;(Func&lt;Fin&lt;A&gt;&gt;)**

Lift the function into the isotope monadic space

```csharp
public static Isotope<A> iso<A>(Func<Fin<A>> f)
```

#### Type Parameters

`A`<br>

#### Parameters

`f` Func&lt;Fin&lt;A&gt;&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **iso&lt;A&gt;(Func&lt;IsotopeState, A&gt;)**

Lift the function into the isotope monadic space

```csharp
public static Isotope<A> iso<A>(Func<IsotopeState, A> f)
```

#### Type Parameters

`A`<br>

#### Parameters

`f` Func&lt;IsotopeState, A&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **iso&lt;A&gt;(Func&lt;IsotopeState, Fin&lt;A&gt;&gt;)**

Lift the function into the isotope monadic space

```csharp
public static Isotope<A> iso<A>(Func<IsotopeState, Fin<A>> f)
```

#### Type Parameters

`A`<br>

#### Parameters

`f` Func&lt;IsotopeState, Fin&lt;A&gt;&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **iso&lt;Env, A&gt;(Func&lt;Env, IsotopeState, A&gt;)**

Lift the function into the isotope monadic space

```csharp
public static Isotope<Env, A> iso<Env, A>(Func<Env, IsotopeState, A> f)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`f` Func&lt;Env, IsotopeState, A&gt;<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **iso&lt;Env, A&gt;(Func&lt;Env, IsotopeState, Fin&lt;A&gt;&gt;)**

Lift the function into the isotope monadic space

```csharp
public static Isotope<Env, A> iso<Env, A>(Func<Env, IsotopeState, Fin<A>> f)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`f` Func&lt;Env, IsotopeState, Fin&lt;A&gt;&gt;<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **isoAsync&lt;A&gt;(Func&lt;ValueTask&lt;A&gt;&gt;)**

Lift the function into the isotope monadic space

```csharp
public static IsotopeAsync<A> isoAsync<A>(Func<ValueTask<A>> f)
```

#### Type Parameters

`A`<br>

#### Parameters

`f` Func&lt;ValueTask&lt;A&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **isoAsync&lt;A&gt;(Func&lt;ValueTask&lt;Fin&lt;A&gt;&gt;&gt;)**

Lift the function into the isotope monadic space

```csharp
public static IsotopeAsync<A> isoAsync<A>(Func<ValueTask<Fin<A>>> f)
```

#### Type Parameters

`A`<br>

#### Parameters

`f` Func&lt;ValueTask&lt;Fin&lt;A&gt;&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **isoAsync&lt;A&gt;(Func&lt;IsotopeState, ValueTask&lt;A&gt;&gt;)**

Lift the function into the isotope monadic space

```csharp
public static IsotopeAsync<A> isoAsync<A>(Func<IsotopeState, ValueTask<A>> f)
```

#### Type Parameters

`A`<br>

#### Parameters

`f` Func&lt;IsotopeState, ValueTask&lt;A&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **isoAsync&lt;A&gt;(Func&lt;IsotopeState, ValueTask&lt;Fin&lt;A&gt;&gt;&gt;)**

Lift the function into the isotope monadic space

```csharp
public static IsotopeAsync<A> isoAsync<A>(Func<IsotopeState, ValueTask<Fin<A>>> f)
```

#### Type Parameters

`A`<br>

#### Parameters

`f` Func&lt;IsotopeState, ValueTask&lt;Fin&lt;A&gt;&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **isoAsync&lt;Env, A&gt;(Func&lt;Env, IsotopeState, ValueTask&lt;A&gt;&gt;)**

Lift the function into the isotope monadic space

```csharp
public static IsotopeAsync<Env, A> isoAsync<Env, A>(Func<Env, IsotopeState, ValueTask<A>> f)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`f` Func&lt;Env, IsotopeState, ValueTask&lt;A&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **isoAsync&lt;Env, A&gt;(Func&lt;Env, IsotopeState, ValueTask&lt;Fin&lt;A&gt;&gt;&gt;)**

Lift the function into the isotope monadic space

```csharp
public static IsotopeAsync<Env, A> isoAsync<Env, A>(Func<Env, IsotopeState, ValueTask<Fin<A>>> f)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`f` Func&lt;Env, IsotopeState, ValueTask&lt;Fin&lt;A&gt;&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **ask&lt;Env&gt;()**

Gets the environment from the Isotope monad

```csharp
public static Isotope<Env, Env> ask<Env>()
```

#### Type Parameters

`Env`<br>
Environment

#### Returns

Isotope&lt;Env, Env&gt;<br>

### **asks&lt;Env, R&gt;(Func&lt;Env, R&gt;)**

Gets a function of the current environment

```csharp
public static Isotope<Env, R> asks<Env, R>(Func<Env, R> f)
```

#### Type Parameters

`Env`<br>

`R`<br>

#### Parameters

`f` Func&lt;Env, R&gt;<br>

#### Returns

Isotope&lt;Env, R&gt;<br>

### **put(IsotopeState)**

Puts the state back into the Isotope monad

```csharp
public static Isotope<Unit> put(IsotopeState state)
```

#### Parameters

`state` [IsotopeState](./isotope80.isotopestate.md)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **modify(Func&lt;IsotopeState, IsotopeState&gt;)**

Modify the state from the Isotope monad

```csharp
public static Isotope<Unit> modify(Func<IsotopeState, IsotopeState> f)
```

#### Parameters

`f` [Func&lt;IsotopeState, IsotopeState&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **trya(Action, String)**

Try and action

```csharp
public static Isotope<Unit> trya(Action action, string label)
```

#### Parameters

`action` [Action](https://docs.microsoft.com/en-us/dotnet/api/system.action)<br>
Action to try

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Error string if exception is thrown

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **trya(Action, Func&lt;Error, String&gt;)**

Try and action

```csharp
public static Isotope<Unit> trya(Action action, Func<Error, string> makeError)
```

#### Parameters

`action` [Action](https://docs.microsoft.com/en-us/dotnet/api/system.action)<br>
Action to try

`makeError` [Func&lt;Error, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Convert errors to string

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **tryf&lt;A&gt;(Func&lt;A&gt;, String)**

Try a function

```csharp
public static Isotope<A> tryf<A>(Func<A> func, string label)
```

#### Type Parameters

`A`<br>
Return type of the function

#### Parameters

`func` Func&lt;A&gt;<br>
Function to try

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Error string if exception is thrown

#### Returns

Isotope&lt;A&gt;<br>

### **tryf&lt;A&gt;(Func&lt;A&gt;, Func&lt;Error, String&gt;)**

Try a function

```csharp
public static Isotope<A> tryf<A>(Func<A> func, Func<Error, string> makeError)
```

#### Type Parameters

`A`<br>
Return type of the function

#### Parameters

`func` Func&lt;A&gt;<br>
Function to try

`makeError` [Func&lt;Error, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Convert errors to string

#### Returns

Isotope&lt;A&gt;<br>

### **voida(Action)**

Run a void returning action

```csharp
public static Isotope<Unit> voida(Action action)
```

#### Parameters

`action` [Action](https://docs.microsoft.com/en-us/dotnet/api/system.action)<br>
Action to run

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>
Unit

### **log(String, String, String, Int32)**

#### Caution

Use `info | warn | error` instead

---

Log some output

```csharp
public static Isotope<Unit> log(string message, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Parameters

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **info(String, String, String, Int32)**

Log some output as info

```csharp
public static Isotope<Unit> info(string message, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Parameters

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **warn(String, String, String, Int32)**

Log some output as a warning

```csharp
public static Isotope<Unit> warn(string message, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Parameters

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **error(String, String, String, Int32)**

Log some output as an error

```csharp
public static Isotope<Unit> error(string message, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Parameters

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

**Remarks:**

Note: This only logs the error, it doesn't stop the computation. Use `fail` for computation
 termination. `fail` also logs to the output using this function.

### **context&lt;A&gt;(String, Isotope&lt;A&gt;, String, String, Int32)**

Create a logging context

```csharp
public static Isotope<A> context<A>(string context, Isotope<A> iso, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Type Parameters

`A`<br>

#### Parameters

`context` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`iso` Isotope&lt;A&gt;<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

Isotope&lt;A&gt;<br>

### **context&lt;Env, A&gt;(String, Isotope&lt;Env, A&gt;, String, String, Int32)**

Create a logging context

```csharp
public static Isotope<Env, A> context<Env, A>(string context, Isotope<Env, A> iso, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`context` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`iso` Isotope&lt;Env, A&gt;<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **context&lt;A&gt;(String, IsotopeAsync&lt;A&gt;, String, String, Int32)**

Create a logging context

```csharp
public static IsotopeAsync<A> context<A>(string context, IsotopeAsync<A> iso, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Type Parameters

`A`<br>

#### Parameters

`context` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`iso` IsotopeAsync&lt;A&gt;<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **context&lt;Env, A&gt;(String, IsotopeAsync&lt;Env, A&gt;, String, String, Int32)**

Create a logging context

```csharp
public static IsotopeAsync<Env, A> context<Env, A>(string context, IsotopeAsync<Env, A> iso, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`context` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`iso` IsotopeAsync&lt;Env, A&gt;<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **mute&lt;A&gt;(Isotope&lt;A&gt;)**

Mute log

```csharp
public static Isotope<A> mute<A>(Isotope<A> iso)
```

#### Type Parameters

`A`<br>

#### Parameters

`iso` Isotope&lt;A&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **mute&lt;Env, A&gt;(Isotope&lt;Env, A&gt;)**

Mute log

```csharp
public static Isotope<Env, A> mute<Env, A>(Isotope<Env, A> iso)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`iso` Isotope&lt;Env, A&gt;<br>

#### Returns

Isotope&lt;Env, A&gt;<br>

### **mute&lt;A&gt;(IsotopeAsync&lt;A&gt;)**

Mute log

```csharp
public static IsotopeAsync<A> mute<A>(IsotopeAsync<A> iso)
```

#### Type Parameters

`A`<br>

#### Parameters

`iso` IsotopeAsync&lt;A&gt;<br>

#### Returns

IsotopeAsync&lt;A&gt;<br>

### **mute&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;)**

Mute log

```csharp
public static IsotopeAsync<Env, A> mute<Env, A>(IsotopeAsync<Env, A> iso)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`iso` IsotopeAsync&lt;Env, A&gt;<br>

#### Returns

IsotopeAsync&lt;Env, A&gt;<br>

### **stopwatch&lt;A&gt;(Isotope&lt;A&gt;)**

Measure the time interval of an isotope

```csharp
public static Isotope<ValueTuple<A, TimeSpan>> stopwatch<A>(Isotope<A> iso)
```

#### Type Parameters

`A`<br>

#### Parameters

`iso` Isotope&lt;A&gt;<br>

#### Returns

Isotope&lt;ValueTuple&lt;A, TimeSpan&gt;&gt;<br>

### **stopwatch&lt;Env, A&gt;(Isotope&lt;Env, A&gt;)**

Measure the time interval of an isotope

```csharp
public static Isotope<Env, ValueTuple<A, TimeSpan>> stopwatch<Env, A>(Isotope<Env, A> iso)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`iso` Isotope&lt;Env, A&gt;<br>

#### Returns

Isotope&lt;Env, ValueTuple&lt;A, TimeSpan&gt;&gt;<br>

### **stopwatch&lt;A&gt;(IsotopeAsync&lt;A&gt;)**

Measure the time interval of an isotope

```csharp
public static IsotopeAsync<ValueTuple<A, TimeSpan>> stopwatch<A>(IsotopeAsync<A> iso)
```

#### Type Parameters

`A`<br>

#### Parameters

`iso` IsotopeAsync&lt;A&gt;<br>

#### Returns

IsotopeAsync&lt;ValueTuple&lt;A, TimeSpan&gt;&gt;<br>

### **stopwatch&lt;Env, A&gt;(IsotopeAsync&lt;Env, A&gt;)**

Measure the time interval of an isotope

```csharp
public static IsotopeAsync<Env, ValueTuple<A, TimeSpan>> stopwatch<Env, A>(IsotopeAsync<Env, A> iso)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`iso` IsotopeAsync&lt;Env, A&gt;<br>

#### Returns

IsotopeAsync&lt;Env, ValueTuple&lt;A, TimeSpan&gt;&gt;<br>

### **waitUntilClickable(Select)**

Wait for an element to be rendered and clickable, fail if exceeds default timeout

```csharp
public static Isotope<Unit> waitUntilClickable(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **waitUntilClickable(Select, TimeSpan)**

Wait for an element to be rendered and clickable, fail if exceeds timeout specified

```csharp
public static Isotope<Unit> waitUntilClickable(Select selector, TimeSpan timeout)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>

`timeout` [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **Sequence&lt;A&gt;(Seq&lt;Isotope&lt;A&gt;&gt;)**

Flips the sequence of Isotopes to an Isotope of Sequences. It does this by running each Isotope within
 the Seq and collects the results into a single Seq and then re-wraps within an Isotope.

```csharp
public static Isotope<Seq<A>> Sequence<A>(Seq<Isotope<A>> mas)
```

#### Type Parameters

`A`<br>

#### Parameters

`mas` Seq&lt;Isotope&lt;A&gt;&gt;<br>

#### Returns

Isotope&lt;Seq&lt;A&gt;&gt;<br>

**Remarks:**

If an error is encountered along the way, then the computation ends immediately. Therefore the items in the
 sequence after that point are not evaluated.
 
 The resulting state will contain the log of all items evaluated or the first error encountered.
 
 Each item runs in an indexed `context`. i.e. 
 
 [0], [1], [2] ...
 
 Which makes it easier to know which index a log entry is for.

### **Sequence&lt;Env, A&gt;(Seq&lt;Isotope&lt;Env, A&gt;&gt;)**

Flips the sequence of Isotopes to an Isotope of Sequences. It does this by running each Isotope within
 the Seq and collects the results into a single Seq and then re-wraps within an Isotope.

```csharp
public static Isotope<Env, Seq<A>> Sequence<Env, A>(Seq<Isotope<Env, A>> mas)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`mas` Seq&lt;Isotope&lt;Env, A&gt;&gt;<br>

#### Returns

Isotope&lt;Env, Seq&lt;A&gt;&gt;<br>

**Remarks:**

If an error is encountered along the way, then the computation ends immediately. Therefore the items in the
 sequence after that point are not evaluated.
 
 The resulting state will contain the log of all items evaluated or the first error encountered.
 
 Each item runs in an indexed `context`. i.e. 
 
 [0], [1], [2] ...
 
 Which makes it easier to know which index a log entry is for.

### **Sequence&lt;A&gt;(Seq&lt;IsotopeAsync&lt;A&gt;&gt;)**

Flips the sequence of Isotopes to an Isotope of Sequences. It does this by running each Isotope within
 the Seq and collects the results into a single Seq and then re-wraps within an Isotope.

```csharp
public static IsotopeAsync<Seq<A>> Sequence<A>(Seq<IsotopeAsync<A>> mas)
```

#### Type Parameters

`A`<br>

#### Parameters

`mas` Seq&lt;IsotopeAsync&lt;A&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Seq&lt;A&gt;&gt;<br>

**Remarks:**

If an error is encountered along the way, then the computation ends immediately. Therefore the items in the
 sequence after that point are not evaluated.
 
 The resulting state will contain the log of all items evaluated or the first error encountered.
 
 Each item runs in an indexed `context`. i.e. 
 
 [0], [1], [2] ...
 
 Which makes it easier to know which index a log entry is for.

### **Sequence&lt;Env, A&gt;(Seq&lt;IsotopeAsync&lt;Env, A&gt;&gt;)**

Flips the sequence of Isotopes to an Isotope of Sequences. It does this by running each Isotope within
 the Seq and collects the results into a single Seq and then re-wraps within an Isotope.

```csharp
public static IsotopeAsync<Env, Seq<A>> Sequence<Env, A>(Seq<IsotopeAsync<Env, A>> mas)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`mas` Seq&lt;IsotopeAsync&lt;Env, A&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, Seq&lt;A&gt;&gt;<br>

**Remarks:**

If an error is encountered along the way, then the computation ends immediately. Therefore the items in the
 sequence after that point are not evaluated.
 
 The resulting state will contain the log of all items evaluated or the first error encountered.
 
 Each item runs in an indexed `context`. i.e. 
 
 [0], [1], [2] ...
 
 Which makes it easier to know which index a log entry is for.

### **Collect&lt;A&gt;(Seq&lt;Isotope&lt;A&gt;&gt;)**

Flips the sequence of Isotopes to an Isotope of Sequences. It does this by running each Isotope within
 the Seq and collects the results into a single Seq and then re-wraps within an Isotope.

```csharp
public static Isotope<Seq<A>> Collect<A>(Seq<Isotope<A>> mas)
```

#### Type Parameters

`A`<br>

#### Parameters

`mas` Seq&lt;Isotope&lt;A&gt;&gt;<br>

#### Returns

Isotope&lt;Seq&lt;A&gt;&gt;<br>

**Remarks:**

If an error is encountered along the way then it is collected. The process then continues to evaluate the
 subsequent items. The resulting resulting state will contain the log of all items evaluated. If there was
 an error, the resulting state will have an aggregated list of errors.
 
 Each item runs in an indexed `context`. i.e. 
 
 [0], [1], [2] ...
 
 Which makes it easier to know which index a log entry is for.

### **Collect&lt;Env, A&gt;(Seq&lt;Isotope&lt;Env, A&gt;&gt;)**

Flips the sequence of Isotopes to an Isotope of Sequences. It does this by running each Isotope within
 the Seq and collects the results into a single Seq and then re-wraps within an Isotope.

```csharp
public static Isotope<Env, Seq<A>> Collect<Env, A>(Seq<Isotope<Env, A>> mas)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`mas` Seq&lt;Isotope&lt;Env, A&gt;&gt;<br>

#### Returns

Isotope&lt;Env, Seq&lt;A&gt;&gt;<br>

**Remarks:**

If an error is encountered along the way then it is collected. The process then continues to evaluate the
 subsequent items. The resulting resulting state will contain the log of all items evaluated. If there was
 an error, the resulting state will have an aggregated list of errors.
 
 Each item runs in an indexed `context`. i.e. 
 
 [0], [1], [2] ...
 
 Which makes it easier to know which index a log entry is for.

### **Collect&lt;A&gt;(Seq&lt;IsotopeAsync&lt;A&gt;&gt;)**

Flips the sequence of Isotopes to an Isotope of Sequences. It does this by running each Isotope within
 the Seq and collects the results into a single Seq and then re-wraps within an Isotope.

```csharp
public static IsotopeAsync<Seq<A>> Collect<A>(Seq<IsotopeAsync<A>> mas)
```

#### Type Parameters

`A`<br>

#### Parameters

`mas` Seq&lt;IsotopeAsync&lt;A&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Seq&lt;A&gt;&gt;<br>

**Remarks:**

If an error is encountered along the way then it is collected. The process then continues to evaluate the
 subsequent items. The resulting resulting state will contain the log of all items evaluated. If there was
 an error, the resulting state will have an aggregated list of errors.
 
 Each item runs in an indexed `context`. i.e. 
 
 [0], [1], [2] ...
 
 Which makes it easier to know which index a log entry is for.

### **Collect&lt;Env, A&gt;(Seq&lt;IsotopeAsync&lt;Env, A&gt;&gt;)**

Flips the sequence of Isotopes to an Isotope of Sequences. It does this by running each Isotope within
 the Seq and collects the results into a single Seq and then re-wraps within an Isotope.

```csharp
public static IsotopeAsync<Env, Seq<A>> Collect<Env, A>(Seq<IsotopeAsync<Env, A>> mas)
```

#### Type Parameters

`Env`<br>

`A`<br>

#### Parameters

`mas` Seq&lt;IsotopeAsync&lt;Env, A&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, Seq&lt;A&gt;&gt;<br>

**Remarks:**

If an error is encountered along the way then it is collected. The process then continues to evaluate the
 subsequent items. The resulting resulting state will contain the log of all items evaluated. If there was
 an error, the resulting state will have an aggregated list of errors.
 
 Each item runs in an indexed `context`. i.e. 
 
 [0], [1], [2] ...
 
 Which makes it easier to know which index a log entry is for.

### **ToIsotope&lt;A&gt;(Option&lt;A&gt;, String)**

Convert an option to a pure isotope

```csharp
public static Isotope<A> ToIsotope<A>(Option<A> maybe, string label)
```

#### Type Parameters

`A`<br>
Bound value type

#### Parameters

`maybe` Option&lt;A&gt;<br>
Optional value

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Failure value to use if None

#### Returns

Isotope&lt;A&gt;<br>
Pure isotope

### **ToIsotope&lt;A&gt;(Option&lt;A&gt;, Isotope&lt;A&gt;)**

Convert an option to a pure isotope

```csharp
public static Isotope<A> ToIsotope<A>(Option<A> maybe, Isotope<A> alternative)
```

#### Type Parameters

`A`<br>
Bound value type

#### Parameters

`maybe` Option&lt;A&gt;<br>
Optional value

`alternative` Isotope&lt;A&gt;<br>
Alternative to use if None

#### Returns

Isotope&lt;A&gt;<br>
Pure isotope

### **ToIsotope&lt;A&gt;(Try&lt;A&gt;)**

Convert a try to an isotope computation

```csharp
public static Isotope<A> ToIsotope<A>(Try<A> tried)
```

#### Type Parameters

`A`<br>
Bound value type

#### Parameters

`tried` Try&lt;A&gt;<br>
Try value

#### Returns

Isotope&lt;A&gt;<br>
Try computation wrapped in an isotope computation

### **ToIsotope&lt;A&gt;(Try&lt;A&gt;, String)**

Convert a try to an isotope computation

```csharp
public static Isotope<A> ToIsotope<A>(Try<A> tried, string label)
```

#### Type Parameters

`A`<br>
Bound value type

#### Parameters

`tried` Try&lt;A&gt;<br>
Try value

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Failure value to use if Fail

#### Returns

Isotope&lt;A&gt;<br>
Try computation wrapped in an isotope computation

### **ToIsotope&lt;A&gt;(Try&lt;A&gt;, Func&lt;Error, String&gt;)**

Convert a try to an isotope computation

```csharp
public static Isotope<A> ToIsotope<A>(Try<A> tried, Func<Error, string> makeError)
```

#### Type Parameters

`A`<br>
Bound value type

#### Parameters

`tried` Try&lt;A&gt;<br>
Try value

`makeError` [Func&lt;Error, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Failure value to use if Fail

#### Returns

Isotope&lt;A&gt;<br>
Try computation wrapped in an isotope computation

### **ToIsotope&lt;A&gt;(Try&lt;A&gt;, Isotope&lt;A&gt;)**

Convert a try to an isotope computation

```csharp
public static Isotope<A> ToIsotope<A>(Try<A> tried, Isotope<A> alternative)
```

#### Type Parameters

`A`<br>
Bound value type

#### Parameters

`tried` Try&lt;A&gt;<br>
Try value

`alternative` Isotope&lt;A&gt;<br>
Alternative to use if Fail

#### Returns

Isotope&lt;A&gt;<br>
Try computation wrapped in an isotope computation

### **ToIsotope&lt;R&gt;(Either&lt;Error, R&gt;)**

Convert an Either to a pure isotope

```csharp
public static Isotope<R> ToIsotope<R>(Either<Error, R> either)
```

#### Type Parameters

`R`<br>
Right param

#### Parameters

`either` Either&lt;Error, R&gt;<br>
Either to convert

#### Returns

Isotope&lt;R&gt;<br>
Pure isotope

### **ToIsotope&lt;A, B&gt;(Either&lt;A, B&gt;, String)**

Convert an Either to a pure isotope

```csharp
public static Isotope<B> ToIsotope<A, B>(Either<A, B> either, string label)
```

#### Type Parameters

`A`<br>

`B`<br>

#### Parameters

`either` Either&lt;A, B&gt;<br>
Either to convert

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Label for the failure

#### Returns

Isotope&lt;B&gt;<br>
Pure isotope

### **ToIsotope&lt;A, B&gt;(Either&lt;A, B&gt;, Func&lt;A, String&gt;)**

Convert an Either to a pure isotope

```csharp
public static Isotope<B> ToIsotope<A, B>(Either<A, B> either, Func<A, string> makeError)
```

#### Type Parameters

`A`<br>

`B`<br>

#### Parameters

`either` Either&lt;A, B&gt;<br>
Either to convert

`makeError` Func&lt;A, String&gt;<br>
Label for the failure

#### Returns

Isotope&lt;B&gt;<br>
Pure isotope

### **displayed(Select)**

Finds an element by a selector and checks if it is currently displayed

```csharp
public static Isotope<bool> displayed(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Boolean&gt;](./isotope80.isotope-1.md)<br>
True if the element is currently displayed

### **enabled(Select)**

Finds an element by a selector and checks if it is currently enabled

```csharp
public static Isotope<bool> enabled(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Boolean&gt;](./isotope80.isotope-1.md)<br>
True if the element is currently enabled

### **exists(Select)**

Checks if an element exists that matches the selector

```csharp
public static Isotope<bool> exists(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Boolean&gt;](./isotope80.isotope-1.md)<br>
True if a matching element exists

### **elementCount(Select)**

Returns the count of elements matching the selector

```csharp
public static Isotope<int> elementCount(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Int32&gt;](./isotope80.isotope-1.md)<br>

### **obscured(Select)**

Checks whether the centre point of an element is the foremost element at that position on the page.
 (Uses the JavaScript document.elementFromPoint function)

```csharp
public static Isotope<bool> obscured(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Boolean&gt;](./isotope80.isotope-1.md)<br>
true if the element is foremost

### **hasText(Select, String)**

Compares the text of an element with a string

```csharp
public static Isotope<Unit> hasText(Select element, string comparison)
```

#### Parameters

`element` [Select](./isotope80.select.md)<br>
Element to compare

`comparison` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
String to match

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>
Fails if no match, with a contextual error

**Remarks:**

hasText doesn't return a bool Isotope because it's expected you do this:
 
 var ma = hasText(selector, txt) | ...
 
 Where `...` can be what to do if the text doesn't match. That could be
 reporting a different error to the default, or providing an alternative
 success operation.

### **waitUntil&lt;A&gt;(Isotope&lt;A&gt;, Func&lt;A, Boolean&gt;, Option&lt;TimeSpan&gt;, Option&lt;TimeSpan&gt;)**

Wait until the `condition` is `true`, or it times-out

```csharp
public static Isotope<A> waitUntil<A>(Isotope<A> iso, Func<A, bool> condition, Option<TimeSpan> interval, Option<TimeSpan> wait)
```

#### Type Parameters

`A`<br>

#### Parameters

`iso` Isotope&lt;A&gt;<br>

`condition` Func&lt;A, Boolean&gt;<br>

`interval` Option&lt;TimeSpan&gt;<br>

`wait` Option&lt;TimeSpan&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **waitUntil&lt;A&gt;(Isotope&lt;A&gt;, Option&lt;TimeSpan&gt;, Option&lt;TimeSpan&gt;)**

Wait until `iso` succeeds, or it times-out

```csharp
public static Isotope<A> waitUntil<A>(Isotope<A> iso, Option<TimeSpan> interval, Option<TimeSpan> wait)
```

#### Type Parameters

`A`<br>

#### Parameters

`iso` Isotope&lt;A&gt;<br>

`interval` Option&lt;TimeSpan&gt;<br>

`wait` Option&lt;TimeSpan&gt;<br>

#### Returns

Isotope&lt;A&gt;<br>

### **doWhile&lt;A&gt;(Isotope&lt;A&gt;, Func&lt;A, Boolean&gt;, Int32)**

Do while the `condition` is `true`, or it times-out

```csharp
public static Isotope<A> doWhile<A>(Isotope<A> iso, Func<A, bool> condition, int maxRepeats)
```

#### Type Parameters

`A`<br>

#### Parameters

`iso` Isotope&lt;A&gt;<br>

`condition` Func&lt;A, Boolean&gt;<br>

`maxRepeats` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

Isotope&lt;A&gt;<br>

### **doWhileOrFail&lt;A&gt;(Isotope&lt;A&gt;, Func&lt;A, Boolean&gt;, Int32)**

Run `iso` while the `condition` is `true`.
 
 * If it turns `false` or the result of `iso` is returned
 * If the max-attempts are reached, then `fail`.

```csharp
public static Isotope<A> doWhileOrFail<A>(Isotope<A> iso, Func<A, bool> condition, int maxAttempts)
```

#### Type Parameters

`A`<br>

#### Parameters

`iso` Isotope&lt;A&gt;<br>

`condition` Func&lt;A, Boolean&gt;<br>

`maxAttempts` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

Isotope&lt;A&gt;<br>

### **doWhileOrFail&lt;A&gt;(Isotope&lt;A&gt;, Func&lt;A, Boolean&gt;, TimeSpan, Int32)**

Run `iso` while the `condition` is `true`. 
 
 * If it turns `false` or the result of `iso` is returned
 * If the max-attempts are reached, then `fail`.
 * `interval` specifies the delay between attempts

```csharp
public static Isotope<A> doWhileOrFail<A>(Isotope<A> iso, Func<A, bool> continueCondition, TimeSpan interval, int maxAttempts)
```

#### Type Parameters

`A`<br>

#### Parameters

`iso` Isotope&lt;A&gt;<br>

`continueCondition` Func&lt;A, Boolean&gt;<br>

`interval` [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

`maxAttempts` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

Isotope&lt;A&gt;<br>

### **getElementScreenshot(Select)**

Takes a screenshot of a specific element

```csharp
public static Isotope<Option<Screenshot>> getElementScreenshot(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

#### Returns

[Isotope&lt;Option&lt;Screenshot&gt;&gt;](./isotope80.isotope-1.md)<br>

### **saveScreenshot(String)**

Captures a screenshot and saves it to the given file path.
 Creates parent directories if they don't exist.

```csharp
public static Isotope<Unit> saveScreenshot(string filePath)
```

#### Parameters

`filePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
File path to save the screenshot to

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **saveElementScreenshot(Select, String)**

Captures a screenshot of a specific element and saves it to the given file path.
 Creates parent directories if they don't exist.

```csharp
public static Isotope<Unit> saveElementScreenshot(Select selector, string filePath)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

`filePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
File path to save the screenshot to

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **eval&lt;T&gt;(String)**

Runs the javascript and returns a value

```csharp
public static Isotope<T> eval<T>(string javascript)
```

#### Type Parameters

`T`<br>

#### Parameters

`javascript` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

Isotope&lt;T&gt;<br>

### **eval(String)**

Executes JavaScript in the browser context with no return value

```csharp
public static Isotope<Unit> eval(string javascript)
```

#### Parameters

`javascript` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
JavaScript to execute

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **eval&lt;T&gt;(Select, String)**

Executes JavaScript against a specific element.
 The element is available as arguments[0] in the script.

```csharp
public static Isotope<T> eval<T>(Select selector, string javascript)
```

#### Type Parameters

`T`<br>

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

`javascript` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
JavaScript to execute

#### Returns

Isotope&lt;T&gt;<br>

### **evalSafe&lt;T&gt;(String)**

Runs the javascript and returns a value.
 Errors are caught and converted to Isotope failures.

```csharp
public static Isotope<T> evalSafe<T>(string javascript)
```

#### Type Parameters

`T`<br>

#### Parameters

`javascript` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
JavaScript to execute

#### Returns

Isotope&lt;T&gt;<br>

### **evalSafe(String)**

Executes JavaScript in the browser context with no return value.
 Errors are caught and converted to Isotope failures.

```csharp
public static Isotope<Unit> evalSafe(string javascript)
```

#### Parameters

`javascript` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
JavaScript to execute

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **evalSafe&lt;T&gt;(Select, String)**

Executes JavaScript against a specific element.
 The element is available as arguments[0] in the script.
 Errors are caught and converted to Isotope failures.

```csharp
public static Isotope<T> evalSafe<T>(Select selector, string javascript)
```

#### Type Parameters

`T`<br>

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Web element selector

`javascript` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
JavaScript to execute

#### Returns

Isotope&lt;T&gt;<br>

### **setCookie(BrowserCookie)**

Sets a cookie

```csharp
public static Isotope<Unit> setCookie(BrowserCookie cookie)
```

#### Parameters

`cookie` [BrowserCookie](./isotope80.browsercookie.md)<br>
Cookie to set

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **deleteCookie(String)**

Deletes a cookie by name

```csharp
public static Isotope<Unit> deleteCookie(string name)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of the cookie to delete

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **css(String)**

Creates a CSS Selector for use with WebDriver. Equivalent of `By.CssSelector`

```csharp
public static Select css(string cssSelector)
```

#### Parameters

`cssSelector` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Selector

#### Returns

[Select](./isotope80.select.md)<br>
Web element selector

### **xPath(String)**

Creates a XPath Selector for use with WebDriver. Equivalent of `By.XPath`

```csharp
public static Select xPath(string xpath)
```

#### Parameters

`xpath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Selector

#### Returns

[Select](./isotope80.select.md)<br>
Web element selector

### **className(String)**

Creates a Class Name Selector for use with WebDriver. Equivalent of `By.ClassName`

```csharp
public static Select className(string classname)
```

#### Parameters

`classname` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Selector

#### Returns

[Select](./isotope80.select.md)<br>
Web element selector

### **id(String)**

Creates an Id Selector for use with WebDriver. Equivalent of `By.Id`

```csharp
public static Select id(string id)
```

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Selector

#### Returns

[Select](./isotope80.select.md)<br>
Web element selector

### **tagName(String)**

Creates a Tag Name Selector for use with WebDriver. Equivalent of `By.TagName`

```csharp
public static Select tagName(string tagname)
```

#### Parameters

`tagname` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Selector

#### Returns

[Select](./isotope80.select.md)<br>
Web element selector

### **name(String)**

Creates a Name Selector for use with WebDriver. Equivalent of `By.Name`

```csharp
public static Select name(string name)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Selector

#### Returns

[Select](./isotope80.select.md)<br>
Web element selector

### **linkText(String)**

Creates a Link Text Selector for use with WebDriver. Equivalent of `By.LinkText`

```csharp
public static Select linkText(string linktext)
```

#### Parameters

`linktext` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Selector

#### Returns

[Select](./isotope80.select.md)<br>
Web element selector

### **partialLinkText(String)**

Creates a Partial Link Text Selector for use with WebDriver. Equivalent of `By.PartialLinkText`

```csharp
public static Select partialLinkText(string linktext)
```

#### Parameters

`linktext` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Selector

#### Returns

[Select](./isotope80.select.md)<br>
Web element selector

### **waitUntilExistsFor(Option&lt;TimeSpan&gt;, Option&lt;TimeSpan&gt;)**

Wait until element exists query

```csharp
public static Select waitUntilExistsFor(Option<TimeSpan> interval, Option<TimeSpan> wait)
```

#### Parameters

`interval` Option&lt;TimeSpan&gt;<br>
Optional interval between checks

`wait` Option&lt;TimeSpan&gt;<br>
Optional total wait time

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **waitUntilNotExistsFor(Option&lt;TimeSpan&gt;, Option&lt;TimeSpan&gt;)**

Wait until no elements match the selector query

```csharp
public static Select waitUntilNotExistsFor(Option<TimeSpan> interval, Option<TimeSpan> wait)
```

#### Parameters

`interval` Option&lt;TimeSpan&gt;<br>
Optional interval between checks

`wait` Option&lt;TimeSpan&gt;<br>
Optional total wait time

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **atIndex(Int32)**

Select an item at a specific index

```csharp
public static Select atIndex(int ix)
```

#### Parameters

`ix` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Index to select

#### Returns

[Select](./isotope80.select.md)<br>

---

[`< Back`](./)
