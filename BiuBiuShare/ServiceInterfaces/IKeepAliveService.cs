using MagicOnion;

namespace BiuBiuShare.ServiceInterfaces
{
    public interface IKeepAliveService : IService<IKeepAliveService>
    {
        public UnaryResult<bool> SendHeartbeatPacket();
    }
}