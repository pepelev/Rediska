# Rediska

Rediska is [Redis](https://redis.io/) client for .NET with a focus on flexibility and extensibility.

![Rediska](logo.svg)

![](https://github.com/pepelev/Rediska/workflows/.NET%20Core%20build%20and%20test/badge.svg)
![](https://img.shields.io/nuget/v/Rediska.svg?style=flat-square&label=nuget)

## Features

- [Keys](https://redis.io/commands#generic), [Strings](https://redis.io/commands#string), [Transactions](https://redis.io/commands#transactions), [Sets](https://redis.io/commands#set), [Server](https://redis.io/commands#server), [Lists](https://redis.io/commands#list), [HyperLogLog](https://redis.io/commands#hyperloglog), [Hashes](https://redis.io/commands#hash) and [Streams](https://redis.io/commands#stream) commands
- Async API
- Easy way to handle command response differently
- Easy way to write custom commands, e.g. for [Redis modules](https://redis.io/modules)
- RESP 2 support - allows to build a proxy

### Plans (not implemented yet)

- Full set of [commands](https://redis.io/commands)
- [Pipelining](https://redis.io/topics/pipelining)
- [Pub/Sub](https://redis.io/topics/pubsub) 
- [Redis Cluster](https://redis.io/topics/cluster-tutorial)
- [Redis Client Side Caching](https://redis.io/topics/client-side-caching)

## Getting started

Install [NuGet package](https://www.nuget.org/packages/Rediska/) using Package Manager

```
Install-Package Rediska -Version 0.1.0
```

### Show me the code

The types are in the following namespaces
```csharp
using Rediska;
using Rediska.Commands.Strings;
```

```csharp
var factory = new SimpleConnectionFactory();
var endPoint = new IPEndPoint(IPAddress.Loopback, 6379);
using (var connectionResource = await factory.CreateAsync(endPoint))
{
    var connection = connectionResource.Value;

    var set = new SET("users:12:score", "50");
    await connection.ExecuteAsync(set);
    var incr = new INCR("users:12:score");
    await connection.ExecuteAsync(incr);
    var get = new GET("users:12:score");
    var userScore = await connection.ExecuteAsync(get); // 51
}
```

To interact with Redis server you need a [`Connection`](/Rediska/Connection.cs).
You can get one using a factory. In Rediska, each command is represented by a separate
class, so to run a command you need to instantiate it and then
execute using the connection.

### Resource management

The `Connection` class does not implement `IDisposable` interface, instead
`factory.CreateAsync()` returns a [`Resource<Connection>`](/Rediska/Utils/Resource.cs)
that does. This resource tracks underlying `TcpClient`. So you need to dispose the
resource when the connection is no longer needed.

This approach clearly defines who owns the resource.

### A note about the type of `userScore`

The [GET command](https://redis.io/commands/get) replies with a
[bulk string](https://redis.io/topics/data-types#strings) - safe binary string.
This kind of reply is represented with the class
[BulkString](/Rediska/Protocol/BulkString.cs). If you are sure that reply contains
a number you can get its value as follows
```csharp
var number = long.Parse(userScore.ToString())
```

## Project status

The project is currently under active development.
