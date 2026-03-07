[`< Back`](./)

---

# BrowserCookie

Namespace: Isotope80

Immutable browser cookie representation, decoupled from Selenium's Cookie type

```csharp
public class BrowserCookie : System.IEquatable`1[[Isotope80.BrowserCookie, Isotope80, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [BrowserCookie](./isotope80.browsercookie.md)<br>
Implements [IEquatable&lt;BrowserCookie&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## Properties

### **Name**

Cookie name

```csharp
public string Name { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Value**

Cookie value

```csharp
public string Value { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Domain**

Cookie domain

```csharp
public string Domain { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Path**

Cookie path

```csharp
public string Path { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Expiry**

Expiry date, or null if session cookie

```csharp
public Nullable<DateTime> Expiry { get; set; }
```

#### Property Value

[Nullable&lt;DateTime&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

### **Secure**

Whether the cookie is secure

```csharp
public bool Secure { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **HttpOnly**

Whether the cookie is HTTP only

```csharp
public bool HttpOnly { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **SameSite**

SameSite attribute value

```csharp
public string SameSite { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Constructors

### **BrowserCookie(String, String, String, String, Nullable&lt;DateTime&gt;, Boolean, Boolean, String)**

Immutable browser cookie representation, decoupled from Selenium's Cookie type

```csharp
public BrowserCookie(string Name, string Value, string Domain, string Path, Nullable<DateTime> Expiry, bool Secure, bool HttpOnly, string SameSite)
```

#### Parameters

`Name` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Cookie name

`Value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Cookie value

`Domain` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Cookie domain

`Path` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Cookie path

`Expiry` [Nullable&lt;DateTime&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Expiry date, or null if session cookie

`Secure` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Whether the cookie is secure

`HttpOnly` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Whether the cookie is HTTP only

`SameSite` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
SameSite attribute value

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

### **Equals(BrowserCookie)**

```csharp
public bool Equals(BrowserCookie other)
```

#### Parameters

`other` [BrowserCookie](./isotope80.browsercookie.md)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **&lt;Clone&gt;$()**

```csharp
public BrowserCookie <Clone>$()
```

#### Returns

[BrowserCookie](./isotope80.browsercookie.md)<br>

### **Deconstruct(String&, String&, String&, String&, Nullable`1&, Boolean&, Boolean&, String&)**

```csharp
public void Deconstruct(String& Name, String& Value, String& Domain, String& Path, Nullable`1& Expiry, Boolean& Secure, Boolean& HttpOnly, String& SameSite)
```

#### Parameters

`Name` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

`Value` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

`Domain` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

`Path` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

`Expiry` [Nullable`1&](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1&)<br>

`Secure` [Boolean&](https://docs.microsoft.com/en-us/dotnet/api/system.boolean&)<br>

`HttpOnly` [Boolean&](https://docs.microsoft.com/en-us/dotnet/api/system.boolean&)<br>

`SameSite` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>

---

[`< Back`](./)
