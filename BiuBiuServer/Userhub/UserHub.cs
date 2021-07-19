using System.Linq;
using System.Threading.Tasks;
using BiuBiuShare.GrouFri;
using BiuBiuShare.ImInfos;
using BiuBiuShare.SignIn;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.UserHub;
using MagicOnion.Server.Hubs;

namespace BiuBiuServer.Userhub
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
            Broadcast(room).SendMessage(message);
        }

        public async Task SendFriendRequest(FriendRequest request)
        {
            Broadcast(room).SendFriendRequest(request);
        }

        public async Task SendGroupInvitation(GroupInvitation invitation)
        {
            Broadcast(room).SendGroupInvitation(invitation);
        }

        public async Task SendGroupRequest(GroupRequest request)
        {
            Broadcast(room).SendGroupRequest(request);
        }
    }
}