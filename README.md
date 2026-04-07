# CodeChavez.EventBus.Abstractions

A comprehensive abstraction layer for event bus implementations, providing core interfaces and models for building scalable event-driven architectures in .NET 10.

## Features

- **Producer/Consumer Abstractions**: Core interfaces for publishing and consuming events
- **Configuration Options**: Flexible interfaces for configuring consumer and producer behavior
- **Event Models**: Base classes for domain events and integration events
- **MediatR Integration**: Support for MediatR notifications and handlers
- **Cross-Platform Support**: Built for modern .NET

## Core Components

### Interfaces

#### `IConsumerProducerOptions`
Configuration interface for consumer and producer settings.
```csharp
public interface IConsumerProducerOptions
{
    List<string> Servers { get; set; }
    string Group { get; set; }
    string Client { get; set; }
}
```

#### `IConsumerMultiplexOptions`
Specialized configuration for multiplexed consumer scenarios.

### Installation

```bash
dotnet add package CodeChavez.EventBus.Abstractions
```

## Configuration

Configure consumer and producer options by implementing `IConsumerProducerOptions`:

```csharp
public class EventBusOptions : IConsumerProducerOptions
{
    public List<string> Servers { get; set; } = new() { "localhost:9092" };
    public string Group { get; set; } = "my-consumer-group";
    public string Client { get; set; } = "my-client";
}
```

## Target Framework

- **.NET 10** and higher

## License

This project is part of the CodeChavez community projects.

## Repository

[GitHub - CodeChavez.EventBus](https://github.com/codechavez/CodeChavez.EventBus)
