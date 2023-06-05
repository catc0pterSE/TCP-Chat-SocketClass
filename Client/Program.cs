using System.Net.Sockets;

string host = "127.0.0.1";
int port = 8888;
using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
Console.Write("Введите свое имя: ");
string? userName = Console.ReadLine();
Console.WriteLine($"Добро пожаловать, {userName}");
NetworkStream? Stream = null;
StreamReader? Reader = null;
StreamWriter? Writer = null;

try
{
    socket.Connect(host, port);
    Stream = new NetworkStream(socket);
    Reader = new StreamReader(Stream);
    Writer = new StreamWriter(Stream);
    Task.Run(() => ReceiveMessageAsync(Reader));
    await SendMessageAsync(Writer);
}
catch (Exception exception)
{
    Console.WriteLine(exception.Message);
}

Reader?.Close();
Writer?.Close();

async Task SendMessageAsync(StreamWriter writer)
{
    await writer.WriteLineAsync(userName);
    await writer.FlushAsync();
    Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

    while (true)
    {
        string? message = Console.ReadLine();
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();
    }
}

async Task ReceiveMessageAsync(StreamReader reader)
{
    while (true)
    {
        try
        {
            string? message = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(message)) continue;
            Console.WriteLine(message);
        }
        catch
        {
            break;
        }
    }
}