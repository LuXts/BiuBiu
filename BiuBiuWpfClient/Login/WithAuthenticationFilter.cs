using BiuBiuShare.ServiceInterfaces;
using Grpc.Net.Client;
using MagicOnion.Client;
using Panuon.UI.Silver;
using System;
using System.Threading.Tasks;

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
            if (AuthenticationTokenStorage.Current.IsExpired())
            {
                var client = MagicOnionClient.Create<IAccountService>(_channel);
                var authResult
                    = await client.CommonSignInAsync(_signInId, _password);
                if (!authResult.Success)
                {
                    MessageBoxX.Show("无法登录你的账号！");
                    Environment.Exit(0);
                }

                AuthenticationTokenStorage.Update(authResult.Token
                    , authResult
                        .Expiration); // NOTE: You can also read the token expiration date from JWT.

                foreach (var VARIABLE in context.CallOptions.Headers)
                {
                    if (VARIABLE.Key.Equals("auth-token-bin"))
                    {
                        context.CallOptions.Headers.Remove(VARIABLE);
                        break;
                    }
                }
            }

            bool temp = true;
            foreach (var VARIABLE in context.CallOptions.Headers)
            {
                if (VARIABLE.Key.Equals("auth-token-bin"))
                {
                    temp = false;
                    break;
                }
            }

            string t = "";
            t.Equals()
            if (temp)
            {
                context.CallOptions.Headers.Add("auth-token-bin"
                    , AuthenticationTokenStorage.Token);
            }

            return await next(context);
        }
    }
}