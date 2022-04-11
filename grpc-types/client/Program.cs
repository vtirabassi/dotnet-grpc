using Greet;
using Grpc.Core;

const string _target = "127.0.0.1:50051";

var channel = new Channel(_target, ChannelCredentials.Insecure);

channel.ConnectAsync().ContinueWith((task) =>
{
    if (task.Status == TaskStatus.RanToCompletion)
        Console.WriteLine("Client connected with success");
});

//var client = new DummyService.DummyServiceClient(channel);

var client = new GreetingService.GreetingServiceClient(channel);

var greeting = new Greeting()
{
    FirstName = "Vinicius",
    LastName = "Oliveira"
};

#region unary request
//var request = new GreetingRequest()
//{
//    Greeting = greeting
//};

//var response = client.Greet(request);

//Console.WriteLine(response.Result);
#endregion

#region server stream
//var request = new GreetingManyTimesRequest() { Greeting = greeting };
//var response = client.GreetManyTimes(request);

//while (await response.ResponseStream.MoveNext())
//{
//    Console.WriteLine(response.ResponseStream.Current.Result);
//    await Task.Delay(200);
//}
#endregion

#region client stream
//var request = new LongGreetingRequest() { Greeting = greeting };
//var stream = client.LongGreet();

//foreach (var item in Enumerable.Range(1, 10))
//{
//    await stream.RequestStream.WriteAsync(request);
//}

//await stream.RequestStream.CompleteAsync();

//var response = await stream.ResponseAsync;

//Console.WriteLine(response.Result);
#endregion

await CreateGreetEveryone(client);



channel.ShutdownAsync().Wait();

Console.ReadKey();

static async Task CreateGreetEveryone(GreetingService.GreetingServiceClient client)
{
    var stream = client.GreetEveryone();

    var responseTask = Task.Run(async () =>
    {
        while (await stream.ResponseStream.MoveNext())
        {
            Console.WriteLine("Received: " + stream.ResponseStream.Current.Result);
        }
    });

    Greeting[] greetings =
    {
        new Greeting() { FirstName = "Angelo", LastName = "XXX" },
        new Greeting() { FirstName = "Lorena", LastName = "XXX" },
        new Greeting() { FirstName = "Rodrigo", LastName = "XXX" }
    };

    foreach (var greeting in greetings)
    {
        Console.WriteLine("Sending request: " + greeting.ToString());
        await stream.RequestStream.WriteAsync(new GreetEveryoneRequest() { Greeting = greeting });
    }

    await stream.RequestStream.CompleteAsync();
    await responseTask;
}