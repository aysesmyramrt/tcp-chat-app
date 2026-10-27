using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

public partial class ChatForm : Form
{
    private TcpClient client;
    private NetworkStream stream;

    public ChatForm() 
    {
        InitializeComponent();
    }

    private void btnConnect_Click(object sender, EventArgs e)
    {
        client = new TcpClient(txtHost.Text, int.Parse(txtPort.Text));
        stream = client.GetStream();
        Thread t = new Thread(ReceiveMessages);
        t.IsBackground = true;
        t.Start();
        btnConnect.Enabled = false;
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
        byte[] data = Encoding.UTF8.GetBytes(txtMessage.Text);
        stream.Write(data, 0, data.Length);
        txtMessage.Clear();
    }

    private void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Invoke((MethodInvoker)(() => lstMessages.Items.Add(msg)));
        }
    }
}