using MessagePack;

namespace BiuBiuShare.ImInfos
{
    [MessagePackObject(true)]
    public class UserInfoResponse : UserInfo
    {
        public bool Success { get; set; }

        public static UserInfoResponse Failed { get; }
            = new UserInfoResponse() { Success = false };
    }
}