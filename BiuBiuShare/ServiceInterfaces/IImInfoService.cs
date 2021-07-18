using System.Collections.Generic;
using BiuBiuShare.ImInfos;
using MagicOnion;

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
        /// <param name="userId">用户Id</param>
        /// <returns>用户信息</returns>
        UnaryResult<UserInfo> GetUserInfo(ulong userId);

        /// <summary>
        /// 获取修改队列的用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户信息</returns>
        UnaryResult<UserInfo> GetModifyQueueInfo(ulong userId);

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="userInfo">里面已经存储了Id 所以不需要额外再传一个</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> SetUserInfo(UserInfo userInfo);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> SetUserPassword(ulong userId, string oldPassword, string newPassword);

        /// <summary>
        /// 获取群组信息
        /// </summary>
        /// <param name="teamId">群组Id</param>
        /// <returns>群组信息</returns>
        UnaryResult<TeamInfo> GetTeamInfo(ulong teamId);

        /// <summary>
        /// 修改群组信息
        /// </summary>
        /// <param name="teamInfo">里面已经存储了Id 所以不需要额外再传一个</param>
        /// <returns></returns>
        UnaryResult<bool> SetTeamInfo(TeamInfo teamInfo);

        /// <summary>
        /// 获取群成员信息
        /// </summary>
        /// <param name="teamId">群组Id</param>
        /// <returns></returns>
        UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId);

        /// <summary>
        /// 获取某Id的好友信息列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>好友信息列表</returns>
        UnaryResult<List<UserInfo>> GetUserFriendsId(ulong userId);

        /// <summary>
        /// 获取某Id的的群组信息列表
        /// </summary>
        /// <param name="teamId">群组Id</param>
        /// <returns>群组信息列表</returns>
        UnaryResult<List<TeamInfo>> GetUserTeamsId(ulong teamId);
    }
}