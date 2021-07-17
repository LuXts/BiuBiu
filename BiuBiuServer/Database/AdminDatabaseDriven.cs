using BiuBiuServer.Interfaces;
using BiuBiuShare.UserManagement;
using MagicOnion;

namespace BiuBiuServer.Database
{
    public class AdminDatabaseDriven : IAdminDatabaseDriven
    {
        //TODO:函数功能：管理员修改用户密码 输入：用户Id、新密码 输出：修改密码是否成功
        public UnaryResult<bool> ChangePassword(ulong userId, string newPassword)
        {
            throw new System.NotImplementedException();
        }
        //TODO:函数功能：管理员修改用户信息 输入：用户ID、新的用户信息 输出：是否修改成功
        public UnaryResult<bool> ChangeUserInfo(UserInfo newUserInfo)
        {
            throw new System.NotImplementedException();
        }
        //TODO:函数功能：删除用户 输入：用户Id 输出：是否成功
        public UnaryResult<bool> DeleteUser(ulong userId)
        {
            throw new System.NotImplementedException();
        }
        //TODO://函数功能：根据ID和注册信息注册新用户 输入：注册用户信息 输出：注册信息提示信息(-1表示该工号被注册，-2表示该手机号码被注册，0表示数据库因故障未插入成功,1表示成功)
        public UnaryResult<int> RegisteredUsers(ulong userId, RegistrationInformation registrationInformations)
        {
            throw new System.NotImplementedException();
        }
        //TODO:函数功能：审核消息 输入：用户Id与审核结果（0表示不通过，1表示通过）,根据审核结果修改用户数据 输出：是否成功
        public UnaryResult<bool> ReviewMessage(ulong userId, bool reviewResults)
        {
            throw new System.NotImplementedException();
        }
        //TODO:函数功能：按照工号查找用户 输入：用户工号 输出：用户信息
        public UnaryResult<UserInfo> SelectByJobNumber(string jobNumber)
        {
            throw new System.NotImplementedException();
        }
        //TODO:函数功能：按照用户Id查找用户 输入：用户Id 输出：用户信息
        public UnaryResult<UserInfo> SelectByUserId(ulong userId)
        {
            throw new System.NotImplementedException();
        }
    }
}