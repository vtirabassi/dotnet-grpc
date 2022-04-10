using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Greet.GreetingService;

namespace server
{
    public class GreetingServiceImpl : GreetingServiceBase
    {
        public override Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
        {
            var result = string.Format("The complete name is {0} {1}", request.Greeting.FirstName, request.Greeting.LastName);
            return Task.FromResult(new GreetingResponse() { Result = result });
        }

        public override async Task GreetManyTimes(GreetingManyTimesRequest request, IServerStreamWriter<GreetingManyTimesResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("Server received the request");
            Console.WriteLine(request.ToString());

            var result = string.Format("The complete name is {0} {1}", request.Greeting.FirstName, request.Greeting.LastName);

            foreach (int i in Enumerable.Range(1, 10))
            {
                await responseStream.WriteAsync(new GreetingManyTimesResponse() { Result = result });
            }
        }

        public override async Task<LongGreetingResponse> LongGreet(IAsyncStreamReader<LongGreetingRequest> requestStream, ServerCallContext context)
        {
            string result = "";

            while (await requestStream.MoveNext())
            {
                result += String.Format("Hello {0} {1} {2}",
                    requestStream.Current.Greeting.FirstName,
                    requestStream.Current.Greeting.LastName,
                    Environment.NewLine);
            }

            return new LongGreetingResponse() { Result = result };
        }
    }
}
