using System.Threading.Tasks;
using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuShare.UserHub
{
    public interface IUserHub : IStreamingHub<IUserHub, IUserHubReceiver>
    {
        public Task JoinAsync(UserInfo userInfo);

        public Task SendMessage(MessageResponse message);

        public Task SendFriendRequest(FriendRequest request);

        public Task SendGroupInvitation(GroupInvitation invitation);

        public Task SendGroupRequest(GroupRequest request);
    }
}