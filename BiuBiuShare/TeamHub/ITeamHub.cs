using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using MagicOnion;

namespace BiuBiuShare.TeamHub
{
    public interface ITeamHub : IStreamingHub<ITeamHub, ITeamHubReceiver>
    {
        public Task JoinAsync(TeamInfo teamInfo);

        public Task SendMessage(Message message);

        public Task AddUser(UserInfo userInfo);

        public Task DelUser(UserInfo userInfo);
    }
}