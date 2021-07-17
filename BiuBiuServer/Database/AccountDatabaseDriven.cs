using BiuBiuServer.Interfaces;
using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuServer.Database
{
    public class AccountDatabaseDriven : IAccountDatabaseDriven
    {
        // TODO: 输入 电话号码/工号 和 密码 输出登录结果，登录结果构造方式参考 AccountDatabaseDrivenTest 类
        public async UnaryResult<SignInResponse> CommonSign(string signInId, string password)
        {
            throw new System.NotImplementedException();
        }

        // TODO：同上但是是管理员登录接口
        public async UnaryResult<SignInResponse> AdministrantSign(string signInId, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}