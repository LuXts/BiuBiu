using BiuBiuShare.ImInfos;
using BiuBiuShare.UserManagement;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    public interface IAdminDatabaseDriven
    {
        //函数功能：根据ID和注册信息注册新用户 输入：注册用户信息 输出：注册信息提示信息(-1表示该工号被注册，-2表示该手机号码被注册，0表示数据库因故障未插入成功,1表示成功)
        UnaryResult<int> RegisteredUsers(ulong userId, RegisterInfo registerInfos);

        //函数功能：按照工号查找用户 输入：用户工号 输出：用户信息
        UnaryResult<UserInfo> SelectByJobNumber(string jobNumber);

        //函数功能：按照用户Id查找用户 输入：用户Id 输出：用户信息
        UnaryResult<UserInfo> SelectByUserId(ulong userId);

        //函数功能：管理员修改用户密码 输入：用户Id、新密码 输出：修改密码是否成功
        UnaryResult<bool> ChangePassword(ulong userId, string newPassword);

        //函数功能：管理员修改用户信息 输入：用户ID、新的用户信息 输出：是否修改成功
        UnaryResult<bool> ChangeUserInfo(UserInfo newUserInfo);

        //函数功能：删除用户 输入：用户Id 输出：是否成功
        UnaryResult<bool> DeleteUser(ulong userId);

        //函数功能：审核消息 输入：用户Id与审核结果（0表示不通过，1表示通过） 输出：是否成功
        UnaryResult<bool> ReviewMessage(ulong userId, bool reviewResults);
    }
}