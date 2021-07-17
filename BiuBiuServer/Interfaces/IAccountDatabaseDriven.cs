using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuServer.Interfaces
{
    /// <summary>
    /// 登录服务与数据库之间的驱动
    /// </summary>
    public interface IAccountDatabaseDriven
    {
        /// <summary>
        /// 输入 电话号码/工号 和 密码 输出登录结果
        /// </summary>
        /// <param name="signInId">电话号码/工号</param>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public UnaryResult<SignInResponse> CommonSign(string signInId
            , string password);

        /// <summary>
        /// 输入 电话号码/工号 和 密码 输出登录结果
        /// </summary>
        /// <param name="signInId">电话号码/工号</param>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public UnaryResult<SignInResponse> AdministrantSign(
            string signInId, string password);
    }
}