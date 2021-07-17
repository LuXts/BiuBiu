using MessagePack;

namespace BiuBiuShare.Response
{
    [MessagePackObject(true)]
    public class CurrentUserResponse
    {
        public static CurrentUserResponse Anonymous { get; }
            = new CurrentUserResponse()
            {
                IsAuthenticated = false,
                Name = "Anonymous"
            };

        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
    }
}