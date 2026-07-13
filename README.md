# DotNetSerialize
![.NET Framework 4.8](https://img.shields.io/badge/.NET_Framework-4.8-blue)

DotNetSerializer - provides serialization for data structures and primitive values. For data structures, serialization applies only to `properties`. It offers flexible settings and configuration, including support for data versioning, and works with a wide range of data types.

### Support types:

* System types: `bool`, `char`, `float`, `double`, `decimal`, `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`.
* `String` is configurable.
* Collections: `Array`, `List<>`, for other types, it is possible to create a custom collection handler.
* `Struct` is require a custom converter.
* For other types, it is possible to create a custom converter.


## Getting the Code

```bash
git clone https://github.com/wissart/dotnetserializer.git

```


## Requirements

* .NET Framework 4.8 or higher

* Visual Studio 2019+ (for building from source)


## Intergration

### Add the source project

1. Clone the repository: git clone ...

2. In Visual Studio: right-click your solution → Add → Existing Project

3. Select the [DotNetSerializer.Base.csproj] and [DotNetSerializer.Binary.csproj] files

4. In your main project: right-click Dependencies → Add Reference → Projects → select [DotNetSerializer.Base] and [DotNetSerializer.Binary] 


## Quick Start

``` csharp
using DotNetSerializer.Binary;

using var memoryStream = new MemoryStream();
var serializer = new BinarySerializer();
var person = new Person { Id = 1, Name = "John Doe" };

serializer.Serialize(memoryStream, person);

```
For more examples, refer to the official [documentation](#documentation).

## Documentation

[Configuration](https://github.com/wissart/DotNetSerializer/blob/main/docs/serialiaztion/configuration.md)
[Binary Serializer](https://github.com/wissart/DotNetSerializer/blob/main/docs/serialiaztion/binary_serializer.md)


## License

BSD 3-Clause License - see [LICENSE](LICENSE) file for details.