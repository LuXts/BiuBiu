using BiuBiuServer.Interfaces;
using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuServer.Database
{
    /// <summary>
    /// 数据库驱动实现
    /// </summary>
    public class AccountDatabaseDriven : IAccountDatabaseDriven
    {
        // TODO: 输入 电话号码/工号 和 密码 输出登录结果，登录结果构造方式参考 AccountDatabaseDrivenTest 类
        /// <inheritdoc />
        public async UnaryResult<SignInResponse> CommonSign(string signInId, string password)
        {
            throw new System.NotImplementedException();
        }

        // TODO：同上但是是管理员登录接口
        /// <inheritdoc />
        public async UnaryResult<SignInResponse> AdministrantSign(string signInId, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}