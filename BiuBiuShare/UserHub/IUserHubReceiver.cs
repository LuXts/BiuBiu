using BiuBiuShare.GrouFri;
using BiuBiuShare.SignIn;
using BiuBiuShare.TalkInfo;

namespace BiuBiuShare.UserHub
{
    public interface IUserHubReceiver
    {
        public void SendMessage(Message message);

        public void SendFriendRequest(FriendRequest request);

        public void SendGroupInvitation(GroupInvitation invitation);

        public void SendGroupRequest(GroupRequest request);
    }
}