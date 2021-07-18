using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuShare.TeamHub
{
    public interface ITeamHub : IStreamingHub<ITeamHub, ITeamHubReceiver>
    {
        public Task JoinAsync(TeamInfo teamInfo);

        public Task SendMessage(MessageResponse message);

        public Task AddUser(UserInfo userInfo);

        public Task DelUser(UserInfo userInfo);
    }
}