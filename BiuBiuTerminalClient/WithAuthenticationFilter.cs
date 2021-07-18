using System;
using System.Threading.Tasks;
using BiuBiuShare;
using BiuBiuShare.ServiceInterfaces;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuTerminalClient
{
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
}