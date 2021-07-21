using System;
using System.Threading.Tasks;
using BiuBiuShare.ServiceInterfaces;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuWpfClient.Login
{
    public class WithAuthenticationFilter : IClientFilter
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
                    $@"[WithAuthenticationFilter/IAccountService.SignInAsync] Try signing in as '{_signInId}'... ({(AuthenticationTokenStorage.Token == null ? "FirstTime" : "RefreshToken")})");

                var client = MagicOnionClient.Create<IAccountService>(_channel);
                var authResult
                    = await client.AdministrantSignInAsync(_signInId, _password);
                if (!authResult.Success)
                {
                    throw new Exception("Failed to sign-in on the server.");
                }

                Console.WriteLine(authResult.Token);
                Console.WriteLine(
                    $@"[WithAuthenticationFilter/IAccountService.SignInAsync] User authenticated as {authResult.DisplayName} (UserId:{authResult.UserId})");

                AuthenticationTokenStorage.Update(authResult.Token
                    , authResult
                        .Expiration); // NOTE: You can also read the token expiration date from JWT.

                foreach (var VARIABLE in context.CallOptions.Headers)
                {
                    if (VARIABLE.Key.Equals("auth-token-bin"))
                    {
                        context.CallOptions.Headers.Remove(VARIABLE);
                    }
                }
            }

            bool temp = true;
            foreach (var VARIABLE in context.CallOptions.Headers)
            {
                if (VARIABLE.Key.Equals("auth-token-bin"))
                {
                    temp = false;
                }
            }

            if (temp)
            {
                context.CallOptions.Headers.Add("auth-token-bin"
                    , AuthenticationTokenStorage.Token);
            }

            return await next(context);
        }
    }
}