using Greeting;
using Grpc.Core;

namespace Server;

public class GreetingServiceImpl : GreetingService.GreetingServiceBase
{
    public override async Task<Greetingresponse> greet_with_deadline(GreetingRequest request, ServerCallContext context)
    {
        await Task.Delay(300);

        return new Greetingresponse() { Result = "Hello " + request.Name };
    }
}