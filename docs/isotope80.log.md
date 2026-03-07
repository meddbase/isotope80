[`< Back`](./)

---

# Log

Namespace: Isotope80

Nested log

```csharp
public class Log
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Log](./isotope80.log.md)

## Fields

### **Indent**

Number of tabs to indent

```csharp
public int Indent;
```

### **Type**

Log type

```csharp
public LogType Type;
```

### **Message**

Node message

```csharp
public string Message;
```

### **Children**

Child messages

```csharp
public Seq<Log> Children;
```

### **Time**

The time the log was captured.

```csharp
public DateTime Time;
```

### **CallerMemberName**

The name of the method the log was called from.

```csharp
public string CallerMemberName;
```

### **CallerFilePath**

The full path of the file the log was called from.

```csharp
public string CallerFilePath;
```

### **CallerLineNumber**

The line number of the line the log was called from.

```csharp
public int CallerLineNumber;
```

### **Empty**

Empty log

```csharp
public static Log Empty;
```

## Methods

### **Add(Log)**

Add a log entry

```csharp
public ValueTuple<Log, Log> Add(Log log)
```

#### Parameters

`log` [Log](./isotope80.log.md)<br>

#### Returns

[ValueTuple&lt;Log, Log&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.valuetuple-2)<br>

### **Rebase(Int32)**

Set the new base indent for the log

```csharp
public Log Rebase(int indent)
```

#### Parameters

`indent` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Base indent

#### Returns

[Log](./isotope80.log.md)<br>
Rebased log

### **Context(String, DateTime, String, String, Int32)**

Add a message to the log

```csharp
public static Log Context(string ctx, DateTime time, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Parameters

`ctx` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Context

`time` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Log](./isotope80.log.md)<br>

### **Info(String, DateTime, String, String, Int32)**

Add a message to the log

```csharp
public static Log Info(string message, DateTime time, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Parameters

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Message to log

`time` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Log](./isotope80.log.md)<br>

### **Warning(String, DateTime, String, String, Int32)**

Add a message to the log

```csharp
public static Log Warning(string message, DateTime time, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Parameters

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Message to log

`time` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Log](./isotope80.log.md)<br>

### **Error(String, DateTime, String, String, Int32)**

Add a message to the log

```csharp
public static Log Error(string message, DateTime time, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Parameters

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Message to log

`time` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

#### Returns

[Log](./isotope80.log.md)<br>

### **ToString()**

ToString

```csharp
public string ToString()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **ToSeq()**

ToSeq

```csharp
public Seq<string> ToSeq()
```

#### Returns

Seq&lt;String&gt;<br>

---

[`< Back`](./)
