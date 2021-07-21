using System;
using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace BiuBiuWpfClient.Login
{
    public class AuthenticationTokenStorage
    {
        public static AuthenticationTokenStorage Current { get; }
            = new AuthenticationTokenStorage();

        private static readonly object _syncObject = new object();

        public static byte[] Token { get; private set; }
        public static DateTimeOffset Expiration { get; private set; }

        public static string DisplayName;
        public static ulong UserId;

        public bool IsExpired =>
            Token == null || Expiration < DateTimeOffset.Now;

        public static void Update(byte[] token, DateTimeOffset expiration)
        {
            lock (_syncObject)
            {
                Token = token ?? throw new ArgumentNullException(nameof(token));
                Expiration = expiration;
            }
        }

        public static async Task<UserInfoResponse> Login(string signInId, string password, GrpcChannel channel)
        {
            var client = MagicOnionClient.Create<IAccountService>(channel);
            var authResult
                = await client.AdministrantSignInAsync(signInId, password);
            if (authResult.Success)
            {
                Update(authResult.Token, authResult.Expiration);
                return new UserInfoResponse()
                {
                    UserId = authResult.UserId
                    ,
                    Success = true
                    ,
                    DisplayName = authResult.DisplayName
                };
            }
            else
            {
                return UserInfoResponse.Failed;
            }
        }
    }
}