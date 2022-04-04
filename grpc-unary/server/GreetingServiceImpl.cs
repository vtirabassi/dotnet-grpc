using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Greet.GreetingSerivce;

namespace server
{
    public class GreetingServiceImpl : GreetingSerivceBase
    {
        public override Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
        {
            var result = string.Format("The complete name is {0} {1}", request.Greeting.FirstName, request.Greeting.LastName);
            return Task.FromResult(new GreetingResponse() { Result = result });
        }
    }
}
