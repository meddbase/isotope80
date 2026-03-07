[`< Back`](./)

---

# Select

Namespace: Isotope80

Select selector

```csharp
public class Select
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Select](./isotope80.select.md)

**Remarks:**

Selects can be composed associatively. The left-hand-side select gets refined by the right-hand-side.
 Identity selects all.

## Fields

### **Identity**

Identity select

```csharp
public static Select Identity;
```

**Remarks:**

By supporting identity and `+` Select becomes a monoid

### **active**

Selects the currently focused element

```csharp
public static Select active;
```

### **waitUntilExists**

Wait until element exists select

```csharp
public static Select waitUntilExists;
```

### **waitUntilNotExists**

Wait until no elements match the selector (or all matching elements are not displayed)

```csharp
public static Select waitUntilNotExists;
```

### **whenAtLeastOne**

Select must have at least one element

```csharp
public static Select whenAtLeastOne;
```

### **whenSingle**

Select the first element and only the first. Multiple elements is failure

```csharp
public static Select whenSingle;
```

## Properties

### **AtLeastOne**

Select must have at least one matching element

```csharp
public Select AtLeastOne { get; }
```

#### Property Value

[Select](./isotope80.select.md)<br>

### **Single**

Select must have only one matching element

```csharp
public Select Single { get; }
```

#### Property Value

[Select](./isotope80.select.md)<br>

### **WaitUntilExists**

Wait until element exists select

```csharp
public Select WaitUntilExists { get; }
```

#### Property Value

[Select](./isotope80.select.md)<br>

### **WaitUntilNotExists**

Wait until no elements match the selector (or all matching elements are not displayed)

```csharp
public Select WaitUntilNotExists { get; }
```

#### Property Value

[Select](./isotope80.select.md)<br>

## Methods

### **fromBy(By[])**

Create a Select from a By

```csharp
public static Select fromBy(By[] by)
```

#### Parameters

`by` By[]<br>
Selector arrows

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **byId(String)**

Select an element by identifier

```csharp
public static Select byId(string id)
```

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Identifier of the element

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **byLinkText(String)**

Select elements by the text within a link

```csharp
public static Select byLinkText(string linkTextToFind)
```

#### Parameters

`linkTextToFind` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Link text to find

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **byPartialLinkText(String)**

Select elements by the text within a link

```csharp
public static Select byPartialLinkText(string linkTextToFind)
```

#### Parameters

`linkTextToFind` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Link text to find

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **byName(String)**

Select elements by name attribute

```csharp
public static Select byName(string name)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name o

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **byCss(String)**

Select elements using a CSS selector

```csharp
public static Select byCss(string selector)
```

#### Parameters

`selector` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
CSS selector

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **byTag(String)**

Select elements by tag-name

```csharp
public static Select byTag(string tagName)
```

#### Parameters

`tagName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Tag name

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **byXPath(String)**

Select elements by XPath

```csharp
public static Select byXPath(string xpath)
```

#### Parameters

`xpath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
XPath selector

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **byClass(String)**

Select elements by class

```csharp
public static Select byClass(string className)
```

#### Parameters

`className` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Class selector

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

#### Returns

[Select](./isotope80.select.md)<br>

### **waitUntilExistsFor(Option&lt;TimeSpan&gt;, Option&lt;TimeSpan&gt;)**

Wait until element exists select

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

Wait until no elements match the selector (or all matching elements are not displayed)

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

### **Id(String)**

Select an element by identifier

```csharp
public Select Id(string id)
```

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Identifier of the element

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **LinkText(String)**

Select elements by the text within a link

```csharp
public Select LinkText(string linkTextToFind)
```

#### Parameters

`linkTextToFind` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Link text to find

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **PartialLinkText(String)**

Select elements by the text within a link

```csharp
public Select PartialLinkText(string linkTextToFind)
```

#### Parameters

`linkTextToFind` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Link text to find

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **Name(String)**

Select elements by name attribute

```csharp
public Select Name(string name)
```

#### Parameters

`name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name o

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **Css(String)**

Select elements using a CSS selector

```csharp
public Select Css(string selector)
```

#### Parameters

`selector` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
CSS selector

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **Tag(String)**

Select elements by tag-name

```csharp
public Select Tag(string tagName)
```

#### Parameters

`tagName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Tag name

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **XPath(String)**

Select elements by XPath

```csharp
public Select XPath(string xpath)
```

#### Parameters

`xpath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
XPath selector

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **Class(String)**

Select elements by class

```csharp
public Select Class(string className)
```

#### Parameters

`className` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Class selector

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **Index(Int32)**

Select an item at a specific index

```csharp
public Select Index(int ix)
```

#### Parameters

`ix` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **WaitUntilExistsFor(Option&lt;TimeSpan&gt;, Option&lt;TimeSpan&gt;)**

Wait until element exists select

```csharp
public Select WaitUntilExistsFor(Option<TimeSpan> interval, Option<TimeSpan> wait)
```

#### Parameters

`interval` Option&lt;TimeSpan&gt;<br>
Optional interval between checks

`wait` Option&lt;TimeSpan&gt;<br>
Optional total wait time

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **WaitUntilNotExistsFor(Option&lt;TimeSpan&gt;, Option&lt;TimeSpan&gt;)**

Wait until no elements match the selector (or all matching elements are not displayed)

```csharp
public Select WaitUntilNotExistsFor(Option<TimeSpan> interval, Option<TimeSpan> wait)
```

#### Parameters

`interval` Option&lt;TimeSpan&gt;<br>
Optional interval between checks

`wait` Option&lt;TimeSpan&gt;<br>
Optional total wait time

#### Returns

[Select](./isotope80.select.md)<br>
Select

### **ToSeq()**

Maps the select to a runnable Isotope computation

```csharp
public Isotope<Seq<WebElement>> ToSeq()
```

#### Returns

[Isotope&lt;Seq&lt;WebElement&gt;&gt;](./isotope80.isotope-1.md)<br>

### **PrettyPrint()**

Migration of the IWebElement.PrettyPrint
 TODO: Make this a bit more elegant

```csharp
public Isotope<string> PrettyPrint()
```

#### Returns

[Isotope&lt;String&gt;](./isotope80.isotope-1.md)<br>

### **ToString()**

To string

```csharp
public string ToString()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

---

[`< Back`](./)
