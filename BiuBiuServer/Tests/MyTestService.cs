using System;
using System.Threading;
using BiuBiuShare.ImInfos;
using BiuBiuShare.Tests;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Authentication;

namespace BiuBiuServer.Tests
{
    public class MyTestService : ServiceBase<IMyTestService>, IMyTestService
    {
        // `UnaryResult<T>` allows the method to be treated as `async` method.
        public async UnaryResult<int> SumAsync(int x, int y)
        {
            Console.WriteLine($"Received:{x}, {y}");
            return x + y;
        }

        [Authorize(Roles = new[] { "Administrators" })]
        public async UnaryResult<int> SumAsync1(int x, int y)
        {
            Console.WriteLine($"Received:{x}, {y}");
            return x + y;
        }

        public async UnaryResult<int> SumAsync2(int x, int y)
        {
            Console.WriteLine($"Received:{x}, {y}");
            return x + y;
        }
    }
}