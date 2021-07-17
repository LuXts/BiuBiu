using System;
using System.Security.Principal;

namespace BiuBiuServer.Authentication
{
    public class CustomJwtAuthUserIdentity : IIdentity
    {
        public long UserId { get; }

        public bool IsAuthenticated => true;
        public string AuthenticationType => "Jwt";

        public string Name { get; }

        public CustomJwtAuthUserIdentity(long userId, string displayName)
        {
            UserId = userId;
            Name = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }
    }
}