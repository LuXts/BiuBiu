using BiuBiuShare.ImInfos;
using MagicOnion;
using System.Collections.Generic;

namespace BiuBiuShare.ServiceInterfaces
{
    /// <summary>
    /// 用户群组信息服务接口
    /// </summary>
    public interface IImInfoService : IService<IImInfoService>
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>用户信息</returns>
        UnaryResult<UserInfoResponse> GetUserInfo(UserInfo userInfo);

        /// <summary>
        /// 获取修改队列的用户信息
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>用户信息</returns>
        UnaryResult<UserInfoResponse> GetModifyQueueInfo(UserInfo userInfo);

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="userInfo">里面已经存储了Id 所以不需要额外再传一个</param>
        /// <returns>是否成功</returns>
        UnaryResult<UserInfoResponse> SetUserInfo(UserInfo userInfo);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>是否成功</returns>
        UnaryResult<UserInfoResponse> SetUserPassword(UserInfo userInfo, string oldPassword, string newPassword);

        /// <summary>
        /// 获取群组信息
        /// </summary>
        /// <param name="teamInfo">里面已经存储了Id 所以不需要额外再传一个</param>
        /// <returns>群组信息</returns>
        UnaryResult<TeamInfoResponse> GetTeamInfo(TeamInfo teamInfo);

        /// <summary>
        /// 修改群组信息
        /// </summary>
        /// <param name="teamInfo">里面已经存储了Id 所以不需要额外再传一个</param>
        /// <returns></returns>
        UnaryResult<TeamInfoResponse> SetTeamInfo(TeamInfo teamInfo);

        /// <summary>
        /// 获取群成员信息
        /// </summary>
        /// <param name="teamInfo">群组信息</param>
        /// <returns></returns>
        UnaryResult<List<UserInfo>> GetTeamUserInfo(TeamInfo teamInfo);

        /// <summary>
        /// 获取某Id的好友信息列表
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>好友信息列表</returns>
        UnaryResult<List<UserInfo>> GetUserFriendsId(UserInfo userInfo);

        /// <summary>
        /// 获取某Id的群组信息列表
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>群组信息列表</returns>
        UnaryResult<List<TeamInfo>> GetUserTeamsId(UserInfo userInfo);

        /// <summary>
        /// 获取某Id的最后登录时间
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>最后登录时间</returns>
        UnaryResult<ulong> GetUserLastLoginTime(UserInfo userInfo);
    }
}