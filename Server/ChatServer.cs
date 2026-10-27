using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ChatServer
{
    private TcpListener listener;
    private List<TcpClient> clients = new List<TcpClient>();
    private bool running = false;

    public void Start(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        running = true;
        Console.WriteLine("Sunucu baþlatýldý. Port: " + port);

        while (running)
        {
            var client = listener.AcceptTcpClient();
            lock (clients) clients.Add(client);
            Console.WriteLine("Yeni istemci baðlandý.");
            Thread t = new Thread(() => HandleClient(client));
            t.Start();
        }
    }

    private void HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            // Mesajý tüm istemcilere ilet
            lock (clients)
            {
                foreach (var c in clients)
                {
                    if (c != client)
                    {
                        c.GetStream().Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
        lock (clients) clients.Remove(client);
        client.Close();
        Console.WriteLine("Ýstemci ayrýldý.");
    }
}