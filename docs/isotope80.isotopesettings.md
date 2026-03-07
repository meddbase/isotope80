[`< Back`](./)

---

# IsotopeSettings

Namespace: Isotope80

Common settings environment that is threaded through every Isotope computation

```csharp
public class IsotopeSettings
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [IsotopeSettings](./isotope80.isotopesettings.md)

## Fields

### **ErrorStream**

Errors

```csharp
public Subject<Error> ErrorStream;
```

### **LogStream**

Errors

```csharp
public Subject<LogOutput> LogStream;
```

### **Wait**

Wait time

```csharp
public TimeSpan Wait;
```

### **Interval**

Interval time

```csharp
public TimeSpan Interval;
```

## Methods

### **Create(Subject&lt;Error&gt;, Subject&lt;LogOutput&gt;, Nullable&lt;TimeSpan&gt;, Nullable&lt;TimeSpan&gt;)**

Create an IsotopeSettings

```csharp
public static IsotopeSettings Create(Subject<Error> errorStream, Subject<LogOutput> logStream, Nullable<TimeSpan> wait, Nullable<TimeSpan> interval)
```

#### Parameters

`errorStream` Subject&lt;Error&gt;<br>

`logStream` Subject&lt;LogOutput&gt;<br>

`wait` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

`interval` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

#### Returns

[IsotopeSettings](./isotope80.isotopesettings.md)<br>

### **Create(Nullable&lt;TimeSpan&gt;, Nullable&lt;TimeSpan&gt;)**

Create an IsotopeSettings

```csharp
public static IsotopeSettings Create(Nullable<TimeSpan> wait, Nullable<TimeSpan> interval)
```

#### Parameters

`wait` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

`interval` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

#### Returns

[IsotopeSettings](./isotope80.isotopesettings.md)<br>

---

[`< Back`](./)
