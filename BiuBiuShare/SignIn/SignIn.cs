using MessagePack;

namespace BiuBiuShare.SignIn
{
    [MessagePackObject(true)]
    public class SignIn
    {
        public ulong UserId { get; set; }
        public string Password { get; set; }
    }
}