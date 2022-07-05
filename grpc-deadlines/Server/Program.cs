using Greeting;
using Grpc.Core;
using Grpc.Reflection;
using Grpc.Reflection.V1Alpha;
using Server;

Grpc.Core.Server server = null;

const int Port = 50051;

try
{
    var keypair = new KeyCertificatePair(
        File.ReadAllText("ssl/server.crt"),
        File.ReadAllText("ssl/server.key"));
    
    var cacert = File.ReadAllText("ssl/ca.crt");

    var reflectionImpl = new ReflectionServiceImpl(GreetingService.Descriptor, ServerReflection.Descriptor);
    
    var credentials = new SslServerCredentials(new List<KeyCertificatePair>()
    {
        keypair
    }, cacert, true);
    
    server = new Grpc.Core.Server()
    {
        Services =
        {
            GreetingService.BindService(new GreetingServiceImpl()),
            ServerReflection.BindService(reflectionImpl) 
        },
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