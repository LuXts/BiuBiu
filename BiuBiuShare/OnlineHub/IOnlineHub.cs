using BiuBiuShare.ImInfos;
using MagicOnion;
using System.Threading.Tasks;

namespace BiuBiuShare.OnlineHub
{
    public interface IOnlineHub : IStreamingHub<IOnlineHub, IOnlineHubReceiver>
    {
        Task<UserInfo[]> JoinAsync(UserInfo userInfo);

        Task LeaveAsync();
    }
}