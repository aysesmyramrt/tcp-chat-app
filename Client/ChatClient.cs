using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ChatClient
{
    private TcpClient client;
    private NetworkStream stream;

    public void Connect(string host, int port)
    {
        client = new TcpClient(host, port);
        stream = client.GetStream();
        Console.WriteLine("Sunucuya baðlanýldý.");

        Thread t = new Thread(ReceiveMessages);
        t.Start();

        while (true)
        {
            string msg = Console.ReadLine();
            if (msg == "/exit") break;
            byte[] data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }
        client.Close();
    }

    private void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Gelen: " + msg);
        }
    }
}