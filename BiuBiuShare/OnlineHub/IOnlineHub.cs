using System.Collections.Generic;
using System.Threading.Tasks;
using BiuBiuShare.ImInfos;
using MagicOnion;

namespace BiuBiuShare.OnlineHub
{
    public interface IOnlineHub : IStreamingHub<IOnlineHub, IOnlineHubReceiver>
    {
        Task<UserInfo[]> JoinAsync(UserInfo userInfo);

        Task LeaveAsync();
    }
}