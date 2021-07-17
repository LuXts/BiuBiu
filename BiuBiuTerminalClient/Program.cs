using System;
using System.Threading.Tasks;
using BiuBiuShare;
using BiuBiuShare.Tests;
using Grpc.Core;
using Grpc.Net.Client;
using LitJWT;
using LitJWT.Algorithms;
using MagicOnion.Client;
using Microsoft.VisualBasic;

namespace BiuBiuTerminalClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var channel2 = GrpcChannel.ForAddress("https://localhost:5001");

            var client2
                = MagicOnion.Client.MagicOnionClient.Create<IMyTestService>(
                    channel2
                    , new[]
                    {
                        new AdminWithAuthenticationFilter("18578967136", "123456789"
                            , channel2)
                    });
            var n2 = await client2.SumAsync2(9, 4);
            Console.WriteLine(n2);
        }
    }

    internal class WithAuthenticationFilter : IClientFilter
    {
        private readonly string _signInId;
        private readonly string _password;
        private readonly GrpcChannel _channel;

        public WithAuthenticationFilter(string signInId, string password
            , GrpcChannel channel)
        {
            _signInId = signInId ??
                        throw new ArgumentNullException(nameof(signInId));
            _password = password ??
                        throw new ArgumentNullException(nameof(password));
            _channel = channel ??
                       throw new ArgumentNullException(nameof(channel));
        }

        public async ValueTask<ResponseContext> SendAsync(RequestContext context
            , Func<RequestContext, ValueTask<ResponseContext>> next)
        {
            if (AuthenticationTokenStorage.Current.IsExpired)
            {
                Console.WriteLine(
                    $@"[WithAuthenticationFilter/IAccountService.SignInAsync] Try signing in as '{_signInId}'... ({(AuthenticationTokenStorage.Current.Token == null ? "FirstTime" : "RefreshToken")})");

                var client = MagicOnionClient.Create<IAccountService>(_channel);
                var authResult
                    = await client.CommonSignInAsync(_signInId, _password);
                if (!authResult.Success)
                {
                    throw new Exception("Failed to sign-in on the server.");
                }

                Console.WriteLine(authResult.Token);
                Console.WriteLine(
                    $@"[WithAuthenticationFilter/IAccountService.SignInAsync] User authenticated as {authResult.DisplayName} (UserId:{authResult.UserId})");

                AuthenticationTokenStorage.Current.Update(authResult.Token
                    , authResult
                        .Expiration); // NOTE: You can also read the token expiration date from JWT.

                context.CallOptions.Headers.Remove(
                    new Metadata.Entry("auth-token-bin", Array.Empty<byte>()));
            }

            if (!context.CallOptions.Headers.Contains(
                new Metadata.Entry("auth-token-bin", Array.Empty<byte>())))
            {
                context.CallOptions.Headers.Add("auth-token-bin"
                    , AuthenticationTokenStorage.Current.Token);
            }

            return await next(context);
        }
    }

    internal class AuthenticationTokenStorage
    {
        public static AuthenticationTokenStorage Current { get; }
            = new AuthenticationTokenStorage();

        private readonly object _syncObject = new object();

        public byte[] Token { get; private set; }
        public DateTimeOffset Expiration { get; private set; }

        public bool IsExpired =>
            Token == null || Expiration < DateTimeOffset.Now;

        public void Update(byte[] token, DateTimeOffset expiration)
        {
            lock (_syncObject)
            {
                Token = token ?? throw new ArgumentNullException(nameof(token));
                Expiration = expiration;
            }
        }
    }
}