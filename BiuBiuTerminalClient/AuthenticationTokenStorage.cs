using System;
using System.Threading.Tasks;
using BiuBiuShare.ImInfos;

namespace BiuBiuTerminalClient
{
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