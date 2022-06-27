using Grpc.Core;
using Sqrt;
using static Sqrt.SqrtService;

namespace Server
{
    public class SqrtServiceImpl : SqrtServiceBase
    {
        public override async Task<SqrtResponse> sqrt(SqrtRequest request, ServerCallContext context)
        {
            var number = request.Number;

            if (number >= 0)
                return new SqrtResponse() { SquareRoot = Math.Sqrt(number) };
            else
                throw new RpcException(status: new Status(
                    statusCode: StatusCode.InvalidArgument, "number is less 0"));
        }
    }
}
