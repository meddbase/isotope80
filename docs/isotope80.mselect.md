[`< Back`](./)

---

# MSelect

Namespace: Isotope80

Select monoid instance

```csharp
public struct MSelect
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [MSelect](./isotope80.mselect.md)<br>
Implements Monoid&lt;Select&gt;, Semigroup&lt;Select&gt;, Typeclass

## Methods

### **Append(Select, Select)**

Associative binary operator for select composition

```csharp
Select Append(Select x, Select y)
```

#### Parameters

`x` [Select](./isotope80.select.md)<br>
Left select

`y` [Select](./isotope80.select.md)<br>
Right select

#### Returns

[Select](./isotope80.select.md)<br>
Composed select

### **Empty()**

Monoidal unit. For Select this selects all.

```csharp
Select Empty()
```

#### Returns

[Select](./isotope80.select.md)<br>
Unit select

---

[`< Back`](./)
