[`< Back`](./)

---

# IsotopeState

Namespace: Isotope80

Untyped isotope state
 Used to pass the state into a isotope computation

```csharp
public class IsotopeState
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [IsotopeState](./isotope80.isotopestate.md)

## Fields

### **Error**

Errors

```csharp
public Seq<Error> Error;
```

### **Log**

Log

```csharp
public Log Log;
```

### **Context**

Context stack

```csharp
public Stck<string> Context;
```

### **Mute**

Mute log

```csharp
public bool Mute;
```

## Properties

### **IsFaulted**

True if the state is faulted

```csharp
public bool IsFaulted { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

## Methods

### **Create(IsotopeSettings)**

Creates a new instance of IsotopeState with the supplied settings.

```csharp
public static IsotopeState Create(IsotopeSettings settings)
```

#### Parameters

`settings` [IsotopeSettings](./isotope80.isotopesettings.md)<br>

#### Returns

[IsotopeState](./isotope80.isotopestate.md)<br>

### **IfFailedThrow()**

Throw if the state is faulted

```csharp
public Unit IfFailedThrow()
```

#### Returns

Unit<br>

**Remarks:**

If there's one error, then its original context is maintained (stack trace, etc)

---

[`< Back`](./)
