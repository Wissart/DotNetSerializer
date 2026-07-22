# Binary Serialization

This page lists the base configuration parameters and explains how to use binary serialiation.

## Binary Converters

One of the fundamental elements of binary serialization is converter for "value" types. The base class for each binary converter is  the `BinaryConverter` class.

```csharp
    
    public abstract class BinaryConverter
    {
        /// <summary>Gets a value that indicates whether the converter can write.</summary>
        internal virtual bool CanWrite => true;

        /// <summary>Gets the type for converter registration.</summary>
        public abstract Type RegisteredType { get; }
        /// <summary>Gets the type that the converter converts.</summary>
        public abstract Type ConvertedType { get; }
        /// <summary>Gets a value indicating whether the conversion process is complete.</summary>
        public virtual bool IsComplete => false;


        public abstract object Read(BinaryReader reader, BinaryContext context);
        public abstract void Write(BinaryWriter writer, object value, BinaryContext context);
        public abstract bool TryGetSize(BinaryContext context, out int size);
    }
```

Also, provides generic version `BinaryConverter<T>`:

```csharp

    public abstract class BinaryConverter<T> : BinaryConverter
    {
        /// <summary>Gets the type for converter registration.</summary>
        public override Type RegisteredType => typeof(T);
        /// <summary>Gets the type that the converter converts.</summary>
        public override Type ConvertedType => typeof(T);
        

        public override object Read(BinaryReader reader, BinaryContext context)
        {
            return ReadValue(reader, context);
        }
        public abstract T ReadValue(BinaryReader reader, BinaryContext context);

        public override void Write(BinaryWriter writer, object value, BinaryContext context)
        {
            if (!(value is T))
                throw new InvalidCastException($"Wrong value type: {value.GetType()}. {ConvertedType} is required!");

            WriteValue(writer, (T)value, context);
        }
        public abstract void WriteValue(BinaryWriter writer, T value, BinaryContext context);

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }
    }

```

### Built-in Converters

Provides built-in converters for the following types:

* System types: `bool`, `char`, `float`, `double`, `decimal`, `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`
* `String`

For each "value" type that not have implementation, a converter is required. For classes, a converter is optional.

### Converter Registration

For a new converter registration, see the [Binary Options](#converter-registration).

### Additional Converters

Additionaly, provides following converters:

* `FactoryConverter`
* `InAttributeConverter`
* `LambdaConverter`
* `VersionableConverter`

### Summary Table of Converters

|Converter|Purpose|Supports Write?|When to Use|
|---------|-------|---------------|-----------|
|`BinaryConverter<T>`| Base for custom converters |	Yes | Creating any custom converter |
|`FactoryConverter<T>`| Creates instances only | No | For types that only need deserialization |
|`InAttributeConverter<T>`| Attribute-based serialization |	Yes | When using attributes to control serialization|
|`LambdaConverter<T>`| Simple logic for serialization |	Optional | For quick, simple serialization logic |
|`VersionableConverter<T>`| Types with version information |Yes | For versioned data structures |

#### FactoryConverter

`FactoryConverter` - the converter used when only instance creation is required (deserialization only).

```csharp

    public abstract class FactoryConverter<T> : BinaryConverter 
        where T : class
    {
        internal override sealed bool CanWrite => false;

        public override Type ConvertedType => typeof(T);
        public override Type RegisteredType => typeof(T);

        public override object Read(BinaryReader reader, BinaryContext context)
        {
            return Create(reader, context);
        }

        public abstract T Create(BinaryReader reader, BinaryContext context);

        public override void Write(BinaryWriter writer, object value, BinaryContext context)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }
    }

```

#### Example

```csharp

    public class DbConnectionFactoryConverter : FactoryConverter<DbConnection>
    {
        public override DbConnection Create(BinaryReader reader, BinaryContext context)
        {
            string connectionString = reader.ReadString();
            return new SqlConnection(connectionString);
        }
    }

```

**Note:** `FactoryConverter` does not support writing by design. Overriding `CanWrite` is not possible because it is sealed.

#### InAttributeConverter

`InAttributeConverter` - the converter used via attribute-based serialization.

```csharp

    public abstract class InAttributeConverter<T> : BinaryConverter<T>
    {
        public override Type RegisteredType => GetType();
        public override bool IsComplete => true;
    }

```

#### Example

```csharp

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class PersonConverter : InAttributeConverter<Person>
    {
        public override Person ReadValue(BinaryReader reader, BinaryContext context)
        {
            return new Person
            {
                Name = reader.ReadString(),
                Age = reader.ReadInt32()
            };
        }

        public override void WriteValue(BinaryWriter writer, Person value, BinaryContext context)
        {
            writer.Write(value.Name);
            writer.Write(value.Age);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }
    }

    // Usage
    [Converter(typeof(PersonConverter))]
    public Person Data { get; set; }

```

**Note:** To control write support, either implement `WriteValue` normally, or override `CanWrite` to return `false` and override `WriteValue` to throw `NotImplementedException` to make the converter read-only.

#### LambdaConverter

`LambdaConverter` - the converter used for simple types without commplex serialization logic.

```csharp

    public class LambdaConverter<T> : BinaryConverter<T>
    {
        public delegate bool TryGetSizeDelegate(BinaryContext metaData, out int size);

        private readonly Func<BinaryReader, BinaryContext, T> _readMethod;
        private readonly Action<BinaryWriter, T, BinaryContext> _writeMethod;
        private readonly TryGetSizeDelegate _tryGetSize;

        private readonly bool _canWrite;
        internal override bool CanWrite => _canWrite;

        public LambdaConverter(Func<BinaryReader, BinaryContext, T> readMethod, 
            Action<BinaryWriter, T, BinaryContext> writeMethod = null, 
            TryGetSizeDelegate tryGetSize = null)
        {
            _readMethod = readMethod;

            _canWrite = writeMethod == null;
            _writeMethod = writeMethod;

            _tryGetSize = tryGetSize;
            if (_tryGetSize == null)
                _tryGetSize = DefaultGetSize;
        }

        public override T ReadValue(BinaryReader reader, BinaryContext context)
        {
            return _readMethod.Invoke(reader, context);
        }

        public override void WriteValue(BinaryWriter writer, T value, BinaryContext context)
        {
            _writeMethod.Invoke(writer, value, context);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            return _tryGetSize(context, out size);
        }

        private bool DefaultGetSize(BinaryContext metaData, out int size)
        {
            size = -1;
            return false;
        }
    }

```

#### Example

```csharp

   // Read-write converter
    var pointConverter = new LambdaConverter<Point>(
        readMethod: (reader, context) => new Point(reader.ReadInt32(), reader.ReadInt32()),
        writeMethod: (writer, value, context) => { writer.Write(value.X); writer.Write(value.Y); },
        tryGetSize: (context, out int size) => { size = 8; return true; }
    );

    // Read-only converter (write method omitted)
    var readOnlyGuidConverter = new LambdaConverter<Guid>(
        readMethod: (reader, context) => new Guid(reader.ReadBytes(16))
        // writeMethod is null → read-only
    );

```

**Note:** Write support is controlled by whether you provide a `writeMethod` delegate. If `null`, the converter becomes read-only.

#### VersionableConverter

`VersionableConverter` - the converter for types that contain a member storing version information required for serialization.

```csharp

    public abstract class VersionableConverter<T> : BinaryConverter<T>
        where T : class
    {
        public override T ReadValue(BinaryReader reader, BinaryContext context)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            ReadVersion(reader, obj, context);
            ReadObject(reader, obj, context);
            return obj;
        }

        public abstract void ReadVersion(BinaryReader reader, T obj, BinaryContext context);
        public abstract void ReadObject(BinaryReader reader, T obj, BinaryContext context);

        public override void WriteValue(BinaryWriter writer, T value, BinaryContext context)
        {
            WriteVersion(writer, value, context);
            WriteObject(writer, value, context);
        }

        public abstract void WriteVersion(BinaryWriter writer, T value, BinaryContext context);
        public abstract void WriteObject(BinaryWriter writer, T value, BinaryContext context);
    }

```

#### Example

```csharp

    [Versionable]
    public class Customer
    {
        public uint Version { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CustomerConverter : VersionableConverter<Customer>
    {
        public override void ReadVersion(BinaryReader reader, Customer obj, BinaryContext context)
        {
            obj.Version = reader.ReadUInt32();
        }

        public override void ReadObject(BinaryReader reader, Customer obj, BinaryContext context)
        {
            obj.Id = reader.ReadInt32();
            obj.Name = reader.ReadString();
        }

        public override void WriteVersion(BinaryWriter writer, Customer value, BinaryContext context)
        {
            writer.Write(value.Version);
        }

        public override void WriteObject(BinaryWriter writer, Customer value, BinaryContext context)
        {
            writer.Write(value.Id);
            writer.Write(value.Name);
        }

        public override bool TryGetSize(BinaryContext context, out int size)
        {
            size = -1;
            return false;
        }
    }

```

**Note:** To make `VersionableConverter` read-only, override `CanWrite` to return `false` and override `WriteVersion` and `WriteObject` to throw `NotImplementedException`.

## Binary Options

Since `BinaryOptions` derives from `SerializerOptions`, it supports all the options outlined in the [Configuration](configuration.md/#serializer-options).

The `BinaryOptions` class offers the following options:

* Converter registration.
* Serialization mode choise.

### Converter Registration

`BinaryOptions` supports registration of converters derived from `BinaryConverter` only.

#### Example

```csharp

// Adds a new converter
options.Converters.Add<CustomConverter>();
// or 
options.Converters.Add(new CustomConverter());


// Sets a new converter (replaces any existing)
options.Converters.Set<CustomConverter>();
// or 
options.Converters.Set(new CustomConverter());



var list = new List<BinaryConverter>
{
    new CustomConverter1(),
    new CustomConverter2()
};

// Adds a collection of converters
options.Converters.Add(list);

// Sets a collection of converters (replaces any existing)
options.Converters.Set(list);

```

### Serialization Mode

The `BinaryOptions` provides the following serialization modes:

| Mode | Description |
|------|-------------|
| `Default` | Standart processing without caching. |
| `Cached` | Processing with caching enabled. |


The `Cached` mode is selected when the following settings are passed:

* `CachedProcessType` - specified the execution mode for cached processing
* `CachingTargets` - specified which targets are eligible for cahcing.

#### CachedProcessType

The `CachedProcessType` can take the following values: 

| Cached Process Type | Description |
|------|-------------|
| `Single` | Single-threaded cached processing. |
| `Parallel` | Multi-threaded cached processing. |

#### CachingTargets

The `CachingTargets` can take the following values: 

| Caching Targets | Description |
|------|-------------|
| `Collections` | Only collection types are cached. |
| `All` | All eligible types are cached. |

#### Examples

```csharp

// Default Mode
var options = new BinaryOptions();


// Cached Mode — Single-Threaded, Collections Only
var options = new BinaryOptions(CachedProcessType.Single, CachingTargets.Collections);


// Cached Mode — Parallel, All Types
var options = new BinaryOptions(CachedProcessType.Parallel, CachingTargets.All);

```


## Binary Serializer

`BinarySerializer` is main class for binary serialization.

### Methods Overview

|Method|Parameters|Description|
|------|----------|-----------|
| `Deserialize<T>` | `filePath` | Deserializes an object of the specified type from the file. |
| `Deserialize<T>` | `stream` | Deserializes an object of the specified type from the stream. |
| `Serialize<T>` | `filePath`, `obj` | Serializes the specified object to the file. |
| `Serialize<T>` | `stream`, `obj` | Serializes the specified object to the stream. |

### Examples

#### Serialize to File

```csharp

var serializer = new BinarySerializer();
var person = new Person { Id = 1, Name = "John Doe" };

serializer.Serialize("person.bin", person);

```

#### Deserialize from File

```csharp

var serializer = new BinarySerializer();
var person = serializer.Deserialize<Person>("person.bin");

Console.WriteLine($"Name: {person.Name}");

```

#### Serialize to Stream

```csharp

using var memoryStream = new MemoryStream();
var serializer = new BinarySerializer();
var person = new Person { Id = 1, Name = "John Doe" };

serializer.Serialize(memoryStream, person);

```

#### Deserialize from Stream

```csharp

using var memoryStream = new MemoryStream(fileData);
var serializer = new BinarySerializer();
var person = serializer.Deserialize<Person>(memoryStream);

```

#### Using Options

```csharp

var options = new BinaryOptions(CachedProcessType.Single, CachingTargets.All);
var serializer = new BinarySerializer(options);
serializer.Serialize("person.bin", person);

```

### Remarks

* The `BinarySerializer` uses `BinaryOptions` to control serialization behavior.
* Supports various processing modes, and custom converters.
* All built-in value types and common collections are supported out of the box.
* For custom types, you can register converters via `BinaryOptions.Converters`.