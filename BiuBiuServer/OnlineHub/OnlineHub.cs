using BiuBiuShare.ImInfos;
using BiuBiuShare.OnlineHub;
using MagicOnion.Server.Hubs;
using System.Linq;
using System.Threading.Tasks;

namespace BiuBiuServer.OnlineHub
{
    [GroupConfiguration(typeof(ConcurrentDictionaryGroupRepositoryFactory))]
    public class OnlineHub : StreamingHubBase<IOnlineHub, IOnlineHubReceiver>, IOnlineHub
    {
        private IGroup room;
        private UserInfo self;
        private IInMemoryStorage<UserInfo> storage;
        private bool IsOnline = true;

        public async Task<UserInfo[]> JoinAsync(UserInfo userInfo)
        {
            self = userInfo;
            (room, storage) = await Group.AddAsync("Alive User", self);
            Broadcast(room).OnJoin(self);
            return storage.AllValues.ToArray();
        }

        public async Task LeaveAsync()
        {
            await room.RemoveAsync(this.Context);
            Broadcast(room).OnLeave(self);
            IsOnline = false;
        }

        protected override async ValueTask OnConnecting()
        {
            await base.OnConnecting();
        }

        protected override async ValueTask OnDisconnected()
        {
            if (IsOnline)
            {
                await LeaveAsync();
            }
            await base.OnDisconnected();
        }
    }
}