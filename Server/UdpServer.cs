using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MusicFFTServer.Server;

public class UdpServer
{
    private const int DefaultPort = 8987;
    
    private UdpClient udpClient;
    private IPEndPoint clientEndpoint;

    public UdpServer(int port, bool loopback = false)
    {
        this.udpClient = new UdpClient();

        IPAddress address = loopback ? IPAddress.Loopback : IPAddress.Broadcast;
        this.clientEndpoint = new IPEndPoint(address, port);
    }

    public void Close()
    {
        udpClient.Close();
    }
    
    public void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, clientEndpoint);
    }
}
