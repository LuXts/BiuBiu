using System;
using System.Security.Principal;

namespace BiuBiuServer.Authentication
{
    public class CustomJwtAuthUserIdentity : IIdentity
    {
        public ulong UserId { get; }

        public bool IsAuthenticated => true;
        public string AuthenticationType => "Jwt";

        public string Name { get; }

        public CustomJwtAuthUserIdentity(ulong userId, string displayName)
        {
            UserId = userId;
            Name = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }
    }
}