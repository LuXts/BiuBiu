using LitJWT;
using MagicOnion.Server.Authentication.Jwt;
using Microsoft.Extensions.Options;
using System;
using System.Security.Principal;
using System.Text.Json;

namespace BiuBiuServer.Authentication
{
    public class CustomJwtAuthenticationProvider : IJwtAuthenticationProvider
    {
        private readonly JwtAuthenticationOptions _jwtAuthOptions;

        public CustomJwtAuthenticationProvider(
            IOptions<JwtAuthenticationOptions> jwtAuthOptions)
        {
            _jwtAuthOptions = jwtAuthOptions.Value;
        }

        public DecodeResult TryCreatePrincipalFromToken(byte[] bytes
            , out IPrincipal principal)
        {
            var result = _jwtAuthOptions.Decoder.TryDecode(bytes
                , x => JsonSerializer
                    .Deserialize<CustomJwtAuthenticationPayload>(x)
                , out var payload);
            if (result != DecodeResult.Success)
            {
                principal = null;
                return result;
            }

            if (payload.IsAdmin)
            {
                principal = new GenericPrincipal(
                    new CustomJwtAuthUserIdentity(payload.UserId
                        , payload.DisplayName), new[] { "Administrators" });
            }
            else
            {
                principal = new GenericPrincipal(
                    new CustomJwtAuthUserIdentity(payload.UserId
                        , payload.DisplayName), Array.Empty<string>());
            }

            return DecodeResult.Success;
        }

        public JwtAuthenticationTokenResult CreateTokenFromPayload(
            object payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            var expire = DateTimeOffset.Now.Add(_jwtAuthOptions.Expire);
            var encoded = _jwtAuthOptions.Encoder.EncodeAsUtf8Bytes(payload
                , expire
                , (o, writer) =>
                    writer.Write(JsonSerializer.SerializeToUtf8Bytes(o)));

            return new JwtAuthenticationTokenResult(encoded, expire);
        }

        public void ValidatePrincipal(
            ref JwtAuthenticationValidationContext ctx)
        {
        }
    }
}