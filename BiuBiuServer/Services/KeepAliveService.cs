using BiuBiuShare.ServiceInterfaces;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Authentication;

namespace BiuBiuServer.Services
{
    public class KeepAliveService : ServiceBase<IKeepAliveService>, IKeepAliveService
    {
        [Authorize]
        public async UnaryResult<bool> SendHeartbeatPacket()
        {
            return true;
        }
    }
}