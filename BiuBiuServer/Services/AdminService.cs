using BiuBiuServer.Interfaces;
using BiuBiuServer.Tests;
using BiuBiuShare;
using BiuBiuShare.UserManagement;
using Grpc.Core;
using MagicOnion;
using MagicOnion.Server;
using System;
using System.Threading;
using BiuBiuServer.Database;
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.Tool;
using MagicOnion.Server.Authentication;

namespace BiuBiuServer.Services
{
    [Authorize(Roles = new[] { "Administrators" })]
    public class AdminService : ServiceBase<IAdminService>, IAdminService
    {
        private readonly IAdminDatabaseDriven _adminDatabaseDatabaseDriven
            = new AdminDatabaseDriven();

        public async UnaryResult<bool> ChangePassword(ulong userId, string newPassword)
        {
            return await _adminDatabaseDatabaseDriven.ChangePassword(userId, newPassword);
        }

        public async UnaryResult<bool> ChangeUserInfo(UserInfo newUserInfo)
        {
            return await _adminDatabaseDatabaseDriven.ChangeUserInfo(newUserInfo);
        }

        public UnaryResult<bool> DeleteUser(ulong userId)
        {
            return _adminDatabaseDatabaseDriven.DeleteUser(userId);
        }

        public async UnaryResult<int> RegisteredUsers(RegisterInfo registerInfos)
        {
            ulong userId = IdManagement.GenerateId(IdType.UserId);
            return await _adminDatabaseDatabaseDriven.RegisteredUsers(userId, registerInfos);
        }

        public async UnaryResult<bool> ReviewMessage(ulong userId, bool reviewResults)
        {
            return await _adminDatabaseDatabaseDriven.ReviewMessage(userId, reviewResults);
        }

        public async UnaryResult<UserInfo> SelectByJobNumber(string jobNumber)
        {
            return await _adminDatabaseDatabaseDriven.SelectByJobNumber(jobNumber);
        }

        public async UnaryResult<UserInfo> SelectByUserId(ulong userId)
        {
            return await _adminDatabaseDatabaseDriven.SelectByUserId(userId);
        }
    }
}