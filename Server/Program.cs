using System.Net;
using TCP_Chat_SocketClass.Scripts;

ServerObject server = new ServerObject(new IPEndPoint(IPAddress.Any, 8888));
await server.ListenAsync();