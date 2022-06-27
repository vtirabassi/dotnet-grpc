using Grpc.Core;
using Server;
using Sqrt;

const int Port = 50052;

Grpc.Core.Server server = null;

try
{
    server = new Grpc.Core.Server()
    {
        Services = { SqrtService.BindService(new SqrtServiceImpl()) },
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