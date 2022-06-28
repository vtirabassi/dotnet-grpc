using Greeting;
using Grpc.Core;
using Server;

Grpc.Core.Server server = null;

const int Port = 50051;

try
{
    server = new Grpc.Core.Server()
    {
        Services = { GreetingService.BindService(new GreetingServiceImpl()) },
        Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
    };

    server.Start();
    Console.WriteLine("The server is listening on the port : " + Port);
    Console.ReadKey();
}
catch (IOException e)
{
    Console.WriteLine("The server failed to start : " + e.Message);
    throw;
}
finally
{
    if (server != null)
        server.ShutdownAsync().Wait();
}