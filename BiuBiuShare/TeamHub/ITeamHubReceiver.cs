using BiuBiuShare.ImInfos;
using BiuBiuShare.TalkInfo;

namespace BiuBiuShare.TeamHub
{
    public interface ITeamHubReceiver
    {
        public void SendMessage(Message message);

        public void AddUser(UserInfo userInfo);

        public void DelUser(UserInfo userInfo);
    }
}