using BiuBiuShare.ImInfos;
using BiuBiuShare.UserManagement;
using MagicOnion;
using System.Collections.Generic;

namespace BiuBiuShare.ServiceInterfaces
{
    /// <summary>
    /// 管理员操作相关接口
    /// </summary>
    public interface IAdminService : IService<IAdminService>
    {
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="registerInfos">注册信息</param>
        /// <returns>-1表示该工号被注册，-2表示该手机号码被注册，0表示数据库因故障未插入成功,1表示成功</returns>
        UnaryResult<int> RegisteredUsers(RegisterInfo registerInfos);

        /// <summary>
        /// 通过工号查用户
        /// </summary>
        /// <param name="jobNumber">工号</param>
        /// <returns>用户信息</returns>
        UnaryResult<UserInfo> SelectByJobNumber(string jobNumber);

        /// <summary>
        /// 通过Id查用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户信息</returns>
        UnaryResult<UserInfo> SelectByUserId(ulong userId);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> ChangePassword(ulong userId, string newPassword);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="newUserInfo">新的用户信息</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> ChangeUserInfo(UserInfo newUserInfo);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> DeleteUser(ulong userId);

        /// <summary>
        /// 审核信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="reviewResults">审核是否通过</param>
        /// <returns>是否成功</returns>
        UnaryResult<bool> ReviewMessage(ulong userId, bool reviewResults);//？

        /// <summary>
        /// 获取审核列表
        /// </summary>
        /// <returns></returns>
        UnaryResult<List<UserInfo>> GetModifyInfo();
    }
}