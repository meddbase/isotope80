[`< Back`](./)

---

# IsotopeState&lt;A&gt;

Namespace: Isotope80

Typed isotope state, contains an untyped state and a value of A

```csharp
public class IsotopeState<A>
```

#### Type Parameters

`A`<br>
Bound value type

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [IsotopeState&lt;A&gt;](./isotope80.isotopestate-1.md)

## Fields

### **Value**

Return value

```csharp
public A Value;
```

### **State**

Resulting state

```csharp
public IsotopeState State;
```

## Properties

### **IsFaulted**

True if the state is faulted

```csharp
public bool IsFaulted { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

## Constructors

### **IsotopeState(A, IsotopeState)**

Ctor

```csharp
public IsotopeState(A value, IsotopeState state)
```

#### Parameters

`value` A<br>

`state` [IsotopeState](./isotope80.isotopestate.md)<br>

## Methods

### **Map&lt;B&gt;(Func&lt;A, B&gt;)**

Functor map

```csharp
public IsotopeState<B> Map<B>(Func<A, B> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, B&gt;<br>

#### Returns

IsotopeState&lt;B&gt;<br>

---

[`< Back`](./)
