using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using BiuBiuShare.TeamHub;
using MagicOnion.Server.Hubs;
using System.Threading.Tasks;

namespace BiuBiuServer.TeamHub
{
    [GroupConfiguration(typeof(ConcurrentDictionaryGroupRepositoryFactory))]
    public class TeamHub : StreamingHubBase<ITeamHub, ITeamHubReceiver>, ITeamHub
    {
        private IGroup room;
        private TeamInfo self;
        private IInMemoryStorage<TeamInfo> storage;

        public async Task JoinAsync(TeamInfo teamInfo)
        {
            self = teamInfo;
            (room, storage) = await Group.AddAsync(teamInfo.TeamId.ToString(), self);
        }

        public async Task SendMessage(Message message)
        {
            await Task.Run(() => { Broadcast(room).SendMessage(message); });
        }

        public async Task AddUser(UserInfo userInfo)
        {
            await Task.Run(() => { Broadcast(room).AddUser(userInfo); });
        }

        public async Task DelUser(UserInfo userInfo)
        {
            await Task.Run(() => { Broadcast(room).DelUser(userInfo); });
        }
    }
}