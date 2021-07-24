using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.UserHub;
using MagicOnion.Server.Hubs;
using System.Threading.Tasks;

namespace BiuBiuServer.UserHub
{
    public class UserHub : StreamingHubBase<IUserHub, IUserHubReceiver>, IUserHub
    {
        private IGroup room;
        private UserInfo self;
        private IInMemoryStorage<UserInfo> storage;

        public async Task JoinAsync(UserInfo userInfo)
        {
            self = userInfo;
            (room, storage) = await Group.AddAsync(userInfo.UserId.ToString(), self);
        }

        public async Task SendMessage(Message message)
        {
            await Task.Run(() => { Broadcast(room).SendMessage(message); });
        }

        public async Task SendFriendRequest(FriendRequest request)
        {
            await Task.Run(() => { Broadcast(room).SendFriendRequest(request); });
        }

        public async Task SendGroupInvitation(TeamInvitation invitation)
        {
            await Task.Run(() => { Broadcast(room).SendGroupInvitation(invitation); });
        }

        public async Task SendGroupRequest(TeamRequest request)
        {
            await Task.Run(() => { Broadcast(room).SendGroupRequest(request); });
        }
    }
}