# Isotope80.dll v.0.0.0.0 API documentation

# All types

|   |   |   |
|---|---|---|
| [Assertions Class](#assertions-class) | [IsotopeSettings Class](#isotopesettings-class) | [Log Class](#log-class) |
| [Isotope Class](#isotope-class) | [IsotopeState Class](#isotopestate-class) | [Node Class](#node-class) |
| [Isotope\<A\> Class](#isotope\<a\>-class) | [IsotopeState\<A\> Class](#isotopestate\<a\>-class) |   |
# Assertions Class

Namespace: Isotope80


## Methods

| Name | Returns | Summary |
|---|---|---|
| **assert(Isotope\<bool\> fact, string label)** | Isotope\<Unit\> |  |
| **assert(Func\<bool\> fact, string label)** | Isotope\<Unit\> |  |
| **assert(bool fact, string label)** | Isotope\<Unit\> |  |
| **assertElementHasText(string cssSelector, string expected)** | Isotope\<Unit\> |  |
| **assertElementHasText(By selector, string expected)** | Isotope\<Unit\> |  |
| **assertElementHasText(IWebElement el, string expected)** | Isotope\<Unit\> |  |
| **assertElementIsDisplayed(string cssSelector)** | Isotope\<Unit\> |  |
| **assertElementIsDisplayed(By selector)** | Isotope\<Unit\> |  |
| **assertElementIsDisplayed(IWebElement el)** | Isotope\<Unit\> |  |
# Isotope Class

Namespace: Isotope80


## Properties

| Name | Type | Summary |
|---|---|---|
| **url** | Isotope\<string\> | Gets the URL currently displayed by the browser |
| **webDriver** | Isotope\<IWebDriver\> | Web driver accessor - set by the foreachBrowser call |
| **disposeWebDriver** | Isotope\<Unit\> |  |
| **defaultWait** | Isotope\<TimeSpan\> | Default wait accessor |
| **defaultInterval** | Isotope\<TimeSpan\> | Default wait accessor |
| **unitM** | Isotope\<Unit\> | Useful for starting a linq expression if you need lets first<br>i.e.<br>        from _ in unitM<br>        let foo = "123"<br>        let bar = "456"<br>        from x in .... |
| **popLog** | Isotope\<Unit\> |  |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **attribute(IWebElement el, string att)** | Isotope\<string\> |  |
| **Bind(Isotope\<A\> ma, Func\<A, Isotope\<B\>\> f)** | Isotope\<B\> |  |
| **className(string classname)** | By |  |
| **click(By selector)** | Isotope\<Unit\> |  |
| **click(IWebElement element)** | Isotope\<Unit\> | Simulates the mouse-click |
| **Collect(Seq\<Isotope\<A\>\> mas)** | Isotope\<Seq\<A\>\> | Flips the sequence of Isotopes to be a Isotope of Sequences |
| **config(string key)** | Isotope\<string\> | Get a config key |
| **context(string context, Isotope\<A\> iso)** | Isotope\<A\> |  |
| **css(string cssSelector)** | By |  |
| **displayed(string cssSelector)** | Isotope\<bool\> |  |
| **displayed(By selector)** | Isotope\<bool\> |  |
| **displayed(IWebElement el)** | Isotope\<bool\> |  |
| **doWhile(Isotope\<A\> iso, Func\<A, bool\> continueCondition, int maxRepeats)** | Isotope\<A\> |  |
| **doWhileOrFail(Isotope\<A\> iso, Func\<A, bool\> continueCondition, string failureMessage, int maxRepeats)** | Isotope\<A\> |  |
| **doWhileOrFail(Isotope\<A\> iso, Func\<A, bool\> continueCondition, string failureMessage, TimeSpan interval, int maxRepeats)** | Isotope\<A\> |  |
| **enabled(IWebElement el)** | Isotope\<bool\> |  |
| **exists(string cssSelector)** | Isotope\<bool\> |  |
| **exists(By selector)** | Isotope\<bool\> |  |
| **fail(string message)** | Isotope\<A\> | Failure - creates an Isotope monad that always fails |
| **findElement(string cssSelector, bool wait, string errorMessage)** | Isotope\<IWebElement\> | Find an HTML element |
| **findElement(By selector, bool wait, string errorMessage)** | Isotope\<IWebElement\> | Find an HTML element |
| **findElement(IWebElement element, string cssSelector, bool wait, string errorMessage)** | Isotope\<IWebElement\> | Find an HTML element |
| **findElement(IWebElement element, By selector, bool wait, string errorMessage)** | Isotope\<IWebElement\> | Find an HTML element |
| **findElements(By selector, bool wait, string error)** | Isotope\<Seq\<IWebElement\>\> | Find HTML elements |
| **findElements(IWebElement parent, By selector, bool wait, string error)** | Isotope\<Seq\<IWebElement\>\> | Find HTML elements |
| **findElements(IWebElement element, string cssSelector, bool wait, string error)** | Isotope\<Seq\<IWebElement\>\> | Find HTML elements |
| **findElementsOrEmpty(By selector, string error)** | Isotope\<Seq\<IWebElement\>\> |  |
| **findElementsOrEmpty(IWebElement element, string cssSelector, string error)** | Isotope\<Seq\<IWebElement\>\> |  |
| **findElementsOrEmpty(IWebElement element, By selector, string error)** | Isotope\<Seq\<IWebElement\>\> |  |
| **findOptionalElement(By selector, string errorMessage)** | Isotope\<Option\<IWebElement\>\> |  |
| **findOptionalElement(IWebElement element, string cssSelector, string errorMessage)** | Isotope\<Option\<IWebElement\>\> |  |
| **findOptionalElement(IWebElement element, By selector, string errorMessage)** | Isotope\<Option\<IWebElement\>\> | Find an HTML element |
| **findSelectElement(string cssSelector)** | Isotope\<SelectElement\> | Find a &lt;select&gt; element |
| **findSelectElement(By selector)** | Isotope\<SelectElement\> | Find a &lt;select&gt; element |
| **findSelectElement(IWebElement container, string cssSelector)** | Isotope\<SelectElement\> |  |
| **findSelectElement(IWebElement container, By selector)** | Isotope\<SelectElement\> |  |
| **getSelectedOption(SelectElement select)** | Isotope\<IWebElement\> |  |
| **getSelectedOptionText(SelectElement sel)** | Isotope\<string\> |  |
| **getSelectedOptionValue(SelectElement sel)** | Isotope\<string\> |  |
| **getStyle(IWebElement el, string style)** | Isotope\<string\> |  |
| **getZIndex(IWebElement el)** | Isotope\<int\> |  |
| **hasText(IWebElement element, string comparison)** | Isotope\<bool\> | Compares the text of an element with a string |
| **id(string id)** | By |  |
| **initConfig((string, string)[] config)** | Isotope\<Unit\> |  |
| **initConfig(Map\<string, string\> config)** | Isotope\<Unit\> | Simple configuration setup |
| **initSettings([IsotopeSettings](#isotopesettings-class) settings)** | Isotope\<Unit\> |  |
| **isCheckboxChecked(string cssSelector)** | Isotope\<bool\> |  |
| **isCheckboxChecked(By selector)** | Isotope\<bool\> |  |
| **isCheckboxChecked(IWebElement el)** | Isotope\<bool\> |  |
| **linkText(string linktext)** | By |  |
| **log(string message)** | Isotope\<Unit\> | Log some output |
| **Map(Isotope\<A\> ma, Func\<A, B\> f)** | Isotope\<B\> |  |
| **name(string name)** | By |  |
| **nav(string url)** | Isotope\<Unit\> | Navigate to a URL |
| **obscured(IWebElement element)** | Isotope\<bool\> | Checks whether the centre point of an element is the foremost element at that position on the page.<br>(Uses the JavaScript document.elementFromPoint function) |
| **partialLinkText(string linktext)** | By |  |
| **pause(TimeSpan interval)** | Isotope\<Unit\> | ONLY USE AS A LAST RESORT<br>Pauses the processing for an interval to brute force waiting for actions to complete |
| **PrettyPrint(IWebElement x)** | string |  |
| **pure(A value)** | Isotope\<A\> | Identity - lifts a value of `A` into the Isotope monad<br><br>* Always succeeds * |
| **pushLog(string message)** | Isotope\<Unit\> |  |
| **put([IsotopeState](#isotopestate-class) state)** | Isotope\<Unit\> | Puts the state back into the Isotope monad |
| **Run(Isotope\<A\> ma, [IsotopeSettings](#isotopesettings-class) settings)** | ([IsotopeState](#isotopestate-class) state, A value) | Run the test computation - returning an optional error. <br>The computation succeeds if result.IsNone is true |
| **Run(Isotope\<A\> ma, IWebDriver driver, [IsotopeSettings](#isotopesettings-class) settings)** | ([IsotopeState](#isotopestate-class) state, A value) |  |
| **RunAndThrowOnError(Isotope\<A\> ma, IWebDriver driver, [IsotopeSettings](#isotopesettings-class) settings)** | ([IsotopeState](#isotopestate-class) state, A value) | Run the test computation - throws and error if it fails to pass |
| **Select(Isotope\<A\> ma, Func\<A, B\> f)** | Isotope\<B\> |  |
| **selectByText(string cssSelector, string text)** | Isotope\<Unit\> |  |
| **selectByText(By selector, string text)** | Isotope\<Unit\> | Select a &lt;select&gt; option by text |
| **selectByText(SelectElement select, string text)** | Isotope\<Unit\> | Select a &lt;select&gt; option by text |
| **selectByValue(By selector, string value)** | Isotope\<Unit\> | Select a &lt;select&gt; option by value |
| **selectByValue(SelectElement select, string value)** | Isotope\<Unit\> | Select a &lt;select&gt; option by value |
| **SelectMany(Isotope\<A\> ma, Func\<A, Isotope\<B\>\> f)** | Isotope\<B\> |  |
| **SelectMany(Isotope\<A\> ma, Func\<A, Isotope\<B\>\> bind, Func\<A, B, C\> project)** | Isotope\<C\> |  |
| **sendKeys(string cssSelector, string keys)** | Isotope\<Unit\> | Simulates keyboard by sending `keys`  |
| **sendKeys(By selector, string keys)** | Isotope\<Unit\> | Simulates keyboard by sending `keys`  |
| **sendKeys(IWebElement element, string keys)** | Isotope\<Unit\> | Simulates keyboard by sending `keys`  |
| **Sequence(Seq\<Isotope\<A\>\> mas)** | Isotope\<Seq\<A\>\> | Flips the sequence of Isotopes to be a Isotope of Sequences |
| **setCheckbox(IWebElement el, bool ticked)** | Isotope\<Unit\> |  |
| **setWebDriver(IWebDriver d)** | Isotope\<Unit\> | Web driver setter |
| **setWindowSize(int width, int height)** | Isotope\<Unit\> |  |
| **tagName(string tagname)** | By |  |
| **text(IWebElement element)** | Isotope\<string\> | Gets the text inside an element |
| **ToIsotope(Option\<A\> maybe, string label)** | Isotope\<A\> |  |
| **ToIsotope(Try\<A\> tried, string label)** | Isotope\<A\> |  |
| **ToIsotope(Try\<A\> tried, Func\<Exception, string\> makeError)** | Isotope\<A\> |  |
| **trya(Action action, string label)** | Isotope\<Unit\> | Try an action |
| **tryf(Func\<A\> func, string label)** | Isotope\<A\> | Try a function |
| **value(IWebElement element)** | Isotope\<string\> | Gets the value attribute of an element |
| **voida(Action action)** | Isotope\<Unit\> | Run an action that returns void and transform it into a unit action |
| **waitUntil(Isotope\<A\> iso, Func\<A, bool\> continueCondition, Option\<TimeSpan\> interval, Option\<TimeSpan\> wait)** | Isotope\<A\> |  |
| **waitUntilClickable(By selector)** | Isotope\<IWebElement\> | Wait for an element to be rendered and clickable, fail if exceeds default timeout |
| **waitUntilClickable(IWebElement element)** | Isotope\<Unit\> | Wait for an element to be rendered and clickable, fail if exceeds default timeout |
| **waitUntilClickable(By selector, TimeSpan timeout)** | Isotope\<IWebElement\> |  |
| **waitUntilClickable(IWebElement el, TimeSpan timeout)** | Isotope\<Unit\> |  |
| **waitUntilElementsExists(By selector, Option\<TimeSpan\> interval, Option\<TimeSpan\> wait)** | Isotope\<Seq\<IWebElement\>\> |  |
| **waitUntilElementsExists(IWebElement parent, By selector, Option\<TimeSpan\> interval, Option\<TimeSpan\> wait)** | Isotope\<Seq\<IWebElement\>\> |  |
| **waitUntilExists(By selector)** | Isotope\<IWebElement\> |  |
| **waitUntilExists(By selector, TimeSpan timeout)** | Isotope\<IWebElement\> | Checks for an element and retires for the timeout period |
| **waitUntilExists(IWebElement element, By selector, TimeSpan timeout)** | Isotope\<Unit\> | Wait for an element to be rendered and visible, fail if exceeds timeout |
| **xPath(string xpath)** | By |  |
## Fields

| Name | Type | Summary |
|---|---|---|
| **get** | Isotope\<[IsotopeState](#isotopestate-class)\> | Gets the state from the Isotope monad |
# Isotope<A> Class

Namespace: Isotope80

Base class: MulticastDelegate


## Properties

| Name | Type | Summary |
|---|---|---|
| **Method** | MethodInfo |  |
| **Target** | Object |  |
## Constructors

| Name | Summary |
|---|---|
| **Isotope\<A\>(Object object, IntPtr method)** |  |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **BeginInvoke([IsotopeState](#isotopestate-class) state, AsyncCallback callback, Object object)** | IAsyncResult |  |
| **EndInvoke(IAsyncResult result)** | IsotopeState\<A\> |  |
| **Invoke([IsotopeState](#isotopestate-class) state)** | IsotopeState\<A\> |  |
# IsotopeSettings Class

Namespace: Isotope80


## Methods

| Name | Returns | Summary |
|---|---|---|
| **Create(bool? disposeOnCompletion, Action\<string, int\> loggingAction, Action\<string, [Log](#log-class)\> failureAction, TimeSpan? wait, TimeSpan? interval)** | [IsotopeSettings](#isotopesettings-class) |  |
| **With(bool? DisposeOnCompletion, Action\<string, int\> LoggingAction, Action\<string, [Log](#log-class)\> FailureAction, TimeSpan? Wait, TimeSpan? Interval)** | [IsotopeSettings](#isotopesettings-class) |  |
## Fields

| Name | Type | Summary |
|---|---|---|
| **DisposeOnCompletion** | bool |  |
| **LoggingAction** | Action\<string, int\> |  |
| **FailureAction** | Action\<string, [Log](#log-class)\> |  |
| **Wait** | TimeSpan |  |
| **Interval** | TimeSpan |  |
# IsotopeState Class

Namespace: Isotope80


## Methods

| Name | Returns | Summary |
|---|---|---|
| **Create([IsotopeSettings](#isotopesettings-class) settings)** | [IsotopeState](#isotopestate-class) |  |
## Fields

| Name | Type | Summary |
|---|---|---|
| **Error** | Option\<string\> |  |
| **Log** | [Log](#log-class) |  |
# IsotopeState<A> Class

Namespace: Isotope80


## Constructors

| Name | Summary |
|---|---|
| **IsotopeState\<A\>(A value, [IsotopeState](#isotopestate-class) state)** |  |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Map(Func\<A, T\> mapper)** | IsotopeState\<T\> |  |
## Fields

| Name | Type | Summary |
|---|---|---|
| **Value** | A |  |
| **State** | [IsotopeState](#isotopestate-class) |  |
# Log Class

Namespace: Isotope80


## Properties

| Name | Type | Summary |
|---|---|---|
| **Empty** | [Log](#log-class) |  |
## Constructors

| Name | Summary |
|---|---|
| **Log(Seq\<[Node](#node-class)\> nodes, Option\<[Node](#node-class)\> current)** |  |
## Methods

| Name | Returns | Summary |
|---|---|---|
| **Append(string message, Action\<string, int\> action, int depth)** | [Log](#log-class) |  |
| **Pop()** | [Log](#log-class) |  |
| **Push(string message, Action\<string, int\> action, int depth)** | [Log](#log-class) |  |
| **ToString()** | string |  |
| **ToString(int indent)** | Seq\<string\> |  |
| **Trace()** | string |  |
| **Trace(int indent)** | Seq\<string\> |  |
| **With(Seq\<[Node](#node-class)\>? nodes, Option\<[Node](#node-class)\>? current)** | [Log](#log-class) |  |
# Node Class

Namespace: Isotope80


## Methods

| Name | Returns | Summary |
|---|---|---|
| **Append(string message, Action\<string, int\> action, int depth)** | [Node](#node-class) |  |
| **New(string message)** | [Node](#node-class) |  |
| **Pop()** | [Node](#node-class) |  |
| **Push(string message, Action\<string, int\> action, int depth)** | [Node](#node-class) |  |
| **ToString(int indent)** | Seq\<string\> |  |
| **Trace(int indent)** | Seq\<string\> |  |
| **With(string message, [Log](#log-class) children)** | [Node](#node-class) |  |
## Fields

| Name | Type | Summary |
|---|---|---|
| **Message** | string |  |
| **Children** | [Log](#log-class) |  |
