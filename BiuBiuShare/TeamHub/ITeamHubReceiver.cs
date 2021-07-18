using BiuBiuShare.ImInfos;
using BiuBiuShare.Response;

namespace BiuBiuShare.TeamHub
{
    public interface ITeamHubReceiver
    {
        public void SendMessage(MessageResponse message);

        public void AddUser(UserInfo userInfo);

        public void DelUser(UserInfo userInfo);
    }
}