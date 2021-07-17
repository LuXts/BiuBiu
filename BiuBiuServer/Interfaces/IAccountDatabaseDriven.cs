using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    public interface IAccountDatabaseDriven :
    {
        // 函数功能：输入 电话号码/工号 和 密码 输出登录结果
        public UnaryResult<SignInResponse> CommonSign(string signInId
            , string password);

        // 函数功能：输入 电话号码/工号 和 密码 输出管理员登录结果
        public UnaryResult<SignInResponse> AdministrantSign(
            string signInId, string password);
    }
}