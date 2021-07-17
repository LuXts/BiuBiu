using BiuBiuServer.Interfaces;
using BiuBiuShare.ImInfos;
using BiuBiuShare.UserManagement;
using MagicOnion;

namespace BiuBiuServer.Tests
{
    public class AdminDatabaseDrivenTest : IAdminDatabaseDriven
    {
        public async UnaryResult<bool> ChangePassword(ulong userId, string oldPassword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> ChangeUserInfo(ulong userId, UserInfo newUserInfo)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<bool> ChangeUserInfo(UserInfo newUserInfo)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> DeleteUser(ulong userId)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<string> RegisteredUsers(RegisterInfo registerInfos)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> ReviewMessage(ulong userId, bool reviewResults)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<int> RegisteredUsers(ulong userId, RegisterInfo registerInfos)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<UserInfo> SelectByJobNumber(string jobNumber)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<UserInfo> SelectByUserId(ulong userId)
        {
            throw new System.NotImplementedException();
        }

        public UnaryResult<bool> ChangePassword(ulong userId, string newPassword)
        {
            throw new System.NotImplementedException();
        }
    }
}