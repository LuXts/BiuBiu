using BiuBiuServer.Database;
using BiuBiuServer.Interfaces;
using BiuBiuShare.ImInfos;
using BiuBiuShare.ServiceInterfaces;
using BiuBiuShare.Tool;
using BiuBiuShare.UserManagement;
using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Authentication;
using System.Collections.Generic;

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

        public async UnaryResult<List<UserInfo>> GetModifyInfo()
        {
            return await _adminDatabaseDatabaseDriven.GetModifyInfo();
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