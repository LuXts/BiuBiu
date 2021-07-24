using BiuBiuShare.GrouFri;
using BiuBiuShare.TalkInfo;

namespace BiuBiuShare.UserHub
{
    public interface IUserHubReceiver
    {
        public void SendMessage(Message message);

        public void SendFriendRequest(FriendRequest request);

        public void SendGroupInvitation(TeamInvitation invitation);

        public void SendGroupRequest(TeamRequest request);
    }
}