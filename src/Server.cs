using System.Net;
using System.Net.Sockets;
using System.Text;

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

// Uncomment this block to pass the first stage
TcpListener server = new TcpListener(IPAddress.Any, 4221);
server.Start();

using TcpClient client = await server.AcceptTcpClientAsync();
await using NetworkStream stream = client.GetStream();

byte[] requestBuffer = new byte[1024];
int bytesRead = await stream.ReadAsync(requestBuffer, 0, requestBuffer.Length);
string request = 
    System.Text.Encoding.UTF8.GetString(requestBuffer, 0, bytesRead);
Console.WriteLine($"Received request: {request}");

var requestLines = request.Split("\r\n");
var requests = requestLines[0].Split("/");

// Prepare and send response
string response = requests[1].Equals(" HTTP" ) ? "HTTP/1.1 200 OK\r\n\r\n" : "HTTP/1.1 404 Not Found\r\n\r\n";

byte[] responseBuffer = System.Text.Encoding.UTF8.GetBytes(response);
stream.Write(responseBuffer, 0, responseBuffer.Length);