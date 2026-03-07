[`< Back`](./)

---

# WebElement

Namespace: Isotope80

Immutable web-element that doesn't suffer the problems of the lazy IWebElement (namely if the IWebDriver is
 disposed then IWebElement fails, which is a problem for lazy processes).

```csharp
public class WebElement : System.IEquatable`1[[Isotope80.WebElement, Isotope80, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [WebElement](./isotope80.webelement.md)<br>
Implements [IEquatable&lt;WebElement&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## Properties

### **Selector**

SelectQuery selector that found this element

```csharp
public Select Selector { get; set; }
```

#### Property Value

[Select](./isotope80.select.md)<br>

### **SelectionIndex**

This structure was made from a @select. This is the index into the @select results

```csharp
public int SelectionIndex { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **Id**

Element id attribute

```csharp
public string Id { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **TagName**

Element tag name

```csharp
public string TagName { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Text**

string Text

```csharp
public string Text { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Enabled**

Element enabled flag

```csharp
public bool Enabled { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **Selected**

Indicates whether or not this element is selected.

```csharp
public bool Selected { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **Location**

Gets a  object containing the coordinates of the upper-left corner of this element relative to the upper-left corner of the page.

```csharp
public Point Location { get; set; }
```

#### Property Value

Point<br>

### **Size**

Gets a  object containing the height and width of this element.

```csharp
public Size Size { get; set; }
```

#### Property Value

Size<br>

### **Displayed**

Indicates whether or not this element is displayed.

```csharp
public bool Displayed { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **Value**

Element value attribute

```csharp
public string Value { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Title**

Element title attribute

```csharp
public string Title { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Constructors

### **WebElement(Select, Int32, String, String, String, Boolean, Boolean, Point, Size, Boolean, String, String)**

Immutable web-element that doesn't suffer the problems of the lazy IWebElement (namely if the IWebDriver is
 disposed then IWebElement fails, which is a problem for lazy processes).

```csharp
public WebElement(Select Selector, int SelectionIndex, string Id, string TagName, string Text, bool Enabled, bool Selected, Point Location, Size Size, bool Displayed, string Value, string Title)
```

#### Parameters

`Selector` [Select](./isotope80.select.md)<br>
SelectQuery selector that found this element

`SelectionIndex` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
This structure was made from a @select. This is the index into the @select results

`Id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Element id attribute

`TagName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Element tag name

`Text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
string Text

`Enabled` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Element enabled flag

`Selected` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Indicates whether or not this element is selected.

`Location` Point<br>
Gets a  object containing the coordinates of the upper-left corner of this element relative to the upper-left corner of the page.

`Size` Size<br>
Gets a  object containing the height and width of this element.

`Displayed` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Indicates whether or not this element is displayed.

`Value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Element value attribute

`Title` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Element title attribute

## Methods

### **ToString()**

```csharp
public string ToString()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **GetHashCode()**

```csharp
public int GetHashCode()
```

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **Equals(Object)**

```csharp
public bool Equals(object obj)
```

#### Parameters

`obj` [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **Equals(WebElement)**

```csharp
public bool Equals(WebElement other)
```

#### Parameters

`other` [WebElement](./isotope80.webelement.md)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **&lt;Clone&gt;$()**

```csharp
public WebElement <Clone>$()
```

#### Returns

[WebElement](./isotope80.webelement.md)<br>

### **Deconstruct(Select&, Int32&, String&, String&, String&, Boolean&, Boolean&, Point&, Size&, Boolean&, String&, String&)**

```csharp
public void Deconstruct(Select& Selector, Int32& SelectionIndex, String& Id, String& TagName, String& Text, Boolean& Enabled, Boolean& Selected, Point& Location, Size& Size, Boolean& Displayed, String& Value, String& Title)
```

#### Parameters

`Selector` [Select&](./isotope80.select&.md)<br>

`SelectionIndex` [Int32&](https://docs.microsoft.com/en-us/dotnet/api/system.int32&)<br>

`Id` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

`TagName` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

`Text` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

`Enabled` [Boolean&](https://docs.microsoft.com/en-us/dotnet/api/system.boolean&)<br>

`Selected` [Boolean&](https://docs.microsoft.com/en-us/dotnet/api/system.boolean&)<br>

`Location` Point&<br>

`Size` Size&<br>

`Displayed` [Boolean&](https://docs.microsoft.com/en-us/dotnet/api/system.boolean&)<br>

`Value` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

`Title` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

---

[`< Back`](./)
