# TCP Chat App

A small multi-client chat application in C# built directly on `System.Net.Sockets`. A TCP server accepts several clients and relays each incoming message to every other connected client. There are two clients: a console client and a Windows Forms client.

## How it works

- **Server** (`Server/ChatServer.cs`): `TcpListener` on a given port, bound to all interfaces. Each accepted client is added to a shared list and served on its own thread. Every chunk it receives (1024-byte buffer) is written to all other clients. Disconnected clients are removed from the list.
- **Console client** (`Client/ChatClient.cs`): connects to a host and port, receives messages on a background thread, and sends each line typed in the console. Type `/exit` to quit.
- **WinForms client** (`Client/ChatForm.cs`): a Connect button reads the host and port from text boxes. A background thread receives messages and adds them to a list box through `Invoke`, and a Send button sends the text box contents. Messages are UTF-8 encoded.
- **`ChatPacket.cs`**: a draft packet structure (message type, length, payload) for a typed protocol. The current code does not use it. Messages are sent as raw UTF-8 text.

## Tech stack

C#, .NET, Windows Forms, `System.Net.Sockets` (`TcpListener` / `TcpClient`), `System.Threading`

## Project structure

```
tcp-chat-app/
├── Server/ChatServer.cs     # multi-client relay server
├── Client/ChatClient.cs     # console client
├── Client/ChatForm.cs       # Windows Forms client (code-behind)
├── ChatPacket.cs            # draft packet structure (unused)
└── networkprojeect.sln      # Visual Studio solution file
```

## Running

The repository contains the source files only. The solution file lists no projects, and the `.csproj` files, the `Program.cs` entry points and the `ChatForm.Designer.cs` form layout are not included. To run it:

1. In Visual Studio, create a **Console App** for the server and add `ChatServer.cs`. Call it from `Main`:
   ```csharp
   new ChatServer().Start(5000);
   ```
2. Create a **Windows Forms App** for the client and add `ChatForm.cs`. In the designer, add `txtHost`, `txtPort`, `txtMessage`, `btnConnect`, `btnSend` and `lstMessages`, then wire `btnConnect_Click` and `btnSend_Click`. Alternatively, add `ChatClient.cs` to a console app and call `new ChatClient().Connect("127.0.0.1", 5000);`.
3. Start the server, then start two or more clients against the same host and port.

## Author

Ayşe Sümeyra Mert, Computer Engineering student
