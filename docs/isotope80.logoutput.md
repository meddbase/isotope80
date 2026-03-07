[`< Back`](./)

---

# LogOutput

Namespace: Isotope80

Log output

```csharp
public struct LogOutput
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [LogOutput](./isotope80.logoutput.md)<br>
Attributes [IsReadOnlyAttribute](./system.runtime.compilerservices.isreadonlyattribute.md)

## Fields

### **Message**

Log message

```csharp
public string Message;
```

### **Type**

Severity type

```csharp
public LogType Type;
```

### **Indent**

Indentation

```csharp
public int Indent;
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

## Constructors

### **LogOutput(String, LogType, Int32, DateTime, String, String, Int32)**

Ctor

```csharp
LogOutput(string message, LogType type, int indent, DateTime time, string callerMemberName, string callerFilePath, int callerLineNumber)
```

#### Parameters

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`type` [LogType](./isotope80.logtype.md)<br>

`indent` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

`time` [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)<br>

`callerMemberName` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerFilePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`callerLineNumber` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

## Methods

### **ToString()**

Tabbed format display

```csharp
string ToString()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **ToVerboseString(Int32)**

Tabbed format display including the file path, line number and time.

```csharp
string ToVerboseString(int expectedMaxMessageLength)
```

#### Parameters

`expectedMaxMessageLength` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of characters reserved for the message so the output looks lined up.

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

**Remarks:**

Message format: [TIME]: [INDENT][MESSAGE][GAP][FILE]:line [LINE]

---

[`< Back`](./)
