# Configuration

This page lists the base configuration parameters to prepare data for serialization and set serializer options for execution.

## Attributes

### Versionable

The `VersionableAttribute` marks a class for versioned serialization. It defines:

* The property name that stores the version (Version by default).
* Version restrictions — which versions are allowed during deserialization (none by default).

```csharp

// Default — property "Version", no restrictions
[Versionable]
public class DefaultExample 
{ 
    public uint Version { get; set; } 
}

// Maximum version only
[Versionable(5)]
public class MaxOnlyExample 
{ 
    public uint Version { get; set; } 
}

// Exact versions only
[Versionable(new uint[] { 1, 2, 3 })]
public class ExactExample 
{ 
    public uint Version { get; set; } 
}

// Custom property name and maximum version 
[Versionable("DataVersion", 10)]
public class CustomPropExample 
{ 
    public uint DataVersion { get; set; } 
}

```

#### Constructor Overloads:

The attribute provides multiple constructors for flexible configuration:

|Constructor|Description|
|-----------|-----------|
| `[Versionable]` | Uses default settings: property name = "Version", no version restrictions. |
| `[Versionable(uint maxVersion)]` | Allows versions up to maxVersion (inclusive). |
| `[Versionable(uint[] exactVersions)]` | Allows only the versions listed in exactVersions. |
| `[Versionable(uint maxVersion, uint[] excludedVersions)]` | Allows versions up to maxVersion, except those in excludedVersions. |
| `[Versionable(string versionPropertyName, uint maxVersion)]` | Custom property name + maximum allowed version. |
| `[Versionable(string versionPropertyName, uint[] exactVersions)]` | Custom property name + exact allowed versions. |
| `[Versionable(string versionPropertyName, uint maxVersion, uint[] excludedVersions)]` | Custom property name + maximum allowed version + excluded versions. |

#### Parameters:

|Parameter|Type|Description|
|---------|----|-----------|
| `versionPropertyName` | `string` | Name of the property that stores the version. |
| `maxVersion` | `uint` | Maximum allowed version (inclusive). |
| `exactVersions` | `uint[]` | List of versions explicitly supported. |
| `excludedVersions` | `uint[]` | 	List of versions to exclude from support. |


### RequireVersion

The `RequireVersionAttribute` marks a member to define the version bounds for serialization.

```csharp

public class MinOnlyExample
{
    // Minimum version only — maximum defaults to uint.MaxValue
    [RequireVersion(4)]
    public int ExampleProperty { get; set; }
}

public class MinMaxExample
{
    // Minimum and maximum version
    [RequireVersion(4, 15)]
    public int ExampleProperty { get; set; }
}

```

#### Constructor:

`RequireVersionAttribute(uint min, uint max = uint.MaxValue)`

|Parameter|Type|Description|
|---------|----|-----------|
| `min` | `uint` | The minimum allowed version (inclusive). |
| `max` | `uint` | The maximum allowed version (inclusive). Optional; defaults to uint.MaxValue. |


### Collection

The `CollectionAttribute` marks a memeber to define the strategy used for determining a collection size.

```csharp

public class Prefix1DExample
{
    // Default - SizeType is set to Prefix
    [Collection]
    public int[] CollectionProperty { get; set; }
}

public class Prefix3DExample
{
    // Default - SizeType is set to Prefix
    [Collection]
    public int[,,] CollectionProperty { get; set; }
}

public class Fixed1DExample
{
    // SizeType is set to Fixed, and the size is set to 10 for the 1-dimensional collection.
    [Collection(10)]
    public int[] CollectionProperty { get; set; }
}

public class Fixed3DExample
{
    // SizeType is set to Fixed, and the sizes are set to [10, 12, 15] for the 3-dimensional collection.
    [Collection(10, 12, 15)]
    public int[,,] CollectionProperty { get; set; }
}

```

#### Constructor Overloads:

The attribute provides multiple constructors for flexible configuration:

|Constructor|Description|
|-----------|-----------|
| `[Collection]` | Uses the default strategy: SizeType is set to "Prefix," meaning the collection size is read from or written to the data dynamically. |
| `[Collection(params int[] shape)]` | Uses the "Fixed" strategy, where the collection size is statically defined by the provided shape values. |

#### Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `shape` | `params int[]` | One or more integers specifying the fixed size for each dimension of the collection. |


### StringFormat

The `StringFormatAttribute` marks a member to define the encoding and the strategy used for determining a string size.

```csharp

public class DefaultExample
{
    // Default - Encoding is set to "UTF-8", SizeType is set to "Prefix", Size is set to 0
    [StringFormat]
    public string ExampleProperty { get; set; }
}

public class SizeTypeOnlyExample
{
    // SizeType is set to "SignEnd"
    [StringFormat(StringSizeType.SignEnd)]
    public string ExampleProperty { get; set; }
}

public class SizeOnlyExample
{
    // SizeType set to "Fixed" and Size is set to 32
    [StringFormat(32)]
    public string ExampleProperty { get; set; }
}

public class AllExample
{
    // Encoding is set to "ASCII", SizeType is set to "Fixed", Size is set to 32
    [StringFormat("ASCII", StringSizeType.Fixed, 32)]
    public string ExampleProperty { get; set; }
}

```

#### Constructor Overloads:

The attribute provides multiple constructors for flexible configuration:

|Constructor|Description|
|-----------|-----------|
| `[StringFormat]` | Uses default settings: encoding = `"UTF-8"`, SizeType = `Prefix`, size = `0`.|
| `[StringFormat(int size)]` | Uses the Fixed size strategy with the specified size. Encoding defaults to `"UTF-8"` |
| `[StringFormat(StringSizeType sizeType)]` | Uses the specified SizeType strategy. Encoding defaults to `"UTF-8"`, and size defaults to `0` |
| `[StringFormat(string encoding = "UTF-8", StringSizeType sizeType = StringSizeType.Prefix, int size = 0)]` | Fully configurable: specifies the encoding, size strategy, and fixed size value. |


#### Parameters:

|Parameter|Type|Description|
|---------|----|-----------|
| `encoding` | `string` | The encoding to use for the string (e.g., `"UTF-8"`, `"ASCII"`). Defaults to `"UTF-8"`. |
| `sizeType` | `StringSizeType` | The strategy for determining the string size. Defaults to `Prefix`. |
| `size` | `int` | The fixed size of the string. Only used when `sizeType` is `Fixed`. Defaults to `0` |

#### Remarks

- The encoding parameter supports any valid `.NET` encoding name (e.g., `"UTF-8"`, `"ASCII"`, `"Unicode"`).
- The `SizeType` enum defines how the string size is determined:
  - `Prefix` — size is written before the string data (default).
  - `Fixed` — string is padded or truncated to a fixed size.
  - `SignEnd` — string is terminated by a sign or sentinel value.
- The `size` parameter is only used when `SizeType` is set to `Fixed`.


### Converter

The `ConverterAttribute` marks member to set up a converter that serializes the value.

```csharp

public class ConverterExample
{
    // Sets up converter type
    [Converter(typeof(CustomConverter))]
    public int ConvertableProperty { get; set; }
}

```

#### Constructor:

|Constructor|Description|
|-----------|-----------|
| `[Converter(Type converterType)]` | Specifies the type of the custom converter to use for serialization. |

|Parameter|Type|Description|
|---------|----|-----------|
| `converterType` | `Type` | The type of the custom converter that implements the serialization logic. |


## Collection Handlers

To manipulate collections during serialization, special handler are used. These are classes derived from `CollectionHandler`. If a handler is not implemented for a specific collection type, it is possible to create a sutom collection handler.

### Built-in Handlers

Provides built-in handlers for common collection types:

|Handler|Supported Types|
|-----------|-----------|
| `ArrayHandler` | Single-dimensional and multi-dimensional arrays (`T[]`, `T[,,]`) |
| `ListHandler` | `List<T>` |


### Creating a Custom Collection Handler

```csharp

public class CustomCollectionHandler : CollectionHandler
{
    public override Type CollectionType => typeof(CustomCollection);

    public override object CreateCollectionWithItems(Type[] elementTypes, object[] items)
    {
        // Implementation
    }

    // Implement all other abstract methods...
}

```

For a new collection handler registration, see the [Serializer Options](#serializer-options).

### Methods Overview

|Method|Parameters|Description|
|------|----------|-----------|
| `CreateCollectionWithItems` | `elementTypes`, `items`| 	Creates a populated collection. |
| `CreateCollectionWithItems` | `elementTypes`, `items`, `sizes` | Creates a populated multidimensional collection. |
| `CreateCollection` | `elementTypes`, `sizes` | Creates an empty collection with specified dimensions. |
| `GetRank` | `declaredCollectionType` | Returns the number of dimensions. |
| `GetElementType` | `elementTypes` | Returns the element type of the collection. |
| `GetCapacity` | `collection` | Returns capacity per dimension. |
| `GetItemsCount` | `collection` | Returns item count per dimension. |
| `AddItem` | `collection`, `item`, `indices` | Adds an item at the specified indices. |


## Serializer Options

To define the base options for serialization, parameters are used in the abstract `SerializerOptions` class. It defines the following:

* Default attributes — applied to members that require the attribute but are not explicitly marked.
* Collection handlers — used to manipulate collection data during serialization.

### Default Attributes

Allows setting up default attributes for any attributes derived from the following types:

* `StringFormatAttribute`
* `CollectionAttribute`

#### Registering a Default Attribute

```csharp

/// Replaces the "StringFormatAttribute" with default parameters by a new instance with custom parameters.
options.DefaultAttributes.Set(new StringFormatAttribute("ASCII", StringSizeType.SignEnd, 0));

```

### Collection Handlers

Provides the abilty to add custom collection handlers or to replace built-in handlers. 

#### Registering a Collection Handlers

```csharp

// Adds the custom collection handler for a custom collection.
options.CollectionHandlers.Add<CustomCollectionHandler>();
// or
options.CollectionHandlers.Add(new CustomCollectionHandler());


/// Replaces the existing "ArrayHandler" with a custom "NewArrayHandler".
options.CollectionHandlers.Set(new NewArrayHandler());

```