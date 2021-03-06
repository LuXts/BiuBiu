using MessagePack;

namespace BiuBiuShare.ImInfos
{
    /// <summary>
    /// 用户信息类
    /// </summary>
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
        public bool Permissions { get; set; }

        /// <summary>
        /// 约定 UserId = 0 代表没有这个用户
        /// </summary>
        public static UserInfo NullUser = new UserInfo() { UserId = 0 };
    }
}