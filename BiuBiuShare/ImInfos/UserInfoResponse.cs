using MessagePack;

namespace BiuBiuShare.ImInfos
{
    [MessagePackObject(true)]
    public class UserInfoResponse : UserInfo
    {
        public bool Success { get; set; }

        public static UserInfoResponse Failed { get; }
            = new UserInfoResponse() { Success = false };

        public UserInfoResponse()
        {
        }

        public UserInfoResponse(UserInfo userInfo)
        {
            this.UserId = userInfo.UserId;
            this.PhoneNumber = userInfo.PhoneNumber;
            this.Permissions = userInfo.Permissions;
            this.JobNumber = userInfo.JobNumber;
            this.IconId = userInfo.IconId;
            this.Email = userInfo.Email;
            this.DisplayName = userInfo.DisplayName;
            this.Description = userInfo.Description;
        }
    }
}