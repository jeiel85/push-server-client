# PushServer.Client SDK

WebSocket-based push notification client SDK for .NET.

## Features

- **WebSocket Communication** - Real-time bidirectional messaging
- **Auto-Reconnection** - Automatic reconnection with exponential backoff
- **JWT Authentication** - Token-based authentication support
- **Message Encryption** - AES-256 encryption support (coming soon)
- **Cross-Platform** - .NET 8+, .NET Standard 2.1

## Installation

```bash
dotnet add package PushServer.Client
```

## Quick Start

```csharp
using PushServer.Client;

// Create options
var options = new PushClientOptions
{
    ServerUrl = "ws://localhost:7000/",
    DeviceId = "my-device-001",
    CompanyId = "demo",
    ReceiverCode = "1002"
};

// Create client
using var client = new PushClientSDK(options);

// Subscribe to events
client.ConnectionChanged += (sender, connected) =>
    Console.WriteLine(connected ? "Connected!" : "Disconnected!");

client.MessageReceived += (sender, message) =>
    Console.WriteLine($"Received: {message}");

// Connect
client.Connect();

// Register with server
client.ConnectAndRegister();

// Send a message
client.SendMessage("1003", new { OrderId = 12345, Amount = 10000 });
```

## Authentication

### JWT Authentication

```csharp
var options = new PushClientOptions
{
    ServerUrl = "ws://localhost:7000/",
    UseJwtAuth = true,
    JwtToken = "your-jwt-token-here"
};
```

## License

MIT
