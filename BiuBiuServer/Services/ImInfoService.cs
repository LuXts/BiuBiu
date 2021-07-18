using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuServer.Tests;
using BiuBiuShare;
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Authentication;

namespace BiuBiuServer.Services
{
    /// <summary>
    /// 用户群组信息服务实现
    /// </summary>
    public class ImInfoService : ServiceBase<IImInfoService>, IImInfoService
    {
        // TODO: 测试接口(等数据库驱动实现完成后替换)
        private static readonly IImInfoDatabaseDriven _imInfoDatabaseDriven
            = new ImInfoDatabaseDrivenTest();

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<UserInfo> GetUserInfo(ulong userId)
        {
            return _imInfoDatabaseDriven.GetUserInfo(userId);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<UserInfo> GetModifyQueueInfo(ulong userId)
        {
            return _imInfoDatabaseDriven.GetModifyQueueInfo(userId);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<bool> SetUserInfo(UserInfo userInfo)
        {
            return _imInfoDatabaseDriven.SetUserInfo(userInfo);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<bool> SetUserPassword(ulong userId
            , string oldPassword, string newPassword)
        {
            return _imInfoDatabaseDriven.SetUserPassword(userId, oldPassword
                , newPassword);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<TeamInfo> GetTeamInfo(ulong teamId)
        {
            return _imInfoDatabaseDriven.GetTeamInfo(teamId);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<bool> SetTeamInfo(TeamInfo teamInfo)
        {
            return _imInfoDatabaseDriven.SetTeamInfo(teamInfo);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId)
        {
            return _imInfoDatabaseDriven.GetTeamUserInfo(teamId);
        }

        [Authorize]
        public UnaryResult<List<UserInfo>> GetUserFriendsId(ulong userId)
        {
            return _imInfoDatabaseDriven.GetUserFriendsId(userId);
        }

        [Authorize]
        public UnaryResult<List<TeamInfo>> GetUserTeamsId(ulong teamId)
        {
            return _imInfoDatabaseDriven.GetUserTeamsId(teamId);
        }
    }
}