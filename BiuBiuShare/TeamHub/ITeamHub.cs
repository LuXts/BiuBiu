using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;
using MagicOnion;
using System.Threading.Tasks;

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