using BiuBiuShare.ImInfos;

namespace BiuBiuShare.OnlineHub
{
    public interface IOnlineHubReceiver
    {
        void OnJoin(UserInfo user);

        void OnLeave(UserInfo user);
    }
}