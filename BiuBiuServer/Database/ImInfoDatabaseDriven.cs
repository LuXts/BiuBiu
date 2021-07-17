using System;
using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuShare.ImInfos;
using MagicOnion;

namespace BiuBiuServer.Database
{
    /// <summary>
    /// 数据库驱动实现
    /// </summary>
    public class ImInfoDatabaseDriven : IImInfoDatabaseDriven
    {
        // TODO: 输入用户ID，获取用户信息
        /// <inheritdoc />
        public async UnaryResult<UserInfo> GetUserInfo(ulong userId)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 获取修改队列里面的用户信息
        // 就是如果有之前未审核的修改申请，就拿那个修改申请的值回来，
        // 否则和上面那个函数返回值一样
        /// <inheritdoc />
        public async UnaryResult<UserInfo> GetModifyQueueInfo(ulong userId)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 修改用户信息
        // userInfo.UserId 就是用户 ID
        /// <inheritdoc />
        public async UnaryResult<bool> SetUserInfo(UserInfo userInfo)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 修改用户密码
        // 这个输入参数简单
        /// <inheritdoc />
        public async UnaryResult<bool> SetUserPassword(ulong userId
            , string oldPassword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 获取群组信息
        /// <inheritdoc />
        public async UnaryResult<TeamInfo> GetTeamInfo(ulong teamId)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 设置群组信息
        /// <inheritdoc />
        public async UnaryResult<bool> SetTeamInfo(TeamInfo teamInfo)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 获取群员信息（连表查询）
        /// <inheritdoc />
        public async UnaryResult<List<UserInfo>> GetTeamUserInfo(ulong teamId)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 获取好友信息
        public UnaryResult<List<UserInfo>> GetUserFriendsId(ulong userId)
        {
            throw new NotImplementedException();
        }

        // TODO: 获取群成员信息
        public UnaryResult<List<TeamInfo>> GetUserTeamsId(ulong teamId)
        {
            throw new NotImplementedException();
        }
    }
}