using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuShare.ImInfos;
using MagicOnion;

namespace BiuBiuServer.Tests
{
    public class ImInfoDatabaseDrivenTest : IImInfoDatabaseDriven
    {
        public async UnaryResult<UserInfo> GetUserInfo(ulong userId)
        {
            return new UserInfo() { UserId = userId, Description = "WDNMD" };
        }

        public UnaryResult<UserInfo> GetModifyQueueInfo(ulong userId)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<bool> SetUserInfo(UserInfo userInfo)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<bool> SetUserPassword(ulong userId, string password)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<TeamInfo> GetTeamInfo(ulong teamId)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<bool> SetTeamInfo(TeamInfo teamInfo)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId)
        {
            throw new System.NotImplementedException();
        }
    }
}