using System.Collections.Generic;
using BiuBiuShare.ImInfos;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    public interface IImInfoDatabaseDriven
    {
        public UnaryResult<UserInfo> GetUserInfo(ulong userId);

        public UnaryResult<UserInfo> GetModifyQueueInfo(ulong userId);

        public UnaryResult<bool> SetUserInfo(UserInfo userInfo);

        public UnaryResult<bool> SetUserPassword(ulong userId, string password);

        public UnaryResult<TeamInfo> GetTeamInfo(ulong teamId);

        public UnaryResult<bool> SetTeamInfo(TeamInfo teamInfo);

        public UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId);
    }
}