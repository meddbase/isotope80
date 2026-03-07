[`< Back`](./)

---

# Assertions

Namespace: Isotope80

Common assertions

```csharp
public static class Assertions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Assertions](./isotope80.assertions.md)

## Methods

### **assert(Isotope&lt;Boolean&gt;, String)**

Assert that fact is True

```csharp
public static Isotope<Unit> assert(Isotope<bool> fact, string label)
```

#### Parameters

`fact` [Isotope&lt;Boolean&gt;](./isotope80.isotope-1.md)<br>
Fact to assert

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Label on failure

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **assert(Func&lt;Boolean&gt;, String)**

Assert that fact is True

```csharp
public static Isotope<Unit> assert(Func<bool> fact, string label)
```

#### Parameters

`fact` [Func&lt;Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-1)<br>
Fact to assert

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Label on failure

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **assert(Boolean, String)**

Assert that fact is True

```csharp
public static Isotope<Unit> assert(bool fact, string label)
```

#### Parameters

`fact` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Fact to assert

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Label on failure

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **assertElementHasText(String, String)**

Assert that an element has text that matches `expected`

```csharp
public static Isotope<Unit> assertElementHasText(string cssSelector, string expected)
```

#### Parameters

`cssSelector` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
CSS selector

`expected` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Label on failure

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **assertElementHasText(Select, String)**

Assert that an element has text that matches `expected`

```csharp
public static Isotope<Unit> assertElementHasText(Select selector, string expected)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Element selector

`expected` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Label on failure

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **assertElementIsDisplayed(String)**

Assert element is displayed

```csharp
public static Isotope<Unit> assertElementIsDisplayed(string cssSelector)
```

#### Parameters

`cssSelector` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
CSS selector

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

### **assertElementIsDisplayed(Select)**

Assert element is displayed

```csharp
public static Isotope<Unit> assertElementIsDisplayed(Select selector)
```

#### Parameters

`selector` [Select](./isotope80.select.md)<br>
Element selector

#### Returns

[Isotope&lt;Unit&gt;](./isotope80.isotope-1.md)<br>

---

[`< Back`](./)
