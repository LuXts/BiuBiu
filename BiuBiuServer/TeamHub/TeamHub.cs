using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using BiuBiuShare.Response;
using BiuBiuShare.TeamHub;
using MagicOnion.Server.Hubs;

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

        public async Task SendMessage(MessageResponse message)
        {
            Broadcast(room).SendMessage(message);
        }

        public async Task AddUser(UserInfo userInfo)
        {
            Broadcast(room).AddUser(userInfo);
        }

        public async Task DelUser(UserInfo userInfo)
        {
            Broadcast(room).DelUser(userInfo);
        }
    }
}