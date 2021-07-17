using BiuBiuShare.UserManagement;
using MagicOnion;
using Microsoft.AspNetCore.Hosting.Server;

namespace BiuBiuShare
{
    public interface IAdminService : IService<IAdminService>
    {
        UnaryResult<int> RegisteredUsers(RegistrationInformation registrationInformations);//?
        UnaryResult<UserInfo> SelectByJobNumber(string jobNumber);
        UnaryResult<UserInfo> SelectByUserId(ulong userId);
        UnaryResult<bool> ChangePassword(ulong userId,string newPassword);
        UnaryResult<bool> ChangeUserInfo(UserInfo newUserInfo);//?
        UnaryResult<bool> DeleteUser(ulong userId);
        UnaryResult<bool> ReviewMessage(ulong userId, bool reviewResults);//？

    }
}