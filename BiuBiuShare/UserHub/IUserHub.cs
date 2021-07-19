using System.Threading.Tasks;
using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.SignIn;
using BiuBiuShare.TalkInfo;
using MagicOnion;

namespace BiuBiuShare.UserHub
{
    public interface IUserHub : IStreamingHub<IUserHub, IUserHubReceiver>
    {
        public Task JoinAsync(UserInfo userInfo);

        public Task SendMessage(Message message);

        public Task SendFriendRequest(FriendRequest request);

        public Task SendGroupInvitation(TeamInvitation invitation);

        public Task SendGroupRequest(TeamRequest request);
    }
}