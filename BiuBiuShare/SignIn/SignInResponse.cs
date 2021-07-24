using MessagePack;
using System;

namespace BiuBiuShare.SignIn
{
    [MessagePackObject(true)]
    public class SignInResponse : SignIn
    {
        public string DisplayName { get; set; }
        public byte[] Token { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public bool Success { get; set; }

        public static SignInResponse Failed { get; }
            = new SignInResponse() { Success = false };

        public SignInResponse()
        {
        }

        public SignInResponse(ulong userId, string displayName
            , byte[] token, DateTimeOffset expiration)
        {
            Success = true;
            UserId = userId;
            DisplayName = displayName;
            Token = token;
            Expiration = expiration;
        }
    }
}