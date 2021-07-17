using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuServer.Tests;
using BiuBiuShare;
using BiuBiuShare.ImInfos;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Authentication;

namespace BiuBiuServer.Services
{
    public class ImInfoService : ServiceBase<IImInfoService>, IImInfoService
    {
        // TODO: 测试接口(等数据库驱动实现完成后替换)
        private IImInfoDatabaseDriven _imInfoDatabaseDriven
            = new ImInfoDatabaseDrivenTest();

        [Authorize]
        public UnaryResult<UserInfo> GetUserInfo(ulong userId)
        {
            return _imInfoDatabaseDriven.GetUserInfo(userId);
        }

        [Authorize]
        public UnaryResult<UserInfo> GetModifyQueueInfo(ulong userId)
        {
            return _imInfoDatabaseDriven.GetModifyQueueInfo(userId);
        }

        [Authorize]
        public UnaryResult<bool> SetUserInfo(UserInfo userInfo)
        {
            return _imInfoDatabaseDriven.SetUserInfo(userInfo);
        }

        [Authorize]
        public UnaryResult<bool> SetUserPassword(ulong userId, string password)
        {
            return _imInfoDatabaseDriven.SetUserPassword(userId, password);
        }

        [Authorize]
        public UnaryResult<TeamInfo> GetTeamInfo(ulong teamId)
        {
            return _imInfoDatabaseDriven.GetTeamInfo(teamId);
        }

        [Authorize]
        public UnaryResult<bool> SetTeamInfo(TeamInfo teamInfo)
        {
            return _imInfoDatabaseDriven.SetTeamInfo(teamInfo);
        }

        [Authorize]
        public UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId)
        {
            return _imInfoDatabaseDriven.GetTeamUserInfo(teamId);
        }
    }
}