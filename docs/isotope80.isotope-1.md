[`< Back`](./)

---

# Isotope&lt;A&gt;

Namespace: Isotope80

Environment-free isotope computation

```csharp
public struct Isotope<A>
```

#### Type Parameters

`A`<br>
Bound value

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [Isotope&lt;A&gt;](./isotope80.isotope-1.md)

## Constructors

### **Isotope(Func&lt;IsotopeState, IsotopeState&lt;A&gt;&gt;)**

Ctor

```csharp
Isotope(Func<IsotopeState, IsotopeState<A>> thunk)
```

#### Parameters

`thunk` Func&lt;IsotopeState, IsotopeState&lt;A&gt;&gt;<br>
Thunk

## Methods

### **Run(IsotopeSettings)**

Invoke the test computation

```csharp
ValueTuple<IsotopeState, A> Run(IsotopeSettings settings)
```

#### Parameters

`settings` [IsotopeSettings](./isotope80.isotopesettings.md)<br>
Optional settings

#### Returns

ValueTuple&lt;IsotopeState, A&gt;<br>
Returning an optional error. 
 The computation succeeds if result.IsNone is true

### **RunAndThrowOnError(IsotopeSettings)**

Invoke the test computation

```csharp
ValueTuple<IsotopeState, A> RunAndThrowOnError(IsotopeSettings settings)
```

#### Parameters

`settings` [IsotopeSettings](./isotope80.isotopesettings.md)<br>
Optional settings

#### Returns

ValueTuple&lt;IsotopeState, A&gt;<br>
Returning an optional error. 
 The computation succeeds if result.IsNone is true

### **Pure(A)**

Lift the pure value into the monadic space

```csharp
Isotope<A> Pure(A value)
```

#### Parameters

`value` A<br>

#### Returns

[Isotope&lt;A&gt;](./isotope80.isotope-1.md)<br>

### **Fail(Error)**

Lift the error into the monadic space

```csharp
Isotope<A> Fail(Error error)
```

#### Parameters

`error` Error<br>

#### Returns

[Isotope&lt;A&gt;](./isotope80.isotope-1.md)<br>

### **Bind&lt;B&gt;(Func&lt;A, Isotope&lt;B&gt;&gt;)**

Monadic bind

```csharp
Isotope<B> Bind<B>(Func<A, Isotope<B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, Isotope&lt;B&gt;&gt;<br>

#### Returns

Isotope&lt;B&gt;<br>

### **Bind&lt;Env, B&gt;(Func&lt;A, Isotope&lt;Env, B&gt;&gt;)**

Monadic bind

```csharp
Isotope<Env, B> Bind<Env, B>(Func<A, Isotope<Env, B>> f)
```

#### Type Parameters

`Env`<br>

`B`<br>

#### Parameters

`f` Func&lt;A, Isotope&lt;Env, B&gt;&gt;<br>

#### Returns

Isotope&lt;Env, B&gt;<br>

### **Bind&lt;B&gt;(Func&lt;A, IsotopeAsync&lt;B&gt;&gt;)**

Monadic bind

```csharp
IsotopeAsync<B> Bind<B>(Func<A, IsotopeAsync<B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, IsotopeAsync&lt;B&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;B&gt;<br>

### **Bind&lt;Env, B&gt;(Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;)**

Monadic bind

```csharp
IsotopeAsync<Env, B> Bind<Env, B>(Func<A, IsotopeAsync<Env, B>> f)
```

#### Type Parameters

`Env`<br>

`B`<br>

#### Parameters

`f` Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, B&gt;<br>

### **SelectMany&lt;B&gt;(Func&lt;A, Isotope&lt;B&gt;&gt;)**

Monadic bind

```csharp
Isotope<B> SelectMany<B>(Func<A, Isotope<B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, Isotope&lt;B&gt;&gt;<br>

#### Returns

Isotope&lt;B&gt;<br>

### **SelectMany&lt;Env, B&gt;(Func&lt;A, Isotope&lt;Env, B&gt;&gt;)**

Monadic bind

```csharp
Isotope<Env, B> SelectMany<Env, B>(Func<A, Isotope<Env, B>> f)
```

#### Type Parameters

`Env`<br>

`B`<br>

#### Parameters

`f` Func&lt;A, Isotope&lt;Env, B&gt;&gt;<br>

#### Returns

Isotope&lt;Env, B&gt;<br>

### **SelectMany&lt;B&gt;(Func&lt;A, IsotopeAsync&lt;B&gt;&gt;)**

Monadic bind

```csharp
IsotopeAsync<B> SelectMany<B>(Func<A, IsotopeAsync<B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, IsotopeAsync&lt;B&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;B&gt;<br>

### **SelectMany&lt;Env, B&gt;(Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;)**

Monadic bind

```csharp
IsotopeAsync<Env, B> SelectMany<Env, B>(Func<A, IsotopeAsync<Env, B>> f)
```

#### Type Parameters

`Env`<br>

`B`<br>

#### Parameters

`f` Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, B&gt;<br>

### **SelectMany&lt;B, C&gt;(Func&lt;A, Isotope&lt;B&gt;&gt;, Func&lt;A, B, C&gt;)**

Monadic bind

```csharp
Isotope<C> SelectMany<B, C>(Func<A, Isotope<B>> bind, Func<A, B, C> project)
```

#### Type Parameters

`B`<br>

`C`<br>

#### Parameters

`bind` Func&lt;A, Isotope&lt;B&gt;&gt;<br>

`project` Func&lt;A, B, C&gt;<br>

#### Returns

Isotope&lt;C&gt;<br>

### **SelectMany&lt;Env, B, C&gt;(Func&lt;A, Isotope&lt;Env, B&gt;&gt;, Func&lt;A, B, C&gt;)**

Monadic bind

```csharp
Isotope<Env, C> SelectMany<Env, B, C>(Func<A, Isotope<Env, B>> bind, Func<A, B, C> project)
```

#### Type Parameters

`Env`<br>

`B`<br>

`C`<br>

#### Parameters

`bind` Func&lt;A, Isotope&lt;Env, B&gt;&gt;<br>

`project` Func&lt;A, B, C&gt;<br>

#### Returns

Isotope&lt;Env, C&gt;<br>

### **SelectMany&lt;B, C&gt;(Func&lt;A, IsotopeAsync&lt;B&gt;&gt;, Func&lt;A, B, C&gt;)**

Monadic bind

```csharp
IsotopeAsync<C> SelectMany<B, C>(Func<A, IsotopeAsync<B>> bind, Func<A, B, C> project)
```

#### Type Parameters

`B`<br>

`C`<br>

#### Parameters

`bind` Func&lt;A, IsotopeAsync&lt;B&gt;&gt;<br>

`project` Func&lt;A, B, C&gt;<br>

#### Returns

IsotopeAsync&lt;C&gt;<br>

### **SelectMany&lt;Env, B, C&gt;(Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;, Func&lt;A, B, C&gt;)**

Monadic bind

```csharp
IsotopeAsync<Env, C> SelectMany<Env, B, C>(Func<A, IsotopeAsync<Env, B>> bind, Func<A, B, C> project)
```

#### Type Parameters

`Env`<br>

`B`<br>

`C`<br>

#### Parameters

`bind` Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;<br>

`project` Func&lt;A, B, C&gt;<br>

#### Returns

IsotopeAsync&lt;Env, C&gt;<br>

### **Map&lt;B&gt;(Func&lt;A, B&gt;)**

Functor map

```csharp
Isotope<B> Map<B>(Func<A, B> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, B&gt;<br>

#### Returns

Isotope&lt;B&gt;<br>

### **Select&lt;B&gt;(Func&lt;A, B&gt;)**

Functor map

```csharp
Isotope<B> Select<B>(Func<A, B> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, B&gt;<br>

#### Returns

Isotope&lt;B&gt;<br>

### **MapFail(Func&lt;Seq&lt;Error&gt;, Seq&lt;Error&gt;&gt;)**

Map the alternative value (errors)

```csharp
Isotope<A> MapFail(Func<Seq<Error>, Seq<Error>> f)
```

#### Parameters

`f` [Func&lt;Seq&lt;Error&gt;, Seq&lt;Error&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Mapping function

#### Returns

[Isotope&lt;A&gt;](./isotope80.isotope-1.md)<br>
Mapped isotope computation

### **BiMap&lt;B&gt;(Func&lt;A, B&gt;, Func&lt;Seq&lt;Error&gt;, Seq&lt;Error&gt;&gt;)**

Map both sides of the isotope (success and failure)

```csharp
Isotope<B> BiMap<B>(Func<A, B> Succ, Func<Seq<Error>, Seq<Error>> Fail)
```

#### Type Parameters

`B`<br>

#### Parameters

`Succ` Func&lt;A, B&gt;<br>
Success mapping function

`Fail` [Func&lt;Seq&lt;Error&gt;, Seq&lt;Error&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Failure mapping function

#### Returns

Isotope&lt;B&gt;<br>
Mapped isotope computation

### **MapFail(Func&lt;Seq&lt;Error&gt;, Error&gt;)**

Map the alternative value (errors)

```csharp
Isotope<A> MapFail(Func<Seq<Error>, Error> f)
```

#### Parameters

`f` [Func&lt;Seq&lt;Error&gt;, Error&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Mapping function

#### Returns

[Isotope&lt;A&gt;](./isotope80.isotope-1.md)<br>
Mapped isotope computation

### **BiMap&lt;B&gt;(Func&lt;A, B&gt;, Func&lt;Seq&lt;Error&gt;, Error&gt;)**

Map both sides of the isotope (success and failure)

```csharp
Isotope<B> BiMap<B>(Func<A, B> Succ, Func<Seq<Error>, Error> Fail)
```

#### Type Parameters

`B`<br>

#### Parameters

`Succ` Func&lt;A, B&gt;<br>
Success mapping function

`Fail` [Func&lt;Seq&lt;Error&gt;, Error&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Failure mapping function

#### Returns

Isotope&lt;B&gt;<br>
Mapped isotope computation

---

[`< Back`](./)
