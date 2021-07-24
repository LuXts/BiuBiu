using BiuBiuServer.Database;
using BiuBiuServer.Interfaces;
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Authentication;
using System.Collections.Generic;

namespace BiuBiuServer.Services
{
    /// <summary>
    /// 用户群组信息服务实现
    /// </summary>
    public class ImInfoService : ServiceBase<IImInfoService>, IImInfoService
    {
        private static readonly IImInfoDatabaseDriven _imInfoDatabaseDriven
            = new ImInfoDatabaseDriven();

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<UserInfoResponse> GetUserInfo(UserInfo userInfo)
        {
            return _imInfoDatabaseDriven.GetUserInfo(userInfo.UserId);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<UserInfoResponse> GetModifyQueueInfo(UserInfo userInfo)
        {
            return _imInfoDatabaseDriven.GetModifyQueueInfo(userInfo.UserId);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<UserInfoResponse> SetUserInfo(UserInfo userInfo)
        {
            return _imInfoDatabaseDriven.SetUserInfo(userInfo);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<UserInfoResponse> SetUserPassword(UserInfo userInfo
            , string oldPassword, string newPassword)
        {
            return _imInfoDatabaseDriven.SetUserPassword(userInfo.UserId, oldPassword
                , newPassword);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<TeamInfoResponse> GetTeamInfo(TeamInfo teamInfo)
        {
            return _imInfoDatabaseDriven.GetTeamInfo(teamInfo.TeamId);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<TeamInfoResponse> SetTeamInfo(TeamInfo teamInfo)
        {
            return _imInfoDatabaseDriven.SetTeamInfo(teamInfo);
        }

        /// <inheritdoc />
        [Authorize]
        public UnaryResult<List<UserInfo>> GetTeamUserInfo(TeamInfo teamInfo)
        {
            return _imInfoDatabaseDriven.GetTeamUserInfo(teamInfo.TeamId);
        }

        [Authorize]
        public UnaryResult<List<UserInfo>> GetUserFriendsId(UserInfo userInfo)
        {
            return _imInfoDatabaseDriven.GetUserFriendsId(userInfo.UserId);
        }

        [Authorize]
        public UnaryResult<List<TeamInfo>> GetUserTeamsId(UserInfo userInfo)
        {
            return _imInfoDatabaseDriven.GetUserTeamsId(userInfo.UserId);
        }

        public UnaryResult<ulong> GetUserLastLoginTime(UserInfo userInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}