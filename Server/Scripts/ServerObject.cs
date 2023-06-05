using System.Net;
using System.Net.Sockets;

namespace TCP_Chat_SocketClass.Scripts;

public class ServerObject
{
    private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private readonly List<ClientObject> _clients = new List<ClientObject>();

    public ServerObject(IPEndPoint ipEndPoint)
    {
        _socket.Bind(ipEndPoint);
    }

    protected internal void RemoveConnection(string id)
    {
        ClientObject? client = _clients.FirstOrDefault(client => client.Id == id);

        if (client != null)
            _clients.Remove(client);

        client?.Close();
    }

    public async Task ListenAsync()
    {
        try
        {
            _socket.Listen();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                Socket clientSocket = await _socket.AcceptAsync();
                ClientObject client = new ClientObject(clientSocket, this);
                _clients.Add(client);
                Task.Run(client.ProcessAsync);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            Disconnect();
        }
    }
    
    public async Task BroadcastMessageAsync(string message, string id)
    {
        foreach (ClientObject client in _clients)
        {
            if (client.Id == id)
                continue;

            await client.Writer.WriteLineAsync(message);
            await client.Writer.FlushAsync();
        }
    }
    
    private void Disconnect()
    {
        foreach (ClientObject client in _clients)
        {
            client.Close();
        }
        
        _socket.Close();
    }
}