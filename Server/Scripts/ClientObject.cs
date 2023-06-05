using System.Net.Sockets;

namespace TCP_Chat_SocketClass.Scripts;

public class ClientObject
{
    private readonly Socket _socket;
    private readonly ServerObject _server;
    public ClientObject(Socket clientSocket, ServerObject serverObject)
    {
        _socket = clientSocket;
        _server = serverObject;

        NetworkStream stream = new NetworkStream(clientSocket);
        Writer = new StreamWriter(stream);
        Reader = new StreamReader(stream);
    }

    public string Id { get; } = Guid.NewGuid().ToString();
    public StreamWriter Writer { get; }
    private StreamReader Reader { get; }

    public async Task ProcessAsync()
    {
        try
        {
            string? userName = await Reader.ReadLineAsync();
            string? message = $"{userName} вошел в чат";
            await _server.BroadcastMessageAsync(message, Id);
            Console.WriteLine(message);

            while (true)
            {
                try
                {
                    message = await Reader.ReadLineAsync();
                    if (message == null)
                        continue;

                    message = $"{userName}: {message}";
                    Console.WriteLine(message);
                    await _server.BroadcastMessageAsync(message, Id);
                }
                catch
                {
                    message = $"{userName} покинул чат";
                    Console.WriteLine(message);
                    await _server.BroadcastMessageAsync(message, Id);
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            _server.RemoveConnection(Id);
        }
    }
    
    public void Close()
    {
        Writer.Close();
        Reader.Close();
        _socket.Close();
    }
}