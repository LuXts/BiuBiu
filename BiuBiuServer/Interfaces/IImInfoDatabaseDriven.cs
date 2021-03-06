using BiuBiuShare.ImInfos;
using MagicOnion;
using System.Collections.Generic;

namespace BiuBiuServer.Interfaces
{
    /// <summary>
    /// 用户群组信息服务与数据库之间的驱动
    /// </summary>
    public interface IImInfoDatabaseDriven
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户信息</returns>
        public UnaryResult<UserInfoResponse> GetUserInfo(ulong userId);

        /// <summary>
        /// 获取修改队列的用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户信息</returns>
        public UnaryResult<UserInfoResponse> GetModifyQueueInfo(ulong userId);

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="userInfo">里面已经存储了Id 所以不需要额外再传一个</param>
        /// <returns>是否成功</returns>
        public UnaryResult<UserInfoResponse> SetUserInfo(UserInfo userInfo);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>是否成功</returns>
        public UnaryResult<UserInfoResponse> SetUserPassword(ulong userId
            , string oldPassword, string newPassword);

        /// <summary>
        /// 获取群组信息
        /// </summary>
        /// <param name="teamId">群组Id</param>
        /// <returns>群组信息</returns>
        public UnaryResult<TeamInfoResponse> GetTeamInfo(ulong teamId);

        /// <summary>
        /// 修改群组信息
        /// </summary>
        /// <param name="teamInfo">里面已经存储了Id 所以不需要额外再传一个</param>
        /// <returns></returns>
        public UnaryResult<TeamInfoResponse> SetTeamInfo(TeamInfo teamInfo);

        /// <summary>
        /// 获取群成员信息
        /// </summary>
        /// <param name="teamId">群组Id</param>
        /// <returns></returns>
        public UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId);

        /// <summary>
        /// 获取某Id的好友信息列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>好友信息列表</returns>
        UnaryResult<List<UserInfo>> GetUserFriendsId(ulong userId);

        /// <summary>
        /// 获取某用户的的群组信息列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>群组信息列表</returns>
        UnaryResult<List<TeamInfo>> GetUserTeamsId(ulong userId);

        /// <summary>
        /// 获取某Id的最后登录时间
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>最后登录时间</returns>
        UnaryResult<ulong> GetUserLastLoginTime(ulong userId);

        /// <summary>
        /// 获取某Id的最后登录时间
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>设置最后登录时间</returns>
        UnaryResult<bool> SetUserLastLoginTime(ulong userId, ulong lastLoginTime);
    }
}