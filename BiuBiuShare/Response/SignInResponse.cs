using System;
using MessagePack;

namespace BiuBiuShare.Response
{
    [MessagePackObject(true)]
    public class SignInResponse
    {
        public long UserId { get; set; }
        public string DisplayName { get; set; }
        public byte[] Token { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public bool Success { get; set; }

        public static SignInResponse Failed { get; }
            = new SignInResponse() { Success = false };

        public SignInResponse()
        {
        }

        public SignInResponse(long userId, string displayName
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