
using Greet;
using Grpc.Core;
using server;
using System;
using System.IO;

const int _port = 50051;

Server server = null;

try
{
    server = new Server()
    {
        Services = { GreetingSerivce.BindService(new GreetingServiceImpl())},
        Ports = { new ServerPort("localhost", _port, ServerCredentials.Insecure) }
    };

    server.Start();
    Console.WriteLine($"Server listening on port {_port}");
    Console.ReadKey();
}
catch (IOException ex)
{
    Console.WriteLine($"Server failed to start, message: {ex.Message}");
    throw;
}
finally
{
    if (server is not null)
        server.ShutdownAsync().Wait();

    Console.WriteLine($"Finish Server");
}

