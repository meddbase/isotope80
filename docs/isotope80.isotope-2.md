[`< Back`](./)

---

# Isotope&lt;Env, A&gt;

Namespace: Isotope80

Isotope computation with an environment

```csharp
public struct Isotope<Env, A>
```

#### Type Parameters

`Env`<br>
Environment

`A`<br>
Bound value

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [Isotope&lt;Env, A&gt;](./isotope80.isotope-2.md)

## Constructors

### **Isotope(Func&lt;Env, IsotopeState, IsotopeState&lt;A&gt;&gt;)**

Ctor

```csharp
Isotope(Func<Env, IsotopeState, IsotopeState<A>> thunk)
```

#### Parameters

`thunk` Func&lt;Env, IsotopeState, IsotopeState&lt;A&gt;&gt;<br>
Thunk

## Methods

### **Run(Env, IsotopeSettings)**

Invoke the test computation

```csharp
ValueTuple<IsotopeState, A> Run(Env env, IsotopeSettings settings)
```

#### Parameters

`env` Env<br>
Environment

`settings` [IsotopeSettings](./isotope80.isotopesettings.md)<br>
Optional settings

#### Returns

ValueTuple&lt;IsotopeState, A&gt;<br>
Returning an optional error. 
 The computation succeeds if result.IsNone is true

### **RunAndThrowOnError(Env, IsotopeSettings)**

Invoke the test computation

```csharp
ValueTuple<IsotopeState, A> RunAndThrowOnError(Env env, IsotopeSettings settings)
```

#### Parameters

`env` Env<br>
Environment

`settings` [IsotopeSettings](./isotope80.isotopesettings.md)<br>
Optional settings

#### Returns

ValueTuple&lt;IsotopeState, A&gt;<br>
Returning an optional error. 
 The computation succeeds if result.IsNone is true

### **Pure(A)**

Lift the pure value into the monadic space

```csharp
Isotope<Env, A> Pure(A value)
```

#### Parameters

`value` A<br>

#### Returns

[Isotope&lt;Env, A&gt;](./isotope80.isotope-2.md)<br>

### **Fail(Error)**

Lift the error into the monadic space

```csharp
Isotope<Env, A> Fail(Error error)
```

#### Parameters

`error` Error<br>

#### Returns

[Isotope&lt;Env, A&gt;](./isotope80.isotope-2.md)<br>

### **Bind&lt;B&gt;(Func&lt;A, Isotope&lt;B&gt;&gt;)**

Monadic bind

```csharp
Isotope<Env, B> Bind<B>(Func<A, Isotope<B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, Isotope&lt;B&gt;&gt;<br>

#### Returns

Isotope&lt;Env, B&gt;<br>

### **Bind&lt;B&gt;(Func&lt;A, Isotope&lt;Env, B&gt;&gt;)**

Monadic bind

```csharp
Isotope<Env, B> Bind<B>(Func<A, Isotope<Env, B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, Isotope&lt;Env, B&gt;&gt;<br>

#### Returns

Isotope&lt;Env, B&gt;<br>

### **Bind&lt;B&gt;(Func&lt;A, IsotopeAsync&lt;B&gt;&gt;)**

Monadic bind

```csharp
IsotopeAsync<Env, B> Bind<B>(Func<A, IsotopeAsync<B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, IsotopeAsync&lt;B&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, B&gt;<br>

### **Bind&lt;B&gt;(Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;)**

Monadic bind

```csharp
IsotopeAsync<Env, B> Bind<B>(Func<A, IsotopeAsync<Env, B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, B&gt;<br>

### **SelectMany&lt;B&gt;(Func&lt;A, Isotope&lt;B&gt;&gt;)**

Monadic bind

```csharp
Isotope<Env, B> SelectMany<B>(Func<A, Isotope<B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, Isotope&lt;B&gt;&gt;<br>

#### Returns

Isotope&lt;Env, B&gt;<br>

### **SelectMany&lt;B&gt;(Func&lt;A, Isotope&lt;Env, B&gt;&gt;)**

Monadic bind

```csharp
Isotope<Env, B> SelectMany<B>(Func<A, Isotope<Env, B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, Isotope&lt;Env, B&gt;&gt;<br>

#### Returns

Isotope&lt;Env, B&gt;<br>

### **SelectMany&lt;B&gt;(Func&lt;A, IsotopeAsync&lt;B&gt;&gt;)**

Monadic bind

```csharp
IsotopeAsync<Env, B> SelectMany<B>(Func<A, IsotopeAsync<B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, IsotopeAsync&lt;B&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, B&gt;<br>

### **SelectMany&lt;B&gt;(Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;)**

Monadic bind

```csharp
IsotopeAsync<Env, B> SelectMany<B>(Func<A, IsotopeAsync<Env, B>> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;<br>

#### Returns

IsotopeAsync&lt;Env, B&gt;<br>

### **SelectMany&lt;B, C&gt;(Func&lt;A, Isotope&lt;B&gt;&gt;, Func&lt;A, B, C&gt;)**

Monadic bind

```csharp
Isotope<Env, C> SelectMany<B, C>(Func<A, Isotope<B>> bind, Func<A, B, C> project)
```

#### Type Parameters

`B`<br>

`C`<br>

#### Parameters

`bind` Func&lt;A, Isotope&lt;B&gt;&gt;<br>

`project` Func&lt;A, B, C&gt;<br>

#### Returns

Isotope&lt;Env, C&gt;<br>

### **SelectMany&lt;B, C&gt;(Func&lt;A, Isotope&lt;Env, B&gt;&gt;, Func&lt;A, B, C&gt;)**

Monadic bind

```csharp
Isotope<Env, C> SelectMany<B, C>(Func<A, Isotope<Env, B>> bind, Func<A, B, C> project)
```

#### Type Parameters

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
IsotopeAsync<Env, C> SelectMany<B, C>(Func<A, IsotopeAsync<B>> bind, Func<A, B, C> project)
```

#### Type Parameters

`B`<br>

`C`<br>

#### Parameters

`bind` Func&lt;A, IsotopeAsync&lt;B&gt;&gt;<br>

`project` Func&lt;A, B, C&gt;<br>

#### Returns

IsotopeAsync&lt;Env, C&gt;<br>

### **SelectMany&lt;B, C&gt;(Func&lt;A, IsotopeAsync&lt;Env, B&gt;&gt;, Func&lt;A, B, C&gt;)**

Monadic bind

```csharp
IsotopeAsync<Env, C> SelectMany<B, C>(Func<A, IsotopeAsync<Env, B>> bind, Func<A, B, C> project)
```

#### Type Parameters

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
Isotope<Env, B> Map<B>(Func<A, B> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, B&gt;<br>

#### Returns

Isotope&lt;Env, B&gt;<br>

### **Select&lt;B&gt;(Func&lt;A, B&gt;)**

Functor map

```csharp
Isotope<Env, B> Select<B>(Func<A, B> f)
```

#### Type Parameters

`B`<br>

#### Parameters

`f` Func&lt;A, B&gt;<br>

#### Returns

Isotope&lt;Env, B&gt;<br>

### **MapFail(Func&lt;Seq&lt;Error&gt;, Seq&lt;Error&gt;&gt;)**

Map the alternative value (errors)

```csharp
Isotope<Env, A> MapFail(Func<Seq<Error>, Seq<Error>> f)
```

#### Parameters

`f` [Func&lt;Seq&lt;Error&gt;, Seq&lt;Error&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Mapping function

#### Returns

[Isotope&lt;Env, A&gt;](./isotope80.isotope-2.md)<br>
Mapped isotope computation

### **BiMap&lt;B&gt;(Func&lt;A, B&gt;, Func&lt;Seq&lt;Error&gt;, Seq&lt;Error&gt;&gt;)**

Map both sides of the isotope (success and failure)

```csharp
Isotope<Env, B> BiMap<B>(Func<A, B> Succ, Func<Seq<Error>, Seq<Error>> Fail)
```

#### Type Parameters

`B`<br>

#### Parameters

`Succ` Func&lt;A, B&gt;<br>
Success mapping function

`Fail` [Func&lt;Seq&lt;Error&gt;, Seq&lt;Error&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Failure mapping function

#### Returns

Isotope&lt;Env, B&gt;<br>
Mapped isotope computation

### **MapFail(Func&lt;Seq&lt;Error&gt;, Error&gt;)**

Map the alternative value (errors)

```csharp
Isotope<Env, A> MapFail(Func<Seq<Error>, Error> f)
```

#### Parameters

`f` [Func&lt;Seq&lt;Error&gt;, Error&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Mapping function

#### Returns

[Isotope&lt;Env, A&gt;](./isotope80.isotope-2.md)<br>
Mapped isotope computation

### **BiMap&lt;B&gt;(Func&lt;A, B&gt;, Func&lt;Seq&lt;Error&gt;, Error&gt;)**

Map both sides of the isotope (success and failure)

```csharp
Isotope<Env, B> BiMap<B>(Func<A, B> Succ, Func<Seq<Error>, Error> Fail)
```

#### Type Parameters

`B`<br>

#### Parameters

`Succ` Func&lt;A, B&gt;<br>
Success mapping function

`Fail` [Func&lt;Seq&lt;Error&gt;, Error&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Failure mapping function

#### Returns

Isotope&lt;Env, B&gt;<br>
Mapped isotope computation

---

[`< Back`](./)
