﻿using BiuBiuServer.Interfaces;
using BiuBiuShare.UserManagement;
using MagicOnion;

namespace BiuBiuServer.Tests
{
    public class AdminDatabaseDrivenTest : IAdminDatabaseDriven
    {
        public async  UnaryResult<bool> ChangePassword(ulong userId, string oldPassword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> ChangeUserInfo(ulong userId, UserInfo newUserInfo)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> DeleteUser(ulong userId)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<string> RegisteredUsers(RegistrationInformation registrationInformations)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<bool> ReviewMessage(ulong userId, bool reviewResults)
        {
            throw new System.NotImplementedException();
        }

        public async UnaryResult<UserInfo> SelectByJobNumber(string jobNumber)
        {
            throw new System.NotImplementedException();
        }

        public async  UnaryResult<UserInfo> SelectByUserId(ulong userId)
        {
            throw new System.NotImplementedException();
        }
    }
}