using Greeting;
using Grpc.Core;

var clientcert = File.ReadAllText("ssl/client.crt");
var clientkey = File.ReadAllText("ssl/client.key");
var cacert = File.ReadAllText("ssl/ca.crt");
var channelCredentials = new SslCredentials(cacert,
    new KeyCertificatePair(clientcert, clientkey));

var channel = new Channel("localhost", 50051, channelCredentials);

await channel.ConnectAsync().ContinueWith((task) =>
{
    if (task.Status == TaskStatus.RanToCompletion)
        Console.WriteLine("The client connected successfully");
});

var client = new GreetingService.GreetingServiceClient(channel);

try
{
    var response = client.greet_with_deadline(new GreetingRequest() { Name = "John" },
        deadline: DateTime.UtcNow.AddSeconds(1));

    Console.WriteLine(response.Result);
}
catch (RpcException e) when (e.StatusCode == StatusCode.DeadlineExceeded)
{
    Console.WriteLine("Error : " + e.Status.Detail);
}

channel.ShutdownAsync().Wait();
Console.ReadKey();