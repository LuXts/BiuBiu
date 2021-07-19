using BiuBiuShare.SignIn;
using MagicOnion;

namespace BiuBiuShare.ServiceInterfaces
{
    /// <summary>
    /// 登录服务接口
    /// </summary>
    public interface IAccountService : IService<IAccountService>
    {
        /// <summary>
        /// 普通用户登录
        /// </summary>
        /// <param name="signInId">电话号码/工号</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果</returns>
        UnaryResult<SignInResponse> CommonSignInAsync(string signInId
            , string password);

        /// <summary>
        /// 管理员用户登录
        /// </summary>
        /// <param name="signInId">电话号码/工号</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果</returns>
        UnaryResult<SignInResponse> AdministrantSignInAsync(
            string signInId, string password);
    }
}