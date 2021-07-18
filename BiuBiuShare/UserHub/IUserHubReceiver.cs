using BiuBiuShare.GrouFri;
using BiuBiuShare.Response;

namespace BiuBiuShare.UserHub
{
    public interface IUserHubReceiver
    {
        public void SendMessage(MessageResponse message);

        public void SendFriendRequest(FriendRequest request);

        public void SendGroupInvitation(GroupInvitation invitation);

        public void SendGroupRequest(GroupRequest request);
    }
}