using MessagePack;

namespace BiuBiuShare.ImInfos
{
    // 用户信息类
    [MessagePackObject(true)]
    public class UserInfo
    {
        public ulong UserId { get; set; }
        public string DisplayName { get; set; }
        public string JobNumber { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ulong IconId { get; set; }
    }
}